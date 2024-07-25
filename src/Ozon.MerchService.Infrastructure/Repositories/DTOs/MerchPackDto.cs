namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class MerchPackDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long[] Items { get; set; }
}