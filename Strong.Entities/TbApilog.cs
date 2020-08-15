using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    public partial class TbApilog
    {
        public int Logid { get; set; }
        public int? Errortype { get; set; }
        public string Errormsg { get; set; }
        public int? Userid { get; set; }
        public DateTime? Createtime { get; set; }
        public string Apiname { get; set; }
        public DateTime? Returntime { get; set; }
        public string Params { get; set; }
        public int? Parentid { get; set; }
        public string Requestid { get; set; }
        public decimal? Actiontime { get; set; }
    }
}
