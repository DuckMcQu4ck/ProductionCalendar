using ProductionCalendar.Models;
using ProductionCalendar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductionCalendar.Queries
{
    public class WorkWeekQueryBuilder
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions();
        private readonly string _token;
        private string _countryCode;
        private DateOnly? _date;
        private WeekType _weekType = WeekType.FiveDay;

        public WorkWeekQueryBuilder(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, string token, string countryCode)
        {
            _httpClient = httpClient;
            _token = token;
            _countryCode = countryCode;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        /// <summary>
        /// Устанавливает код страны, для которой выполняется запрос.
        /// </summary>
        /// <param name="countryCode">Код страны (например, "RU").</param>
        public WorkWeekQueryBuilder ForCountry(string countryCode)
        {
            _countryCode = countryCode;
            return this;
        }

        /// <summary>
        /// Устанавливает дату, для которой требуется определить рабочую неделю.
        /// </summary>
        /// <param name="date">Дата, входящая в нужную рабочую неделю.</param>
        /// <returns>Текущий экземпляр <see cref="WorkWeekQueryBuilder"/>.</returns>
        public WorkWeekQueryBuilder OnDate(DateOnly date)
        {
            _date = date;
            return this;
        }

        /// <summary>
        /// Устанавливает тип рабочей недели (пятидневка или шестидневка).
        /// </summary>
        /// <param name="weekType">Тип рабочей недели, значение перечисления <see cref="WeekType"/>.</param>
        public WorkWeekQueryBuilder WithWeekType(WeekType weekType)
        {
            _weekType = weekType;
            return this;
        }

        /// <summary>
        /// Выполняет запрос к API и возвращает объект <see cref="WorkWeek"/>, представляющий рабочую неделю,
        /// в которую входит заданная дата, с учетом выбранного типа рабочей недели.
        /// </summary>
        public async Task<WorkWeek?> GetAsync()
        {
            if (string.IsNullOrEmpty(_countryCode))
                throw new ArgumentException("Код страны не может быть пустым", nameof(_countryCode));

            if (_date == null)
                throw new InvalidOperationException("Необходимо задать дату перед вызовом GetAsync.");

            string url = StringUtils.BuildUrl(
                Endpoints.BaseUrl,
                Endpoints.GetWorkWeek,
                _token,
                _countryCode,
                StringUtils.FormatDay(_date.Value)
            );

            url = StringUtils.AppendParams(
                url,
                ("week_type", ((int)_weekType).ToString())
            );

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Ошибка при запросе к API: {response.StatusCode} {response.ReasonPhrase}");

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                throw new HttpRequestException("Ответ от API пустой или некорректный.");

            return JsonSerializer.Deserialize<WorkWeek>(content, _jsonSerializerOptions);
        }

    }
}
