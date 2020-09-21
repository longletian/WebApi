namespace WebApi.Common.DataBase
{
    public class FreeSqlHelper
    {
        #region 单例模式双层if加lock
        //private static FreeSqlHelper instance = null;
        //private static readonly object objects = new object();
        //private FreeSqlHelper()
        //{

        //}
        //static FreeSqlHelper freeSqlHelper
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            lock (objects)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new FreeSqlHelper();
        //                }
        //            }

        //        }
        //        return freeSqlHelper;
        //    }
        //}
        #endregion

        private FreeSqlHelper()
        {

        }
        public static FreeSqlHelper freeSqlHelper 
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            static Nested()
            {

            }
            ///嵌套类可以访问封闭类的私有成员
            internal static readonly FreeSqlHelper instance = new FreeSqlHelper();
        }
    }
}
