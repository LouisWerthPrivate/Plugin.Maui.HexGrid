using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Plugin.Maui.HexGrid.Drawing;
using Plugin.Maui.HexGrid.Internal;
using Plugin.Maui.HexGrid.Interaction;
using Plugin.Maui.HexGrid.Layout;

namespace Plugin.Maui.HexGrid.Controls;

/// <summary>
/// Displays a bindable hexagonal grid backed by a <see cref="GraphicsView" />.
/// </summary>
public sealed class HexGridView : ContentView
{
	/// <summary>
	/// Identifies the <see cref="ItemsSource" /> bindable property.
	/// </summary>
	public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
		nameof(ItemsSource),
		typeof(IEnumerable),
		typeof(HexGridView),
		default(IEnumerable),
		propertyChanged: OnItemsSourceChanged);

	/// <summary>
	/// Identifies the <see cref="SelectedItem" /> bindable property.
	/// </summary>
	public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
		nameof(SelectedItem),
		typeof(object),
		typeof(HexGridView),
		default,
		BindingMode.TwoWay,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="PreviewTextPath" /> bindable property.
	/// </summary>
	public static readonly BindableProperty PreviewTextPathProperty = BindableProperty.Create(
		nameof(PreviewTextPath),
		typeof(string),
		typeof(HexGridView),
		default(string),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="CommandParameterPath" /> bindable property.
	/// </summary>
	public static readonly BindableProperty CommandParameterPathProperty = BindableProperty.Create(
		nameof(CommandParameterPath),
		typeof(string),
		typeof(HexGridView),
		default(string),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="FillColorPath" /> bindable property.
	/// </summary>
	public static readonly BindableProperty FillColorPathProperty = BindableProperty.Create(
		nameof(FillColorPath),
		typeof(string),
		typeof(HexGridView),
		default(string),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="SelectedFillColorPath" /> bindable property.
	/// </summary>
	public static readonly BindableProperty SelectedFillColorPathProperty = BindableProperty.Create(
		nameof(SelectedFillColorPath),
		typeof(string),
		typeof(HexGridView),
		default(string),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="StrokeColorPath" /> bindable property.
	/// </summary>
	public static readonly BindableProperty StrokeColorPathProperty = BindableProperty.Create(
		nameof(StrokeColorPath),
		typeof(string),
		typeof(HexGridView),
		default(string),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="TextColorPath" /> bindable property.
	/// </summary>
	public static readonly BindableProperty TextColorPathProperty = BindableProperty.Create(
		nameof(TextColorPath),
		typeof(string),
		typeof(HexGridView),
		default(string),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="HexTappedCommand" /> bindable property.
	/// </summary>
	public static readonly BindableProperty HexTappedCommandProperty = BindableProperty.Create(
		nameof(HexTappedCommand),
		typeof(ICommand),
		typeof(HexGridView));

	/// <summary>
	/// Identifies the <see cref="HexSize" /> bindable property.
	/// </summary>
	public static readonly BindableProperty HexSizeProperty = BindableProperty.Create(
		nameof(HexSize),
		typeof(double),
		typeof(HexGridView),
		36d,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="MinHexSize" /> bindable property.
	/// </summary>
	public static readonly BindableProperty MinHexSizeProperty = BindableProperty.Create(
		nameof(MinHexSize),
		typeof(double),
		typeof(HexGridView),
		24d,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="MaxHexSize" /> bindable property.
	/// </summary>
	public static readonly BindableProperty MaxHexSizeProperty = BindableProperty.Create(
		nameof(MaxHexSize),
		typeof(double),
		typeof(HexGridView),
		48d,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="HexSpacing" /> bindable property.
	/// </summary>
	public static readonly BindableProperty HexSpacingProperty = BindableProperty.Create(
		nameof(HexSpacing),
		typeof(double),
		typeof(HexGridView),
		6d,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="AdaptiveLayout" /> bindable property.
	/// </summary>
	public static readonly BindableProperty AdaptiveLayoutProperty = BindableProperty.Create(
		nameof(AdaptiveLayout),
		typeof(bool),
		typeof(HexGridView),
		true,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="MinColumns" /> bindable property.
	/// </summary>
	public static readonly BindableProperty MinColumnsProperty = BindableProperty.Create(
		nameof(MinColumns),
		typeof(int),
		typeof(HexGridView),
		4,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="MaxColumns" /> bindable property.
	/// </summary>
	public static readonly BindableProperty MaxColumnsProperty = BindableProperty.Create(
		nameof(MaxColumns),
		typeof(int),
		typeof(HexGridView),
		8,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="OverscanRows" /> bindable property.
	/// </summary>
	public static readonly BindableProperty OverscanRowsProperty = BindableProperty.Create(
		nameof(OverscanRows),
		typeof(int),
		typeof(HexGridView),
		2,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="FillColor" /> bindable property.
	/// </summary>
	public static readonly BindableProperty FillColorProperty = BindableProperty.Create(
		nameof(FillColor),
		typeof(Color),
		typeof(HexGridView),
		Color.FromArgb("#E2E8F0"),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="SelectedFillColor" /> bindable property.
	/// </summary>
	public static readonly BindableProperty SelectedFillColorProperty = BindableProperty.Create(
		nameof(SelectedFillColor),
		typeof(Color),
		typeof(HexGridView),
		Color.FromArgb("#F59E0B"),
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="StrokeColor" /> bindable property.
	/// </summary>
	public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(
		nameof(StrokeColor),
		typeof(Color),
		typeof(HexGridView),
		Colors.Black,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="TextColor" /> bindable property.
	/// </summary>
	public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
		nameof(TextColor),
		typeof(Color),
		typeof(HexGridView),
		Colors.Black,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="PreviewFontSize" /> bindable property.
	/// </summary>
	public static readonly BindableProperty PreviewFontSizeProperty = BindableProperty.Create(
		nameof(PreviewFontSize),
		typeof(double),
		typeof(HexGridView),
		14d,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="StrokeThickness" /> bindable property.
	/// </summary>
	public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(
		nameof(StrokeThickness),
		typeof(double),
		typeof(HexGridView),
		1.5d,
		propertyChanged: OnLayoutPropertyChanged);

	/// <summary>
	/// Identifies the <see cref="StartWithOffsetRow" /> bindable property.
	/// </summary>
	public static readonly BindableProperty StartWithOffsetRowProperty = BindableProperty.Create(
		nameof(StartWithOffsetRow),
		typeof(bool),
		typeof(HexGridView),
		false,
		propertyChanged: OnLayoutPropertyChanged);

	readonly HexGridDrawable drawable = new();
	readonly GraphicsView graphicsView;
	readonly HexGridLayoutEngine layoutEngine = new();
	readonly HexGridHitTester hitTester = new();
	readonly float tapThreshold = 6f;

	INotifyCollectionChanged? notifyCollectionChanged;
	IReadOnlyList<object?> items = Array.Empty<object?>();
	PointF? interactionStartPoint;
	bool interactionMoved;

	/// <summary>
	/// Gets or sets the collection rendered by the control.
	/// </summary>
	public IEnumerable? ItemsSource
	{
		get => (IEnumerable?)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	/// <summary>
	/// Gets or sets the currently selected item.
	/// </summary>
	public object? SelectedItem
	{
		get => GetValue(SelectedItemProperty);
		set => SetValue(SelectedItemProperty, value);
	}

	/// <summary>
	/// Gets or sets the binding path used to extract the text shown inside each hex cell.
	/// </summary>
	public string? PreviewTextPath
	{
		get => (string?)GetValue(PreviewTextPathProperty);
		set => SetValue(PreviewTextPathProperty, value);
	}

	/// <summary>
	/// Gets or sets the binding path used to resolve the command parameter for a tapped cell.
	/// </summary>
	public string? CommandParameterPath
	{
		get => (string?)GetValue(CommandParameterPathProperty);
		set => SetValue(CommandParameterPathProperty, value);
	}

	/// <summary>
	/// Gets or sets the binding path used to resolve the fill color for each hex cell.
	/// </summary>
	public string? FillColorPath
	{
		get => (string?)GetValue(FillColorPathProperty);
		set => SetValue(FillColorPathProperty, value);
	}

	/// <summary>
	/// Gets or sets the binding path used to resolve the selected fill color for each hex cell.
	/// </summary>
	public string? SelectedFillColorPath
	{
		get => (string?)GetValue(SelectedFillColorPathProperty);
		set => SetValue(SelectedFillColorPathProperty, value);
	}

	/// <summary>
	/// Gets or sets the binding path used to resolve the stroke color for each hex cell.
	/// </summary>
	public string? StrokeColorPath
	{
		get => (string?)GetValue(StrokeColorPathProperty);
		set => SetValue(StrokeColorPathProperty, value);
	}

	/// <summary>
	/// Gets or sets the binding path used to resolve the text color for each hex cell.
	/// </summary>
	public string? TextColorPath
	{
		get => (string?)GetValue(TextColorPathProperty);
		set => SetValue(TextColorPathProperty, value);
	}

	/// <summary>
	/// Gets or sets the command executed after a cell tap is resolved.
	/// </summary>
	public ICommand? HexTappedCommand
	{
		get => (ICommand?)GetValue(HexTappedCommandProperty);
		set => SetValue(HexTappedCommandProperty, value);
	}

	/// <summary>
	/// Gets or sets the preferred hex radius used during layout.
	/// </summary>
	public double HexSize
	{
		get => (double)GetValue(HexSizeProperty);
		set => SetValue(HexSizeProperty, value);
	}

	/// <summary>
	/// Gets or sets the minimum hex radius allowed when adaptive layout is enabled.
	/// </summary>
	public double MinHexSize
	{
		get => (double)GetValue(MinHexSizeProperty);
		set => SetValue(MinHexSizeProperty, value);
	}

	/// <summary>
	/// Gets or sets the maximum hex radius allowed when adaptive layout is enabled.
	/// </summary>
	public double MaxHexSize
	{
		get => (double)GetValue(MaxHexSizeProperty);
		set => SetValue(MaxHexSizeProperty, value);
	}

	/// <summary>
	/// Gets or sets the spacing between adjacent hex cells.
	/// </summary>
	public double HexSpacing
	{
		get => (double)GetValue(HexSpacingProperty);
		set => SetValue(HexSpacingProperty, value);
	}

	/// <summary>
	/// Gets or sets a value indicating whether the control should resize hexes to fit the viewport width.
	/// </summary>
	public bool AdaptiveLayout
	{
		get => (bool)GetValue(AdaptiveLayoutProperty);
		set => SetValue(AdaptiveLayoutProperty, value);
	}

	/// <summary>
	/// Gets or sets the minimum number of columns targeted by adaptive layout.
	/// </summary>
	public int MinColumns
	{
		get => (int)GetValue(MinColumnsProperty);
		set => SetValue(MinColumnsProperty, value);
	}

	/// <summary>
	/// Gets or sets the maximum number of columns allowed during layout.
	/// </summary>
	public int MaxColumns
	{
		get => (int)GetValue(MaxColumnsProperty);
		set => SetValue(MaxColumnsProperty, value);
	}

	/// <summary>
	/// Gets or sets the number of off-screen rows rendered above and below the viewport.
	/// </summary>
	public int OverscanRows
	{
		get => (int)GetValue(OverscanRowsProperty);
		set => SetValue(OverscanRowsProperty, value);
	}

	/// <summary>
	/// Gets or sets the fill color for unselected cells.
	/// </summary>
	public Color FillColor
	{
		get => (Color)GetValue(FillColorProperty);
		set => SetValue(FillColorProperty, value);
	}

	/// <summary>
	/// Gets or sets the fill color for the selected cell.
	/// </summary>
	public Color SelectedFillColor
	{
		get => (Color)GetValue(SelectedFillColorProperty);
		set => SetValue(SelectedFillColorProperty, value);
	}

	/// <summary>
	/// Gets or sets the stroke color used to outline cells.
	/// </summary>
	public Color StrokeColor
	{
		get => (Color)GetValue(StrokeColorProperty);
		set => SetValue(StrokeColorProperty, value);
	}

	/// <summary>
	/// Gets or sets the text color used for preview text.
	/// </summary>
	public Color TextColor
	{
		get => (Color)GetValue(TextColorProperty);
		set => SetValue(TextColorProperty, value);
	}

	/// <summary>
	/// Gets or sets the font size used for preview text.
	/// </summary>
	public double PreviewFontSize
	{
		get => (double)GetValue(PreviewFontSizeProperty);
		set => SetValue(PreviewFontSizeProperty, value);
	}

	/// <summary>
	/// Gets or sets the thickness of the cell outline.
	/// </summary>
	public double StrokeThickness
	{
		get => (double)GetValue(StrokeThicknessProperty);
		set => SetValue(StrokeThicknessProperty, value);
	}

	/// <summary>
	/// Gets or sets a value indicating whether the first rendered row should use the offset pattern.
	/// </summary>
	public bool StartWithOffsetRow
	{
		get => (bool)GetValue(StartWithOffsetRowProperty);
		set => SetValue(StartWithOffsetRowProperty, value);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HexGridView" /> class.
	/// </summary>
	public HexGridView()
	{
		graphicsView = new GraphicsView
		{
			Drawable = drawable,
			HorizontalOptions = LayoutOptions.Fill,
			VerticalOptions = LayoutOptions.Fill
		};

		graphicsView.StartInteraction += OnGraphicsViewStartInteraction;
		graphicsView.DragInteraction += OnGraphicsViewDragInteraction;
		graphicsView.EndInteraction += OnGraphicsViewEndInteraction;
		graphicsView.CancelInteraction += OnGraphicsViewCancelInteraction;

		Content = graphicsView;
		SizeChanged += OnViewSizeChanged;
		UpdateItems();
		RefreshLayout();
	}

	/// <summary>
	/// Occurs when a hex cell is tapped.
	/// </summary>
	public event EventHandler<HexTappedEventArgs>? HexTapped;

	static void OnItemsSourceChanged(BindableObject bindable, object? oldValue, object? newValue)
	{
		var view = (HexGridView)bindable;
		view.Unsubscribe(oldValue as INotifyCollectionChanged);
		view.Subscribe(newValue as INotifyCollectionChanged);
		view.UpdateItems();
		view.RefreshLayout();
	}

	static void OnLayoutPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
		=> ((HexGridView)bindable).RefreshLayout();

	void Subscribe(INotifyCollectionChanged? source)
	{
		notifyCollectionChanged = source;
		if (notifyCollectionChanged is not null)
		{
			notifyCollectionChanged.CollectionChanged += OnItemsCollectionChanged;
		}
	}

	void Unsubscribe(INotifyCollectionChanged? source)
	{
		if (source is not null)
		{
			source.CollectionChanged -= OnItemsCollectionChanged;
		}
		if (ReferenceEquals(notifyCollectionChanged, source))
		{
			notifyCollectionChanged = null;
		}
	}

	void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		UpdateItems();
		RefreshLayout();
	}

	void OnViewSizeChanged(object? sender, EventArgs e)
		=> RefreshLayout();

	void OnGraphicsViewStartInteraction(object? sender, TouchEventArgs e)
	{
		interactionStartPoint = e.Touches.FirstOrDefault();
		interactionMoved = false;
	}

	void OnGraphicsViewDragInteraction(object? sender, TouchEventArgs e)
	{
		if (interactionMoved || interactionStartPoint is null || e.Touches.Length == 0)
		{
			return;
		}

		var deltaX = e.Touches[0].X - interactionStartPoint.Value.X;
		var deltaY = e.Touches[0].Y - interactionStartPoint.Value.Y;
		interactionMoved = MathF.Abs(deltaX) > tapThreshold || MathF.Abs(deltaY) > tapThreshold;
	}

	void OnGraphicsViewEndInteraction(object? sender, TouchEventArgs e)
	{
		if (!e.IsInsideBounds || interactionMoved || e.Touches.Length == 0)
		{
			interactionStartPoint = null;
			interactionMoved = false;
			return;
		}

		var location = e.Touches[0];
		var cell = hitTester.HitTest(location, drawable.LayoutSnapshot);
		interactionStartPoint = null;
		interactionMoved = false;
		if (cell is null)
		{
			return;
		}

		SelectedItem = cell.Item;
		if (HexTappedCommand?.CanExecute(cell.CommandParameter) == true)
		{
			HexTappedCommand.Execute(cell.CommandParameter);
		}

		HexTapped?.Invoke(this, new HexTappedEventArgs(cell.Item, cell.CommandParameter, cell.Index, location));
		RefreshLayout();
	}

	void OnGraphicsViewCancelInteraction(object? sender, EventArgs e)
	{
		interactionStartPoint = null;
		interactionMoved = false;
	}

	void UpdateItems()
	{
		items = MaterializeItems(ItemsSource);
	}

	void RefreshLayout()
	{
		UpdateDrawableStyle();

		var availableWidth = (float)(Width > 0 ? Width : graphicsView.Width);
		var availableHeight = (float)(Height > 0 ? Height : graphicsView.Height);
		if (availableWidth <= 0)
		{
			return;
		}

		if (availableHeight <= 0)
		{
			availableHeight = float.PositiveInfinity;
		}

		var snapshot = layoutEngine.Build(
			items,
			availableWidth,
			availableHeight,
			CreateLayoutOptions(),
			SelectedItem,
			GetPreviewText,
			GetCommandParameter,
			GetFillColor,
			GetSelectedFillColor,
			GetStrokeColor,
			GetTextColor);

		drawable.LayoutSnapshot = snapshot;
		if (Math.Abs(graphicsView.HeightRequest - snapshot.TotalHeight) > 0.5)
		{
			graphicsView.HeightRequest = snapshot.TotalHeight;
			graphicsView.InvalidateMeasure();
		}
		if (Math.Abs(HeightRequest - snapshot.TotalHeight) > 0.5)
		{
			HeightRequest = snapshot.TotalHeight;
			InvalidateMeasure();
		}
		graphicsView.Invalidate();
	}

	void UpdateDrawableStyle()
	{
		drawable.FillColor = FillColor;
		drawable.SelectedFillColor = SelectedFillColor;
		drawable.StrokeColor = StrokeColor;
		drawable.StrokeSize = (float)StrokeThickness;
		drawable.TextColor = TextColor;
		drawable.FontSize = (float)PreviewFontSize;
	}

	HexGridLayoutOptions CreateLayoutOptions()
		=> new()
		{
			HexSize = (float)HexSize,
			MinHexSize = (float)MinHexSize,
			MaxHexSize = (float)MaxHexSize,
			HexSpacing = (float)HexSpacing,
			AdaptiveLayout = AdaptiveLayout,
			MinColumns = Math.Max(1, MinColumns),
			MaxColumns = Math.Max(0, MaxColumns),
			OverscanRows = Math.Max(0, OverscanRows),
			StartWithOffsetRow = StartWithOffsetRow,
			Padding = 8f
		};

	string GetPreviewText(object? item)
	{
		var candidate = BindingPathAccessor.GetValue(item, PreviewTextPath)?.ToString();
		if (string.IsNullOrWhiteSpace(candidate))
		{
			candidate = item?.ToString();
		}

		candidate ??= string.Empty;
		return candidate.Trim();
	}

	object? GetCommandParameter(object? item)
		=> string.IsNullOrWhiteSpace(CommandParameterPath)
			? item
			: BindingPathAccessor.GetValue(item, CommandParameterPath);

	Color GetFillColor(object? item)
		=> ResolveColor(item, FillColorPath, FillColor);

	Color GetSelectedFillColor(object? item)
		=> ResolveColor(item, SelectedFillColorPath, SelectedFillColor);

	Color GetStrokeColor(object? item)
		=> ResolveColor(item, StrokeColorPath, StrokeColor);

	Color GetTextColor(object? item)
		=> ResolveColor(item, TextColorPath, TextColor);

	Color ResolveColor(object? item, string? path, Color fallback)
	{
		var candidate = BindingPathAccessor.GetValue(item, path);
		if (candidate is Color color)
		{
			return color;
		}

		if (candidate is string text && !string.IsNullOrWhiteSpace(text))
		{
			try
			{
				return Color.FromArgb(text);
			}
			catch (ArgumentException)
			{
				return fallback;
			}
		}

		return fallback;
	}

	static IReadOnlyList<object?> MaterializeItems(IEnumerable? source)
	{
		if (source is null)
		{
			return Array.Empty<object?>();
		}

		var materialized = new List<object?>();
		foreach (var item in source)
		{
			materialized.Add(item);
		}

		return materialized;
	}
}
