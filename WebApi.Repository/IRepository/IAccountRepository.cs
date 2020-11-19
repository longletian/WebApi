using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;
using WebApi.Repository.Base;

namespace WebApi.Repository.IRepository
{
   public interface IAccountRepository : IBaseRepository<AccountModel>
    {
    }
}
