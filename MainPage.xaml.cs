using CommunityToolkit.Maui.Alerts;

namespace TestAppControls;

public partial class MainPage : ContentPage
{
	private string Text = "";

	public MainPage()
	{
		InitializeComponent();
		LabelEntry.TextChangedCallback = new Action<string, string>((oldvalue, newvalue) => 
		{
			Text = newvalue;
		});
	}

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
		Toast.Make(Text).Show();
    }
}


