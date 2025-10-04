# BooksWebApi

## Описание проекта

BooksWebApi — это RESTful Web API для управления библиотекой книг, разработанный на ASP.NET Core. Проект предоставляет функционал для управления книгами, авторами, категориями, филиалами библиотек и событиями, связанными с книгами.

## Технологический стек

- **Framework**: ASP.NET Core
- **Язык программирования**: C#
- **База данных**: PostgreSQL
- **ORM**: Dapper (Micro-ORM)
- **Архитектура**: Многоуровневая архитектура (Controllers, Services, DTO)

## Структура проекта

```
BooksWebApi/
├── WebApplication2/
│   ├── Connection/              # Подключение к базе данных
│   ├── Controllers/             # API контроллеры
│   │   ├── Book/               # Контроллеры для управления книгами
│   │   │   ├── AuthorController.cs
│   │   │   ├── BookController.cs
│   │   │   ├── BranchController.cs
│   │   │   └── CategoryController.cs
│   │   ├── BookEvent/          # Контроллеры для событий
│   │   └── Dashboard/          # Контроллеры для дашборда
│   ├── DTO/                    # Data Transfer Objects
│   │   ├── Auth/
│   │   ├── Author/
│   │   ├── Book/
│   │   ├── BookEvent/
│   │   ├── Branch/
│   │   ├── Category/
│   │   └── Dashboard/
│   ├── Services/               # Бизнес-логика
│   │   ├── Auth/
│   │   ├── Author/
│   │   ├── Book/
│   │   │   └── BookService.cs
│   │   ├── BookEvent/
│   │   ├── Branch/
│   │   ├── Category/
│   │   ├── Dashboard/
│   │   └── File/               # Сервис для работы с файлами
│   ├── coverlink/              # Хранение обложек книг
│   ├── Program.cs              # Точка входа приложения
│   ├── WebApplication2.csproj  # Файл проекта
│   ├── WebApplication2.http    # HTTP запросы для тестирования
│   ├── appsettings.json        # Конфигурация приложения
│   └── appsettings.Development.json
└── WebApplication2.sln         # Solution файл
```

## Основные компоненты

### Controllers (Контроллеры)

#### BookController
Основной контроллер для управления книгами:
- `GET /api/book/list` - Получение списка всех книг
- `POST /api/book/add` - Добавление новой книги
- `PUT /api/book/edit` - Редактирование книги
- `DELETE /api/book/{id}` - Удаление книги
- `GET /api/book/search` - Поиск книг по названию, автору или категории
- `GET /api/book/{id}/detail` - Получение детальной информации о книге

### Services (Сервисы)

#### BookService
Содержит бизнес-логику для работы с книгами:
- `BookListedGet()` - Получение списка книг с информацией об авторах, категориях и филиалах
- `AddBook(AddBook book, IFormFile? coverImage)` - Добавление книги с возможностью загрузки обложки
- `EditBook(EditBook book, IFormFile? coverImage)` - Редактирование книги
- `RemoveBook(int id)` - Удаление книги с автоматическим удалением обложки
- `SearchBooks(string? searchTerm, int? categoryId)` - Поиск книг
- `GetBookDetail(int bookId)` - Получение детальной информации о книге

### DTO (Data Transfer Objects)

Используются для передачи данных между слоями:
- `AddBook` - DTO для добавления книги
- `EditBook` - DTO для редактирования книги
- `BookListDto` - DTO для отображения списка книг
- `BookDetailDto` - DTO для детальной информации о книге

## Примеры использования API

### Получение списка всех книг

```http
GET /api/book/list
```

**Ответ:**
```json
[
  {
    "id": 1,
    "title": "Название книги",
    "description": "Описание книги",
    "fragment": "Фрагмент текста",
    "cover_link": "/coverlink/image.jpg",
    "author_name": "Имя автора",
    "category_name": "Категория",
    "branch_name": "Филиал"
  }
]
```

### Добавление новой книги

```http
POST /api/book/add
Content-Type: multipart/form-data

Title: Название книги
Description: Описание
Fragment: Фрагмент
AuthorId: 1
CategoryId: 1
BranchId: 1
coverImage: [файл изображения]
```

### Поиск книг

```http
GET /api/book/search?searchTerm=книга&categoryId=1
```

### Получение детальной информации о книге

```http
GET /api/book/1/detail
```

## Unit-тестирование

Для проекта рекомендуется использовать следующие фреймворки:
- **xUnit** или **NUnit** - для написания тестов
- **Moq** - для создания mock-объектов
- **FluentAssertions** - для улучшения читаемости assertions

### Пример структуры тестового проекта

```
BooksWebApi.Tests/
├── Controllers/
│   └── BookControllerTests.cs
├── Services/
│   └── BookServiceTests.cs
└── BooksWebApi.Tests.csproj
```

### Примеры unit-тестов

См. файлы тестов в отдельной папке проекта (будут добавлены далее).

## Установка и запуск

### Предварительные требования

- .NET 6.0 или выше
- PostgreSQL
- Visual Studio 2022 или VS Code

### Шаги установки

1. Клонируйте репозиторий:
```bash
git clone https://github.com/bru1f0rc3/BooksWebApi.git
```

2. Перейдите в папку проекта:
```bash
cd BooksWebApi/WebApplication2
```

3. Настройте строку подключения к базе данных в `appsettings.json`

4. Восстановите зависимости:
```bash
dotnet restore
```

5. Запустите приложение:
```bash
dotnet run
```

Приложение будет доступно по адресу: `http://localhost:5000` (или порт, указанный в настройках)

## Конфигурация

Основные настройки приложения находятся в файле `appsettings.json`:
- Строка подключения к базе данных
- Настройки логирования
- Пути к файлам и обложкам

## База данных

Проект использует следующие основные таблицы:
- `Books` - информация о книгах
- `Authors` - авторы книг
- `Categories` - категории книг
- `Branches` - филиалы библиотек
- `BookEvents` - события, связанные с книгами

## Особенности реализации

1. **Работа с файлами**: Поддержка загрузки и хранения обложек книг
2. **Поиск**: Реализован полнотекстовый поиск по названию и автору (case-insensitive)
3. **Валидация**: Обработка ошибок и возврат соответствующих HTTP статусов
4. **Dependency Injection**: Используется встроенный DI контейнер ASP.NET Core

## Разработка и тестирование

### Запуск в режиме разработки

```bash
dotnet watch run
```

### Тестирование API

Используйте файл `WebApplication2.http` для тестирования endpoints через REST Client в VS Code.

## Лицензия

Этот проект является учебным проектом.

## Контакты

Автор: bru1f0rc3
GitHub: https://github.com/bru1f0rc3

## Дальнейшее развитие

- [ ] Добавить аутентификацию и авторизацию
- [ ] Реализовать пагинацию для списков
- [ ] Добавить Swagger/OpenAPI документацию
- [ ] Расширить функционал поиска
- [ ] Добавить кэширование
- [ ] Реализовать логирование с использованием Serilog
- [ ] Добавить Docker support
