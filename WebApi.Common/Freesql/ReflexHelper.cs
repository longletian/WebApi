using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebApi.Models;

namespace WebApi.Common
{
   public class ReflexHelper
    {
        public static Type[] GetTypesByTableAttribute()
        {
            List<Type> tableAssembies = new List<Type>();
            foreach (Type type in Assembly.GetAssembly(typeof(IEntity)).GetExportedTypes())
            {
                foreach (Attribute attribute in type.GetCustomAttributes())
                {
                    if (attribute is TableAttribute tableAttribute)
                    {
                        if (tableAttribute.DisableSyncStructure == false)
                        {
                            tableAssembies.Add(type);
                        }
                    }
                }
            };
            return tableAssembies.ToArray();
        }
    }
}
