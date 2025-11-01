# 📚 Library Management System API

> Современная система управления библиотекой с REST API, разработанная на ASP.NET Core 8.0

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-13+-336791?logo=postgresql)](https://www.postgresql.org/)
[![Tests](https://img.shields.io/badge/Tests-70%20passed-success)](./WebApplication2.Tests)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

## 📋 Содержание

- [О проекте](#о-проекте)
- [Особенности](#особенности)
- [Технологический стек](#технологический-стек)
- [Структура проекта](#структура-проекта)
- [Установка и запуск](#установка-и-запуск)
- [API Документация](#api-документация)
- [База данных](#база-данных)
- [Тестирование](#тестирование)
- [Архитектура](#архитектура)

## 🎯 О проекте

Система управления библиотекой - это полнофункциональное веб-приложение для автоматизации работы библиотеки. Поддерживает управление книгами, пользователями, выдачу и возврат книг, формирование отчетов.

### Основные возможности

- ✅ Управление каталогом книг (CRUD операции)
- ✅ Система авторизации и аутентификации (JWT)
- ✅ **Email верификация** при регистрации и смене пароля/email
- ✅ Ролевая модель доступа (Admin, Librarian, User)
- ✅ Управление авторами, категориями и филиалами
- ✅ Система выдачи и возврата книг
- ✅ Бронирование и списки желаемого
- ✅ Генерация PDF отчетов
- ✅ Поиск и фильтрация книг
- ✅ История операций пользователей

## ✨ Особенности

### 🏗️ Архитектура

- **Clean Architecture** - разделение на слои (Controllers, Services, DTOs)
- **Dependency Injection** - инверсия зависимостей через интерфейсы
- **Repository Pattern** - абстракция работы с данными
- **SOLID принципы** - чистый, поддерживаемый код

### 🔒 Безопасность

- JWT токены для аутентификации
- **Email верификация** с 6-значными кодами (срок действия 15 минут)
- Защита смены email и пароля через верификацию
- Ролевая авторизация
- Хеширование паролей
- Защита API endpoints

### 📊 Производительность

- Асинхронные операции (async/await)
- Индексы в базе данных
- Кеширование (опционально)
- Connection pooling для БД

### 🧪 Качество кода

- **70 Unit-тестов** (xUnit, Moq, FluentAssertions)
- XML документация для IntelliSense
- Следование best practices
- 100% покрытие контроллеров

## 🛠️ Технологический стек

### Backend

- **ASP.NET Core 8.0** - Веб-фреймворк
- **C# 12** - Язык программирования
- **Dapper** - Micro ORM для работы с БД
- **PostgreSQL** - Реляционная база данных
- **JWT Bearer** - Аутентификация

### Библиотеки

- **PDFsharp & iText7** - Генерация PDF отчетов
- **Npgsql** - PostgreSQL драйвер
- **BCrypt.Net** - Хеширование паролей

### Testing

- **xUnit** - Тестовый фреймворк
- **Moq** - Мокирование зависимостей
- **FluentAssertions** - Выразительные проверки

## 📁 Структура проекта

```
BooksWebApi/
├── 📂 Controllers/           # REST API контроллеры
│   ├── 📂 Book/             # Управление книгами
│   │   ├── BookController.cs
│   │   ├── AuthorController.cs
│   │   ├── CategoryController.cs
│   │   └── BranchController.cs
│   ├── 📂 BookEvent/        # События книг
│   │   ├── BookEventController.cs
│   │   └── BookEventReportController.cs
│   └── 📂 Dashboard/        # Пользователи и авторизация
│       ├── AuthController.cs
│       └── UserController.cs
│
├── 📂 Services/             # Бизнес-логика
│   ├── 📂 Book/
│   ├── 📂 Auth/
│   ├── 📂 Author/
│   ├── 📂 Category/
│   ├── 📂 Branch/
│   ├── 📂 BookEvent/
│   ├── 📂 Dashboard/
│   └── 📂 File/
│
├── 📂 Interfaces/           # Интерфейсы сервисов
│   ├── IBookService.cs
│   ├── IAuthService.cs
│   ├── IAuthorService.cs
│   ├── ICategoryService.cs
│   ├── IBranchService.cs
│   ├── IUserService.cs
│   ├── IBookEventService.cs
│   ├── IBookEventReportService.cs
│   └── IFileService.cs
│
├── 📂 DTO/                  # Data Transfer Objects
│   ├── 📂 Book/
│   ├── 📂 Author/
│   ├── 📂 Category/
│   ├── 📂 Branch/
│   ├── 📂 BookEvent/
│   ├── 📂 Dashboard/
│   └── 📂 Auth/
│
├── 📂 Connection/           # Подключение к БД
│   └── DbConnect.cs
│
├── 📄 Program.cs            # Точка входа
└── 📄 appsettings.json      # Конфигурация

WebApplication2.Tests/
├── 📂 Controllers/          # Тесты контроллеров
│   ├── AuthControllerTests.cs        (4 теста)
│   ├── BookControllerTests.cs        (11 тестов)
│   ├── AuthorControllerTests.cs      (8 тестов)
│   ├── CategoryControllerTests.cs    (8 тестов)
│   ├── BranchControllerTests.cs      (8 тестов)
│   ├── UserControllerTests.cs        (7 тестов)
│   ├── BookEventControllerTests.cs   (19 тестов)
│   └── BookEventReportControllerTests.cs (5 тестов)
└── 📄 README.md
```

## 🚀 Установка и запуск

### Предварительные требования

- ✅ .NET 8.0 SDK или выше
- ✅ PostgreSQL 13 или выше
- ✅ Visual Studio 2022 / VS Code / Rider

### Шаг 1: Клонирование репозитория

```bash
git clone https://github.com/yourusername/BooksWebApi.git
cd BooksWebApi
```

### Шаг 2: Настройка базы данных

1. Создайте базу данных PostgreSQL:

```sql
CREATE DATABASE library_db;
```

2. Выполните SQL скрипт для создания таблиц:

```bash
psql -U postgres -d library_db -f database.sql
```

### Шаг 3: Настройка конфигурации

#### Настройка подключения к базе данных

Отредактируйте файл `WebApplication2/Connection/DbConnect.cs`:

```csharp
public class DbConnect
{
    public NpgsqlConnection GetConnection()
    {
        var connectionString = "Host=localhost;Port=5432;Database=library_db;Username=postgres;Password=yourpassword";
        return new NpgsqlConnection(connectionString);
    }
}
```

#### Настройка JWT и Email

Отредактируйте `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "your-super-secret-key-with-minimum-32-characters",
    "Issuer": "LibraryManagementSystem",
    "Audience": "LibraryManagementSystem"
  },
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "FromEmail": "your-email@gmail.com",
    "FromName": "BooksAPI",
    "EnableSsl": "true"
  }
}
```

### Шаг 4: Запуск приложения

```bash
# Восстановить зависимости
dotnet restore

# Собрать проект
dotnet build

# Запустить приложение
dotnet run --project WebApplication2

# Или в режиме watch (авто-перезагрузка)
dotnet watch run --project WebApplication2
```

Приложение будет доступно по адресу: `https://localhost:7200`

### Шаг 5: Запуск тестов

```bash
# Запустить все тесты
dotnet test

# С подробным выводом
dotnet test --verbosity detailed

# С покрытием кода
dotnet test /p:CollectCoverage=true
```

## 📡 API Документация

### Базовый URL

```
https://localhost:7200/api
```

### Аутентификация

Для защищенных endpoints требуется JWT токен в заголовке:

```http
Authorization: Bearer <your_jwt_token>
```

---

## 🔐 Authentication API

### POST `/auth/login`

Вход в систему

**Request Body:**
```json
{
  "login": "admin",
  "password": "admin123"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "role": "Admin",
  "userId": 1,
  "fullName": "Администратор"
}
```

---

## 📚 Books API

### GET `/book/list`

Получить список всех книг

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "Война и мир",
    "description": "Роман-эпопея",
    "fragment": "Ну, князь, Генуя и Лукка...",
    "cover_link": "/covers/war_peace.jpg",
    "author_id": 2,
    "author_name": "Лев Толстой",
    "category_id": 1,
    "category_name": "Классическая литература",
    "branch_id": 1,
    "branch_name": "Центральная библиотека",
    "publish_year": 1869,
    "quantity": 3,
    "available_quantity": 2
  }
]
```

### GET `/book/{id}/detail`

Получить детальную информацию о книге

**Parameters:**
- `id` (integer) - ID книги

**Response:** `200 OK`
```json
{
  "id": 1,
  "title": "Война и мир",
  "description": "Роман-эпопея Льва Толстого...",
  "fragment": "Ну, князь, Генуя и Лукка...",
  "cover_link": "/covers/war_peace.jpg",
  "author_id": 2,
  "author_name": "Лев Толстой",
  "category_id": 1,
  "category_name": "Классическая литература",
  "branch_id": 1,
  "branch_name": "Центральная библиотека",
  "publish_year": 1869,
  "isbn": "978-5-17-123456-2",
  "quantity": 3,
  "available_quantity": 2
}
```

### POST `/book/add`

Добавить новую книгу (требуется роль Admin)

**Request Body:**
```json
{
  "title": "Преступление и наказание",
  "description": "Роман Федора Достоевского",
  "fragment": "В начале июля, в чрезвычайно жаркое время...",
  "cover": <IFormFile>,
  "authorId": 3,
  "categoryId": 1,
  "branchId": 2,
  "publishYear": 1866,
  "isbn": "978-5-17-123456-3"
}
```

**Response:** `200 OK`
```json
{
  "message": "Book added successfully",
  "bookId": 7
}
```

### PUT `/book/edit`

Редактировать книгу (требуется роль Admin)

**Request Body:**
```json
{
  "id": 1,
  "title": "Война и мир (обновленное издание)",
  "description": "Обновленное описание...",
  "fragment": "Обновленный фрагмент...",
  "coverLink": "/covers/war_peace_new.jpg",
  "authorId": 2,
  "categoryId": 1,
  "branchId": 1
}
```

**Response:** `200 OK`

### DELETE `/book/{id}`

Удалить книгу (требуется роль Admin)

**Parameters:**
- `id` (integer) - ID книги

**Response:** `200 OK`

### GET `/book/search`

Поиск книг

**Query Parameters:**
- `searchTerm` (string, optional) - Поисковый запрос
- `categoryId` (integer, optional) - ID категории
- `authorId` (integer, optional) - ID автора
- `branchId` (integer, optional) - ID филиала

**Example:**
```
GET /book/search?searchTerm=война&categoryId=1
```

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "Война и мир",
    "author_name": "Лев Толстой",
    "category_name": "Классическая литература"
  }
]
```

---

## 👤 Authors API

### GET `/author`

Получить список всех авторов

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "full_name": "Александр Пушкин"
  },
  {
    "id": 2,
    "full_name": "Лев Толстой"
  }
]
```

### GET `/author/{id}`

Получить автора по ID

### POST `/author`

Создать автора (Admin)

**Request Body:**
```json
{
  "full_name": "Антон Чехов"
}
```

### PUT `/author/{id}`

Обновить автора (Admin)

### DELETE `/author/{id}`

Удалить автора (Admin)

---

## 📑 Categories API

### GET `/category`

Получить все категории

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "Классическая литература"
  },
  {
    "id": 2,
    "name": "Романы"
  }
]
```

### GET `/category/{id}`

Получить категорию по ID

### POST `/category`

Создать категорию (Admin)

### PUT `/category/{id}`

Обновить категорию (Admin)

### DELETE `/category/{id}`

Удалить категорию (Admin)

---

## 🏢 Branches API

### GET `/branch`

Получить все филиалы

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "Центральная библиотека"
  }
]
```

### GET `/branch/{id}`

Получить филиал по ID

### POST `/branch`

Создать филиал (Admin)

### PUT `/branch/{id}`

Обновить филиал (Admin)

### DELETE `/branch/{id}`

Удалить филиал (Admin)

---

## 📖 Book Events API

### GET `/book-event/list`

Получить все события книг

### GET `/book-event/active-requests`

Получить активные запросы на книги (Admin, Librarian)

**Response:** `200 OK`
```json
[
  {
    "bookevent_id": 1,
    "account_id": 3,
    "book_id": 1,
    "book_title": "Война и мир",
    "user_name": "Иван Иванов",
    "event_type_name": "Request",
    "event_date": "2024-11-01T10:00:00Z",
    "author_name": "Лев Толстой",
    "category_name": "Романы",
    "branch_name": "Центральная библиотека"
  }
]
```

### GET `/book-event/user/{accountId}/history`

Получить историю книг пользователя

### GET `/book-event/user/{accountId}/requested`

Получить запрошенные книги пользователя

### GET `/book-event/user/{accountId}/taked`

Получить взятые книги пользователя

### GET `/book-event/user/{accountId}/saved`

Получить сохраненные книги пользователя

### POST `/book-event/request`

Запросить книгу

**Query Parameters:**
- `accountId` (integer) - ID пользователя
- `bookId` (integer) - ID книги

### POST `/book-event/accept/{eventId}/{librarianId}`

Принять запрос на книгу (Admin, Librarian)

### POST `/book-event/reject/{eventId}/{librarianId}`

Отклонить запрос (Admin, Librarian)

### POST `/book-event/save`

Сохранить книгу в избранное

### POST `/book-event/take`

Взять книгу

### POST `/book-event/return/{eventId}`

Вернуть книгу

### DELETE `/book-event/remove-saved`

Удалить из избранного

### DELETE `/book-event/cancel-request`

Отменить запрос

### GET `/book-event/taked`

Получить все взятые книги

### GET `/book-event/request/{eventId}/detail`

Получить детали запроса

---

## 📊 Reports API

### GET `/book-event-report/generate`

Сгенерировать PDF отчет (Admin, Librarian)

**Query Parameters:**
- `EventTypeId` (integer, optional) - ID типа события
- `StartDate` (datetime, optional) - Дата начала
- `EndDate` (datetime, optional) - Дата окончания
- `BookTitle` (string, optional) - Название книги
- `UserName` (string, optional) - Имя пользователя

**Response:** `200 OK`
- Content-Type: `application/pdf`
- File: `BookEventsReport.pdf`

---

## 👥 Users API

### POST `/user/send-verification-code`

Отправить код верификации на email для новой регистрации

**Request Body:**
```json
{
  "email": "newuser@example.com"
}
```

**Response:** `200 OK`
```json
{
  "message": "Код верификации отправлен на ваш email",
  "expiresIn": "15 минут"
}
```

### POST `/user/send-verification-code-for-change`

Отправить код верификации для смены email или пароля

**Request Body:**
```json
{
  "email": "current-or-new-email@example.com"
}
```

**Response:** `200 OK`
```json
{
  "message": "Код верификации отправлен на указанный email",
  "expiresIn": "15 минут"
}
```

### POST `/user/verify-code`

Проверить код верификации

**Request Body:**
```json
{
  "email": "user@example.com",
  "code": "123456"
}
```

**Response:** `200 OK`
```json
{
  "message": "Email успешно верифицирован",
  "verified": true
}
```

### POST `/user/register-with-verification`

Создать пользователя с верификацией email

**Request Body:**
```json
{
  "email": "newuser@example.com",
  "code": "123456",
  "login": "newuser",
  "password": "password123",
  "full_name": "Новый Пользователь",
  "phone": "+79991234567"
}
```

**Response:** `200 OK`
```json
{
  "message": "Пользователь успешно зарегистрирован",
  "email": "newuser@example.com"
}
```

### PUT `/user/change-email`

Изменить email пользователя (требуется код верификации на НОВЫЙ email)

**Request Body:**
```json
{
  "id": 1,
  "email": "newemail@example.com",
  "code": "123456"
}
```

**Response:** `200 OK`
```json
{
  "message": "Email успешно изменен"
}
```

### PUT `/user/change-password`

Изменить пароль (требуется код верификации на текущий email)

**Request Body:**
```json
{
  "id": 1,
  "old_password": "oldpassword",
  "new_password": "newpassword",
  "email": "current@example.com",
  "code": "123456"
}
```

**Response:** `200 OK`
```json
{
  "message": "Пароль успешно изменен"
}
```

---

## 💾 База данных

### Схема базы данных

Проект использует PostgreSQL с следующими основными таблицами:

- **roles** - Роли пользователей
- **accounts** - Пользователи системы
- **authors** - Авторы книг
- **categories** - Категории книг
- **branches** - Филиалы библиотеки
- **books** - Каталог книг
- **event_types** - Типы событий
- **book_events** - События книг (выдача, возврат, бронирование)

### SQL Скрипт

Полный SQL скрипт для создания базы данных доступен в файле [`database.sql`](./database.sql)

Скрипт включает:
- ✅ Создание всех таблиц
- ✅ Настройку внешних ключей
- ✅ Создание индексов для производительности
- ✅ Представления (views) для частых запросов
- ✅ Триггеры для автоматизации
- ✅ Тестовые данные для разработки

### Установка БД

```bash
# Создать базу данных
createdb library_db

# Выполнить скрипт
psql -d library_db -f database.sql
```

---

## 🧪 Тестирование

Проект имеет **70 unit-тестов** с полным покрытием всех контроллеров.

### Структура тестов

```
✅ AuthController        - 4 теста
✅ BookController        - 11 тестов
✅ AuthorController      - 8 тестов
✅ CategoryController    - 8 тестов
✅ BranchController      - 8 тестов
✅ UserController        - 7 тестов
✅ BookEventController   - 19 тестов
✅ BookEventReportController - 5 тестов
```

### Запуск тестов

```bash
# Все тесты
dotnet test

# С покрытием
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Отдельный класс тестов
dotnet test --filter "FullyQualifiedName~BookControllerTests"

# С подробным выводом
dotnet test --verbosity detailed
```

### Используемые инструменты

- **xUnit** - Testing framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library

### Пример теста

```csharp
[Fact]
public async Task GetBooks_ShouldReturnListOfBooks()
{
    // Arrange
    var books = new List<BookListDTO> { /* ... */ };
    _mockBookService.Setup(s => s.BookListedGet()).ReturnsAsync(books);

    // Act
    var result = await _controller.GetBooks();

    // Assert
    var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
    var returnBooks = okResult.Value.Should().BeAssignableTo<List<BookListDTO>>().Subject;
    returnBooks.Should().HaveCount(2);
}
```

---

## 🏛️ Архитектура

### Принципы разработки

- ✅ **SOLID** - Принципы объектно-ориентированного программирования
- ✅ **DRY** - Don't Repeat Yourself
- ✅ **KISS** - Keep It Simple, Stupid
- ✅ **Clean Architecture** - Разделение ответственности
- ✅ **Dependency Injection** - Инверсия зависимостей

### Слои приложения

```
┌─────────────────────────────────────┐
│         Controllers                  │  ← Presentation Layer
│  (API Endpoints, HTTP Handlers)     │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│         Services                     │  ← Business Logic Layer
│  (Business Rules, Validation)       │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      Data Access (Dapper)           │  ← Data Layer
│  (Database Queries, Repository)     │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      PostgreSQL Database            │  ← Database
└─────────────────────────────────────┘
```

### Dependency Injection

Все сервисы регистрируются через интерфейсы:

```csharp
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
// ... и т.д.
```

### Преимущества архитектуры

- ✅ **Тестируемость** - легко мокировать зависимости
- ✅ **Расширяемость** - легко добавлять новый функционал
- ✅ **Поддерживаемость** - понятная структура кода
- ✅ **Переиспользуемость** - модульные компоненты

---

## 📧 Настройка Email для отправки кодов верификации

### Получение App Password для Gmail

1. Включите двухфакторную аутентификацию в [Google Account Security](https://myaccount.google.com/security)
2. Перейдите в [App Passwords](https://myaccount.google.com/apppasswords)
3. Создайте новый App Password для "Почта" / "BooksAPI"
4. Скопируйте сгенерированный 16-значный пароль (убрав все пробелы)

### Настройка в appsettings.json

```json
{
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587",
    "SmtpUser": "ваша-почта@gmail.com",
    "SmtpPassword": "ваш-app-password-без-пробелов",
    "FromEmail": "ваша-почта@gmail.com",
    "FromName": "BooksAPI",
    "EnableSsl": "true"
  }
}
```

### Процесс верификации

#### Для регистрации нового пользователя:

1. **POST** `/api/user/send-verification-code` - Отправить код на email
   ```json
   {
     "email": "newuser@example.com"
   }
   ```

2. **POST** `/api/user/register-with-verification` - Зарегистрироваться с кодом
   ```json
   {
     "email": "newuser@example.com",
     "code": "123456",
     "login": "username",
     "password": "password123",
     "full_name": "Иван Иванов",
     "phone": "+79991234567"
   }
   ```

#### Для смены email:

1. **POST** `/api/user/send-verification-code-for-change` - Отправить код на НОВЫЙ email
   ```json
   {
     "email": "newemail@example.com"
   }
   ```

2. **PUT** `/api/user/change-email` - Сменить email с кодом
   ```json
   {
     "id": 1,
     "email": "newemail@example.com",
     "code": "123456"
   }
   ```

#### Для смены пароля:

1. **POST** `/api/user/send-verification-code-for-change` - Отправить код на текущий email
   ```json
   {
     "email": "current@example.com"
   }
   ```

2. **PUT** `/api/user/change-password` - Сменить пароль с кодом
   ```json
   {
     "id": 1,
     "old_password": "oldpass",
     "new_password": "newpass",
     "email": "current@example.com",
     "code": "123456"
   }
   ```

### Шаблоны писем

**Код верификации:**
- 6-значный код
- Срок действия: 15 минут
- Красивое HTML письмо с брендингом

**Приветственное письмо:**
- Автоматически отправляется после регистрации
- Список возможностей системы

**Уведомление о смене пароля:**
- Автоматически отправляется после смены пароля
- Предупреждение о безопасности

### Другие SMTP провайдеры

**Yandex:** `smtp.yandex.ru:587`  
**Mail.ru:** `smtp.mail.ru:587`  
**Outlook:** `smtp-mail.outlook.com:587`

### Устранение проблем

**Проблема:** Получатель видит "Здравствуйте!" вместо имени
- **Причина:** Не передано поле `full_name` при регистрации
- **Решение:** Убедитесь, что поле `full_name` заполнено в запросе `/register-with-verification`

**Проблема:** Email не отправляется
- Проверьте правильность App Password (без пробелов)
- Убедитесь, что двухфакторная аутентификация включена в Gmail
- Проверьте настройки SMTP в `appsettings.json`

---