using Microsoft.Maui.Graphics;
using Plugin.Maui.HexGrid.Models;

namespace Plugin.Maui.HexGrid.Layout;

internal sealed class HexGridLayoutEngine
{
	readonly float sqrtThree = MathF.Sqrt(3f);

	public HexGridLayoutSnapshot Build(
		IReadOnlyList<object?> items,
		float availableWidth,
		float availableHeight,
		HexGridLayoutOptions options,
		object? selectedItem,
		Func<object?, string> previewTextSelector,
		Func<object?, object?> commandParameterSelector,
		Func<object?, Color> fillColorSelector,
		Func<object?, Color> selectedFillColorSelector,
		Func<object?, Color> strokeColorSelector,
		Func<object?, Color> textColorSelector)
	{
		if (availableWidth <= 0)
		{
			return HexGridLayoutSnapshot.Empty;
		}

		var radius = ResolveRadius(availableWidth, options);
		var hexWidth = sqrtThree * radius;
		var hexHeight = radius * 2f;
		var rowOffset = (hexWidth / 2f) + Math.Max(0, options.HexSpacing / 2f);
		var columnStep = hexWidth + Math.Max(0, options.HexSpacing);
		var rowStep = (hexHeight * 0.75f) + Math.Max(0, options.HexSpacing);
		var innerWidth = Math.Max(0, availableWidth - (options.Padding * 2f));
		var nonOffsetRowCapacity = CalculateRowCapacity(innerWidth, radius, 0f, columnStep);
		var offsetRowCapacity = CalculateRowCapacity(innerWidth, radius, rowOffset, columnStep);

		if (options.MaxColumns > 0)
		{
			nonOffsetRowCapacity = Math.Min(nonOffsetRowCapacity, options.MaxColumns);
			offsetRowCapacity = Math.Min(offsetRowCapacity, options.MaxColumns);
		}

		nonOffsetRowCapacity = Math.Max(1, nonOffsetRowCapacity);
		offsetRowCapacity = Math.Max(1, offsetRowCapacity);
		var visibleCells = new List<HexGridCellSnapshot>();
		var itemIndex = 0;
		var rowIndex = 0;

		while (itemIndex < items.Count)
		{
			var isOffsetRow = ((rowIndex + (options.StartWithOffsetRow ? 1 : 0)) % 2) != 0;
			var rowCapacity = isOffsetRow ? offsetRowCapacity : nonOffsetRowCapacity;
			var itemsInRow = Math.Min(rowCapacity, items.Count - itemIndex);
			var centerY = options.Padding + (rowIndex * rowStep) + (hexHeight / 2f);
			var startX = options.Padding + radius + (isOffsetRow ? rowOffset : 0f);
			for (var columnIndex = 0; columnIndex < itemsInRow; columnIndex++)
			{
				var item = items[itemIndex + columnIndex];
				var centerX = startX + (columnIndex * columnStep);
				visibleCells.Add(new HexGridCellSnapshot
				{
					Index = itemIndex + columnIndex,
					Item = item,
					CommandParameter = commandParameterSelector(item),
					PreviewText = previewTextSelector(item),
					Center = new PointF(centerX, centerY),
					Bounds = new RectF(centerX - radius, centerY - (hexHeight / 2f), hexWidth, hexHeight),
					Vertices = BuildVertices(centerX, centerY, radius, hexHeight),
					IsSelected = selectedItem is not null && Equals(item, selectedItem),
					FillColor = fillColorSelector(item),
					SelectedFillColor = selectedFillColorSelector(item),
					StrokeColor = strokeColorSelector(item),
					TextColor = textColorSelector(item)
				});
			}

			itemIndex += itemsInRow;
			rowIndex++;
		}

		var totalRows = rowIndex;
		var totalHeight = items.Count == 0
			? 0
			: (options.Padding * 2f) + hexHeight + ((totalRows - 1) * rowStep);

		return new HexGridLayoutSnapshot
		{
			VisibleCells = visibleCells,
			TotalHeight = totalHeight,
			CanvasWidth = availableWidth,
			HexRadius = radius,
			HexWidth = hexWidth,
			HexHeight = hexHeight
		};
	}

	float ResolveRadius(float availableWidth, HexGridLayoutOptions options)
	{
		var requestedRadius = Math.Clamp(options.HexSize, options.MinHexSize, options.MaxHexSize);
		if (!options.AdaptiveLayout)
		{
			return requestedRadius;
		}

		var spacing = Math.Max(0, options.HexSpacing);
		var innerWidth = Math.Max(0, availableWidth - (options.Padding * 2f));
		var minColumns = Math.Max(1, options.MinColumns);
		var fitRadius = CalculateRadiusForWidth(innerWidth, minColumns, spacing);
		if (fitRadius <= 0)
		{
			return requestedRadius;
		}

		return Math.Clamp(Math.Min(requestedRadius, fitRadius), options.MinHexSize, options.MaxHexSize);
	}

	static int CalculateRowCapacity(float innerWidth, float radius, float rowOffset, float columnStep)
	{
		var usableWidth = innerWidth - rowOffset;
		if (usableWidth <= radius * 2f)
		{
			return 1;
		}

		return Math.Max(1, (int)MathF.Floor((usableWidth - (radius * 2f)) / Math.Max(1f, columnStep)) + 1);
	}

	float CalculateRadiusForWidth(float innerWidth, int baseCapacity, float spacing)
	{
		if (baseCapacity <= 1)
		{
			return innerWidth / sqrtThree;
		}

		return (innerWidth - ((baseCapacity - 1) * spacing)) / Math.Max(1f, baseCapacity * sqrtThree);
	}

	PointF[] BuildVertices(float centerX, float centerY, float radius, float hexHeight)
	{
		var halfWidth = (sqrtThree * radius) / 2f;
		var halfRadius = radius / 2f;
		return
		[
			new PointF(centerX, centerY - radius),
			new PointF(centerX + halfWidth, centerY - halfRadius),
			new PointF(centerX + halfWidth, centerY + halfRadius),
			new PointF(centerX, centerY + radius),
			new PointF(centerX - halfWidth, centerY + halfRadius),
			new PointF(centerX - halfWidth, centerY - halfRadius)
		];
	}
}
