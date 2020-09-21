using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models.Entity
{
    public  interface IEntity: IEntity<int>
    {

    }
    public interface IEntity<TPrimaryKey> : ITrack
    {
        TPrimaryKey Id { get; set; }
    }
}
