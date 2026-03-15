using Microsoft.Maui.Graphics;
using Plugin.Maui.HexGrid.Layout;

namespace Plugin.Maui.HexGrid.Drawing;

internal sealed class HexGridDrawable : IDrawable
{
	PathF? cachedHexPath;
	float cachedRadius;

	public HexGridLayoutSnapshot LayoutSnapshot { get; set; } = HexGridLayoutSnapshot.Empty;

	public Color FillColor { get; set; } = Color.FromArgb("#E2E8F0");

	public Color SelectedFillColor { get; set; } = Color.FromArgb("#F59E0B");

	public Color StrokeColor { get; set; } = Colors.Black;

	public Color TextColor { get; set; } = Colors.Black;

	public float StrokeSize { get; set; } = 1.5f;

	public float FontSize { get; set; } = 14f;

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		canvas.SaveState();
		canvas.StrokeLineJoin = LineJoin.Round;
		canvas.StrokeLineCap = LineCap.Round;
		canvas.FontColor = TextColor;
		canvas.FontSize = FontSize;

		if (LayoutSnapshot.VisibleCells.Count == 0)
		{
			canvas.RestoreState();
			return;
		}

		var hexPath = GetHexPath(LayoutSnapshot.HexRadius, LayoutSnapshot.HexHeight);
		foreach (var cell in LayoutSnapshot.VisibleCells)
		{
			canvas.SaveState();
			canvas.Translate(cell.Center.X, cell.Center.Y);
			canvas.FillColor = cell.IsSelected ? cell.SelectedFillColor : cell.FillColor;
			canvas.StrokeColor = cell.StrokeColor;
			canvas.StrokeSize = StrokeSize;
			canvas.FontColor = cell.TextColor;
			canvas.FillPath(hexPath);
			canvas.DrawPath(hexPath);
			canvas.DrawString(
				cell.PreviewText,
				-(LayoutSnapshot.HexWidth / 2f),
				-(LayoutSnapshot.HexHeight / 2f),
				LayoutSnapshot.HexWidth,
				LayoutSnapshot.HexHeight,
				HorizontalAlignment.Center,
				VerticalAlignment.Center);
			canvas.RestoreState();
		}

		canvas.RestoreState();
	}

	PathF GetHexPath(float radius, float hexHeight)
	{
		if (cachedHexPath is not null && Math.Abs(cachedRadius - radius) < 0.001f)
		{
			return cachedHexPath;
		}

		var halfWidth = (MathF.Sqrt(3f) * radius) / 2f;
		var halfRadius = radius / 2f;
		var path = new PathF();
		path.MoveTo(0, -radius);
		path.LineTo(halfWidth, -halfRadius);
		path.LineTo(halfWidth, halfRadius);
		path.LineTo(0, radius);
		path.LineTo(-halfWidth, halfRadius);
		path.LineTo(-halfWidth, -halfRadius);
		path.Close();

		cachedRadius = radius;
		cachedHexPath = path;
		return path;
	}
}
