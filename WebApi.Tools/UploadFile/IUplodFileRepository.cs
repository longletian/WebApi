using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace WebApi.Tools.UploadFile
{
    public  interface IUplodFileRepository
    {
        /// <summary>
        /// 导出excel文件
        /// </summary>
        /// <param name="entities">数据</param>
        /// <param name="dicColumns">标题头</param>
        /// <returns></returns>
        MemoryStream ExportExcel<T>(List<T> entities, Dictionary<string, string> dicColumns, string exportFileName = null, string title = null);
      
        /// <summary>
        /// 导入excel文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        DataSet ReadExcelToDataSet(string filePath, string sheetName = null);
      
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        bool UploadFile(string filePath, string uploadPath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> InputExcel<T>(IFormFile file) where T : class, new();
        
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> InputExcels<T>(IFormFile file) where T : class, new();
        
        
        /// <summary>
        /// 文
        /// </summary>
        /// <param name="file"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> InputExcels1<T>(IFormFile file) where T : class, new();
    }
}
