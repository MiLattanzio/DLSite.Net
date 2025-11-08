namespace DLSite.Net.Crawler;

public class Client
{
    public HttpClient HttpClient { get; }
    public Doujin Doujin { get; }
    
    public Client()
    {
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new System.Net.CookieContainer()
        };
        HttpClient = new HttpClient(handler);
        HttpClient.DefaultRequestHeaders.Add("User-Agent", 
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/120.0.0.0 Safari/537.36");
        HttpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        HttpClient.DefaultRequestHeaders.Add("Accept-Language", "it-IT,it;q=0.9,en-US;q=0.8,en;q=0.7");
        HttpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        Doujin = new Doujin(this);
    }
    
}