using System.Text.RegularExpressions;

namespace Blog.Shared.Extensions;

public static class StringExtension
{
    public static Guid ToGuid(this string str)
    {
        return Guid.TryParse(str, out var guid) ? guid : Guid.Empty;
    }
    
    public static string JoinString(this IEnumerable<string>? arr, char character)
    {
        if (arr is null) return string.Empty;
        var enumerable = arr as string[] ?? arr.ToArray();
        return string.Join(character, enumerable);
    }

    public static string Slugable(this string str)
    {
        return RemoveDiacritics(str).Split(Array.Empty<char>()).JoinString('-');
    }
    
    public static string RemoveDiacritics(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        try
        {
            str = str.ToLower().Trim();
            str = Regex.Replace(str, @"[\r|\n]", " ");
            str = Regex.Replace(str, @"\s+", " ");
            str = Regex.Replace(str, "[áàảãạâấầẩẫậăắằẳẵặ]", "a");
            str = Regex.Replace(str, "[éèẻẽẹêếềểễệ]", "e");
            str = Regex.Replace(str, "[iíìỉĩị]", "i");
            str = Regex.Replace(str, "[óòỏõọơớờởỡợôốồổỗộ]", "o");
            str = Regex.Replace(str, "[úùủũụưứừửữự]", "u");
            str = Regex.Replace(str, "[yýỳỷỹỵ]", "y");
            str = Regex.Replace(str, "[đ]", "d");

            //Remove special character
            str = Regex.Replace(str, "[\"`'’~!@#$%^&*()\\-+=?/>.<,{}[]|]\\]", "");
            str = str.Trim(); 
            return str;
        }
        catch (Exception)
        {
            return str;
        }
    }

}