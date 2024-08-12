using System.ComponentModel.DataAnnotations;

namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
}