using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models
{  
   public class ResponseData
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int MsgCode { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 状态返回数据
        /// </summary>
        public object Data { get; set; }
    }
}
