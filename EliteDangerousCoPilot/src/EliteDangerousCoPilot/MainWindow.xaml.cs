using System;
using System.ComponentModel;
using System.Windows;
using NSW.EliteDangerous.API;
using NSW.EliteDangerous.API.Statuses;

namespace NSW.EliteDangerous.Copilot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEliteDangerousAPI _api;
        public MainWindow(IEliteDangerousAPI api)
        {
            InitializeComponent();
            _api = api;
            _api.PlayerChanged += OnApiPlayerChanged;
            _api.ShipChanged += OnApiShipChanged;
            _api.LocationChanged += OnApiLocationChanged;
            _api.StatusChanged += OnApiStatusChanged;
        }

        private void OnApiStatusChanged(object sender, ApiStatus e)
        {
            tbApiStatus.Text = e switch
            {
                ApiStatus.Pending => "Поиск журнала...",
                ApiStatus.Running => "Журнал обрабатывается",
                ApiStatus.Stopped => "Обработка журнала выключена"
            };
        }

        private void OnApiLocationChanged(object sender, LocationStatus e)
        {
            if (e.StarSystem != null)
            {
                gbStarSystem.Visibility = Visibility.Visible;
                tbStarSystem_Name.Text = e.StarSystem.Name;
                tbStarSystem_Population.Text = e.StarSystem.Population.ToString("N");
                tbStarSystem_Government.Text = e.StarSystem.Government ?? "Нет";
                tbStarSystem_Security.Text = e.StarSystem.Security ?? "Нет";
                tbStarSystem_Economy.Text = e.StarSystem.Economy ?? "Нет";
                tbStarSystem_SecondEconomy.Text = e.StarSystem.SecondEconomy ?? "Нет";
            }
            else
            {
                gbStarSystem.Visibility = Visibility.Hidden;
            }

            if (e.Body != null)
            {
                gbSystemBody.Visibility = Visibility.Visible;
                tbSystemBody_Type.Text = GetBodyType(e.Body.Type);
                tbSystemBody_Name.Text = e.Body.Name;
            }
            else
            {
                gbSystemBody.Visibility = Visibility.Hidden;
            }

            if (e.Station != null)
            {
                gbStation.Visibility = Visibility.Visible;
                tbStation_Name.Text = e.Station.Name;
                tbStation_Type.Text = e.Station.Type;
                tbStation_Government.Text = e.Station.Government;
                tbStation_Economy.Text = e.Station.Economy;
            }
            else
            {
                gbStation.Visibility = Visibility.Hidden;
            }
        }

        private static string GetBodyType(BodyType bodyType)
            => bodyType switch
            {
                BodyType.AsteroidCluster => "Астероидный кластер",
                BodyType.Planet => "Планета",
                BodyType.PlanetaryRing => "Планетарное кольцо",
                BodyType.Star => "Звезда",
                BodyType.Station => "Станция",
                BodyType.StellarRing => "Звездное кольцо",
                _ => "Не определено",
            };

        private void OnApiShipChanged(object sender, ShipStatus e)
        {
            tbShip_Type.Text = e.ShipType;
            tbShip_Name.Text = !string.IsNullOrEmpty(e.Name) ? $"{e.Name} [{e.Identifier}]" : string.Empty;
        }

        private void OnApiPlayerChanged(object sender, PlayerStatus e)
        {
            tbPilot_Name.Text = e.Commander;
            tbPilot_ID.Text = e.FrontierId;
        }

        private void OnMainWindowContentRendered(object sender, EventArgs e)
        {
            _api.Start();
            OnApiStatusChanged(this, _api.Status);
            OnApiPlayerChanged(this, _api.Player);
            OnApiShipChanged(this, _api.Ship);
            OnApiLocationChanged(this, _api.Location);
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e) => _api.Stop();
    }
}
