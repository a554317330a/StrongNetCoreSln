using SqlSugar;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_User_Role", "角色操作记录表")]
    public partial class TB_User_Role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键")]
        public int Id { get; set; }
        [SugarColumn(ColumnDescription = "用户ID")]
        public int? Userid { get; set; }
        [SugarColumn(ColumnDescription = "角色ID")]
        public int? Roleid { get; set; }
    }
}
