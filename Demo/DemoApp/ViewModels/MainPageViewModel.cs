using System.Windows.Input;
using MauiGestures;
using Vapolia.UserInteraction;
using Point = Microsoft.Maui.Graphics.Point;

namespace DemoApp.ViewModels;

public class MainPageViewModel : BindableObject
{
    private readonly INavigation navigation;
    private Point pan, pinch;
    private GestureStatus? panStatus;
    private double rotation, scale;

    public Point Pan { get => pan; set { pan = value; OnPropertyChanged(); } }
    public GestureStatus? PanStatus { get => panStatus; set { panStatus = value; OnPropertyChanged(); } }
    public Point Pinch { get => pinch; set { pinch = value; OnPropertyChanged(); } }
    public double Rotation { get => rotation; set { rotation = value; OnPropertyChanged(); } }
    public double Scale { get => scale; set { scale = value; OnPropertyChanged(); } }

    public MainPageViewModel(INavigation navigation)
    {
        this.navigation = navigation;
    }

        
    public ICommand PanPointCommand => new Command<PanEventArgs>(args =>
    {
        var point = args.Point;
        Pan = point;
        PanStatus = args.Status;
    });
        
    public ICommand PinchCommand => new Command<PinchEventArgs>(args =>
    {
        Pinch = args.Center;
        Rotation = args.RotationDegrees;
        Scale = args.Scale;
    });

    public ICommand TextSwipedCommand => new Command(async () =>
    {
        await UserInteraction.Alert("Swipe gesture detected", "Item swiped");

        // await navigation.PushAsync(new ContentPage {
        //     Title = "Web",
        //     Content = new Grid {
        //         BackgroundColor = Yellow,
        //         Children = { new WebView { Source = new UrlWebViewSource { Url = "https://vapolia.fr" }, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill} }}});
    });
    
    public ICommand OpenVapoliaCommand => new Command(async () =>
    {
        await UserInteraction.Alert("Open Vapolia command received", "Item tapped");

        // await navigation.PushAsync(new ContentPage {
        //     Title = "Web",
        //     Content = new Grid {
        //         BackgroundColor = Yellow,
        //         Children = { new WebView { Source = new UrlWebViewSource { Url = "https://vapolia.fr" }, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill} }}});
    });

        
    public ICommand OpenVapoliaPointCommand => new Command<PointEventArgs>(async args =>
    {
        Pan = args.Point;
        var absXy = args.GetCoordinates();
        
        await UserInteraction.Alert($"Open Vapolia Point command received at position ({args.Point.X:0.0},{args.Point.Y:0.0}) relative to the element, and ({absXy.X:0.0},{absXy.Y:0.0}) relative to the root view", "Item tapped");

        await UserInteraction.Menu(default, position: args.GetAbsoluteBoundsF(), cancelButton: "Cancel", otherButtons: ["Do something"]);
    });
}