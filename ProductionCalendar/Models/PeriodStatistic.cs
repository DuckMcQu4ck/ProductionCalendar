using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductionCalendar.Models
{
    public class PeriodStatistic
    {
        /// <summary>
        /// Количество календарных дней в периоде
        /// </summary>
        [JsonPropertyName("calendar_days")]
        public int CalendarDays { get; set; }

        /// <summary>
        /// Количество календарных дней в периоде без учета праздничных дней
        /// (полезный показатель для расчета продолжительности отпуска работника)
        /// </summary>
        [JsonPropertyName("calendar_days_without_holidays")]
        public int CalendarDaysWithoutHolidays { get; set; }

        /// <summary>
        /// Количество рабочих дней в периоде
        /// </summary>
        [JsonPropertyName("work_days")]
        public int WorkDays { get; set; }

        /// <summary>
        /// Количество выходных дней в периоде (без учета праздничных)
        /// </summary>
        [JsonPropertyName("weekends")]
        public int Weekends { get; set; }

        /// <summary>
        /// Количество праздничных дней в периоде
        /// </summary>
        [JsonPropertyName("holidays")]
        public int Holidays { get; set; }

        /// <summary>
        /// Количество сокращенных рабочих дней в периоде
        /// </summary>
        [JsonPropertyName("shortened_working_days")]
        public int ShortendWorkingDays { get; set; }

        /// <summary>
        /// Количество рабочего времени за период
        /// </summary>
        [JsonPropertyName("working_hours")]
        public int WorkingHours { get; set; }
    }
}
