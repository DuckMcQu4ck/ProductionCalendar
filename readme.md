
# ProductionCalendar Library

Библиотека **ProductionCalendar** позволяет интегрировать данные производственного календаря в ваше приложение, предоставляя удобный Fluent API для получения информации о рабочих и нерабочих днях, праздничных, сокращённых рабочих днях и статистике периода, а также информации о рабочей неделе.

---

## Конфигурация библиотеки

**Стек технологий и необходимые пакеты:**
- **.NET 5/6/7** – для кроссплатформенной разработки.
- **HttpClient** – для выполнения HTTP-запросов.
- **System.Text.Json** – для сериализации и десериализации JSON.


---

## Получение токена

Получите токен доступа на [https://production-calendar.ru/token](https://production-calendar.ru/token).

---

## Работа с библиотекой

### 1. Конфигурация календаря

Для инициализации календаря используется класс **ProductionCalendarBuilder**. Пример настройки:

```csharp
// Создаём HttpClient вручную
var httpClient = new HttpClient();

// Указываем токен API
var token = "TOKEN";

// Инициализируем календарь
var calendar = new ProductionCalendarBuilder()
                    .WithToken(token)
                    .WithHttpClient(httpClient)
                    .ForCountry("RU")
                    .Build();
```

> **Примечание:** Замените `"TOKEN"` на полученный токен, а `"RU"` на нужный код страны, если требуется.

### 2. Получение периода

#### 2.1. Получение периода с указанием месяца, для шестидневной рабочей недели и в компактном виде

```csharp
var period = await calendar.Period()
                           .ForMonth(2025, 3)                   // Задаём период: март 2025
                           .WithWeekType(WeekType.SixDay)         // Используем шестидневную рабочую неделю
                           .IncludeWschDays()                   // Включаем "WSCH-дни" (выходные с сохранением зарплаты)
                           .WithCompactView()                   // Включаем компактный вид (только особые дни)
                           .GetAsListAsync();
```

Метод `GetAsListAsync` возвращает данные периода, десериализованные в виде коллекции (`ICollection<Day>`).

#### 2.2. Получение периода в виде Dictionary и доступ к дню по его ключу

```csharp
var periodDictionary = await calendar.Period()
                           .ForMonth(2025, 3)
                           .WithWeekType(WeekType.SixDay)
                           .IncludeWschDays()
                           .WithCompactView()
                           .GetAsDictionaryAsync();

// Пример доступа ко дню по ключу (например, "01.03.2025")
if (periodDictionary.Days.TryGetValue("01.03.2025", out Day day))
{
    Console.WriteLine($"День: {day.Date}, Тип: {day.TypeText}");
}
```

Метод `GetAsDictionaryAsync` возвращает данные периода, где коллекция дней представлена в виде словаря с ключом типа `string`.

### 3. Получение информации о рабочей неделе

Для получения информации о рабочей неделе используется класс **WorkWeekQueryBuilder**. Пример запроса:

```csharp
var workWeek = await calendar.WorkWeek()
                             .OnDate(new DateOnly(2024, 5, 1))  // Задаём дату для определения рабочей недели
                             .WithWeekType(WeekType.FiveDay)      // Указываем пятидневную рабочую неделю
                             .GetAsync();

Console.WriteLine(JsonSerializer.Serialize(workWeek, new JsonSerializerOptions { WriteIndented = true }));
```

Метод `GetAsync` возвращает объект `WorkWeek`, содержащий информацию о начале и окончании рабочей недели, а также тип недели.

---

## Пример использования

Ниже приведён полный пример консольного приложения, демонстрирующий работу с библиотекой:

```csharp
using ProductionCalendar;
using ProductionCalendar.Models;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

Console.OutputEncoding = Encoding.UTF8;

// Создаём HttpClient вручную
var httpClient = new HttpClient();
// Указываем токен API
var token = "TOKEN";

// Настройка параметров сериализации JSON
var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    WriteIndented = true
};

// Инициализируем календарь с помощью Fluent API
var calendar = new ProductionCalendarBuilder()
                    .WithToken(token)
                    .WithHttpClient(httpClient)
                    .ForCountry("RU")
                    .Build();

// Получение и вывод информации о периоде
var period = await calendar.Period()
                           .ForMonth(2025, 3)
                           .WithWeekType(WeekType.SixDay)
                           .IncludeWschDays()
                           .WithCompactView()
                           .GetAsListAsync();

Console.WriteLine("Период:");
Console.WriteLine(JsonSerializer.Serialize(period, options));

// Получение периода в виде словаря
var periodDictionary = await calendar.Period()
                           .ForMonth(2025, 3)
                           .WithWeekType(WeekType.SixDay)
                           .IncludeWschDays()
                           .WithCompactView()
                           .GetAsDictionaryAsync();

Console.WriteLine("Период в виде словаря:");
Console.WriteLine(JsonSerializer.Serialize(periodDictionary, options));

// Получение и вывод информации о рабочей неделе
var workWeek = await calendar.WorkWeek()
                             .OnDate(new DateOnly(2024, 5, 1))
                             .WithWeekType(WeekType.FiveDay)
                             .GetAsync();

Console.WriteLine("\nРабочая неделя:");
Console.WriteLine(JsonSerializer.Serialize(workWeek, options));
```

