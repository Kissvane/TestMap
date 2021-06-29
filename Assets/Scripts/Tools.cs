using UnityEngine;

/// <summary>
/// Tools
/// </summary>
public static class Tools
{
    /// <summary>
    /// Get a random string with only capital letters
    /// </summary>
    /// <param name="NameLength">the length of the random string</param>
    /// <returns>the random string</returns>
    public static string GetRandomName(int NameLength)
    {
        string result = "";

        for (int i = 0; i < NameLength; i++)
        {
            result = string.Concat(result, (char)Random.Range(65, 90));
        }

        return result;
    }

    /// <summary>
    /// Get a random shade from an original color
    /// </summary>
    /// <param name="original"></param>
    /// <param name="ShadeRange"></param>
    /// <returns>the random color</returns>
    public static Color GetRandomShade(Color original, float ShadeRange)
    {
        Color.RGBToHSV(original, out float H, out float S, out float V);
        return Color.HSVToRGB(H, Random.Range(0.5f + ShadeRange, 0.5f + ShadeRange * 2), V + Random.Range(-ShadeRange / 2f, ShadeRange / 2f));
    }
}
