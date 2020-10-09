using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Strong.Entities.DBModel
{
    public class TasksQz  
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsIdentity =true,IsPrimaryKey =true)]
        public int Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 任务分组
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务运行时间表达式
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string Cron { get; set; }
        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200)]
        public string AssemblyName { get; set; }
        /// <summary>
        /// 任务所在类
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 200)]
        public string ClassName { get; set; }
 
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 触发器类型（0、simple 1、cron）
        /// </summary>
        public int TriggerType { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        public int IntervalSecond { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; } = false;

        [SugarColumn(IsNullable = true)]
        /// <summary>
        /// 执行传参
        /// </summary>
        public string JobParams { get; set; }


        public int RunTimes { get; set; }

        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
