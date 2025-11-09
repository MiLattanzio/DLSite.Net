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
        Assert.DoesNotThrowAsync(async () => page = await _client.Doujin.FirstPage());
        Assert.That(page, Is.Not.Null);
        Assert.That(page.TotalCount, Is.GreaterThan(0));
        Assert.That(page.PageCount, Is.GreaterThan(0));
        Assert.That(page.CurrentPage, Is.EqualTo(1));
        Assert.That(page.Items.Count, Is.GreaterThan(0));
        foreach (var item in page.Items)
        {
           Assert.That(item, Is.Not.Null);
           Assert.That(item.Title, Is.Not.Empty);
           Assert.That(item.WorkType, Is.Not.Empty);
           Assert.That(item.Price, Is.Not.Null);
           Assert.That(item.ImageUrl, Is.Not.Null);
           Assert.That(item.DetailUrl, Is.Not.Null);
        }
        AllSearchResultPage? lastPage = null;
        Assert.DoesNotThrowAsync(async () => lastPage = await _client.Doujin.LastPage(page));
        Assert.That(lastPage, Is.Not.Null);
    }
}