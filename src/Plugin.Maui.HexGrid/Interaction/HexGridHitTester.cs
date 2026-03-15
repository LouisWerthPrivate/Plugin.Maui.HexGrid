using Microsoft.Maui.Graphics;
using Plugin.Maui.HexGrid.Layout;
using Plugin.Maui.HexGrid.Models;

namespace Plugin.Maui.HexGrid.Interaction;

internal sealed class HexGridHitTester
{
	public HexGridCellSnapshot? HitTest(PointF point, HexGridLayoutSnapshot snapshot)
	{
		for (var index = snapshot.VisibleCells.Count - 1; index >= 0; index--)
		{
			var cell = snapshot.VisibleCells[index];
			if (!cell.Bounds.Contains(point))
			{
				continue;
			}

			if (PointInPolygon(point, cell.Vertices))
			{
				return cell;
			}
		}

		return null;
	}

	static bool PointInPolygon(PointF point, IReadOnlyList<PointF> polygon)
	{
		var inside = false;
		for (var i = 0; i < polygon.Count; i++)
		{
			var j = i == 0 ? polygon.Count - 1 : i - 1;
			var current = polygon[i];
			var previous = polygon[j];

			var intersects = ((current.Y > point.Y) != (previous.Y > point.Y))
				&& (point.X < ((previous.X - current.X) * (point.Y - current.Y) / ((previous.Y - current.Y) + float.Epsilon)) + current.X);
			if (intersects)
			{
				inside = !inside;
			}
		}

		return inside;
	}
}
