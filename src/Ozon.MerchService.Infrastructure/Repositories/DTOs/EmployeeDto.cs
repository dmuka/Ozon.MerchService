namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class EmployeeDto
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    
    public List<MerchPackRequestDto> MerchPackRequests { get; set; }
}