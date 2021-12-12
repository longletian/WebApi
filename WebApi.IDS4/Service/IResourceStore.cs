using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IDS4.Service
{
   public interface IResourceStore
    {
        Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames);
        Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames);
        Task<ApiResource> FindApiResourceAsync(string name);
        Task<Resources> GetAllResourcesAsync();
    }
}
