using SqlSugar;
using System;
using System.Collections.Generic;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_Role", "角色表")]
    public partial class TbRole
    {
        [SugarColumn(IsPrimaryKey = true,IsIdentity =true, ColumnDescription = "主键ID")]
        public int Roleid { get; set; }
        [SugarColumn( ColumnDescription = "角色名")]
        public string Rname { get; set; }
        [SugarColumn(ColumnDescription = "备注")]
        public string Memo { get; set; }
        [SugarColumn(ColumnDescription = "操作人")]
        public int? Createuid { get; set; }
        [SugarColumn(ColumnDescription = "添加时间",DefaultValue ="getdate()", IsOnlyIgnoreInsert = true)]
        public DateTime Addtime { get; set; }
        [SugarColumn(ColumnDescription = "排序")]
        public int? Sort { get; set; }
        [SugarColumn(ColumnDescription = "角色身份")]
        public string RoleIdentity { get; set; }
    }
}
