using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    public partial class TbRoleLog
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal? Oid { get; set; }
        public string Remark { get; set; }
        public string BeforeContent { get; set; }
        public string Content { get; set; }
        public string Savesql { get; set; }
        public string Sqlremark { get; set; }
        public DateTime? OperateDate { get; set; }
    }
}
