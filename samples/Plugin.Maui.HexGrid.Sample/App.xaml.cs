using Microsoft.Extensions.DependencyInjection;

namespace Plugin.Maui.HexGrid.Sample;

public partial class App : Application
{
	readonly MainPage mainPage;

	public App(MainPage mainPage)
	{
		InitializeComponent();
		this.mainPage = mainPage;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(mainPage);
	}
}