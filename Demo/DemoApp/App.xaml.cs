namespace DemoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = PlatformAppTheme;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage())
            {
                Title = "DemoApp",
                Height = 800,
                Width = 600
            };
        }
    }
}
