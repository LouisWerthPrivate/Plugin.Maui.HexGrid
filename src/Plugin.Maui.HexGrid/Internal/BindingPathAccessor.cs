using System.Collections.Concurrent;
using System.Reflection;

namespace Plugin.Maui.HexGrid.Internal;

internal static class BindingPathAccessor
{
	static readonly ConcurrentDictionary<string, PropertyInfo[]?> Cache = new(StringComparer.Ordinal);

	public static object? GetValue(object? source, string? path)
	{
		if (source is null)
		{
			return null;
		}

		if (string.IsNullOrWhiteSpace(path))
		{
			return source;
		}

		var key = $"{source.GetType().AssemblyQualifiedName}|{path}";
		var properties = Cache.GetOrAdd(key, _ => ResolvePath(source.GetType(), path));
		if (properties is null)
		{
			return null;
		}

		object? current = source;
		foreach (var property in properties)
		{
			if (current is null)
			{
				return null;
			}

			current = property.GetValue(current);
		}

		return current;
	}

	static PropertyInfo[]? ResolvePath(Type sourceType, string path)
	{
		var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		if (segments.Length == 0)
		{
			return null;
		}

		var properties = new PropertyInfo[segments.Length];
		var currentType = sourceType;
		for (var index = 0; index < segments.Length; index++)
		{
			var property = currentType.GetProperty(
				segments[index],
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
			if (property is null)
			{
				return null;
			}

			properties[index] = property;
			currentType = property.PropertyType;
		}

		return properties;
	}
}
