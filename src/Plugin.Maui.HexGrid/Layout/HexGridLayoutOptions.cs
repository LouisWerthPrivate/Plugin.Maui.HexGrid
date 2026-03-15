namespace Plugin.Maui.HexGrid.Layout;

internal sealed class HexGridLayoutOptions
{
	public float HexSize { get; init; }

	public float MinHexSize { get; init; }

	public float MaxHexSize { get; init; }

	public float HexSpacing { get; init; }

	public bool AdaptiveLayout { get; init; }

	public int MinColumns { get; init; }

	public int MaxColumns { get; init; }

	public int OverscanRows { get; init; }

	public bool StartWithOffsetRow { get; init; }

	public float Padding { get; init; }
}
