using InnSearchTelegramBot.Interfaces;
using InnSearchTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace InnSearchTelegramBot.Commands;


class InnCommand(ITelegramBotClient BotClient, DadataApiClient DadataClient) : IBotCommand
{
    public string Name => "/inn";

    public async Task ExecuteAsync(Message msg, string[]? args, CancellationToken ct)
    {
        if (args is null || args.Length == 0)
        {
            await BotClient.SendMessage(
                msg.Chat.Id,
                "Пожалуйста, укажите один или несколько ИНН через пробел. \nПример: <code>/inn 7731457980</code>",
                parseMode: ParseMode.Html,
                cancellationToken: ct);
            return;
        }

        var result = await DadataClient.FindByInnAsync(args);

        if (result.Count == 0)
        {
            await BotClient.SendMessage(
                msg.Chat.Id,
                "По указанным ИНН ничего не найдено.",
                cancellationToken: ct);
            return;
        }

        var sorted = result.OrderBy(r => r.Name).ToList();

        var responseText = string.Join("\n\n", sorted.Select(r =>
            $"<b>{r.Name}</b>\nИНН: <code>{r.Inn}</code>\n<code>{r.Address}</code>"));

        await BotClient.SendMessage(
            msg.Chat.Id,
            responseText,
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }
}
