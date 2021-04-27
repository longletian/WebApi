using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.IDS4.Service
{
   public  interface IClientStore
    {
        Task<Client> FindClientByIdAsync(string clientId);
    }
}
