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
            @this.Entity<IdentityRoleGroup>(e =>
            {
                #region 角色数据
                e.HasData(new List<IdentityRoleGroup>()
                    {
                        new IdentityRoleGroup{ RoleGroupCode="jt",ParentRoleGroupCode="", RoleGroupName="集团",RoleGroupRemark="集团",SortNum=0},
                        new IdentityRoleGroup{ RoleGroupCode="jt.zgs",ParentRoleGroupCode="jt", RoleGroupName="子公司",RoleGroupRemark="子公司",SortNum=1},
                        new IdentityRoleGroup{ RoleGroupCode="jt.zgs.kfb",ParentRoleGroupCode="jt.zgs", RoleGroupName="开发部",RoleGroupRemark="开发部",SortNum=1},
                        new IdentityRoleGroup{ RoleGroupCode="jt.zgs.xsb",ParentRoleGroupCode="jt.zgs", RoleGroupName="销售部",RoleGroupRemark="销售部",SortNum=2},
                        new IdentityRoleGroup{ RoleGroupCode="jt.zgs.ssb",ParentRoleGroupCode="jt.zgs", RoleGroupName="实施部",RoleGroupRemark="开发部",SortNum=3},
                        new IdentityRoleGroup{ RoleGroupCode="jt.zgs.cwb",ParentRoleGroupCode="jt.zgs",  RoleGroupName="财务部",RoleGroupRemark="财务部",SortNum=4},
                        new IdentityRoleGroup{ RoleGroupCode="jt.zgs.xsb.xsb1",ParentRoleGroupCode="jt.zgs.xsb", RoleGroupName="销售一部",RoleGroupRemark="销售一部",SortNum=0},
                        new IdentityRoleGroup{ RoleGroupCode="jt.zgs.xsb.xsb2",ParentRoleGroupCode="jt.zgs.xsb", RoleGroupName="销售二部",RoleGroupRemark="销售二部",SortNum=1},
                     });
                #endregion
            });

            @this.Entity<IdentityRole>(e =>
            {
                e.HasData(new List<IdentityRole>
                {
                    new IdentityRole { RoleName="系统管理员",RoleRemark="系统管理员", SortNum=0},
                    new IdentityRole { RoleName="网格员",RoleRemark="网格员", SortNum=1},
                    new IdentityRole { RoleName="坐席员",RoleRemark="坐席员", SortNum=2},
                    new IdentityRole { RoleName="处置人员",RoleRemark="处置人员", SortNum=3}
                });
            });

            #region 导航数据
            @this.Entity<MenuModel>(e =>
            {
                e.HasData(new List<MenuModel>()
                {
                    new MenuModel {  MenuCode="yywh",MenuName="应用维护",MenuPath="/1/", MenuRemark="应用维护", MenuUrl="/yywh",ParentMenuCode="" ,SortNum=0 },
                    new MenuModel {  MenuCode="yywh.zzjggl",MenuName="组织结构管理",MenuPath="/1/2/", MenuRemark="组织结构管理", MenuUrl="/xxtz",ParentMenuCode="yywh" ,SortNum=0 },
                    new MenuModel { MenuCode="yywh.zzjggl.bmgl",MenuName="部门管理",MenuPath="/1/2/3", MenuRemark="部门管理", MenuUrl="/bmgl",ParentMenuCode="yywh.zzjggl" ,SortNum=0 },
                    new MenuModel { MenuCode="yywh.zzjggl.yhgl",MenuName="用户管理",MenuPath="/1/2/4", MenuRemark="用户管理", MenuUrl="/yhgl",ParentMenuCode="yywh.zzjggl",SortNum=1 },
                    new MenuModel {  MenuCode="yywh.jsqxgl",MenuName="角色权限管理",MenuPath="/1/3", MenuRemark="角色权限管理", MenuUrl="/jsqxgl",ParentMenuCode="yywh" ,SortNum=1 },
                    new MenuModel { MenuCode="yywh.jsqxgl.jsgl",MenuName="角色管理",MenuPath="/1/3/6", MenuRemark="企业走访", MenuUrl="/jsgl",ParentMenuCode="yywh.jsqxgl" ,SortNum=0 },
                    new MenuModel { MenuCode="yywh.jsqxgl.qxgl",MenuName="权限管理",MenuPath="/1/3/7", MenuRemark="权限管理", MenuUrl="/qxgl",ParentMenuCode="yywh.jsqxgl",SortNum=1 },
                    new MenuModel {  MenuCode="ddd",MenuName="钉钉端",MenuPath="/8/", MenuRemark="钉钉端", MenuUrl="/ddd",ParentMenuCode="" ,SortNum=0 },
                    new MenuModel {  MenuCode="ddd.zhjj",MenuName="智慧经济",MenuPath="/8/9", MenuRemark="智慧经济", MenuUrl="/zhjj",ParentMenuCode="ddd" ,SortNum=0 },
                    new MenuModel {  MenuCode="ddd.zhzf",MenuName="智慧执法",MenuPath="/8/10", MenuRemark="智慧执法", MenuUrl="/zhzf",ParentMenuCode="ddd" ,SortNum=1 },
                    new MenuModel {  MenuCode="ddd.zhpa",MenuName="智慧平安",MenuPath="/8/11", MenuRemark="智慧平安", MenuUrl="/zhpa",ParentMenuCode="ddd" ,SortNum=2 },
                    new MenuModel {  MenuCode="ddd.zhzf.zczsb",MenuName="自处置上报",MenuPath="/8/9/12", MenuRemark="自处置上报", MenuUrl="/zczsb",ParentMenuCode="ddd.zhjj" ,SortNum=0 },
                    new MenuModel {  MenuCode="zhzf",MenuName="智慧执法",MenuPath="/13/", MenuRemark="智慧执法", MenuUrl="/zhzf",ParentMenuCode="" ,SortNum=2 },
                    new MenuModel {  MenuCode="zhzf.ysll",MenuName="预受理事件",MenuPath="/13/14", MenuRemark="预受理事件", MenuUrl="/yslsj",ParentMenuCode="zhzf" ,SortNum=0 },
                    new MenuModel {  MenuCode="zhzf.dpql",MenuName="待派遣事件",MenuPath="/13/15", MenuRemark="待派遣事件", MenuUrl="/dpqsj",ParentMenuCode="zhzf" ,SortNum=1 },
                 });
            });
            #endregion

            @this.Entity<IdentityUser>(e =>
            {
                #region 用户数据
                e.HasData(new List<IdentityUser>()
                 {
                    new IdentityUser()
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
                    new IdentityUser()
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

            @this.Entity<IdentityUserRole>(e =>
            {
                #region 用户角色组
                e.HasData(new List<IdentityUserRole>()
                {
                     new IdentityUserRole(1,1),
                });
                #endregion
            });

            @this.Entity<IdentityPermission>(e =>
            {
                e.HasData(new List<IdentityPermission>()
                {
                    new IdentityPermission {PermissionCode="zhzf",PermissionName="智慧执法模块",PermissionRemark= "智慧执法模块", ParentPermissionCode=""},
                    new IdentityPermission {PermissionCode="ddd",PermissionName="钉钉端模块",PermissionRemark= "钉钉端模块", ParentPermissionCode=""},
                    new IdentityPermission { PermissionCode = "yywh", PermissionName = "应用维护模块", PermissionRemark = "应用维护模块", ParentPermissionCode = "" }
                });
            });

            @this.Entity<IdentityMenuPermission>(e =>
            {
                e.HasData(new List<IdentityMenuPermission>()
                {
                    new IdentityMenuPermission{  MenuId=1, PermissionId=3},
                    new IdentityMenuPermission{  MenuId=2, PermissionId=3},
                    new IdentityMenuPermission{  MenuId=3, PermissionId=3},
                    new IdentityMenuPermission{  MenuId=4, PermissionId=3},
                    new IdentityMenuPermission{ MenuId=8,PermissionId=2},
                    new IdentityMenuPermission{ MenuId=13,PermissionId=1},
                });
            });

            @this.Entity<OperateModel>(e =>
            {
                e.HasData(new List<OperateModel>()
                {
                    new OperateModel { OperateCode="ysll.sl", OperateName="预受理事件受理", OperateParentCode="zhzf.ysll", OperateRemark="预受理事件受理"},
                    new OperateModel { OperateCode="ysll.ja", OperateName="预受理事件受理结案", OperateParentCode="zhzf.ysll", OperateRemark="预受理事件受理"},
                });
            });

            @this.Entity<IdentityOperatePermission>(e =>
            {
                e.HasData(new List<IdentityOperatePermission>()
                {
                    new IdentityOperatePermission {OperateId=1,PermissionId=1},
                    new IdentityOperatePermission {OperateId=2,PermissionId=1}
                });
            });

            @this.Entity<IdentityRolePermission>(e =>
            {
                e.HasData(new List<IdentityRolePermission>()
                {
                    new IdentityRolePermission { PermissionId=1,RoleId=1},
                    new IdentityRolePermission { PermissionId=1,RoleId=3},
                    new IdentityRolePermission { PermissionId=2,RoleId=2},
                });
            });

            @this.Entity<IdentityGroup>(e =>
            {
                #region 组
                e.HasData(new List<IdentityGroup>()
                {
                    new IdentityGroup{GroupCode="sdzzf", GroupName="三墩镇政府", UserGroupRemark="" , ParentGroupCode=""},
                    new IdentityGroup{GroupCode="sdzzf.jfb", GroupName="经发办", UserGroupRemark="经发办" , ParentGroupCode="sdzzf"},
                    new IdentityGroup{GroupCode="sdzzf.jfb.mtbm", GroupName="矛调部门", UserGroupRemark="矛调部门" , ParentGroupCode="sdzzf.jfb"},
                    new IdentityGroup{GroupCode="sdzzf.jfb.znbm", GroupName="职能部门", UserGroupRemark="职能部门" , ParentGroupCode="sdzzf.jfb"},
                    new IdentityGroup{GroupCode="sdzzf.jfb.sqc", GroupName="社区(村)", UserGroupRemark="社区(村)" , ParentGroupCode="sdzzf.jfb"},
                    new IdentityGroup{GroupCode="wbdw", GroupName="外部单位", UserGroupRemark="" , ParentGroupCode=""},
                    new IdentityGroup{GroupCode="wbdw.trgc", GroupName="同人广场", UserGroupRemark="经发办" , ParentGroupCode="wbdw"},
                    new IdentityGroup{GroupCode="wbdw.ypsd", GroupName="优盘时代", UserGroupRemark="优盘时代" , ParentGroupCode="wbdw"},
                    new IdentityGroup{GroupCode="wbdw.trjh", GroupName="同人精华", UserGroupRemark="同人精华" , ParentGroupCode="wbdw"},
                    new IdentityGroup{GroupCode="wbdw.jqrs", GroupName="剑桥人社", UserGroupRemark="剑桥人社" , ParentGroupCode="wbdw"},
                    new IdentityGroup{GroupCode="wbdw.zjgc", GroupName="紫金广场", UserGroupRemark="紫金广场" , ParentGroupCode="wbdw"}
                });
                #endregion
            });

            @this.Entity<ApiModel>(e =>
            {
                e.HasData(new List<ApiModel>()
                {
                    new ApiModel { ApiName = "登陆接口", LinkUrl = "api/account/login", ApiRemark = "登陆接口", ApiType = Convert.ToInt32(RequestEnum.Post), IsEnable = false, IsAuth = false},
                    new ApiModel { ApiName = "注册接口", LinkUrl = "api/account/regirst", ApiRemark = "注册接口", ApiType = Convert.ToInt32(RequestEnum.Post), IsEnable = false, IsAuth = false},
                    new ApiModel { ApiName = "修改密码", LinkUrl = "api/account/{password}", ApiRemark = "修改密码", ApiType = Convert.ToInt32(RequestEnum.Put), IsEnable = false, IsAuth = false },
                    new ApiModel { ApiName = "删除用户", LinkUrl = "api/account/{userId}", ApiRemark = "删除用户", ApiType = Convert.ToInt32(RequestEnum.Delete), IsEnable = false, IsAuth = false}
                });
            });

            @this.Entity<IdentityGroupRole>(e =>
            {
                e.HasData(new List<IdentityGroupRole>()
                {
                    new IdentityGroupRole{ RoleId=1,GroupId=1},
                });
            });

            @this.Entity<IdentityGroupUser>(e =>
            {
                e.HasData(new List<IdentityGroupUser>()
                {
                    new IdentityGroupUser{ GroupId=4, UserId=1 },
                    new IdentityGroupUser{ GroupId=5, UserId=2 }
                });

            });

            @this.Entity<AccountModel>(e =>
            {
                #region 账号数据
                e.HasData(new List<AccountModel>()
                {
                    new AccountModel {
                        AccountName="11143343@qq.com",
                        AccountPasswd="123456",
                        AccountPasswdEncrypt="123456",
                        AccountType=2,
                        AccountState=1
                        },
                    new AccountModel{
                        AccountName="admin22@qq.com",
                        AccountPasswd="123456",
                        AccountPasswdEncrypt="123456",
                        AccountType=2,
                        AccountState=1,
                    },
                        new AccountModel{
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
