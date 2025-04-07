using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionCalendar
{
    /// <summary>
    /// Предоставляет возможность построить и настроить объект ProductionCalendar
    /// с использованием Fluent-интерфейса.
    /// </summary>
    public class ProductionCalendarBuilder
    {
        private string _token;
        private HttpClient _httpClient;
        private string _countryCode;

        /// <summary>
        /// Устанавливает токен авторизации для API ProductionCalendar.
        /// </summary>
        /// <param name="token">Токен авторизации.</param>
        public ProductionCalendarBuilder WithToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token не может быть пустым или null", nameof(token));

            _token = token;
            return this;
        }

        /// <summary>
        /// Устанавливает экземпляр HttpClient для выполнения запросов.
        /// </summary>
        /// <param name="client">Настроенный экземпляр HttpClient.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductionCalendarBuilder WithHttpClient(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client), "HttpClient не может быть null");
            return this;
        }

        /// <summary>
        /// Устанавливает код страны, для которой будет использоваться календарь по умолчанию.
        /// </summary>
        /// <param name="countryCode">ISO-код страны (например, RU, KZ и т.п.).</param>
        public ProductionCalendarBuilder ForCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                throw new ArgumentException("Код страны не может быть пустым или null", nameof(countryCode));

            _countryCode = countryCode;
            return this;
        }

        /// <summary>
        /// Создает настроенный экземпляр ProductionCalendar.
        /// </summary>
        /// <exception cref="InvalidOperationException">Выбрасывается, если не заданы обязательные параметры.</exception>
        public ProductionCalendar Build()
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("Не задан Token. Используйте метод WithToken.");

            if (_httpClient is null)
                throw new InvalidOperationException("HttpClient должен быть задан явно через метод WithHttpClient.");

            return new ProductionCalendar(_httpClient, _token, _countryCode);
        }
    }
}
