using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Common.BaseHelper.Md5Helper;
using WebApi.Models;
using WebApi.Models.Models;
using WebApi.Repository.Base.Unitwork;
using WebApi.Repository.IRepository;
using WebApi.Services.Base;
using WebApi.Services.IService;

namespace WebApi.Services.Service
{
    public class UserService : BaseService<IdentityUser>, IUserService
    {
      
    }
}
