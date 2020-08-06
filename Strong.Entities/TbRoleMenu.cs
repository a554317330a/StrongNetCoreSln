using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    public partial class TbRoleMenu
    {
        public int Id { get; set; }
        public int? Roleid { get; set; }
        public string Mflag { get; set; }
    }
}
