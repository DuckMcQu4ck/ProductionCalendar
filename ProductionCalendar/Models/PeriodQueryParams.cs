using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductionCalendar.Models
{
    public class PeriodQueryParams
    {
        /// <summary>
        /// Позволяет задать регион РФ (в ряде регионов присутствуют свои региональные праздники,
        /// для которых производственный календарь отличается). В качестве номера региона задается
        /// однозначные или двузначные коды ГИБДД. Трехзначные не поддерживаются.
        /// </summary>
        public int? Region { get; set; }

        /// <summary>
        /// Если задать данному параметру значение true,
        /// то результат будет выдаваться в сокращенном формате, только особые дни,
        /// которые отличаются от обычного календаря. По умолчанию этот параметр равен false
        /// и календарь выдает все сутки заданного периода
        /// </summary>
        public bool? IsCompact { get; set; }

        /// <summary>
        /// Тип рабочей недели. По умолчанию это 5-и дневная рабочая неделя, можно задать и 6-и дневную
        /// </summary>
        public WeekType? WeekType { get; set; }

        /// <summary>
        /// Параметр показывает нужно ли учитывать так называемые нерабочие дни с сохранением
        /// заработной платы, которые начали практиковать с 2020 года (В период пандемии COVID-19).
        /// В народе эти дни прозвали "выходные дни, которые как бы есть и одновременно которых 
        /// как бы нет". wsch сокращенно от Weekends of the Schrodinger. Так называемая отсылка
        /// к всем известному коту Шредингера. По умолчанию параметр равен 0, то есть подобные
        /// выходные не учитываются.
        /// </summary>
        public bool? IncludeWschDays { get; set; }

        /// <summary>
        /// Ответ придет в формате словаря
        /// </summary>
        public bool? AsDictionary { get; set; }

        internal Dictionary<string, string> GetParams()
        {
            Dictionary<string, string> keyValues = [];

            if(Region != null) keyValues.Add("region", Region.Value.ToString());
            if(IsCompact != null) keyValues.Add("compact", IsCompact.Value.ToString());
            if(WeekType != null) keyValues.Add("week_type", ((int)WeekType.Value).ToString());
            if(IncludeWschDays != null) keyValues.Add("wsch", IncludeWschDays.Value.ToString());
            if(AsDictionary != null) keyValues.Add("days_as_object", AsDictionary.Value.ToString());

            return keyValues;
        }


    }
}
