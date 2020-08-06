using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    public partial class TbUserRole
    {
        public int Id { get; set; }
        public int? Userid { get; set; }
        public int? Roleid { get; set; }
    }
}
