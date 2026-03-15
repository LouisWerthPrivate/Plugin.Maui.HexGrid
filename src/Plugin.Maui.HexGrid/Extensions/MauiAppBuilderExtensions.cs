using Microsoft.Maui.Hosting;

namespace Plugin.Maui.HexGrid;

/// <summary>
/// Provides registration extensions for the hex grid control library.
/// </summary>
public static class MauiAppBuilderExtensions
{
	/// <summary>
	/// Adds the hex grid library to the MAUI app builder.
	/// </summary>
	/// <param name="builder">The MAUI app builder.</param>
	/// <returns>The same <see cref="MauiAppBuilder" /> instance.</returns>
	public static MauiAppBuilder UseHexGrid(this MauiAppBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder);
		return builder;
	}
}
