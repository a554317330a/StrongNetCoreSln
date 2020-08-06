using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    public partial class TbUser
    {
        public int Userid { get; set; }
        public string Loginname { get; set; }
        public string Pwd { get; set; }
        public string Realname { get; set; }
        public string Town { get; set; }
        public string Sex { get; set; }
        public string Tel { get; set; }
        public string Unit { get; set; }
        public string Duty { get; set; }
        public string Email { get; set; }
        public string Issysadmin { get; set; }
        public string Memo { get; set; }
        public int? Createuid { get; set; }
        public DateTime? Addtime { get; set; }
        public int? Sort { get; set; }
        public int? Unitid { get; set; }
        public int? ResponseNumber { get; set; }
        public string Bhavelog { get; set; }
    }
}
