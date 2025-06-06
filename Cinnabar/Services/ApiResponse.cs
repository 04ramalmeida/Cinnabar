using System.Net;

namespace Cinnabar.Services;

public class ApiResponse<T>
{
    public bool IsSuccess {get; init;}
    
    public HttpStatusCode StatusCode {get; set;}
    
    public string? Message { get; set; }
    
    public T? Object {get; init;}
}