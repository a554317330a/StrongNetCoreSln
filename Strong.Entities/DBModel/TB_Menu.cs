using SqlSugar;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_Menu", "菜单表")]
    public partial class TB_Menu
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "菜单主键，简称")]
        public string Mflag { get; set; }
        [SugarColumn(ColumnDescription = "名称")]
        public string Mname { get; set; }
        [SugarColumn(ColumnDescription = "地址", IsNullable = true)]
        public string Murl { get; set; }
        [SugarColumn(ColumnDescription = "菜单等级")]
        public int? Mlevel { get; set; }
        [SugarColumn(ColumnDescription = "排序",IsNullable =true)]
        public int? Sort { get; set; }
        [SugarColumn(ColumnDescription = "是否启用", IsNullable = true)]
        public int Mvisible { get; set; } = 1;
        [SugarColumn(ColumnDescription = "图标地址", IsNullable = true)]
        public string Ico { get; set; }
        [SugarColumn(IsIdentity = true, ColumnDescription = "ID")]
        public int Id { get; set; }
        [SugarColumn(ColumnDescription = "父ID")]
        public int? Mparent { get; set; }
    }
}
