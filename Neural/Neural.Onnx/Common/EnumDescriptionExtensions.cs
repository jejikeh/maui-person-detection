using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Neural.Onnx.Common;

public static class EnumDescriptionExtensions
{
    // Note that we never need to expire these cache items, so we just use ConcurrentDictionary rather than MemoryCache
    private static readonly
        ConcurrentDictionary<string, string> _displayNameCache = new ConcurrentDictionary<string, string>();

    public static string DisplayName(this Enum value)
    {
        var key = $"{value.GetType().FullName}.{value}";

        var displayName = _displayNameCache.GetOrAdd(key, x =>
        {
            var name = (DescriptionAttribute[])value
                .GetType()
                .GetTypeInfo()
                .GetField(value.ToString())!
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return name.Length > 0 ? name[0].Description : value.ToString();
        });

        return displayName;
    }
}