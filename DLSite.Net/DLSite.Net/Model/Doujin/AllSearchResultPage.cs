using System.Xml.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace DLSite.Net.Model.Doujin;

public class AllSearchResultPage
{
    private HtmlDocument _page { get; } = new();
    private JObject _pager { get; set; } = new();
    

    public int TotalCount
    {
        get
        {
            _pager.TryGetValue("count", out var total);
            if (total is JValue jv) return jv.Value<int>();
            return 0;
        }
    }
    
    public int PageCount
    {
        get
        {
            _pager.TryGetValue("last_page", out var total);
            if (total is JValue jv) return jv.Value<int>();
            return 0;
        }
    }

    public AllSearchResultPage(string html)
    {
        _page.LoadHtml(html);
        ExtractPager();
    }

    private void ExtractPager()
    {
        var node = _page.DocumentNode.SelectSingleNode("//*[@id=\"container\"]/script");
        if (null == node) return;
        var script = node.InnerText.Trim();
        
        if (script.IndexOf("\"pager\":{", StringComparison.Ordinal) >= 0 &&
            script.IndexOf("window['", StringComparison.Ordinal) >= 0 &&
            script.IndexOf("] =", StringComparison.Ordinal) >= 0)
        {
            // Estrai l'oggetto completo assegnato a window['...'] = { ... };
            var fullJson = ExtractAssignedObject(script);
            if (fullJson is null) return;

            // Prendi SOLO "pager": { ... } (parsing a parentesi)
            var pagerJson = ExtractObjectAfterKey(fullJson, "\"pager\"");
            if (pagerJson is null) return;

            try
            {
                _pager = JObject.Parse(pagerJson);
            }
            catch { /* ignora e prova altri script */ }
        }
    }
    
    private static string? ExtractAssignedObject(string text)
    {
        // Trova l’uguale dopo window['...']
        int w = text.IndexOf("window['", StringComparison.Ordinal);
        if (w < 0) return null;

        int eq = text.IndexOf('=', w);
        if (eq < 0) return null;

        // Vai al primo '{' dopo '='
        int braceStart = text.IndexOf('{', eq);
        if (braceStart < 0) return null;

        int depth = 0;
        for (int i = braceStart; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '{') depth++;
            else if (c == '}')
            {
                depth--;
                if (depth == 0)
                {
                    // Include la '}' finale. Potrebbe seguire un ';'
                    var json = text.Substring(braceStart, i - braceStart + 1);
                    return json;
                }
            }
        }
        return null;
    }
    
    private static string? ExtractObjectAfterKey(string text, string key)
    {
        int i = text.IndexOf(key, StringComparison.Ordinal);
        if (i < 0) return null;

        // Salta eventuali spazi, due punti, ecc… fino al primo '{'
        int braceStart = text.IndexOf('{', i);
        if (braceStart < 0) return null;

        int depth = 0;
        for (int p = braceStart; p < text.Length; p++)
        {
            char c = text[p];
            if (c == '{') depth++;
            else if (c == '}')
            {
                depth--;
                if (depth == 0)
                {
                    return text.Substring(braceStart, p - braceStart + 1);
                }
            }
        }
        return null;
    }
}