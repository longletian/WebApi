using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class UserModelDto
    {
        public Guid Id { get; set; }

        public string AccountName { get; set; }

        public string AccessToken { get; set; }

        public List<MenuViewDto> MenuViewDtos { get; set; }
    }
}
