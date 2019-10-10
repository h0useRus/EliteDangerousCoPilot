using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NSW.EliteDangerous.Copilot.Helpers;

namespace NSW.EliteDangerous.Copilot.Controls
{
    public enum MajorFaction
    {
        Unknown,
        Federation,
        Empire,
        Alliance,
        Independent
    }

    public partial class MajorFactionPanel : UserControl
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(MajorFactionPanel));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(RankText), typeof(string), typeof(MajorFactionPanel));
        public static readonly DependencyProperty ReputationProperty = DependencyProperty.Register(nameof(ReputationText), typeof(string), typeof(MajorFactionPanel));
        public static readonly DependencyProperty RankValueProperty = DependencyProperty.Register(nameof(RankValue), typeof(int), typeof(MajorFactionPanel),
            new FrameworkPropertyMetadata(-1,
                FrameworkPropertyMetadataOptions.AffectsRender, 
                OnFactionChanged
            ));

        public static readonly DependencyProperty ReputationValueProperty = DependencyProperty.Register(nameof(ReputationValue), typeof(int), typeof(MajorFactionPanel),
            new FrameworkPropertyMetadata(-1,
                FrameworkPropertyMetadataOptions.AffectsRender, 
                OnReputationValueChanged
            ));

        public static readonly DependencyProperty FactionProperty = DependencyProperty.Register(nameof(Faction), typeof(MajorFaction), typeof(MajorFactionPanel),
            new FrameworkPropertyMetadata(MajorFaction.Unknown,
                FrameworkPropertyMetadataOptions.AffectsRender, 
                OnFactionChanged
            ));

        public MajorFaction Faction
        {
            get => (MajorFaction)GetValue(FactionProperty);
            set => SetValue(FactionProperty, value);
        }

        protected int RankValue
        {
            get => (int)GetValue(RankValueProperty);
            private set => SetValue(RankValueProperty, value);
        }

        protected int ReputationValue
        {
            get => (int)GetValue(ReputationValueProperty);
            private set => SetValue(ReputationValueProperty, value);
        }

        protected ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            private set => SetValue(ImageProperty, value);
        }

        protected string RankText
        {
            get => (string)GetValue(TextProperty);
            private set => SetValue(TextProperty, value);
        }
        protected string ReputationText
        {
            get => (string)GetValue(ReputationProperty);
            private set => SetValue(ReputationProperty, value);
        }

        public MajorFactionPanel()
        {
            InitializeComponent();
            DataContext = this;
            RankValue = 0;
            ReputationValue = 2;
        }

        public void DataBind(int rank, int reputation)
        {
            RankValue = rank;
            ReputationValue = reputation;
        }

        private static void OnReputationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MajorFactionPanel c)
            {
                c.ReputationText = AppRes.GetResource<string>($"Faction.Reputation.{e.NewValue}.Text");
                c.tbRepuation.Foreground =  new SolidColorBrush(e.NewValue switch
                {
                    0 => Colors.Red,
                    1 => Colors.Orange,
                    2 => Colors.DarkGray,
                    3 => Colors.Green,
                    4 => Colors.Blue,
                    5 => Colors.Magenta,
                    _ => Colors.Black
                });
            }
        }

        private static void OnFactionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MajorFactionPanel c)
            {
                c.Image = AppRes.GetResource<ImageSource>(c.RankValue > 0
                    ? $"Faction.{c.Faction}.Navy.Icon"
                    : $"Faction.{c.Faction}.Icon");
                var text = AppRes.GetResource<string>($"Faction.{c.Faction}.{c.RankValue}.Text");
                if (string.IsNullOrWhiteSpace(text))
                {
                    c.tbRank.Visibility = Visibility.Collapsed;
                    c.Image = AppRes.GetResource<ImageSource>($"Faction.{c.Faction}.Icon");
                }
                else
                {
                    c.Image = AppRes.GetResource<ImageSource>($"Faction.{c.Faction}.Navy.Icon");
                    c.RankText = text;
                    c.tbRank.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
