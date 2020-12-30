using Microsoft.AspNetCore.Builder;
using System;
using System.IO;
using System.Text;
using tusdotnet;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;
using WebApi.Tools.SignalR;

namespace WebApi.Api.ConfigureExtensions
{
    public static class ConfigureExtensions
    {
       /// <summary>
       /// swagger-ui
       /// </summary>
       /// <param name="app"></param>
        public static void UseSwaggUIConfigure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        /// <summary>
        /// 路由配置
        /// </summary>
        /// <param name="app"></param>
        public static void UseRoutingConfigure(this IApplicationBuilder app)
        {
            app.UseRouting();
        }

        /// <summary>
        /// 使用实时通讯
        /// </summary>
        /// <param name="app"></param>
        public static void UseSignalConfigure(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/messagehub");
            });
        }

        /// <summary>
        /// 断点续传
        /// </summary>
        /// <param name="app"></param>
        public static void UseTusConfiguration(this IApplicationBuilder app)
        {

            var tusFiles = "";
            app.UseTus(context => 
            new DefaultTusConfiguration {
                //文件存储路径
                Store=new TusDiskStore(""),
                UrlPath="/files",
                //元数据是否允许空值
                MetadataParsingStrategy = MetadataParsingStrategy.AllowEmptyValues,
                //文件过期后不再更新
                Expiration = new AbsoluteExpiration(TimeSpan.FromMinutes(5)),
                Events =new tusdotnet.Models.Configuration.Events
                {
                    //上传完成事件回调
                    OnFileCompleteAsync = async ctx =>
                    {
                        //获取上传文件
                        var file = await ctx.GetFileAsync();

                        //获取上传文件元数据
                        var metadatas = await file.GetMetadataAsync(ctx.CancellationToken);

                        //获取上述文件元数据中的目标文件名称
                        var fileNameMetadata = metadatas["name"];

                        //目标文件名以base64编码，所以这里需要解码
                        var fileName = fileNameMetadata.GetString(Encoding.UTF8);

                        var extensionName = Path.GetExtension(fileName);

                        //将上传文件转换为实际目标文件
                        File.Move(Path.Combine(tusFiles, ctx.FileId), Path.Combine(tusFiles, $"{ctx.FileId}{extensionName}"));
                    }
                }
            });        
        }
    }
}
