[![NuGet](https://img.shields.io/nuget/v/Vapolia.MauiGesture.svg?style=for-the-badge)](https://www.nuget.org/packages/Vapolia.MauiGesture/)  
[![NuGet](https://img.shields.io/nuget/vpre/Vapolia.MauiGesture.svg?style=for-the-badge)](https://www.nuget.org/packages/Vapolia.MauiGesture/)  
![Nuget](https://img.shields.io/nuget/dt/Vapolia.MauiGesture)

# Supported Platforms

iOS, Android, Windows, Mac

# Maui Gesture Effects

Add "advanced" gestures to Maui. Available on all views.
Most gesture commands include the event position.

```xaml
    <Label Text="Click here" IsEnabled="True" ui:Gesture.TapCommand="{Binding OpenLinkCommand}" />
```
Or in code:
```csharp
    var label = new Label();
    Gesture.SetTapCommand(label, new Command(() => { /*your code*/ }));
```

# Quick start
Add the above nuget package to your Maui project   
then add this line to your maui app builder:

```c#
using MauiGestures;
...
builder.AddAdvancedGestures();
``` 

The views on which the gesture is applied should have the property `IsEnabled="True"` and `InputTransparent="False"` which activates user interaction on them.

# Examples

Add Gesture.TapCommand on any supported xaml view:
```xaml
        <StackLayout ui:Gesture.TapCommand="{Binding OpenLinkCommand}">
            <Label Text="1.Tap this to open an url"  />
        </StackLayout>
```
Declare the corresponding namespace:
```xaml
    <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             ...
             xmlns:ui="clr-namespace:MauiGestures;assembly=MauiGestures"
    >
```
And in the viewmodel:
   ```csharp     
    public Command OpenLinkCommand => new Command(() =>
    {
        //do something
    });
```
# Supported Gestures

 *  `TapCommand (ICommand)`
 *  `DoubleTapCommand (ICommand)`
 *  `PanCommand (ICommand)`
 *  `LongPressCommand (ICommand)`
 *  `TapPointCommand (ICommand or Command<Point>)` where point is the absolute tap position relative to the view
 *  `DoubleTapPoinCommand (ICommand or Command<Point>)` where point is the absolute double tap position relative to the view
 *  `PanPointCommand (ICommand or Command<PanEventArgs>)` where point is the absolute position relative to the view
 *  `LongPressPointCommand (ICommand or Command<Point>) ` where point is the absolute tap position relative to the view
 *  `SwipeLeftCommand`
 *  `SwipeRightCommand`
 *  `SwipeTopCommand`
 *  `SwipeBottomCommand`
 *  `PinchCommand (Command<PinchEventArgs>)` where `PinchEventArg` contains `StartingPoints`, `CurrentPoints`, `Center`, `Scale`, `RotationRadians`, `RotationDegrees`, `Status`
 
 Properties:
 
 * `IsPanImmediate` Set to true to receive the PanCommand or PanPointCommand event on touch down, instead of after a minimum move distance. Default to false.
 
# Examples

## Some commands in XAML

```xml
<StackLayout ui:Gesture.TapCommand="{Binding OpenCommand}" IsEnabled="True">
    <Label Text="1.Tap this text to open an url" />
</StackLayout>

<StackLayout ui:Gesture.DoubleTapPointCommand="{Binding OpenPointCommand}" IsEnabled="True">
    <Label Text="2.Double tap this text to open an url" />
</StackLayout>

<BoxView
    ui:Gesture.PanPointCommand="{Binding PanPointCommand}"
    HeightRequest="200" WidthRequest="300"
    InputTransparent="False"
    IsEnabled="True"
     />
```

In the viewmodel:

```csharp
public ICommand OpenCommand => new Command(async () =>
{
   //...
});

public ICommand OpenPointCommand => new Command<Point>(point =>
{
    PanX = point.X;
    PanY = point.Y;
    //...
});

public ICommand PanPointCommand => new Command<PanEventArgs>(args =>
{
    var point = args.Point;
    PanX = point.X;
    PanY = point.Y;
    //...
});

``` 

## Exemple on a Grid containing an horizontal slider (set value on tap)

```csharp
//Tap anywhere to set value
Gesture.SetTapPointCommand(this, new Command<Point>(pt =>
{
    var delta = (pt.X - Padding.Left) / (Width - Padding.Left - Padding.Right);
    if(delta<0 || delta>1)
        return;
    Value = (int)Math.Round((Maximum - Minimum) * delta);
}));
```
        

# Limitations

Only commands are supported (PR welcome for events). No .NET events. So you must use the MVVM pattern.

Swipe commands are not supported on Windows because of a curious bug (event not received). If you find it, notify me!
PinchCommand is not supported (yet) on Windows. PR welcome.

If your command is not receiving events, make sure that:
- you used the correct handler. Ie: the `LongPressPointCommand` should be `new Command<Point>(pt => ...)`
- you set `IsEnabled="True"` and `InputTransparent="False"` on the element

Windows requires the fall creator update.  
