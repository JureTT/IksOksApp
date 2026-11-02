namespace IksOks
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private async void OnIgracVsIgracClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage("igrac"));
        }

        private async void OnSlabiPCClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage("slabi"));
        }

        private async void OnNormalniPCClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage("normalni"));
        }

        private async void OnJakiPCClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage("jaki"));
        }

        private void OnIzlazClicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}