# WebApplication2.Tests

## Описание

Проект с unit-тестами для веб-API управления библиотекой книг.

## Структура тестов

- **Controllers/BookControllerTests.cs** - Тесты для контроллера управления книгами
- **Controllers/AuthControllerTests.cs** - Тесты для контроллера аутентификации

## Используемые технологии

- **xUnit** - Фреймворк для unit-тестирования
- **Moq** - Библиотека для создания mock-объектов
- **FluentAssertions** - Библиотека для более читаемых проверок в тестах

## Запуск тестов

### Из командной строки

```powershell
# Запустить все тесты
dotnet test

# Запустить тесты с подробным выводом
dotnet test --verbosity detailed

# Запустить тесты с отчетом о покрытии кода
dotnet test /p:CollectCoverage=true
```

### Из Visual Studio

1. Откройте решение `WebApplication2.sln`
2. Перейдите в Test Explorer (Тест -> Проводник тестов)
3. Нажмите "Запустить все тесты"

### Из Visual Studio Code

1. Установите расширение ".NET Core Test Explorer"
2. Откройте панель Test Explorer
3. Нажмите кнопку "Run All Tests"

## Описание тестов

### BookControllerTests

#### GetBooks
- ✅ Должен возвращать список всех книг
- ✅ Должен возвращать пустой список, если книг нет

#### AddBook
- ✅ Должен успешно добавлять книгу
- ✅ Должен возвращать BadRequest при ошибке

#### EditBook
- ✅ Должен успешно редактировать книгу
- ✅ Должен возвращать BadRequest при ошибке

#### RemoveBook
- ✅ Должен успешно удалять книгу

#### SearchBooks
- ✅ Должен возвращать найденные книги по поисковому запросу
- ✅ Должен возвращать книги по категории

#### GetBookDetail
- ✅ Должен возвращать детальную информацию о книге
- ✅ Должен возвращать NotFound, если книга не найдена

### AuthControllerTests

#### Login
- ✅ Должен возвращать токен при успешной аутентификации
- ✅ Должен возвращать Unauthorized при неверных учетных данных
- ✅ Должен возвращать токен с правильной ролью для обычного пользователя
- ✅ Должен вызывать сервис с переданными учетными данными

## Покрытие кода

Текущее покрытие кода тестами охватывает основные сценарии использования контроллеров:
- Успешные операции
- Обработка ошибок
- Валидация данных

## Расширение тестов

Для добавления новых тестов:

1. Создайте новый класс тестов в соответствующей папке
2. Наследуйте от базового класса или создайте независимый класс
3. Используйте атрибут `[Fact]` для простых тестов
4. Используйте атрибут `[Theory]` для параметризованных тестов

Пример:

```csharp
[Theory]
[InlineData(1)]
[InlineData(2)]
[InlineData(3)]
public async Task GetBookDetail_ShouldReturnBook_ForValidIds(int bookId)
{
    // Arrange
    var expectedBook = new BookDetailDTO { id = bookId };
    _mockBookService.Setup(s => s.GetBookDetail(bookId))
        .ReturnsAsync(expectedBook);

    // Act
    var result = await _controller.GetBookDetail(bookId);

    // Assert
    var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
    var book = okResult.Value.Should().BeAssignableTo<BookDetailDTO>().Subject;
    book.id.Should().Be(bookId);
}
```

## Примечания

- Все тесты используют mock-объекты для изоляции тестируемого кода
- Тесты не требуют подключения к базе данных
- Тесты выполняются быстро и независимо друг от друга
