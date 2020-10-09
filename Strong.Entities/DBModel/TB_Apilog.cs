using SqlSugar;
using System;

namespace Strong.Entities.DBModel
{
    [SugarTable("TB_ApiLog", "调用日志表")]
    public class TB_Apilog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键ID")]
        public int Logid { get; set; }

        [SugarColumn(ColumnDescription = "错误类型,1:正常,2:调用错误,3:逻辑漏洞")]
        public int? Errortype { get; set; }

        [SugarColumn(ColumnDescription = "错误信息",ColumnDataType ="Text",IsNullable =true)]
        public string Errormsg { get; set; }

        [SugarColumn(ColumnDescription = "用户ID")]
        public int? Userid { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime? Createtime { get; set; }

        [SugarColumn(ColumnDescription = "接口名称")]
        public string Apiname { get; set; }

        [SugarColumn(ColumnDescription = "过程耗时", IsNullable = true)]
        public DateTime? Returntime { get; set; }

        [SugarColumn(ColumnDescription = "参数", ColumnDataType = "Text", IsNullable = true)]
        public string Params { get; set; }

        [SugarColumn(ColumnDescription = "父ID，用于错误类型2,3", IsNullable = true)]
        public int? Parentid { get; set; }

        [SugarColumn(ColumnDescription = "请求ID",IsNullable =true)]
        public string Requestid { get; set; }

        [SugarColumn(ColumnDescription = "请求时长", IsNullable = true)]
        public decimal? Actiontime { get; set; }
    }
}
