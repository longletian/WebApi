using System;

namespace WebApi.Common.BaseHelper
{
    public class CommonHelper
    {
        /// <summary>
        /// 转化为时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
    }
}