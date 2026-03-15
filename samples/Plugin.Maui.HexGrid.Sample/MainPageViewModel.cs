using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Plugin.Maui.HexGrid.Sample;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
	HexSampleItem? selectedItem;
	string lastTappedText = "Select a value to highlight it.";

	public MainPageViewModel()
	{
		Items = new ObservableCollection<HexSampleItem>(CreateItems());
		HexTappedCommand = new Command<HexSampleItem?>(OnHexTapped);
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	public ObservableCollection<HexSampleItem> Items { get; }

	public string FillColorPath { get; } = nameof(HexSampleItem.FillColor);

	public string SelectedFillColorPath { get; } = nameof(HexSampleItem.SelectedFillColor);

	public string StrokeColorPath { get; } = nameof(HexSampleItem.StrokeColor);

	public string TextColorPath { get; } = nameof(HexSampleItem.TextColor);

	public Color DefaultFillColor { get; } = Color.FromArgb("#5B34F2");

	public Color DefaultSelectedFillColor { get; } = Color.FromArgb("#4522C8");

	public Color DefaultStrokeColor { get; } = Colors.White;

	public Color DefaultTextColor { get; } = Colors.White;

	public ICommand HexTappedCommand { get; }

	public HexSampleItem? SelectedItem
	{
		get => selectedItem;
		set
		{
			if (ReferenceEquals(selectedItem, value))
			{
				return;
			}

			selectedItem = value;
			OnPropertyChanged();
		}
	}

	public string LastTappedText
	{
		get => lastTappedText;
		private set
		{
			if (lastTappedText == value)
			{
				return;
			}

			lastTappedText = value;
			OnPropertyChanged();
		}
	}

	void OnHexTapped(HexSampleItem? item)
	{
		if (item is null)
		{
			return;
		}

		SelectedItem = item;
		LastTappedText = item.Label;
	}

	IEnumerable<HexSampleItem> CreateItems()
	{
		foreach (var label in new[]
		{
			"AA",
			"BB",
			"CC",
			"DD",
			"EE",			
			"FF",
			"GG",
			"HH",
			"II",			
			"JJ",
			"KK",
			"LL",
			"MM",			
			"NN",
			"OO",
			"PP",
			"QQ",
			"RR",
			"SS"
		})
		{
			yield return new HexSampleItem
			{
				Label = label,
				Preview = label,
				FillColor = DefaultFillColor,
				SelectedFillColor = DefaultSelectedFillColor,
				StrokeColor = DefaultStrokeColor,
				TextColor = DefaultTextColor
			};
		}
	}

	void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public sealed class HexSampleItem
{
	public required string Label { get; init; }

	public required string Preview { get; init; }

	public required Color FillColor { get; init; }

	public required Color SelectedFillColor { get; init; }

	public required Color StrokeColor { get; init; }

	public required Color TextColor { get; init; }
}
