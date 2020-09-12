using SqlSugar;
using System;
using System.Collections.Generic;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_Menu","菜单表")]
    public partial class TbMenu
    {
        [SugarColumn(IsPrimaryKey =true,ColumnDescription ="菜单主键，简称")]
        public string Mflag { get; set; }
        [SugarColumn(ColumnDescription ="名称")]
        public string Mname { get; set; }
        [SugarColumn(ColumnDescription = "地址")]
        public string Murl { get; set; }
        [SugarColumn(ColumnDescription = "菜单等级")]
        public int? Mlevel { get; set; }
        [SugarColumn(ColumnDescription = "排序")]
        public int? Sort { get; set; }
        [SugarColumn(ColumnDescription = "是否启用")]
        public string Mvisible { get; set; }
        [SugarColumn(ColumnDescription = "图标地址")]
        public string Ico { get; set; }
        [SugarColumn(IsIdentity =true,ColumnDescription = "ID")]
        public int Id { get; set; }
        [SugarColumn(ColumnDescription = "父ID")]
        public int? Mparent { get; set; }
    }
}
