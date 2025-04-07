using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionCalendar.Models
{
    /// <summary>
    /// Тип суток
    /// </summary>
    public enum DayType
    {
        /// <summary>
        /// Рабочий день
        /// </summary>
        WorkingDay = 1,

        /// <summary>
        /// Выходной день
        /// </summary>
        DayOff = 2,

        /// <summary>
        /// Государственный праздник
        /// </summary>
        PublicHoliday = 3,

        /// <summary>
        /// Региональный праздник
        /// </summary>
        RegionalHoliday = 4,

        /// <summary>
        /// Предпраздничный сокращенный рабочий день
        /// </summary>
        ShortendWorkingDay = 5,

        /// <summary>
        /// Дополнительный / перенесенный выходной день
        /// </summary>
        AdditionalDayOff = 6,
    }
}
