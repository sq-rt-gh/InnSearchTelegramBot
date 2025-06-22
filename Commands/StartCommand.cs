using InnSearchTelegramBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InnSearchTelegramBot.Commands;

class StartCommand(ITelegramBotClient BotClient) : IBotCommand
{
    public string Name => "/start";

    public async Task ExecuteAsync(Message msg, string[]? args, CancellationToken ct)
    {
        await BotClient.SendMessage(
            msg.Chat.Id,
            "Приветствую! Я Telegram-бот для поиска информации о компании по ИНН. Напишите /help, чтобы узнать список доступных команд.",
            cancellationToken: ct);
    }
}
