using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Renci.SshNet.Security.Cryptography;
using RestSharp;

namespace WebApi.Common.BaseHelper.DingTalkHelper
{
    /// <summary>
    /// 对接浙政钉
    /// </summary>
    public class ZDingTalkHelper
    {
        /// <summary>
        /// 对接视频接口
        /// </summary>
        /// <param name="method">方法方式</param>
        /// <param name="canonicalURI">baseurl + url</param>
        /// <param name="requestParas">请求参数</param>
        /// <returns></returns>
        public static string HttpRequest(HttpMethod method, string canonicalURI,
            Dictionary<string, string> requestParas = null)
        {
            var requestUrl = AppSetting.GetSection("ZDingTalk:VideoBaseUrl") + canonicalURI; //域名配置地址
            try
            {
                var message = new HttpRequestMessage
                {
                    Method = method,
                };
                if (method == HttpMethod.Post)
                {
                    var paras = new Dictionary<string, string>();
                    if (requestParas?.Count() > 0)
                    {
                        foreach (var dic in requestParas)
                        {
                            paras.Add(dic.Key, Uri.UnescapeDataString(dic.Value));
                        }

                        message.Content = new FormUrlEncodedContent(paras);
                        message.Content.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    }
                }
                else if (method == HttpMethod.Get)
                {
                    requestUrl += $"?{DicionartyToUrlParameters(requestParas)}";
                }

                message.RequestUri = new Uri(requestUrl);
                using (var client = new System.Net.Http.HttpClient())
                {
                    var request = client.SendAsync(message);
                    var reponseContent = request.Result.Content.ReadAsStringAsync();
                    if (!request.Result.IsSuccessStatusCode)
                    {
                        throw new Exception(reponseContent.Result);
                    }

                    return reponseContent.Result;
                }
            }
            catch (Exception ex)
            {
                //记录日志
                throw;
            }
        }

        /// <summary>
        /// 政务钉请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="canonicalURI"></param>
        /// <param name="requestParas"></param>
        /// <returns></returns>
        public static string DingGovRequest(Method method, string canonicalURI,
            Dictionary<string, string> requestParas = null)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            var nonce = ConvertDateTimeToInt(DateTime.Now) + "1234";
            var requestUrl = AppSetting.GetSection("ZDingTalk:DingUrl").ToString() ; //域名配置地址
            
            var client = new RestClient(requestUrl);
            try
            {
                var request = new RestRequest(canonicalURI,method);

                var bytesToSign = $"{method}\n{timestamp}\n{nonce}\n{canonicalURI}";
                if (requestParas?.Count() > 0)
                {
                    requestParas = requestParas.OrderBy(m => m.Key).ToDictionary(m => m.Key, p => p.Value);
                    //参数参与签名
                    bytesToSign += '\n' + DicionartyToUrlParameters(requestParas);
                }

                #region 请求头

                Dictionary<string, string> dict = new Dictionary<string, string>()
                {
                    // {"X-Hmac-Auth-IP","1.1.1.1"},
                    // {"X-Hmac-Auth-MAC","MAC"},
                    {"X-Hmac-Auth-Timestamp",timestamp},
                    {"X-Hmac-Auth-Version","1.0"},
                    {"X-Hmac-Auth-Nonce",nonce},
                    {"apiKey",AppSetting.GetSection("ZDingTalk:AppKey").ToString()},
                    {"Content-Type","application/json"},
                    {"X-Hmac-Auth-Signature",GetSignature(bytesToSign,  AppSetting.GetSection("ZDingTalk:AppSecret").ToString())},
                };
                client.AddDefaultHeaders(dict);
                #endregion

                if (method == Method.POST)
                {
                    var paras = new Dictionary<string, string>();
                    if (requestParas?.Count() > 0)
                    {
                        foreach (var dic in requestParas)
                        {
                            //paras.Add(dic.Key, Uri.UnescapeDataString(dic.Value));
                            request.AddParameter(dic.Key, Uri.UnescapeDataString(dic.Value));
                        }  
                    }
                }
                else if (method == Method.GET)
                {
                    requestUrl += $"?{DicionartyToUrlParameters(requestParas)}";
                }
                var response = client.Execute(request);
                if (!response.IsSuccessful)
                {
                    throw new Exception(response.Content);
                }
                return response.Content;
            }
            catch (Exception ex)
            {
                //记录日志
                throw;
            }
        }
        
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="canonicalURI"></param>
        /// <param name="fileInfo">文件对象</param>
        /// <param name="requestParas"></param>
        /// <returns></returns>
        public static string DingGovRequestFormData(Method method, string canonicalURI, IFormFile fileInfo, Dictionary<string, string> requestParas = null)
        {
            try
            {
                var client = new RestClient(AppSetting.GetSection("ZDingTalk:DingUrl").ToString());
                var request = new RestRequest(canonicalURI, method)
                    .AddHeader("Content-Type", "multipart/form-data")
                    .AddFile("media", fileInfo.CopyTo,fileInfo.Name, fileInfo.Length,fileInfo.ContentType);
                foreach (var item in requestParas)
                {
                    request.AddQueryParameter(item.Key, item.Value);
                    request.AddParameter(item.Key, item.Value);
                };
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    return response.Content;
                }
                return "";
            }
            catch (Exception ex)
            {
                //记录日志
                throw;
            }
        }
        
        /// <summary>
        /// 对参数进行url字符拼接
        /// </summary>
        /// <param name="dicParameter"></param>
        /// <returns></returns>
        private static string DicionartyToUrlParameters(Dictionary<string, string> dicParameter)
        {
            var HttpRequestParams = string.Empty;
            if (dicParameter != null)
            {
                foreach (var item in dicParameter)
                {
                    if (!string.IsNullOrEmpty(HttpRequestParams))
                        HttpRequestParams += "&";
                    HttpRequestParams = string.Format("{0}{1}={2}", HttpRequestParams, item.Key, item.Value);
                }
            }

            return HttpRequestParams;
        }
        
        /// <summary>
        /// 转化为时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="message">将bytesToSign作为消息</param>
        /// <param name="secret">将SecretKey作为秘钥</param>
        /// <returns></returns>
        private static string GetSignature(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

    }
}