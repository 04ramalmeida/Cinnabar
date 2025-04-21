using System.Net;

namespace Cinnabar.Services;

public class ApiResponse
{
    public bool IsSuccess {get; set;}
    
    public HttpStatusCode StatusCode {get; set;}
    
    public string? Message { get; set; }
    
    public object Object {get; set;}
}