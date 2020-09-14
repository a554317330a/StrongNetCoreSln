using SqlSugar;
using System;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_User", "角色菜单表")]
    public partial class TB_User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键")]
        public int Userid { get; set; }
        [SugarColumn(ColumnDescription = "登录名")]
        public string Loginname { get; set; }
        [SugarColumn(ColumnDescription = "密码")]
        public string Pwd { get; set; }
        [SugarColumn(ColumnDescription = "真实姓名")]
        public string Realname { get; set; }
        [SugarColumn(ColumnDescription = "性别", IsNullable = true)]
        public string Sex { get; set; }
        [SugarColumn(ColumnDescription = "电话号码", IsNullable = true)]
        public string Tel { get; set; }
        [SugarColumn(ColumnDescription = "所属机构", IsNullable = true)]
        public string Unit { get; set; }
        [SugarColumn(ColumnDescription = "职务",IsNullable =true)]
        public string Duty { get; set; }
        [SugarColumn(ColumnDescription = "电子邮件", IsNullable = true)]
        public string Email { get; set; }
        [SugarColumn(ColumnDescription = "是否超管", DefaultValue = "0")]
        public string Issysadmin { get; set; }
        [SugarColumn(ColumnDescription = "备注", IsNullable = true)]
        public string Memo { get; set; }
        [SugarColumn(ColumnDescription = "创建人", IsNullable = true)]
        public int? Createuid { get; set; }
        [SugarColumn(ColumnDescription = "添加时间", IsNullable = true, IsOnlyIgnoreInsert = true)]
        public DateTime? Addtime { get; set; } = DateTime.Now;
        [SugarColumn(ColumnDescription = "排序", IsNullable = true)]
        public int? Sort { get; set; }
        [SugarColumn(ColumnDescription = "机构编码", IsNullable = true)]
        public int? Unitid { get; set; }
        //[SugarColumn(ColumnDescription = "接口调用次数")]
        //public int? ResponseNumber { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Bhavelog { get; set; }
    }
}
