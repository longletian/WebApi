
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using RestSharp;
using System.Collections.Generic;
using System.Net.Http;

namespace WebApi.Common.BaseHelper.NetHelper
{
    public static class RestHttpHelper
    {

        /// <summary>
        /// 支持post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="method">方法</param>
        /// <param name="dict">body请求</param>
        /// <param name="headerDict">头部请求参数</param>
        /// <returns></returns>
        public static string RequestData(string url, Method method, Dictionary<string, string> dict = null, Dictionary<string, string> headerDict = null)
        {
            try
            {
                var client = new RestClient(AppSetting.GetSection("BaseClientUrl").ToString());
                var request = new RestRequest(url, method);
                if (headerDict?.Count > 0)
                    request.AddHeaders(headerDict);
                if (method == Method.GET)
                {
                    foreach (var item in dict)
                    {
                        request.AddQueryParameter(item.Key, item.Value);
                    }
                }
                else if (method == Method.POST)
                {
                    foreach (var item in dict)
                    {
                        request.AddJsonBody(new { item.Key, item.Value });
                    }
                }
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    return response.Content;
                }
                return "";
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 支持form-data格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string PostRequestFormData(string url, Method method,bool QueryParams ,IFormFile file=null, Dictionary<string, string> dict = null, Dictionary<string, string> headerDict = null)
        {
            try
            {
                var client = new RestClient(AppSetting.GetSection("BaseClientUrl").ToString());
                var request = new RestRequest(url, method);
                if (headerDict?.Count > 0)
                    request.AddHeaders(headerDict);
                if (method == Method.POST)
                {
                    request.AddHeader("Content-Type", "multipart/form-data");
                    request.AddFile("file", c => file.OpenReadStream(), file.FileName, file.Length);
                    foreach (var item in dict)
                    {
                        if (QueryParams) { request.AddQueryParameter(item.Key, item.Value); }
                        request.AddParameter(item.Key,item.Value);
                    }
                }
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    return response.Content;
                }
                return "";
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
