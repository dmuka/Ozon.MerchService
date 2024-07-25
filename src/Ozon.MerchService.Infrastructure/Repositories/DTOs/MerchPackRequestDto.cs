namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class MerchPackRequestDto
{
    public int Id { get; set; }
    public int MerchpackTypeId { get; set; }
    public string MerchPackItems { get; set; }
    public int EmployeeId { get; set; }
    public int ClothingSizeId { get; set; }
    public string HrEmail { get; set; }
    public int RequestTypeId { get; set; }
    public DateTimeOffset RequestedAt { get; set; }
    public DateTimeOffset Issued { get; set; }
    public int RequestStatusId { get; set; }
}