using DLSite.Net.Exception;
using DLSite.Net.Model.Doujin;

namespace DLSite.Net.Crawler;

public class Doujin
{
    private readonly Client _client;
    
    protected internal Doujin(Client client)
    {
        _client = client;
    }

    public async Task<AllSearchResultPage> All()
    {
        var result = await _client.HttpClient.GetAsync(Constant.Url.DoujinAllUrl);
        if (!result.IsSuccessStatusCode) throw new SearchFailedException(result);
        var html = await result.Content.ReadAsStringAsync();
        return new AllSearchResultPage(html);
    }
}