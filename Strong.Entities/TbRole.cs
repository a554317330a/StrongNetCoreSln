using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    public partial class TbRole
    {
        public int Roleid { get; set; }
        public string Rname { get; set; }
        public string Memo { get; set; }
        public int? Createuid { get; set; }
        public DateTime? Addtime { get; set; }
        public int? Sort { get; set; }
        public string Identity { get; set; }
    }
}
