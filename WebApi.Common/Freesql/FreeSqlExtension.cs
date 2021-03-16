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
            @this.Entity<IdentityRole>(e =>
            {
                #region 角色数据
                e.HasData(new List<IdentityRole>()
                    {
                        new IdentityRole{ RoleName="系统管理员",RoleRemark="系统管理员", SortNum=1},
                        new IdentityRole { RoleName="网格员",RoleRemark="网格员", SortNum=2},
                        new IdentityRole { RoleName="坐席员",RoleRemark="坐席员", SortNum=2},
                        new IdentityRole { RoleName="网格员",RoleRemark="网格员", SortNum=2},
                        new IdentityRole { RoleName="处置人员",RoleRemark="处置人员", SortNum=2},
                        new IdentityRole { RoleName="督察员",RoleRemark="督察员", SortNum=2},
                        new IdentityRole { RoleName="社区网格员",RoleRemark="社区网格员", SortNum=2},
                        new IdentityRole { RoleName="指挥长",RoleRemark="指挥长", SortNum=2},
                        new IdentityRole { RoleName="副区域长",RoleRemark="副区域长", SortNum=2},
                        new IdentityRole { RoleName="社区坐席员",RoleRemark="社区坐席员", SortNum=2}
                });
                #endregion
            });

            @this.Entity<IdentityUser>(e =>
            {
                #region 用户数据
                e.HasData(new List<IdentityUser>()
                 {
                    new IdentityUser()
                    {
                        UserName="admin",
                        NickName="系统管理员",
                        CreateTime=DateTime.Now,
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

            });


            @this.Entity<IdentityRolePermission>(e =>
            {


            });

            @this.Entity<IdentityGroup>(e =>
            {
                #region 组
                e.HasData(new List<IdentityGroup>()
                {
                      new IdentityGroup{GroupCode="/1/", GroupName="三墩镇政府", UserGroupRemark="" , ParentGroupCode=""},
                      new IdentityGroup{GroupCode="/2/", GroupName="外部单位", UserGroupRemark="" , ParentGroupCode=""},
                      new IdentityGroup{GroupCode="1/140/", GroupName="社区(村)", UserGroupRemark="" , ParentGroupCode="1"},
                      new IdentityGroup{GroupCode="/1/310/", GroupName="经发办", UserGroupRemark="" , ParentGroupCode="1"},
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
                    //new IdentityGroupRole{ RoleId=,GroupId=},
                });
            });


            @this.Entity<IdentityGroupUser>(e =>
            {


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
                        AccountState=1,
                        Active= UserActive.Active
                        },
                    new AccountModel{
                        AccountName="admin22@qq.com",
                        AccountPasswd="123456",
                        AccountPasswdEncrypt="123456",
                        AccountType=2,
                        AccountState=1,
                        Active= UserActive.Active
                    },
                        new AccountModel{
                        AccountName="admin33@qq.com",
                        AccountPasswd="123456",
                        AccountPasswdEncrypt="123456",
                        AccountType=2,
                        AccountState=1,
                        Active= UserActive.Active
                    }
                });
                #endregion
            });
            return @this;
        }
    }
}
