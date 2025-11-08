using DLSite.Net.Crawler;
using DLSite.Net.Model.Doujin;

namespace Test;

public class Tests
{
    private Client _client;
    
    [SetUp]
    public void Setup()
    {
        _client = new Client();
    }

    [Test]
    public void AllReturnsAllDoujin()
    {
        AllSearchResultPage? page = null;
        Assert.DoesNotThrowAsync(async () => page = await _client.Doujin.All());
        Assert.NotNull(page);
        Assert.Greater(page.TotalCount, 0);
        Assert.Greater(page.PageCount, 0);
    }
}