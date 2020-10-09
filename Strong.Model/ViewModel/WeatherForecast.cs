using System;

namespace Strong.Model.ViewModel
{
    /// <summary>
    /// 天气预报DTO1
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public int Temperature { get; set; }
        /// <summary>
        /// 总结
        /// </summary>
        public string Summary { get; set; }
    }
}
