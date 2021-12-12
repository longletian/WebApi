using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IDS4.Service
{
   public interface ICorsPolicyService
    {
        Task<bool> IsOriginAllowedAsync(string origin);
    }
}
