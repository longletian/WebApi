using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using WebApi.Models.Enums;

namespace WebApi.Common
{
    public static class CapOptionsExtenstion
    {
        public static CapOptions GetCapOptions(this CapOptions @this, IConfiguration Configuration)
        {
            //发布订阅 发布 消费 确认
            //至少你要配置一个消息队列和一个事件存储
            IConfigurationSection defaultStorage = Configuration.GetSection("CAP:DefaultStorage");
            IConfigurationSection defaultMessageQueue = Configuration.GetSection("CAP:DefaultMessageQueue");
            if (Enum.TryParse(defaultStorage.Value, out CapStorageType capStorageType))
            {
                if (!Enum.IsDefined(typeof(CapStorageType), capStorageType))
                {
                    Log.Error($"CAP配置CAP:DefaultStorage:{defaultStorage.Value}无效");
                }

                switch (capStorageType)
                {
                    case CapStorageType.InMemoryStorage:
                        // 支持内存存储（吞吐量的提高，这也意味着可以容忍消息有丢失的情况存在）
                        @this.UseInMemoryStorage();
                        break;
                    case CapStorageType.Mysql:
                        string mySqlCon = Configuration.GetConnectionString("PostgreSQL");
                        @this.UsePostgreSql(mySqlCon);
                        break;
                    default:
                        break;
                }

            }
            else
            {
                Log.Error($"CAP配置CAP:DefaultStorage:{defaultStorage.Value}无效");
            }

            if (Enum.TryParse(defaultMessageQueue.Value, out CapMessageQueueType capMessageQueueType))
            {
                if (!Enum.IsDefined(typeof(CapMessageQueueType), capMessageQueueType))
                {
                    Log.Error($"CAP配置CAP:DefaultMessageQueue:{defaultMessageQueue.Value}无效");
                }
                IConfigurationSection configurationSection = Configuration.GetSection($"ConnectionStrings:{capMessageQueueType}");

                switch (capMessageQueueType)
                {

                    case CapMessageQueueType.InMemoryQueue:
                        @this.UseInMemoryStorage();
                        break;
                    case CapMessageQueueType.RabbitMQ:
                        @this.UseRabbitMQ(options =>
                        {
                            options.HostName = Configuration["CAP:RabbitMQ:HostName"];
                            options.UserName = Configuration["CAP:RabbitMQ:UserName"];
                            options.Password = Configuration["CAP:RabbitMQ:Password"];
                            options.VirtualHost = Configuration["CAP:RabbitMQ:VirtualHost"];
                        });
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Log.Error($"CAP配置CAP:DefaultMessageQueue:{defaultMessageQueue.Value}无效");
            }

            return @this;

        }

    }
}
