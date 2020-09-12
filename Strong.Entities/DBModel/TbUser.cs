using SqlSugar;
using System;
using System.Collections.Generic;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_User", "角色菜单表")]
    public partial class TbUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键")]
        public int Userid { get; set; }
        [SugarColumn(ColumnDescription = "登录名")]
        public string Loginname { get; set; }
        [SugarColumn(ColumnDescription = "密码")]
        public string Pwd { get; set; }
        [SugarColumn(ColumnDescription = "真实姓名")]
        public string Realname { get; set; }
        [SugarColumn(ColumnDescription = "性别")]
        public string Sex { get; set; }
        [SugarColumn(ColumnDescription = "电话号码")]
        public string Tel { get; set; }
        [SugarColumn(ColumnDescription = "所属机构")]
        public string Unit { get; set; }
        [SugarColumn(ColumnDescription = "职务")]
        public string Duty { get; set; }
        [SugarColumn(ColumnDescription = "电子邮件")]
        public string Email { get; set; }
        [SugarColumn(ColumnDescription = "是否超管",DefaultValue ="0")]
        public string Issysadmin { get; set; }
        [SugarColumn(ColumnDescription = "备注")]
        public string Memo { get; set; }
        [SugarColumn(ColumnDescription = "创建人")]
        public int? Createuid { get; set; }
        [SugarColumn(ColumnDescription = "添加时间", DefaultValue = "getdate()", IsOnlyIgnoreInsert = true)]
        public DateTime? Addtime { get; set; }
        [SugarColumn(ColumnDescription = "排序")]
        public int? Sort { get; set; }
        [SugarColumn(ColumnDescription = "机构编码")]
        public int? Unitid { get; set; }
        //[SugarColumn(ColumnDescription = "接口调用次数")]
        //public int? ResponseNumber { get; set; }
        public string Bhavelog { get; set; }
    }
}
