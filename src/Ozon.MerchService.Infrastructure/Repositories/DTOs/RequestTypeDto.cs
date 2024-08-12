using System.ComponentModel.DataAnnotations;

namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class RequestTypeDto : BaseEntity
{
    [Key]
    public new int Id { get; set; }
}