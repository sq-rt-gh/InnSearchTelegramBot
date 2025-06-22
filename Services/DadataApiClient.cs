using Dadata;

namespace InnSearchTelegramBot.Services;


public record class DadataApiResponse(string Name, string Address, string Inn);


public class DadataApiClient
{
    private readonly SuggestClientAsync api;

    public DadataApiClient(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("API ключ Dadata не может быть пустым", nameof(token));

        api = new SuggestClientAsync(token);
    }

    public async Task<List<DadataApiResponse>> FindByInnAsync(params string[] inns)
    {
        var results = new List<DadataApiResponse>();

        foreach (var inn in inns.Distinct())
        {
            try
            {
                var response = await api.FindParty(inn);
                var party = response.suggestions.FirstOrDefault();

                if (party?.data != null)
                {
                    var name = party.data.name.short_with_opf ?? "не найдено";
                    var address = party.data.address.value ?? "не найдено";
                    results.Add(new DadataApiResponse(name, address, inn));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе ИНН {inn}: {ex.Message}");
            }
        }

        return results;
    }
}
