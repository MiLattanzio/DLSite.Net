namespace DLSite.Net.Constant;

public class Url
{
    public const string BaseUrl = "https://www.dlsite.com";
    
    public const string DoujinUrl = $"{BaseUrl}/{Category.Doujin}";
    public const string DoujinAllUrl = $"{DoujinUrl}/fsr/=/order/trend/per_page/100";
    
    public const string DoujinGamesUrl = $"{DoujinUrl}/works/type/=/work_type_category/{WorkType.Game}";
    public const string DoujinVideoUrl = $"{DoujinUrl}/works/type/=/work_type_category/{WorkType.Video}";
    public const string DoujinAudioUrl = $"{DoujinUrl}/works/type/=/work_type_category/{WorkType.Audio}";
    public const string DoujinMangaUrl = $"{DoujinUrl}/works/type/=/work_type_category/{WorkType.Manga}";
    public const string DoujinIllustrationUrl = $"{DoujinUrl}/works/type/=/work_type_category/{WorkType.Illustration}";
    
    
}