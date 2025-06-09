using CommunityToolkit.Maui.Views;

namespace TestMulti.Controls;

public partial class AddThemePopup : Popup
{
    public string Result { get; private set; }

    public AddThemePopup(string title)
    {
        InitializeComponent();
        TitleLabel.Text = title;
    }

    private void OnOkClicked(object sender, EventArgs e)
    {
        Result = ThemeEntry.Text;
        Close(Result);
    }
}