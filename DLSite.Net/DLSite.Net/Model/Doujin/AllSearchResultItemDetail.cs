using HtmlAgilityPack;

namespace DLSite.Net.Model.Doujin;

public class AllSearchResultItemDetail
{
    private HtmlDocument Page { get; } = new();

    public string Title
    {
        get
        {
            var node = Page.DocumentNode.SelectSingleNode("//*[@id=\"work_name\"]");
            if (node == null) return string.Empty;
            return HtmlEntity.DeEntitize(node.InnerText.Trim());
        }
    }

    public string Circle
    {
        get
        {
            // --- Circolo (nome + url) ---
            var makerAnchor = Page.DocumentNode.SelectSingleNode(".//table[@id='work_maker']//span[contains(@class,'maker_name')]/a");
            if (makerAnchor != null)
            {
                return HtmlEntity.DeEntitize(makerAnchor.InnerText.Trim()); var href = makerAnchor.GetAttributeValue("href", "").Trim();
            }
            return string.Empty;
        }
    }

    public string? CircleUrl
    {
        get
        {
            var node = Page.DocumentNode.SelectSingleNode(".//table[@id='work_maker']//span[contains(@class,'maker_name')]/a");
            if (node == null) return null;
            return NormalizeUrl(node.GetAttributeValue("href", string.Empty).Trim());
        }
    }

    public string? ReleaseDate => GetCellTextByHeader(Page.DocumentNode, "販売日", preferLinkText: true);

    public string? Audience
    {
        get
        {
            var etaSpan = GetCellNodeByHeader(Page.DocumentNode, "年齢指定")
                ?.SelectSingleNode(".//span[contains(@class,'icon_')]");
            return 
                etaSpan?.GetAttributeValue("title", null)
                ?? etaSpan?.InnerText?.Trim()
                ?? null;
        }
    }
    
    public AllSearchResultItemDetail(string html)
    {
        Page.LoadHtml(html);
    }
    
    private static HtmlNode? GetCellNodeByHeader(HtmlNode root, string headerContains)
    {
        return root.SelectSingleNode($".//table[@id='work_outline']//tr[th[contains(normalize-space(.), '{headerContains}')]]/td");
    }

    // Restituisce il testo della cella; se preferLinkText è true, prova prima il testo dell'<a>
    private static string GetCellTextByHeader(HtmlNode root, string headerContains, bool preferLinkText = false)
    {
        var td = GetCellNodeByHeader(root, headerContains);
        if (td == null) return string.Empty;

        string? text = null;

        if (preferLinkText)
        {
            var a = td.SelectSingleNode(".//a");
            text = a?.InnerText?.Trim();
        }

        text ??= td.InnerText?.Trim();
        return HtmlEntity.DeEntitize(text ?? "");
    }
    
    private static string NormalizeUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return "";
        url = url.Trim();
        if (url.StartsWith("//")) return "https:" + url;
        if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return url;
        // base host per DLsite
        return "https://www.dlsite.com" + (url.StartsWith("/") ? url : "/" + url);
    }
    
}