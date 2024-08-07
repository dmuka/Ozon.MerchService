using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ozon.MerchService.Infrastructure.Repositories.Attributes;

namespace Ozon.MerchService.Infrastructure.Repositories.DTOs;

[Table("merchpack_requests")]
public class MerchPackRequestDto
{
    [Key]
    public int Id { get; set; }
    [Column("merchpack_type_id")]
    public int MerchpackTypeId { get; set; }
    [Json]
    [Column("merchpack_items")]
    public string MerchPackItems { get; set; }
    [Column("employee_id")]
    public long EmployeeId { get; set; }
    [Column("clothing_size_id")]
    public int ClothingSizeId { get; set; }
    [Column("hr_email")]
    public string HrEmail { get; set; }
    [Column("request_type_id")]
    public int RequestTypeId { get; set; }
    [Column("requested_at")]
    public DateTimeOffset RequestedAt { get; set; }
    [Column("issued")]
    public DateTimeOffset? Issued { get; set; }
    [Column("request_status_id")]
    public int RequestStatusId { get; set; }
}