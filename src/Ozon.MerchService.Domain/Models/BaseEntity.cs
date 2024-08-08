using System.ComponentModel.DataAnnotations;

namespace Ozon.MerchService.Domain.Models;

public class BaseEntity
{
    [Key]
    public long Id { get; protected set; }
}