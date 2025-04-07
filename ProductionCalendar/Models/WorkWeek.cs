using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductionCalendar.Models
{
    public class WorkWeek
    {
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        [JsonPropertyName("country_text")]
        public string CountryName { get; set; } = string.Empty;

        [JsonPropertyName("work_week_type")]
        public string WorkWeekType { get; set; } = string.Empty;

        [JsonPropertyName("work_week_start")]
        public DateOnly WorkWeekStart { get; set; }
        
        [JsonPropertyName("work_week_end")]
        public DateOnly WorkWeekEnd { get; set; }
    }
}
