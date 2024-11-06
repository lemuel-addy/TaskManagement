namespace TaskManagement.Api.Dtos;
#nullable disable
public class ApiResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; }
    public string ResponseCode { get; set; }
    public T Data { get; set; }
    public bool  IsSuccessful { get; set; }
}