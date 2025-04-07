using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductionCalendar.Models
{
    public class Day
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Тип суток
        /// </summary>
        [JsonPropertyName("type_id")]
        public DayType Type { get; set; }   

        /// <summary>
        /// Текстовое представление типа суток
        /// </summary>
        [JsonPropertyName("type_text")]
        public string TypeText { get; set; } = string.Empty;

        /// <summary>
        /// Cокращенное наименование дня недели
        /// </summary>
        [JsonPropertyName("week_day")]
        public string WeekDay { get; set; } = string.Empty;

        /// <summary>
        /// Количество рабочих часов в данных сутках для 40-й часовой рабочей недели
        /// </summary>
        [JsonPropertyName("working_hours")]
        public int WorkingHours { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("is_project")]
        public bool IsProject { get; set; }

        [JsonPropertyName("is_wsch")]
        public bool IsWsch {  get; set; }
    }
}
