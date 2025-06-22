using InnSearchTelegramBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InnSearchTelegramBot.Commands;


class HelpCommand(ITelegramBotClient BotClient) : IBotCommand
{
    public string Name => "/help";

    public async Task ExecuteAsync(Message msg, string[]? args, CancellationToken ct)
    {
        await BotClient.SendMessage(
            msg.Chat.Id,
            """
Список доступных команд:
/start — начать работу
/help — справка по командам
/hello — информация обо мне
/inn - получить информацию о компании по ИНН
/last - повторить последнее действие
""",
            cancellationToken: ct);
    }
}
