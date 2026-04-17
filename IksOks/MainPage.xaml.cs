using IksOks;
using IksOks.GameLogic;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace IksOks
{
    public partial class MainPage : ContentPage
    {
        private Igra _igra;
        private bool _xNaPotezu = true;
        private bool _krajanigre = false;
        private Dictionary<string, Button> _gumbi;

        public MainPage()
        {
            InitializeComponent();
            _igra = new Igra();
            _igra.PokreniPartiju();
            InicijalizirajGumbe();
        }

        private void InicijalizirajGumbe()
        {
            _gumbi = new Dictionary<string, Button>
            {
                {"A1", B00}, {"B1", B01}, {"C1", B02},
                {"A2", B10}, {"B2", B11}, {"C2", B12},
                {"A3", B20}, {"B3", B21}, {"C3", B22}
            };
        }

        private void OnPoljeClicked(object sender, EventArgs e)
        {
            if (_krajanigre) return;

            var button = (Button)sender;
            string polje = button.CommandParameter.ToString();

            string vrsta = _xNaPotezu ? "X" : "O";
            Znak znak;
            if (_xNaPotezu)
            {
                znak = new Iks();
                znak.Upisi(polje);
            }
            else
            {
                znak = new Oks();
                znak.Upisi(polje);
            }

            bool uspjeh = _igra.UnosZnaka(znak);

            if (!uspjeh)
            {
                StatusLabel.Text = "To polje je zauzeto!";
                return;
            }

            button.Text = vrsta;
            button.BackgroundColor = _xNaPotezu ? Colors.LightBlue : Colors.LightSalmon;
            button.IsEnabled = false;

            string rezultat = _igra.ProvjeraTablice();

            if (rezultat == "X" || rezultat == "O")
            {
                StatusLabel.Text = $"Pobijedio je: {rezultat} 🎉";
                _krajanigre = true;
            }
            else if (ProvjeraRemi())
            {
                StatusLabel.Text = "Remi! 🤝";
                _krajanigre = true;
            }
            else
            {
                _xNaPotezu = !_xNaPotezu;
                StatusLabel.Text = $"Na potezu je: {(_xNaPotezu ? "X" : "O")}";
            }
        }

        private bool ProvjeraRemi()
        {
            return _gumbi.Values.All(b => !b.IsEnabled || b.Text != null && b.Text != "");
        }

        private void OnNovaIgraClicked(object sender, EventArgs e)
        {
            _igra = new Igra();
            _igra.PokreniPartiju();
            _xNaPotezu = true;
            _krajanigre = false;
            StatusLabel.Text = "Na potezu je: X";

            foreach (var gumb in _gumbi.Values)
            {
                gumb.Text = "";
                gumb.BackgroundColor = Colors.LightGray;
                gumb.IsEnabled = true;
            }
        }
    }
}