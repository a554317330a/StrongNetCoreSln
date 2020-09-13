using SqlSugar;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_Role_Menu", "角色菜单表")]
    public partial class TB_Role_Menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键")]
        public int Id { get; set; }
        [SugarColumn(ColumnDescription = "角色ID")]
        public int? Roleid { get; set; }
        [SugarColumn(ColumnDescription = "菜单名称")]
        public string Mflag { get; set; }
    }
}
