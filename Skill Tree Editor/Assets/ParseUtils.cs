using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
public static class ParseUtils
{
    public static Color ColorFromUInt(uint hex)
    {
        Color c;
        c.b = (byte)((hex) & 0xFF);
        c.g = (byte)((hex>>8) & 0xFF);
        c.r = (byte)((hex>>16) & 0xFF);
        c.a = 1;
        return c;
    }
        
    public static string ParseString(this XElement element, string name, string undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return value;
    }
    public static int[] ParseIntArray(string values)
    {
        var data = values.Split(",");
        int[] array = new int[data.Length];
        int index = 0;
        foreach (var val in data)
        {
            array[index++] = int.Parse(val);
        }
        return array;
    }
    public static ushort ParseUshort(this XElement element, string name, ushort undefined = 0)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return (ushort)(value.StartsWith("0x") ? int.Parse(value.Substring(2), NumberStyles.HexNumber) : int.Parse(value));
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
    public static long ParseLong(this XElement element, string name, long undefined = 0)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return long.Parse(value);
    }

    public static uint ParseUInt(this XElement element, string name, bool isHex = true, uint undefined = 0)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return Convert.ToUInt32(value, isHex ? 16 : 10);
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

    public static T ParseEnum<T>(this XElement element, string name, T undefined) where T : Enum
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return (T)Enum.Parse(typeof(T), value.Replace(" ", ""));
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
    public static ushort[] ParseUshortArray(this XElement element, string name, string separator, ushort[] undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return ParseStringArray(element, name, separator).Select(k => (ushort)(k.StartsWith("0x") ? int.Parse(k.Substring(2), NumberStyles.HexNumber) : int.Parse(k))).ToArray();
    }

    public static List<int> ParseIntList(this XElement element, string name, string separator, List<int> undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return ParseStringArray(element, name, separator).Select(int.Parse).ToList();
    }
        
    public static List<uint> ParseUIntList(this XElement element, string name, string separator, List<uint> undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        return ParseStringArray(element, name, separator).Select(uint.Parse).ToList();
    }
    public static float[] ParseFloatArray(this XElement element, string name, string seperator, float[] undefined = null)
    {
        var value = name[0].Equals('@') ? element.Attribute(name.Remove(0, 1))?.Value : element.Element(name)?.Value;
        if (string.IsNullOrWhiteSpace(value)) return undefined;
        value = Regex.Replace(value, @"\s+", "");
        return ParseStringArray(element, name, seperator).Select(k => float.Parse(k)).ToArray();
    }
}
