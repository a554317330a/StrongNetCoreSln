using SqlSugar;
using System;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_Role", "角色表")]
    public partial class TB_Role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键ID")]
        public int Roleid { get; set; }
        [SugarColumn(ColumnDescription = "角色名")]
        public string Rname { get; set; }
        [SugarColumn(ColumnDescription = "备注", IsNullable = true)]
        public string Memo { get; set; }
        [SugarColumn(ColumnDescription = "操作人", IsNullable = true)]
        public int? Createuid { get; set; }
        [SugarColumn(ColumnDescription = "添加时间", IsNullable = true, IsOnlyIgnoreInsert = true)]
        public DateTime Addtime { get; set; } = DateTime.Now;
        [SugarColumn(ColumnDescription = "排序", IsNullable = true)]
        public int? Sort { get; set; }
        [SugarColumn(ColumnDescription = "角色身份", IsNullable = true)]
        public string RoleIdentity { get; set; }
    }
}
