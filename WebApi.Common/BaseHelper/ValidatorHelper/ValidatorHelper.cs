using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace WebApi.Common.BaseHelper.ValidatorHelper
{
    public static class ValidatorHelper
    {
        private static readonly object Locker = new object();
        private static ConcurrentDictionary<string, IValidator> _cacheValidators;

        /// <summary>
        /// 在程序集中定义所有请求对象的validator,将程序集内的validator初始化到内存中
        /// 默认给在webapi.Models
        /// </summary>
        /// <param name="assembly"></param>
        public static void Initialize(Assembly assembly)
        {
            lock (Locker)
            {
                if (_cacheValidators == null)
                {
                    _cacheValidators = new ConcurrentDictionary<string, IValidator>();
                    var results = AssemblyScanner.FindValidatorsInAssembly(assembly);
                    foreach (var result in results)
                    {
                        var modelType = result.InterfaceType.GenericTypeArguments[0];
                        _cacheValidators.TryAdd(modelType.FullName, (IValidator)Activator.CreateInstance(result.ValidatorType));
                    }
                }
            }
        }

        /// <summary>
        /// 对数据合法性进行验证，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IsValid<T>(this T request, out string msg) where T : class
        {
            msg = string.Empty;

            if (_cacheValidators == null || !_cacheValidators.TryGetValue(request.GetType().FullName, out var validator))
                return true;
            var result = validator.Validate(request);
            if (!result.IsValid)
            {
                // 返回第一个错误信息
                msg = result.Errors[0].ErrorMessage;
                return false;
            }
            return true;
        }


    }
}
