# InnSearchTelegramBot 

**InnSearchTelegramBot** — это Telegram-бот, который позволяет получать информацию о компаниях по ИНН с использованием Dadata API.

> Реализовано на C# с использованием .NET и архитектуры на основе DI и сервисов.

---

## Возможности

- Получение краткой информации о компании по ИНН
- Поддержка нескольких команд:
  - `/start` — начать работу
  - `/help` — список доступных команд
  - `/hello` — информация об авторе
  - `/inn <ИНН>` — поиск компании по ИНН
  - `/last` — повтор последней выполненной команды

---

## Используемые технологии

- [.NET 8](https://dotnet.microsoft.com/)
- [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
- [Dadata.ru API](https://dadata.ru/api/find-party/)

---

## Установка и запуск

1. **Клонируйте репозиторий:**

```bash
git clone https://github.com/sq-rt-gh/InnSearchTelegramBot.git
cd InnSearchTelegramBot
```

2. **Добавьте User Secrets:**

```bash
dotnet user-secrets init
dotnet user-secrets set TelegramBotToken "<ваш_токен_бота>"
dotnet user-secrets set DadataApiKey "<ваш_DaData_API>"
```

3. **Соберите и запустите проект:**

```bash
dotnet build
dotnet run
```
