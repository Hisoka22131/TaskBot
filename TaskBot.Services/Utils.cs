using System.ComponentModel;

namespace TaskBot.Services;

public static class Utils
{
    public static string GetDescription(this Enum value)
    {
        var type = value.GetType();

        var fieldInfo = type.GetField(value.ToString());
        var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();

        return attribute == null ? value.ToString() : ((DescriptionAttribute)attribute).Description;
    }
}