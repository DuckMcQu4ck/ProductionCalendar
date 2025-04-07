using ProductionCalendar.Models;
using ProductionCalendar.Queries;
using ProductionCalendar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ProductionCalendar
{
    public class ProductionCalendar
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly string _countryCode;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
            Converters =
            {
                new DateOnlyJsonConverter("dd.MM.yyyy")
            }
        };

        /// <summary>
        /// Создает новый экземпляр класса <see cref="ProductionCalendar"/>, предоставляющего API-доступ к данным производственного календаря.
        /// </summary>
        /// <param name="client">HTTP-клиент для выполнения запросов.</param>
        /// <param name="token">Токен авторизации для доступа к API.</param>
        /// <param name="countryCode">Код страны по умолчанию (например, "RU").</param>
        public ProductionCalendar(HttpClient client, string token, string countryCode)
        {
            _httpClient = client;
            _token = token;
            _countryCode = countryCode;
            _httpClient.BaseAddress = new Uri(Endpoints.BaseUrl);
        }

        /// <summary>
        /// Инициализирует построитель запросов для получения данных по производственному периоду
        /// (дни, праздники, рабочие часы и т.п.).
        /// </summary>
        /// <returns>Экземпляр <see cref="PeriodQueryBuilder"/> для конфигурации и выполнения запроса.</returns>
        public PeriodQueryBuilder Period() => new PeriodQueryBuilder(_httpClient, options, _token, _countryCode);

        /// <summary>
        /// Инициализирует построитель запросов для получения информации о рабочей неделе,
        /// соответствующей определённой дате.
        /// </summary>
        /// <returns>Экземпляр <see cref="WorkWeekQueryBuilder"/> для конфигурации и выполнения запроса.</returns>
        public WorkWeekQueryBuilder WorkWeek() => new WorkWeekQueryBuilder(_httpClient, options, _token, _countryCode);
    }
}