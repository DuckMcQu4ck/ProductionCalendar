using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductionCalendar.Models
{
    /// <summary>
    /// Представляет временной период с детальной статистикой и информацией о рабочих днях.
    /// </summary>
    public class Period<TDays>
    {
        /// <summary>
        /// Код страны (ISO или иной формат).
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Название страны (полное или сокращённое).
        /// </summary>
        [JsonPropertyName("country_text")]
        public string CountryName { get; set; } = string.Empty;

        /// <summary>
        /// Числовой идентификатор региона внутри страны.
        /// </summary>
        [JsonPropertyName("region_id")]
        public int RegionCode { get; set; }

        /// <summary>
        /// Дата начала периода.
        /// </summary>
        [JsonPropertyName("dt_start")]
        public DateOnly Start { get; set; }

        /// <summary>
        /// Дата окончания периода.
        /// </summary>
        [JsonPropertyName("dt_end")]
        public DateOnly End { get; set; }

        /// <summary>
        /// Тип рабочей недели (пятидневная или шестидневная).
        /// </summary>
        [JsonPropertyName("work_week_type")]
        public string WorkWeekType { get; set; } = string.Empty;

        /// <summary>
        /// Тип периода (например, месяц, квартал, год, произвольный).
        /// </summary>
        [JsonPropertyName("period")]
        public string PeriodType { get; set; } = string.Empty;

        /// <summary>
        /// Статистическая информация по периоду (количество рабочих дней, выходных и т. п.).
        /// </summary>
        [JsonPropertyName("statistic")]
        public PeriodStatistic Statistic { get; set; } = new();

        /// <summary>
        /// Коллекция дней внутри периода (рабочие, праздничные, выходные и т. п.).
        /// </summary>
        [JsonPropertyName("days")]
        public TDays Days { get; set; }
    }

    public class PeriodCollection : Period<ICollection<Day>> { }
    public class PeriodDictionary : Period<Dictionary<string, Day>> { }

}
