using SqlSugar;
using System;
using System.Collections.Generic;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_Role", "角色操作记录表")]
    public partial class Tb_Role_Log_Process
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity =true, ColumnDescription = "主键")]
        public int Id { get; set; }
        [SugarColumn(ColumnDescription = "对应TB_ROLE_LOG_ID")]
        public decimal? Lid { get; set; }
        [SugarColumn(ColumnDescription = "操作人")]
        public string Oman { get; set; }
        [SugarColumn(ColumnDescription = "操作时间",DefaultValue ="getdate()", IsOnlyIgnoreInsert = true)]
        public DateTime Otime { get; set; }
        [SugarColumn(ColumnDescription = "备注")]
        public string Remark { get; set; }
    }
}
