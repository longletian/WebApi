using System;
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Test
{
    public static class Utilities
    {
        public static void InitializeDbForTests(IFreeSql db)
        {
            db.GetGuidRepository<MenuEntity>().Insert(GetSeedingMessages());
        }

        public static void ReinitializeDbForTests(IFreeSql db)
        {
            //db.GetGuidRepository<MenuModel>().Delete();
            InitializeDbForTests(db);
        }

        public static List<MenuEntity> GetSeedingMessages()
        {
            return new List<MenuEntity>()
            {
                //new MenuModel{ CreateorId = 1,UpdateorId = 1,CreateTime =DateTime.Parse("2020-11-23 15:32:50") ,IsDelete = false,
                //    MenuCode = "hnxy",MenuName = "湖南学院",MenuPath = "hnxy",ParentMenuCode = "",MenuUrl = "/hnxy",MenuRemark = ""},
                //new MenuModel{ CreateorId = 1,UpdateorId = 1,CreateTime =DateTime.Parse("2020-11-23 15:32:50") ,IsDelete = false,
                //    MenuCode = "rjgcx",MenuName = "软件工程系",MenuPath = "rjgcx",ParentMenuCode = "hnxy",MenuUrl = "/rjgcx",MenuRemark = ""},
                //new MenuModel{ CreateorId = 1,UpdateorId = 1,CreateTime =DateTime.Parse("2020-11-23 15:32:50") ,IsDelete = false,
                //    MenuCode = "jzgcx",MenuName = "建筑工程系",MenuPath = "jzgcx",ParentMenuCode = "hnxy",MenuUrl = "/jzgcx",MenuRemark = ""},
             
            };
        }
    }
}