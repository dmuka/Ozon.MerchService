using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.Domain.Models.Extensions;

public static class RequestStatusExtension
{
    public static bool Is(this RequestStatus actualRequestStatus, RequestStatus toCompareRequestStatus)
    {
        return Equals(actualRequestStatus, toCompareRequestStatus);
    }
}