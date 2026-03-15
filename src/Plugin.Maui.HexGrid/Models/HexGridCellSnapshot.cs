using Microsoft.Maui.Graphics;

namespace Plugin.Maui.HexGrid.Models;

internal sealed class HexGridCellSnapshot
{
	public required int Index { get; init; }

	public required object? Item { get; init; }

	public required object? CommandParameter { get; init; }

	public required string PreviewText { get; init; }

	public required PointF Center { get; init; }

	public required RectF Bounds { get; init; }

	public required PointF[] Vertices { get; init; }

	public required bool IsSelected { get; init; }

	public required Color FillColor { get; init; }

	public required Color SelectedFillColor { get; init; }

	public required Color StrokeColor { get; init; }

	public required Color TextColor { get; init; }
}
