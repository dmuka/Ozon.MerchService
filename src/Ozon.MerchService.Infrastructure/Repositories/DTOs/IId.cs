using System.ComponentModel.DataAnnotations;

namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class IId
{
    [Key]
    public int Id { get; set; }
}