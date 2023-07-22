using Microsoft.Maui.Controls.Shapes;

namespace TestAppControls;

public class LabeledEntry : ContentView
{
	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
		nameof(PlaceholderProperty),
		typeof(string),
		typeof(LabeledEntry),
		"",
		propertyChanged: (bindable, oldval, newval) =>
		{
			LabeledEntry view = (LabeledEntry)bindable;
			view.PlaceholderLabel.Text = (string)newval;
		});

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}

	public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
		nameof(KeyboardProperty),
		typeof(Keyboard),
		typeof(LabeledEntry),
		Keyboard.Default,
		propertyChanged: (bindable, oldval, newval) =>
		{
			LabeledEntry view = (LabeledEntry)bindable;
			view.TextEntry.Keyboard = (Keyboard)newval;
		});

	public Keyboard Keyboard
	{
		get => (Keyboard)GetValue(KeyboardProperty);
		set => SetValue(KeyboardProperty, value);
	}

	private Border EntryFrame = new Border
	{
		Padding = 0,
		Stroke = Colors.Gray,
		ZIndex = 0,
		StrokeShape = new RoundRectangle
		{
			CornerRadius = 5
		}
	};

	private Border PlaceholderBorder = new Border
	{
		VerticalOptions = LayoutOptions.Center,
		HorizontalOptions = LayoutOptions.Start,
		Stroke = Colors.Transparent,
        Margin = new Thickness(8, 0, 0, 0),
        StrokeShape = new RoundRectangle
		{
			CornerRadius = new CornerRadius(5,5,0,0)
		}
	};

	private Label PlaceholderLabel = new Label
	{
		VerticalOptions = LayoutOptions.Center,
		HorizontalOptions = LayoutOptions.Center,
		Padding = new Thickness(4,0,4,0)
	};

	private Entry TextEntry = new Entry
	{
		Margin = new Thickness(8,0,8,0)
	};

	public Action<string, string> TextChangedCallback;

	public LabeledEntry()
	{
		TextEntry.TextChanged += TextChanged;

        TextEntry.Focused += TextEntry_Focused;
        TextEntry.Unfocused += TextEntry_Unfocused;

		EntryFrame.Content = TextEntry;
		PlaceholderBorder.Content = PlaceholderLabel;

		PlaceholderBorder.GestureRecognizers.Add(new TapGestureRecognizer
		{
			NumberOfTapsRequired = 1,
			Command = new Command(() => 
			{
				TextEntry.Focus();
			})
		});

		Content = new Grid
		{
			HeightRequest = 50,
            Padding = new Thickness(4),
            Children =
			{
                EntryFrame,
                PlaceholderBorder
			}
		};
	}

    private void TextEntry_Unfocused(object sender, FocusEventArgs e)
    {
		if (!string.IsNullOrEmpty(TextEntry.Text))
			return;

        PlaceholderBorder.TranslateTo(0, 0, 250, Easing.Linear);
        PlaceholderBorder.BackgroundColor = Colors.Transparent;
    }

    private void TextEntry_Focused(object sender, FocusEventArgs e)
    {
        int goUpBy = (int)(EntryFrame.Height / 2) + (int)(PlaceholderLabel.Height / 2);
        PlaceholderBorder.TranslateTo(0, -goUpBy, 250, Easing.Linear);
    }

	private void TextChanged(object sender, TextChangedEventArgs e)
	{
		TextChangedCallback?.Invoke(e.OldTextValue, e.NewTextValue);
	}
}
