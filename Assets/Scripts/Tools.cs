using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Genric tools
/// </summary>
public static class Tools
{
    /// <summary>
    /// Calculte a random string containing only capital letters
    /// </summary>
    /// <param name="nameLength"> the length of the string </param>
    /// <returns> the random string </returns>
    public static string GetRandomName(int nameLength)
    {
        string result = "";

        for (int i = 0; i < nameLength; i++)
        {
            result = string.Concat(result, (char)Random.Range(65, 90));
        }

        return result;
    }

    /// <summary>
    /// Calculate a random shade based on a color. using HSV color representation
    /// </summary>
    /// <param name="original">the base color used to calculate a shade</param>
    /// <param name="shadeRange">range used to modify S and V of original color</param>
    /// <returns></returns>
    public static Color GetRandomShade(Color original, float shadeRange)
    {
        Color.RGBToHSV(original, out float H, out float S, out float V);
        return Color.HSVToRGB(H, Random.Range(0.5f + shadeRange, 0.5f + shadeRange * 2), V + Random.Range(-shadeRange / 2f, shadeRange / 2f));
    }
}
