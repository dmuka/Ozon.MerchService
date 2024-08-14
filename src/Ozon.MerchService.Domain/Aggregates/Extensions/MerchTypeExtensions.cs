using CSharpCourse.Core.Lib.Enums;

namespace Ozon.MerchService.Domain.Models.Extensions;

public static class MerchTypeExtensions
{
    public static ClothingSize GetClothingSize(this int clothingSizeId)
    {
        var enumValues = Enum.GetValues(typeof(ClothingSize));
        var isValidIdValue = false;

        var index = 0;
        while (!isValidIdValue && index < enumValues.Length)
        {
            if ((int)enumValues.GetValue(index++)! == clothingSizeId) isValidIdValue = true;
        }

        if (!isValidIdValue) throw new ArgumentException("Unknown clothing size id value");
        
        var result = clothingSizeId switch
        {
            (int)ClothingSize.XS => ClothingSize.XS,
            (int)ClothingSize.S => ClothingSize.S,
            (int)ClothingSize.M => ClothingSize.M,
            (int)ClothingSize.L => ClothingSize.L,
            (int)ClothingSize.XL => ClothingSize.XL,
            (int)ClothingSize.XXL => ClothingSize.XXL,
            _ => throw new ArgumentException("Unknown clothing size value")

        };

        return result;
    }
}