using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
public static class ParseUtils
{        
    public static string ParseString(this XElement element, string name, string undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return value;
    }
    public static int ParseInt(this XElement element, string name, int undefined = 0)
    {
        try
        {
            var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
            if (string.IsNullOrWhiteSpace(value)) return undefined;

            return value.StartsWith("0x") ? int.Parse(value.Substring(2), NumberStyles.HexNumber) : int.Parse(value);
            //return int.Parse(value);

        }
        catch(Exception e)
        {
            Debug.LogError($"ElementName:{name}\n{e.Message}\n{e.StackTrace}");
        }
        return 0;
    }        
    public static float ParseFloat(this XElement element, string name, float undefined = 0)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return float.Parse(value, CultureInfo.InvariantCulture);
    }
    public static bool ParseBool(this XElement element, string name, bool undefined = false)
    {
        var isAttr = name[0].Equals('@');
        var id = name[0].Equals('@') ? name.Remove(0, 1) : name;
        var value = isAttr ? element.Attribute(id)?.Value : element.Element(id)?.Value;
        if (string.IsNullOrWhiteSpace(value)) 
        {
            if (isAttr && element.Attribute(id) != null || !isAttr && element.Element(id) != null)
                return true;
            return undefined; 
        }
        return bool.Parse(value);
    }
    public static string[] ParseStringArray(this XElement element, string name, string separator, string[] undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        value = Regex.Replace(value, @"\s+", "");
        return value.Split(separator.ToCharArray());
    }

    public static int[] ParseIntArray(this XElement element, string name, string separator, int[] undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return ParseStringArray(element, name, separator).Select(int.Parse).ToArray();
    }
}
