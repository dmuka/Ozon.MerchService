using System.ComponentModel.DataAnnotations;

namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

public class RequestStatusDto : BaseEntity
{
    [Key]
    public new int Id { get; set; }
}