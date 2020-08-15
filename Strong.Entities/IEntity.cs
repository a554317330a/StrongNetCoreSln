using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
