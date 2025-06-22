using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using InnSearchTelegramBot.Services;
using InnSearchTelegramBot.Commands;
using InnSearchTelegramBot.Interfaces;


var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

var telegramToken = config["TelegramBotToken"];
if (string.IsNullOrEmpty(telegramToken))
{
    Console.WriteLine("TelegramBotToken не найден.");
    return;
}

var dadataKey = config["DadataApiKey"];
if (string.IsNullOrEmpty(dadataKey))
{
    Console.WriteLine("DadataApiKey не найден.");
    return;
}


var services = new ServiceCollection();

services.AddLogging(config =>
{
    config.AddSimpleConsole(options =>
    {
        options.TimestampFormat = "dd.MM.yy HH:mm:ss ";
    });
    config.SetMinimumLevel(LogLevel.Information);
});

services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(telegramToken));
services.AddSingleton(new DadataApiClient(dadataKey));
services.AddSingleton<BotService>();

services.AddSingleton<IBotCommand, StartCommand>();
services.AddSingleton<IBotCommand, HelpCommand>();
services.AddSingleton<IBotCommand, HelloCommand>();
services.AddSingleton<IBotCommand, InnCommand>();

var provider = services.BuildServiceProvider();

var botClient = provider.GetRequiredService<ITelegramBotClient>();
var botService = provider.GetRequiredService<BotService>();
var logger = provider.GetRequiredService<ILogger<Program>>();

using var cts = new CancellationTokenSource();

botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);

var me = await botClient.GetMe();
logger.LogInformation($"Бот @{me.Username} запущен. Нажмите Escape для завершения.");

while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
cts.Cancel();

logger.LogInformation("Сервис остановлен.");


async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
{
    await botService.HandleUpdateAsync(update, token);
}

Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
{
    logger.LogError(exception, "Ошибка в TelegramBotClient");
    return Task.CompletedTask;
}