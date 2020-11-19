using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models
{
    public  interface IEntity: IEntity<long>
    {

    }
    public interface IEntity<TPrimaryKey> : ITrack
    {
        TPrimaryKey Id { get; set; }
    }
}
