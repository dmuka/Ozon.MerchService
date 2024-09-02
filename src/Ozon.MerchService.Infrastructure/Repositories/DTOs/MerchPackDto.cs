using System.ComponentModel.DataAnnotations.Schema;

namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

[Table("merchpacks")]
public class MerchPackDto : BaseEntity
{
    public string Items { get; set; }
}