using DLSite.Net.Exception;
using DLSite.Net.Model.Doujin;

namespace DLSite.Net.Crawler.Doujin;

public class Client
{
    private readonly Crawler.Client _client;
    
    protected internal Client(Crawler.Client client)
    {
        _client = client;
    }

    public async Task<AllSearchResultPage> FirstPage()
    {
        var result = await _client.HttpClient.GetAsync(Constant.Url.DoujinAllUrl);
        if (!result.IsSuccessStatusCode) throw new SearchFailedException(result);
        var html = await result.Content.ReadAsStringAsync();
        return new AllSearchResultPage(html);
    }

    public async Task<AllSearchResultPage> Page(AllSearchResultPage page, int index)
    {
        var url = $"{Constant.Url.DoujinUrl}/fsr/=/language/jp/sex_category%5B0%5D/male/order%5B0%5D/trend/options_and_or/and/options%5B0%5D/JPN/options%5B1%5D/ENG/options%5B2%5D/ITA/options%5B3%5D/NM/options_name%5B0%5D/Opere+in+giapponese/options_name%5B1%5D/Opere+Inglesi/options_name%5B2%5D/Opere+senza+specificazione+di+lingua/per_page/100/page/{index}/show_type/3/lang_options%5B0%5D/Giapponese/lang_options%5B1%5D/Inglese/lang_options%5B2%5D/Italiano/lang_options%5B3%5D/Alinguistico";
        var result = await _client.HttpClient.GetAsync(url);
        if (!result.IsSuccessStatusCode) throw new SearchFailedException(result);
        var html = await result.Content.ReadAsStringAsync();
        return new AllSearchResultPage(html);
    }
    
    public async Task<AllSearchResultPage> PreviousPage(AllSearchResultPage page)
    {
        var pageIndex = page.CurrentPage;
        if (pageIndex <= 1) return await FirstPage();
        return await Page(page, pageIndex - 1);
    }

    public async Task<AllSearchResultPage> NextPage(AllSearchResultPage page)
    {
        var pageIndex = page.CurrentPage;
        if (pageIndex >= page.PageCount) return await Page(page, page.PageCount);
        return await Page(page, pageIndex + 1);
    }

    public async Task<AllSearchResultPage> LastPage(AllSearchResultPage page)
    {
        return await Page(page, page.PageCount);   
    }
}