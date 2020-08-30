using SqlSugar;
using System;
using System.Collections.Generic;

namespace Strong.Entities
{
    [SugarTable("Tb_Role_Log", "角色日志表")]
    public partial class TbRoleLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键ID")]
        public int Id { get; set; }
        [SugarColumn(ColumnDescription = "表名")]
        public string OType { get; set; }
        [SugarColumn(ColumnDescription = "状态")]
        public string Status { get; set; }
        [SugarColumn(ColumnDescription = "操作记录")]
        public decimal? Oid { get; set; }
        [SugarColumn(ColumnDescription = "备注")]
        public string Remark { get; set; }
        [SugarColumn(ColumnDescription = "修改前嘻嘻")]
        public string BeforeContent { get; set; }
        [SugarColumn(ColumnDescription = "修改信息")]
        public string Content { get; set; }
        [SugarColumn(ColumnDescription = "用于执行的SQL语句")]
        public string Savesql { get; set; }
        [SugarColumn(ColumnDescription = "sql备注")]
        public string Sqlremark { get; set; }
        [SugarColumn(ColumnDescription = "操作日期",DefaultValue ="getdate()", IsOnlyIgnoreInsert = true)]
        public DateTime? OperateDate { get; set; }
    }
}
