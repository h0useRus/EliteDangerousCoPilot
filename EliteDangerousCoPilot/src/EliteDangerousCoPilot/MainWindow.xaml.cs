using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace NSW.EliteDangerous.Copilot
{
    using API;
    using API.Events;
    using API.Exceptions;
    using API.Statuses;
    using Helpers;
    using Models;

    public partial class MainWindow : Window
    {
        private readonly IEliteDangerousAPI _api;
        private readonly List<ErrorModel> _errors = new List<ErrorModel>();
        private readonly List<string> _journal = new List<string>();

        public MainWindow(IEliteDangerousAPI api)
        {
            InitializeComponent();

            icApiErrors.ItemsSource = _errors;
            icJournal.ItemsSource = _journal;

            _api = api;
            _api.PlayerChanged += OnApiPlayerChanged;
            _api.ShipChanged += OnApiShipChanged;
            _api.LocationChanged += OnApiLocationChanged;
            _api.StatusChanged += OnApiStatusChanged;
            _api.Errors += OnApiErrors;
            _api.BeforeEvent += OnApiBeforeEvent;
        }

        private void OnApiBeforeEvent(object sender, OriginalEvent e) => Dispatcher?.Invoke(() => _journal.Add(e.Source));

        private void OnApiErrors(object sender, JournalException e) => Dispatcher?.Invoke(() => _errors.Add(new ErrorModel(e)));

        private void OnApiStatusChanged(object sender, ApiStatus e) =>
            Dispatcher?.Invoke(() =>
            {
                tbApiStatus.Text = TextHelper.GetText(e);
            });

        private void OnApiLocationChanged(object sender, LocationStatus e) =>
            Dispatcher?.Invoke(() =>
            {
                if (e.StarSystem != null)
                {
                    gbStarSystem.Visibility = Visibility.Visible;
                    tbStarSystem_Name.Text = e.StarSystem.Name;
                    tbStarSystem_Population.Text = e.StarSystem.Population.ToString("#,##0");
                    tbStarSystem_Government.Text = e.StarSystem.Government.ToString();
                    tbStarSystem_Security.Text = e.StarSystem.Security.ToString();
                    tbStarSystem_Economy.Text = e.StarSystem.Economy.ToString();
                    tbStarSystem_SecondaryEconomy.Text = e.StarSystem.SecondEconomy.ToString();
                }
                else
                {
                    gbStarSystem.Visibility = Visibility.Hidden;
                }

                if (e.Body != null)
                {
                    gbSystemBody.Visibility = Visibility.Visible;
                    tbSystemBody_Type.Text = TextHelper.GetText(e.Body.Type);
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
                    tbStation_Government.Text = e.Station.Government.ToString();
                    tbStation_Economy.Text = e.Station.Economy.ToString();
                }
                else
                {
                    gbStation.Visibility = Visibility.Hidden;
                }
            });

        private void OnApiShipChanged(object sender, ShipStatus e) =>
            Dispatcher?.Invoke(() =>
            {
                ShipType.Text = e.ShipType.ToString();
                ShipName.Text = !string.IsNullOrEmpty(e.Name) ? $"{e.Name} [{e.Identifier}]" : string.Empty;
                
            });

        private void OnApiPlayerChanged(object sender, PlayerStatus e) =>
            Dispatcher?.Invoke(() =>
            {
                if (string.IsNullOrWhiteSpace(e.Commander))
                {
                    PilotName.Text = TextHelper.NoInformation;
                    PilotName.Foreground = new SolidColorBrush(Colors.DarkGray);
                    PilotName.FontStyle = FontStyles.Italic;
                }
                else
                {
                    PilotName.Text = e.Commander;
                    PilotName.Foreground = new SolidColorBrush(SystemColors.ControlTextColor);
                    PilotName.FontStyle = FontStyles.Normal;
                }

                if (string.IsNullOrWhiteSpace(e.FrontierId))
                {
                    PilotLicense.Text = TextHelper.NoInformation;
                    PilotLicense.Foreground = new SolidColorBrush(Colors.DarkGray);
                    PilotLicense.FontStyle = FontStyles.Italic;
                }
                else
                {
                    PilotLicense.Text = e.FrontierId;
                    PilotLicense.Foreground = new SolidColorBrush(SystemColors.ControlTextColor);
                    PilotLicense.FontStyle = FontStyles.Normal;
                }

                PilotLegalState.Text = TextHelper.GetText(e.LegalState);
                PilotLegalState.Foreground = new SolidColorBrush(e.LegalState switch
                {
                    LegalState.Clean => Colors.Green,
                    LegalState.Hostile => Colors.Red,
                    LegalState.IllegalCargo => Colors.Orange,
                    LegalState.PassengerWanted => Colors.Orange,
                    LegalState.Speeding => Colors.Orange,
                    LegalState.Wanted => Colors.Salmon,
                    LegalState.Warrant => Colors.Magenta,
                    _ => Colors.Black
                });

                PilotLegalStateBorder.BorderBrush = PilotLegalState.Foreground;

                PilotRankCombat.DataBind((int)e.CombatRank.Rank, e.CombatRank.Progress);
                PilotRankExplore.DataBind((int)e.ExplorationRank.Rank, e.ExplorationRank.Progress);
                PilotRankTrade.DataBind((int)e.TradeRank.Rank, e.TradeRank.Progress);
                PilotRankCqc.DataBind((int)e.CqcRank.Rank, e.CqcRank.Progress);

                PilotRankEmpire.DataBind((int)e.EmpireRank.Rank, e.EmpireRank.Progress, (int)e.EmpireReputation.Reputation);
                PilotRankFederation.DataBind((int)e.FederationRank.Rank, e.FederationRank.Progress, (int)e.FederationReputation.Reputation);
                PilotRankAlliance.DataBind(0, 0, (int)e.AllianceReputation.Reputation);
                PilotRankIndependent.DataBind(0, 0, (int)e.IndependentReputation.Reputation);
            });

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
