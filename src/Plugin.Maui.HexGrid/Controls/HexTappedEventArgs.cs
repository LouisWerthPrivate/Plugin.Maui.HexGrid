using Microsoft.Maui.Graphics;

namespace Plugin.Maui.HexGrid.Controls;

/// <summary>
/// Provides data for the <see cref="HexGridView.HexTapped" /> event.
/// </summary>
public sealed class HexTappedEventArgs : EventArgs
{
	/// <summary>
	/// Initializes a new instance of the <see cref="HexTappedEventArgs" /> class.
	/// </summary>
	/// <param name="item">The tapped item.</param>
	/// <param name="commandParameter">The command parameter resolved for the tapped item.</param>
	/// <param name="index">The zero-based index of the tapped item.</param>
	/// <param name="location">The tap location in view coordinates.</param>
	public HexTappedEventArgs(object? item, object? commandParameter, int index, PointF location)
	{
		Item = item;
		CommandParameter = commandParameter;
		Index = index;
		Location = location;
	}

	/// <summary>
	/// Gets the tapped item.
	/// </summary>
	public object? Item { get; }

	/// <summary>
	/// Gets the command parameter resolved for the tapped item.
	/// </summary>
	public object? CommandParameter { get; }

	/// <summary>
	/// Gets the zero-based index of the tapped item.
	/// </summary>
	public int Index { get; }

	/// <summary>
	/// Gets the tap location in view coordinates.
	/// </summary>
	public PointF Location { get; }
}
