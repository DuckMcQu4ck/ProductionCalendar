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
    public class PeriodQueryBuilder
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions();
        private readonly string _token;
        private readonly PeriodQueryParams _queryParams = new();

        private string _periodSpecifier = string.Empty;
        private string _countryCode = string.Empty;
        private bool _asDictionary = false;

        private void EnsurePeriodNotSet()
        {
            if (!string.IsNullOrEmpty(_periodSpecifier))
                throw new InvalidOperationException("Период уже задан. Нельзя переопределять период повторно.");
        }


        public PeriodQueryBuilder(HttpClient client, JsonSerializerOptions jsonSerializerOptions, string token, string countryCode = "")
        {
            _httpClient = client;
            _token = token;
            _countryCode = countryCode;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        /// <summary>
        /// Устанавливает код страны, для которой будет выполняться запрос.
        /// </summary>
        /// <param name="countryCode">Код страны (например, "RU").</param>
        public PeriodQueryBuilder ForCountry(string countryCode)
        {
            _countryCode = countryCode;
            return this;
        }

        /// <summary>
        /// Устанавливает период запроса на конкретный год.
        /// </summary>
        /// <param name="year">Год (например, 2025).</param>
        public PeriodQueryBuilder ForYear(int year)
        {
            EnsurePeriodNotSet();
            _periodSpecifier = year.ToString();
            return this;
        }

        /// <summary>
        /// Устанавливает период запроса на конкретный квартал указанного года.
        /// </summary>
        /// <param name="year">Год (например, 2025).</param>
        /// <param name="quarter">Номер квартала (от 1 до 4).</param>
        public PeriodQueryBuilder ForQuarter(int year, int quarter)
        {
            EnsurePeriodNotSet();
            if(quarter < 1 || quarter > 4)
                throw new Exception("Номер квартала не может быть меньше 1 и больше 4.")
            _periodSpecifier = $"Q{quarter}{year}";
            return this;
        }

        /// <summary>
        /// Устанавливает период запроса на конкретный месяц указанного года.
        /// </summary>
        /// <param name="year">Год (например, 2025).</param>
        /// <param name="month">Месяц (от 1 до 12).</param>
        public PeriodQueryBuilder ForMonth(int year, int month)
        {
            EnsurePeriodNotSet();
            if (month < 1 || month > 12)
                throw new Exception("Месяц должен быть в промежутке от 1 и до 12.");
            _periodSpecifier = $"{month:D2}.{year}";
            return this;
        }

        /// <summary>
        /// Устанавливает произвольный диапазон дат для запроса.
        /// </summary>
        /// <param name="from">Дата начала периода.</param>
        /// <param name="to">Дата окончания периода.</param>
        public PeriodQueryBuilder ForDateRange(DateOnly from, DateOnly to)
        {
            EnsurePeriodNotSet();
            _periodSpecifier = $"{from:dd.MM.yyyy}-{to:dd.MM.yyyy}";
            return this;
        }

        /// <summary>
        /// Устанавливает регион РФ для получения регионального производственного календаря.
        /// </summary>
        /// <param name="region">Код региона (ГИБДД, однозначный или двузначный).</param>
        public PeriodQueryBuilder WithRegion(int region)
        {
            _queryParams.Region = region;
            return this;
        }

        /// <summary>
        /// Включает компактный вид — возвращаются только особые дни (отличающиеся от стандартного календаря).
        /// </summary>
        public PeriodQueryBuilder WithCompactView()
        {
            _queryParams.IsCompact = true;
            return this;
        }

        /// <summary>
        /// Устанавливает тип рабочей недели: пятидневная или шестидневная.
        /// </summary>
        /// <param name="weekType">Тип недели (5 или 6 рабочих дней).</param>
        public PeriodQueryBuilder WithWeekType(WeekType weekType)
        {
            _queryParams.WeekType = weekType;
            return this;
        }

        /// <summary>
        /// Включает в ответ так называемые WSCH-дни (выходные с сохранением заработной платы, актуальные с 2020 г.).
        /// </summary>
        public PeriodQueryBuilder IncludeWschDays()
        {
            _queryParams.IncludeWschDays = true;
            return this;
        }

        private PeriodQueryBuilder AsDictionary()
        {
            _queryParams.AsDictionary = true;
            return this;
        }


        /// <summary>
        /// Выполняет запрос и возвращает результат в виде <see cref="PeriodCollection"/>, 
        /// где коллекция дней представлена как <see cref="ICollection{T}"/>.
        /// </summary>
        public async Task<PeriodCollection?> GetAsListAsync()
        {
            if (string.IsNullOrEmpty(_countryCode))
                throw new ArgumentException("Код страны не может быть пустым", nameof(_countryCode));

            if (string.IsNullOrEmpty(_periodSpecifier))
                throw new InvalidOperationException("Необходимо задать период перед вызовом GetAsListAsync.");

            string url = StringUtils.BuildUrl(
                Endpoints.BaseUrl,
                Endpoints.GetPeriod,
                _token,
                _countryCode,
                _periodSpecifier
            );

            if (_queryParams.GetParams().Count != 0)
            {
                url = StringUtils.AppendParams(
                    url,
                    _queryParams.GetParams().Select(kv => (kv.Key, kv.Value)).ToArray()
                );
            }

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Ошибка при запросе к API: {response.StatusCode} {response.ReasonPhrase}");

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
                throw new HttpRequestException("Ответ от API пустой или некорректный.");

            return JsonSerializer.Deserialize<PeriodCollection>(content, _jsonSerializerOptions);
        }

        /// <summary>
        /// Выполняет запрос и возвращает результат в виде <see cref="Period{TDays}"/>, 
        /// где коллекция дней представлена как <see cref="Dictionary{String, Day}"/>.
        /// </summary>
        public async Task<Period<Dictionary<string, Day>>> GetAsDictionaryAsync()
        {
            if (string.IsNullOrEmpty(_countryCode))
                throw new ArgumentException("Код страны не может быть пустым", nameof(_countryCode));

            if (string.IsNullOrEmpty(_periodSpecifier))
                throw new InvalidOperationException("Необходимо задать период перед вызовом GetAsDictionaryAsync.");

            string url = StringUtils.BuildUrl(
                Endpoints.BaseUrl,
                Endpoints.GetPeriod,
                _token,
                _countryCode,
                _periodSpecifier
            );

            this.AsDictionary();
            if (_queryParams.GetParams().Count != 0)
            {
                url = StringUtils.AppendParams(
                    url,
                    _queryParams.GetParams().Select(kv => (kv.Key, kv.Value)).ToArray()
                );
            }

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Ошибка при запросе к API: {response.StatusCode} {response.ReasonPhrase}");

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
                throw new HttpRequestException("Ответ от API пустой или некорректный.");

            return JsonSerializer.Deserialize<Period<Dictionary<string, Day>>>(content, _jsonSerializerOptions);
        }
    }
}
