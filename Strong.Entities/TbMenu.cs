using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    public partial class TbMenu
    {
        public string Mflag { get; set; }
        public string Mname { get; set; }
        public string Murl { get; set; }
        public int? Mlevel { get; set; }
        public int? Sort { get; set; }
        public string Mvisible { get; set; }
        public string Ico { get; set; }
        public decimal Id { get; set; }
        public decimal? Mparent { get; set; }
    }
}
