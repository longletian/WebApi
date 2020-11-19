using System;
using System.Collections.Generic;
using System.Text;
using WebApi.Models;

namespace WebApi.Services.IService
{
    public interface IUserService
    {
        ResponseData AccountLogin();

        ResponseData AccountrRegirst();
    }
}
