using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace DLSite.Net.Model.Doujin;

public class AllSearchResultItem
{
    private HtmlNode Node { get; }

    public string Title
    {
        get
        {
            var node = Node.SelectSingleNode(".//dl/dd[2]/div[2]/a");
            if (node == null) return string.Empty;
            return HtmlEntity.DeEntitize(node.InnerText.Trim());
        }
    }

    public string? WorkType
    {
        get
        {
            var node = Node.SelectSingleNode(".//dl/dd[1]/div/a");
            if (node == null) return null;
            return HtmlEntity.DeEntitize(node.InnerText.Trim());
        }
    }
    
    public string? Price
    {
        get
        {
            // Trova il blocco prezzo relativo al nodo
            var priceNode = Node.SelectSingleNode(".//dl/dd[4]/span[1]/span");
            if (priceNode == null) return null;

            // Unisci il testo di tutti i <span> figli (inclusi prefisso, base, suffisso)
            var priceText = string.Concat(
                priceNode.Descendants("span")
                    .Select(s => HtmlEntity.DeEntitize(s.InnerText))
                    .ToArray()
            ).Trim();

            // Rimuove eventuali spazi multipli
            return Regex.Replace(priceText, @"\s+", " ");
        }
    }
    
    public string? ImageUrl
    {
        get
        {
            // Cerca l'immagine dentro l’<a> (relativo al li)
            var imgNode = Node.SelectSingleNode(".//a/img");
            if (imgNode == null) return null;

            var src = imgNode.GetAttributeValue("src", string.Empty).Trim();

            // Gestisci URL relativi al protocollo
            if (src.StartsWith("//"))
                src = "https:" + src;
            return src;
        }
    }

    public string? DetailUrl
    {
        get
        {
            var node = Node.SelectSingleNode(".//dl/dd[2]/div[2]/a");
            if (node == null) return null;
            return node.GetAttributeValue("href", string.Empty).Trim();
        }
    }

    public AllSearchResultItem(HtmlNode node)
    {
        Node = node;
    }
}