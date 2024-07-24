namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class MerchPackRequestDto
{
    public int Id { get; set; }
    public int MerchpackId { get; set; }
    public long[] MerchPackItems { get; set; }
    public int EmployeeId { get; set; }
    public int EmployeeFullName { get; set; }
    public int EmployeeEmail { get; set; }
    public int ClothingSize { get; set; }
    public string HrEmail { get; set; }
    public int RequestType { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public DateTimeOffset Issued { get; set; }
    public int Status { get; set; }
}