using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Entities.DBModel
{

   public class TB_JobLog
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int L_JobId { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public decimal L_SpendTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime L_StartTime { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public bool L_Result { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string Message { get; set; }

    }
}
