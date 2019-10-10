using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

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
                    tbStarSystem_Government.Text = e.StarSystem.Government ?? TextHelper.No;
                    tbStarSystem_Security.Text = e.StarSystem.Security ?? TextHelper.No;
                    tbStarSystem_Economy.Text = e.StarSystem.Economy ?? TextHelper.No;
                    tbStarSystem_SecondaryEconomy.Text = e.StarSystem.SecondEconomy ?? TextHelper.No;
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
                    tbStation_Government.Text = e.Station.Government;
                    tbStation_Economy.Text = e.Station.Economy;
                }
                else
                {
                    gbStation.Visibility = Visibility.Hidden;
                }
            });

        private void OnApiShipChanged(object sender, ShipStatus e) =>
            Dispatcher?.Invoke(() =>
            {
                tbShip_Type.Text = e.ShipType;
                tbShip_Name.Text = !string.IsNullOrEmpty(e.Name) ? $"{e.Name} [{e.Identifier}]" : string.Empty;
            });

        private void OnApiPlayerChanged(object sender, PlayerStatus e) =>
            Dispatcher?.Invoke(() =>
            {
                tbPilot_Name.Text = e.Commander;
                tbPilot_ID.Text = e.FrontierId;
                tbPilot_LegalState.Text = TextHelper.GetText(e.LegalState);
                         
                icPilot_Ranks.ItemsSource = new List<PlayerRank>
                {
                    new PlayerRank
                    {
                        Image = ImageHelper.GetImage(e.CombatRank.Rank),
                        Name = TextHelper.GetText(e.CombatRank.Rank),
                        Progress = e.CombatRank.Progress
                    },
                    new PlayerRank
                    {
                        Image = ImageHelper.GetImage(e.ExplorationRank.Rank),
                        Name = TextHelper.GetText(e.ExplorationRank.Rank),
                        Progress = e.ExplorationRank.Progress
                    },
                    new PlayerRank
                    {
                        Image = ImageHelper.GetImage(e.TradeRank.Rank),
                        Name = TextHelper.GetText(e.TradeRank.Rank),
                        Progress = e.TradeRank.Progress
                    },
                    new PlayerRank
                    {
                        Image = ImageHelper.GetImage(e.CqcRank.Rank),
                        Name = TextHelper.GetText(e.CqcRank.Rank),
                        Progress = e.CqcRank.Progress
                    }
                };

                
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

        private int rank = 0;
        private int rep = 0;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            rank++;
            if (rank > 14)
            {
                rank = 0;
            }

            rep++;
            if (rep > 5)
            {
                rep = 0;
            }

            mfpAlliance.DataBind(rank,rep);
            mfpEmpire.DataBind(rank,rep);
            mfpFederation.DataBind(rank,rep);
            mfpIndependent.DataBind(rank,rep);
        }
    }
}
