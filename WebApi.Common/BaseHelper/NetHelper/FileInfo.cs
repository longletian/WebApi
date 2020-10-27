using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Common.BaseHelper.NetHelper
{ 
    public class FileInfo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string   FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public IFormFile FormFile { get; set; }
    }
}
