using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WebApi.Models.Entity
{
    public class DataDbContext : DbContext 
    {
        public DataDbContext()
        { 
        
        }
        public DataDbContext(DbContextOptions options) : base(options)
        { 
        
        }
    }
}
