namespace DLSite.Net.Exception;

public class SearchFailedException: System.Exception
{
    public HttpResponseMessage ResponseMessage { get; }
    
    public SearchFailedException(HttpResponseMessage responseMessage)
    {
        ResponseMessage = responseMessage;
    }
}