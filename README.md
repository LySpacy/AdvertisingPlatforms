API для работы с данными рекламных платформ и их привязкой к локациям.  
Позволяет загружать файлы с описанием платформ и локаций, а также искать платформы по конкретной локации.

## Запуск проекта

1. Клонируйте репозиторий:

```bash
git clone https://github.com/LySpacy/AdvertisingPlatforms.git
cd AdvertisingPlatforms
```
2. Откройте решение в Visual Studio или другой IDE.

3. Восстановите NuGet-пакеты: 
```bash
dotnet restore
```

4. Постройте проект:
```bash
dotnet build
```

5. Запустите веб-сервис:
 ```bash
dotnet run --project AdvertisingPlatforms.API
```

По умолчанию сервис запускается на http://localhost:5000

## Swagger
Swagger UI доступен по адресу: http://localhost:5000/swagger

Но переадресация происходит автоматически при перехотеле на запущенный хост

## Использование API

### API имеет два основных эндпоинта: загрузка файла и поиск платформ.

**Загрузка файла**

**POST** /api/adplatforms/load

Параметры:
  - **file** — файл с данными рекламных платформ.
Формат файла:
 ```bash
Yandex:/ru
LocalPaper:/ru/svrd/revda,/ru/svrd/pervik
Gazeta:/ru/msk,/ru/permobl,/ru/chelobl
CoolAds:/ru/svrd
```

Возможные ответы:
 - **200 OK** — файл успешно загружен.
 - **400 Bad Request** — файл пуст или содержит некорректные данные.

**Поиск платформ**
**GET** /api/adplatforms/search?location={location}

Параметры:
  - **location** — строка локации (например, /ru/svrd/revda).
  
Пример запроса:
```bash
curl "http://localhost:5000/api/adplatforms/search?location=/ru/svrd"
```
Пример ответа:
```bash
["Yandex", "CoolAds"]
```

## Тестирование

Для запуска юнит-тестов и интеграционных тестов:
```bash
dotnet test
```

Тесты покрывают:
 - Репозиторий InMemoryAdPlatformRepository
 - Сервис AdPlatformService
 - Контроллер AdPlatformController
 - Модель LocationNode

## Примечания

- Все ошибки обрабатываются через глобальный middleware, возвращая корректный JSON с сообщением.
- Сервис использует in-memory хранилище платформ, поэтому данные не сохраняются после перезапуска приложения.
