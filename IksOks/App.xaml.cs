namespace IksOks
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MenuPage())
            {
                BarBackgroundColor = Color.FromArgb("#0a0a1a"),
                BarTextColor = Color.FromArgb("#00d4ff")
            };
        }
    }
}