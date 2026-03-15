using Microsoft.Maui.Graphics;
using Plugin.Maui.HexGrid.Models;

namespace Plugin.Maui.HexGrid.Layout;

internal sealed class HexGridLayoutSnapshot
{
	public static HexGridLayoutSnapshot Empty { get; } = new()
	{
		VisibleCells = Array.Empty<HexGridCellSnapshot>(),
		TotalHeight = 0,
		CanvasWidth = 0,
		HexRadius = 0,
		HexWidth = 0,
		HexHeight = 0
	};

	public required IReadOnlyList<HexGridCellSnapshot> VisibleCells { get; init; }

	public required float TotalHeight { get; init; }

	public required float CanvasWidth { get; init; }

	public required float HexRadius { get; init; }

	public required float HexWidth { get; init; }

	public required float HexHeight { get; init; }
}
