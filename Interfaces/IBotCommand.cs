using Telegram.Bot.Types;

namespace InnSearchTelegramBot.Interfaces;

public interface IBotCommand
{
    string Name { get; }
    Task ExecuteAsync(Message msg, string[]? args, CancellationToken ct);
}
