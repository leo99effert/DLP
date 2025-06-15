using System.Text.Json;

internal class Countries : IData<Country>
{
    public Log Log { get; set; }
    public string Url { get; set; }
    public Countries(Log log, string url)
    {
        Log = log;
        Url = url;
    }
    public async Task<List<Country>> Get()
    {
        using HttpClient client = new HttpClient();
        try
        {
            List<DLP.DTO.Country>? countriesDTO = await GetDto(client);
            return ConvertDtoToModel(countriesDTO);
        }
        catch (Exception ex)
        {
            Log.WriteToLog(LogType.Error, $"Error fetching countries");
            throw new Exception($"Error fetching countries: {ex.Message}", ex);
        }
    }


    private async Task<List<DLP.DTO.Country>?> GetDto(HttpClient client)
    {
        var response = await client.GetStringAsync(Url);
        var countriesDTO = JsonSerializer.Deserialize<List<DLP.DTO.Country>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return countriesDTO;
    }
    private static List<Country> ConvertDtoToModel(List<DLP.DTO.Country>? countriesDTO)
    {
        return countriesDTO.Take(10).Select(dto => new Country { Name = dto.Name.Common }).ToList();
    }
}
