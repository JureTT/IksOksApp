using IksOks.GameLogic;

namespace IksOks
{
    public partial class MainPage : ContentPage
    {
        private Igra _igra;
        private bool _xNaPotezu = true;
        private bool _krajanigre = false;
        private bool _xPocinje = true;
        private int _scoreX = 0;
        private int _scoreO = 0;
        private int _scoreDraw = 0;
        private string _vrstaProtivnika;
        private Igrac _protivnik;
        private Dictionary<string, Button> _gumbi;
        private bool _igracJeX = true; // true if the human player uses X, false if O

        public MainPage(string vrstaProtivnika)
        {
            InitializeComponent();
            _vrstaProtivnika = vrstaProtivnika;
            _xPocinje = true;
            _xNaPotezu = true;
            PostaviProtivnika();
            NovaIgra();
            InicijalizirajGumbe();
        }

        private void PostaviProtivnika()
        {
            switch (_vrstaProtivnika)
            {
                case "slabi":
                    _protivnik = new SlabiKomp { Ime = "Slabi PC", BiraniZnak = new Oks() };
                    break;
                case "normalni":
                    _protivnik = new NormalniKomp { Ime = "Normalni PC", BiraniZnak = new Oks() };
                    break;
                case "jaki":
                    _protivnik = new JakiKomp { Ime = "Jaki PC", BiraniZnak = new Oks() };
                    break;
                default:
                    _protivnik = null;
                    break;
            }
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

        private void NovaIgra()
        {
            _igra = new Igra();
            _igra.PokreniPartiju();
            _krajanigre = false;
            if (_igra != null) _xPocinje = !_xPocinje;
            _xNaPotezu = _xPocinje;

            // If playing vs computer, assign computer's symbol so the human alternates
            if (_protivnik != null)
            {
                // If X starts, human should be X and computer O. If O starts, human is O and computer X.
                _protivnik.BiraniZnak = _xPocinje ? new Iks() : new Oks();
                // If computer is O, human is X (true). If computer is X, human is O (false).
                _igracJeX = _protivnik.BiraniZnak is Oks;
            }
            else
            {
                // Local two-player: keep default (human X when X is to move)
                _igracJeX = true;
            }

            if (_gumbi != null)
            {
                foreach (var gumb in _gumbi.Values)
                {
                    gumb.Text = "";
                    gumb.BackgroundColor = Color.FromArgb("#0d0d2b");
                    gumb.BorderColor = Color.FromArgb("#1e1e4a");
                    gumb.TextColor = Colors.White;
                    gumb.IsEnabled = true;
                }
            }

            AzurirajStatus();

            // Ako PC počinje (O počinje nije moguće u ovoj verziji, X uvijek prvi)
            // ali ako ikad dodamo da PC igra X, ovdje bi se pozvao potez
        }

        private void AzurirajStatus()
        {
            if (_protivnik == null)
            {
                // Local two-player labels
                if (_xNaPotezu)
                {
                    StatusLabel.Text = "IGRAČ X";
                    StatusLabel.TextColor = Color.FromArgb("#00d4ff");
                }
                else
                {
                    StatusLabel.Text = "IGRAČ O";
                    StatusLabel.TextColor = Color.FromArgb("#bf00ff");
                }
            }
            else
            {
                // Versus computer: show whether it's the human's turn or the computer's
                bool jeTvojPotez = (_xNaPotezu && _igracJeX) || (!_xNaPotezu && !_igracJeX);
                if (jeTvojPotez)
                {
                    StatusLabel.Text = "TVOJ POTEZ";
                    StatusLabel.TextColor = Color.FromArgb("#00d4ff");
                }
                else
                {
                    StatusLabel.Text = _protivnik.Ime.ToUpper();
                    StatusLabel.TextColor = Color.FromArgb("#bf00ff");
                }
            }
        }

        private void OnPoljeClicked(object sender, EventArgs e)
        {
            if (_krajanigre) return;
            // If playing vs computer, allow clicks only when it's the human's turn (whether X or O)
            if (_protivnik != null)
            {
                bool jeTvojPotez = (_xNaPotezu && _igracJeX) || (!_xNaPotezu && !_igracJeX);
                if (!jeTvojPotez) return;
            }

            var button = (Button)sender;
            string polje = button.CommandParameter.ToString();

            Znak znak = _igracJeX ? (Znak)new Iks() : new Oks();
            znak.Upisi(polje);

            bool uspjeh = _igra.UnosZnaka(znak);
            if (!uspjeh)
            {
                StatusLabel.Text = "ZAUZETO!";
                StatusLabel.TextColor = Color.FromArgb("#ff4444");
                return;
            }
            if (_igracJeX)
            {
                button.Text = "X";
                button.TextColor = Color.FromArgb("#00d4ff");
                button.BorderColor = Color.FromArgb("#00d4ff44");
            }
            else
            {
                button.Text = "O";
                button.TextColor = Color.FromArgb("#bf00ff");
                button.BorderColor = Color.FromArgb("#bf00ff44");
            }
            button.IsEnabled = false;

            if (ProvjeriKrajIgre()) return;

            _xNaPotezu = false;

            if (_protivnik != null)
            {
                AzurirajStatus();
                // Mali delay pa potez PC-a
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(400);
                    PotezPC();
                });
            }
            else
            {
                AzurirajStatus();
            }
        }
        private void PotezPC()
        {
            if (_krajanigre) return;

            _protivnik.PovuciPotez(_igra);
            AzurirajGumbeNakonPC();

            if (ProvjeriKrajIgre()) return;

            _xNaPotezu = true;
            AzurirajStatus();
        }

        private void AzurirajGumbeNakonPC()
        {
            string[] stupci = { "A", "B", "C" };
            for (int red = 0; red < 3; red++)
            {
                for (int kol = 0; kol < 3; kol++)
                {
                    string polje = stupci[kol] + (red + 1).ToString();
                    if (!_gumbi.ContainsKey(polje)) continue;

                    var btn = _gumbi[polje];
                    string vrijednost = _igra.DohvatiPolje(red, kol);

                    if (vrijednost == "O" && btn.IsEnabled)
                    {
                        btn.Text = "O";
                        btn.TextColor = Color.FromArgb("#bf00ff");
                        btn.BorderColor = Color.FromArgb("#bf00ff44");
                        btn.IsEnabled = false;
                    }
                    else if (vrijednost == "X" && btn.IsEnabled)
                    {
                        btn.Text = "X";
                        btn.TextColor = Color.FromArgb("#00d4ff");
                        btn.BorderColor = Color.FromArgb("#00d4ff44");
                        btn.IsEnabled = false;
                    }
                }
            }
        }

        private bool ProvjeriKrajIgre()
        {
            string rezultat = _igra.ProvjeraTablice();

            if (rezultat == "X" || rezultat == "O")
            {
                _krajanigre = true;
                if (rezultat == "X")
                {
                    _scoreX++;
                    ScoreX.Text = _scoreX.ToString();
                    StatusLabel.Text = _protivnik == null ? "POBIJEDIO X! 🎉" : "POBIJEDIO SI! 🎉";
                    StatusLabel.TextColor = Color.FromArgb("#00d4ff");
                }
                else
                {
                    _scoreO++;
                    ScoreO.Text = _scoreO.ToString();
                    StatusLabel.Text = _protivnik == null ? "POBIJEDIO O! 🎉" : "POBIJEDIO PC! 😈";
                    StatusLabel.TextColor = Color.FromArgb("#bf00ff");
                }
                return true;
            }
            else if (_gumbi.Values.All(b => !b.IsEnabled))
            {
                _krajanigre = true;
                _scoreDraw++;
                ScoreDraw.Text = _scoreDraw.ToString();
                StatusLabel.Text = "REMI! 🤝";
                StatusLabel.TextColor = Color.FromArgb("#555577");
                return true;
            }
            return false;
        }

        private void OnNovaIgraClicked(object sender, EventArgs e)
        {
            NovaIgra();
        }

        private async void OnPromijeniProtivnikaClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnIzlazClicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}