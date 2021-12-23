using FreeSql;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebApi.Models;
using WebApi.Models.Data.Enums;
using WebApi.Models.Enums;

namespace WebApi.Common
{
    /// <summary>
    /// 扩展freesql方法
    /// </summary>
    public static class FreeSqlExtension
    {
        public static ISelect<T> AsTable<T>(this ISelect<T> @this, params string[] tableNames) where T : class
        {
            tableNames?.ToList().ForEach(tableName =>
            {
                @this.AsTable((type, oldname) =>
                {
                    if (type == typeof(T)) return tableName;
                    return null;
                });
            });
            return @this;
        }

        /// <summary>
        /// 配置数据库连接
        /// </summary>
        /// <param name="this"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static FreeSqlBuilder UseConnectionString(this FreeSqlBuilder @this)
        {
            IConfigurationSection dbTypeCode = AppSetting.GetSection("ConnectionStrings:DefaultDB");
            if (Enum.TryParse(dbTypeCode.Value, out DataType dataType))
            {
                if (!Enum.IsDefined(typeof(DataType), dataType))
                {
                    Log.Error($"数据库配置ConnectionStrings:DefaultDB:{dataType}无效");
                }

                IConfigurationSection configurationSection = AppSetting.GetSection($"ConnectionStrings:{dataType}");
                @this.UseConnectionString(dataType, configurationSection.Value);
                if (AppSetting.GetSection("ConnectionStrings:BoolOpenSalve").Value == "true")
                {
                    string dbStr = AppSetting.GetSection("ConnectionStrings:SalveDB").Value;
                    if (!string.IsNullOrEmpty(dbStr))
                    {
                        if (dbStr.IndexOf(',') > 0)
                        {
                            string[] arrayStr = dbStr.Split(',');
                            List<string> lists = new List<string>();
                            for (int i = 0; i < arrayStr.Length; i++)
                            {
                                lists.Add(AppSetting.GetSection($"ConnectionStrings:{arrayStr[i]}").Value);
                            }
                            @this.UseSlave(lists.ToArray());
                        }
                        else
                        {
                            @this.UseSlave(AppSetting.GetSection($"ConnectionStrings:{dbStr}").ToString());
                        }
                    }
                }
            }
            else
            {
                Log.Error($"数据库配置ConnectionStrings:DefaultDB:{dbTypeCode.Value}无效");
            }
            return @this;
        }

        /// <summary>
        /// 请在UseConnectionString配置后调用此方法
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static FreeSqlBuilder CreateDatabaseIfNotExists(this FreeSqlBuilder @this)
        {
            FieldInfo dataTypeFieldInfo = @this.GetType().GetField("_dataType", BindingFlags.NonPublic | BindingFlags.Instance);

            if (dataTypeFieldInfo is null)
            {
                throw new ArgumentException("_dataType is null");
            }
            string connectionString = GetConnectionString(@this);
            DataType dbType = (DataType)dataTypeFieldInfo.GetValue(@this);

            switch (dbType)
            {
                case DataType.MySql:
                    return @this.CreateDatabaseIfNotExistsMySql(connectionString);
                case DataType.PostgreSQL:
                    //return @this.CreateDatabaseIfNotExistsPostgreSql(connectionString);
                    break;
                case DataType.Oracle:
                    break;
                default:
                    break;
            }
            //Log.Error($"不支持创建数据库");
            return @this;
        }

        #region mysql创建数据库
        public static FreeSqlBuilder CreateDatabaseIfNotExistsMySql(this FreeSqlBuilder @this,
            string connectionString = "")
        {
            if (connectionString == "")
            {
                connectionString = GetConnectionString(@this);
            }

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);

            string createDatabaseSql =
                $"USE mysql;CREATE DATABASE IF NOT EXISTS `{builder.Database}` CHARACTER SET '{builder.CharacterSet}' COLLATE 'utf8mb4_general_ci'";

            using MySqlConnection cnn = new MySqlConnection(
                $"Data Source={builder.Server};Port={builder.Port};User ID={builder.UserID};Password={builder.Password};Initial Catalog=mysql;Charset=utf8;SslMode=none;Max pool size=1");

            cnn.Open();

            using (MySqlCommand cmd = cnn.CreateCommand())
            {
                cmd.CommandText = createDatabaseSql;
                cmd.ExecuteNonQuery();
            }
            return @this;
        }
        #endregion

        private static string ExpandFileName(string fileName)
        {
            if (fileName.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
            {
                var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
                if (string.IsNullOrEmpty(dataDirectory))
                {
                    dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
                }
                string name = fileName.Replace("\\", "").Replace("/", "").Substring("|DataDirectory|".Length);
                fileName = Path.Combine(dataDirectory, name);
            }
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            return Path.GetFullPath(fileName);
        }

        /// <summary>
        /// 多库问题
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        private static string GetConnectionString(FreeSqlBuilder @this)
        {
            Type type = @this.GetType();
            FieldInfo fieldInfo =
                type.GetField("_masterConnectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo is null)
            {
                throw new ArgumentException("_masterConnectionString is null");
            }
            return fieldInfo.GetValue(@this).ToString();
        }

        public static ICodeFirst SeedData(this ICodeFirst @this)
        {
            @this.Entity<RoleEntity>(e =>
            {
                e.HasData(new List<RoleEntity>
                {
                    new RoleEntity { RoleName="系统管理员",RoleRemark="系统管理员", SortNum=0},
                    new RoleEntity { RoleName="网格员",RoleRemark="网格员", SortNum=1},
                    new RoleEntity { RoleName="坐席员",RoleRemark="坐席员", SortNum=2},
                    new RoleEntity { RoleName="处置人员",RoleRemark="处置人员", SortNum=3}
                });
            });

            #region 导航数据
            @this.Entity<MenuEntity>(e =>
            {
                e.HasData(new List<MenuEntity>()
                {
                    new MenuEntity {  MenuCode="yywh",MenuName="应用维护",MenuPath="/1/", MenuRemark="应用维护", MenuUrl="/yywh",ParentMenuCode="" ,SortNum=0 },
                    new MenuEntity {  MenuCode="yywh.zzjggl",MenuName="组织结构管理",MenuPath="/1/2/", MenuRemark="组织结构管理", MenuUrl="/xxtz",ParentMenuCode="yywh" ,SortNum=0 },
                    new MenuEntity { MenuCode="yywh.zzjggl.bmgl",MenuName="部门管理",MenuPath="/1/2/3", MenuRemark="部门管理", MenuUrl="/bmgl",ParentMenuCode="yywh.zzjggl" ,SortNum=0 },
                    new MenuEntity { MenuCode="yywh.zzjggl.yhgl",MenuName="用户管理",MenuPath="/1/2/4", MenuRemark="用户管理", MenuUrl="/yhgl",ParentMenuCode="yywh.zzjggl",SortNum=1 },
                    new MenuEntity {  MenuCode="yywh.jsqxgl",MenuName="角色权限管理",MenuPath="/1/3", MenuRemark="角色权限管理", MenuUrl="/jsqxgl",ParentMenuCode="yywh" ,SortNum=1 },
                    new MenuEntity { MenuCode="yywh.jsqxgl.jsgl",MenuName="角色管理",MenuPath="/1/3/6", MenuRemark="企业走访", MenuUrl="/jsgl",ParentMenuCode="yywh.jsqxgl" ,SortNum=0 },
                    new MenuEntity { MenuCode="yywh.jsqxgl.qxgl",MenuName="权限管理",MenuPath="/1/3/7", MenuRemark="权限管理", MenuUrl="/qxgl",ParentMenuCode="yywh.jsqxgl",SortNum=1 },
                    new MenuEntity {  MenuCode="ddd",MenuName="钉钉端",MenuPath="/8/", MenuRemark="钉钉端", MenuUrl="/ddd",ParentMenuCode="" ,SortNum=0 },
                    new MenuEntity {  MenuCode="ddd.zhjj",MenuName="智慧经济",MenuPath="/8/9", MenuRemark="智慧经济", MenuUrl="/zhjj",ParentMenuCode="ddd" ,SortNum=0 },
                    new MenuEntity {  MenuCode="ddd.zhzf",MenuName="智慧执法",MenuPath="/8/10", MenuRemark="智慧执法", MenuUrl="/zhzf",ParentMenuCode="ddd" ,SortNum=1 },
                    new MenuEntity {  MenuCode="ddd.zhpa",MenuName="智慧平安",MenuPath="/8/11", MenuRemark="智慧平安", MenuUrl="/zhpa",ParentMenuCode="ddd" ,SortNum=2 },
                    new MenuEntity {  MenuCode="ddd.zhzf.zczsb",MenuName="自处置上报",MenuPath="/8/9/12", MenuRemark="自处置上报", MenuUrl="/zczsb",ParentMenuCode="ddd.zhjj" ,SortNum=0 },
                    new MenuEntity {  MenuCode="zhzf",MenuName="智慧执法",MenuPath="/13/", MenuRemark="智慧执法", MenuUrl="/zhzf",ParentMenuCode="" ,SortNum=2 },
                    new MenuEntity {  MenuCode="zhzf.ysll",MenuName="预受理事件",MenuPath="/13/14", MenuRemark="预受理事件", MenuUrl="/yslsj",ParentMenuCode="zhzf" ,SortNum=0 },
                    new MenuEntity {  MenuCode="zhzf.dpql",MenuName="待派遣事件",MenuPath="/13/15", MenuRemark="待派遣事件", MenuUrl="/dpqsj",ParentMenuCode="zhzf" ,SortNum=1 },
                 });
            });
            #endregion

            @this.Entity<UserEntity>(e =>
            {
                #region 用户数据
                e.HasData(new List<UserEntity>()
                 {
                    new UserEntity()
                    {
                        UserName="admin",
                        NickName="开发部经理",
                        UserSex=0,
                        Email="11143343@qq.com",
                        BirthDay="1992",
                        CityCode="1992",
                        DistrictCode="1992",
                        ProvinceCode="1992",
                        RealName="系统管理员",
                        TelePhone="19912313123",
                        IsDeleted=false,
                    },
                    new UserEntity()
                    {
                        UserName="admin1",
                        NickName="系统管理员",
                        UserSex=0,
                        Email="11143343@qq.com",
                        BirthDay="1992",
                        CityCode="1992",
                        DistrictCode="1992",
                        ProvinceCode="1992",
                        RealName="系统管理员",
                        TelePhone="19912313123",
                        IsDeleted=false,
                    }
                 });
                #endregion
            });

            @this.Entity<UserRoleEntity>(e =>
            {
                #region 用户角色组
                e.HasData(new List<UserRoleEntity>()
                {
                     new UserRoleEntity(1,1),
                });
                #endregion
            });

            @this.Entity<PermissionEntity>(e =>
            {
                e.HasData(new List<PermissionEntity>()
                {
                    new PermissionEntity {PermissionCode="zhzf",PermissionName="智慧执法模块",PermissionRemark= "智慧执法模块", ParentPermissionCode=""},
                    new PermissionEntity {PermissionCode="ddd",PermissionName="钉钉端模块",PermissionRemark= "钉钉端模块", ParentPermissionCode=""},
                    new PermissionEntity { PermissionCode = "yywh", PermissionName = "应用维护模块", PermissionRemark = "应用维护模块", ParentPermissionCode = "" }
                });
            });

            @this.Entity<MenuPermissionEntity>(e =>
            {
                e.HasData(new List<MenuPermissionEntity>()
                {
                    new MenuPermissionEntity{  MenuId=1, PermissionId=3},
                    new MenuPermissionEntity{  MenuId=2, PermissionId=3},
                    new MenuPermissionEntity{  MenuId=3, PermissionId=3},
                    new MenuPermissionEntity{  MenuId=4, PermissionId=3},
                    new MenuPermissionEntity{ MenuId=8,PermissionId=2},
                    new MenuPermissionEntity{ MenuId=13,PermissionId=1},
                });
            });

            @this.Entity<OperateEntity>(e =>
            {
                e.HasData(new List<OperateEntity>()
                {
                    new OperateEntity { OperateCode="ysll.sl", OperateName="预受理事件受理", OperateParentCode="zhzf.ysll", OperateRemark="预受理事件受理"},
                    new OperateEntity { OperateCode="ysll.ja", OperateName="预受理事件受理结案", OperateParentCode="zhzf.ysll", OperateRemark="预受理事件受理"},
                });
            });

            @this.Entity<OperatePermissionEntity>(e =>
            {
                e.HasData(new List<OperatePermissionEntity>()
                {
                    new OperatePermissionEntity {OperateId=1,PermissionId=1},
                    new OperatePermissionEntity {OperateId=2,PermissionId=1}
                });
            });

            @this.Entity<RolePermissionEntity>(e =>
            {
                e.HasData(new List<RolePermissionEntity>()
                {
                    new RolePermissionEntity { PermissionId=1,RoleId=1},
                    new RolePermissionEntity { PermissionId=1,RoleId=3},
                    new RolePermissionEntity { PermissionId=2,RoleId=2},
                });
            });

            @this.Entity<GroupEntity>(e =>
            {
                #region 组
                e.HasData(new List<GroupEntity>()
                {
                    new GroupEntity{GroupCode= "sdzzf", GroupName= "三墩镇政府", Remark= "" , ParentGroupCode=""},
                    new GroupEntity{GroupCode="sdzzf.jfb", GroupName="经发办", Remark="经发办" , ParentGroupCode="sdzzf"},
                    new GroupEntity{GroupCode="sdzzf.jfb.mtbm", GroupName="矛调部门", Remark="矛调部门" , ParentGroupCode="sdzzf.jfb"},
                    new GroupEntity{GroupCode="sdzzf.jfb.znbm", GroupName="职能部门", Remark="职能部门" , ParentGroupCode="sdzzf.jfb"},
                    new GroupEntity{GroupCode="sdzzf.jfb.sqc", GroupName="社区(村)", Remark="社区(村)" , ParentGroupCode="sdzzf.jfb"},
                    new GroupEntity{GroupCode="wbdw", GroupName="外部单位", Remark="" , ParentGroupCode=""},
                    new GroupEntity{GroupCode="wbdw.trgc", GroupName="同人广场", Remark="经发办" , ParentGroupCode="wbdw"},
                    new GroupEntity{GroupCode="wbdw.ypsd", GroupName="优盘时代", Remark="优盘时代" , ParentGroupCode="wbdw"},
                    new GroupEntity{GroupCode="wbdw.trjh", GroupName="同人精华", Remark="同人精华" , ParentGroupCode="wbdw"},
                    new GroupEntity{GroupCode="wbdw.jqrs", GroupName="剑桥人社", Remark="剑桥人社" , ParentGroupCode="wbdw"},
                    new GroupEntity{GroupCode="wbdw.zjgc", GroupName="紫金广场", Remark="紫金广场" , ParentGroupCode="wbdw"}
                });
                #endregion
            });

            @this.Entity<ApiEntity>(e =>
            {
                e.HasData(new List<ApiEntity>()
                {
                    new ApiEntity { ApiName = "登陆接口", LinkUrl = "api/account/login", ApiRemark = "登陆接口", ApiType = Convert.ToInt32(RequestEnum.Post), IsEnable = false, IsAuth = false},
                    new ApiEntity { ApiName = "注册接口", LinkUrl = "api/account/regirst", ApiRemark = "注册接口", ApiType = Convert.ToInt32(RequestEnum.Post), IsEnable = false, IsAuth = false},
                    new ApiEntity { ApiName = "修改密码", LinkUrl = "api/account/{password}", ApiRemark = "修改密码", ApiType = Convert.ToInt32(RequestEnum.Put), IsEnable = false, IsAuth = false },
                    new ApiEntity { ApiName = "删除用户", LinkUrl = "api/account/{userId}", ApiRemark = "删除用户", ApiType = Convert.ToInt32(RequestEnum.Delete), IsEnable = false, IsAuth = false}
                });
            });

            @this.Entity<GroupRoleEntity>(e =>
            {
                e.HasData(new List<GroupRoleEntity>()
                {
                    new GroupRoleEntity{ RoleId=1,GroupId=1},
                });
            });

            @this.Entity<GroupUserEntity>(e =>
            {
                e.HasData(new List<GroupUserEntity>()
                {
                    new GroupUserEntity{ GroupId=4, UserId=1 },
                    new GroupUserEntity{ GroupId=5, UserId=2 }
                });

            });

            @this.Entity<AccountEntity>(e =>
            {
                #region 账号数据
                e.HasData(new List<AccountEntity>()
                {
                    new AccountEntity {
                        AccountName="11143343@qq.com",
                        AccountPasswd="123456",
                        AccountPasswdEncrypt="123456",
                        AccountType=2,
                        AccountState=1
                        },
                    new AccountEntity{
                        AccountName="admin22@qq.com",
                        AccountPasswd="123456",
                        AccountPasswdEncrypt="123456",
                        AccountType=2,
                        AccountState=1,
                    },
                        new AccountEntity{
                        AccountName="admin33@qq.com",
                        AccountPasswd="123456",
                        AccountPasswdEncrypt="123456",
                        AccountType=2,
                        AccountState=1
                    }
                });
                #endregion
            });
            return @this;
        }
    }
}
