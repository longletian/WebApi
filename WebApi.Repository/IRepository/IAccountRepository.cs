﻿using System;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IAccountRepository : IBaseEntityRepository<AccountModel,long>
    {
    }
}
