using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;
using WebApi.Repository.Base;
using WebApi.Repository.Base.Unitwork;
using WebApi.Repository.IRepository;

namespace WebApi.Repository.Repository
{
  public  class UserRepository:BaseRepository<IdentityUser>,IUserRepository
    {
        public UserRepository(IUnitworkRepository _unitOfWork, DataDbContext dataDbContext) : base(_unitOfWork, dataDbContext)
        { 

        }
    }
}
