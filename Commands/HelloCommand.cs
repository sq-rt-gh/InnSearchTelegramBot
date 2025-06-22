using InnSearchTelegramBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace InnSearchTelegramBot.Commands;

class HelloCommand(ITelegramBotClient BotClient) : IBotCommand
{
    public string Name => "/hello";

    public async Task ExecuteAsync(Message msg, string[]? args, CancellationToken ct)
    {
        await BotClient.SendMessage(
            msg.Chat.Id,
            """
Имя: <b>Олег Гуськов</b> 
Email: <code>whitetiger.guskov@ya.ru</code>
<a href="https://github.com/sq-rt-gh">Ссылка на GitHub</a>
<a href="https://hh.ru/resume/4fc4ab5fff0d62fc5c0039ed1f597856767a61">Резюме на hh.ru</a>
""",
            parseMode: ParseMode.Html,
            linkPreviewOptions: new LinkPreviewOptions().IsDisabled = true,
            cancellationToken: ct);
    }
}
