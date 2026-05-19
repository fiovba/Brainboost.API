namespace BrainBoost.API.Helpers;

public static class SlugHelper
{
    public static string Generate(string text)
    {
        text = text.ToLower();

        text = text
            .Replace("ə", "e")
            .Replace("ü", "u")
            .Replace("ö", "o")
            .Replace("ğ", "g")
            .Replace("ş", "s")
            .Replace("ç", "c")
            .Replace("ı", "i");

        text = text.Replace(" ", "-");

        return text;
    }
}