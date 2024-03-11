using DemoApp.ViewModels;

namespace DemoApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel(Navigation);
    }
}