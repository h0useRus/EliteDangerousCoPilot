using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NSW.EliteDangerous.Copilot.Helpers;

namespace NSW.EliteDangerous.Copilot.Controls
{
    public enum MajorFaction
    {
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
        public static readonly DependencyProperty ReputationBrushProperty = DependencyProperty.Register(nameof(ReputationBrush), typeof(Brush), typeof(MajorFactionPanel));
        public static readonly DependencyProperty RankVisibilityProperty = DependencyProperty.Register(nameof(RankVisibility), typeof(Visibility), typeof(MajorFactionPanel));
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
            new FrameworkPropertyMetadata(MajorFaction.Independent,
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

        protected Brush ReputationBrush
        {
            get => (Brush)GetValue(ReputationBrushProperty);
            private set => SetValue(ReputationBrushProperty, value);
        }

        protected Visibility RankVisibility
        {
            get => (Visibility)GetValue(RankVisibilityProperty);
            private set => SetValue(RankVisibilityProperty, value);
        }

        public MajorFactionPanel()
        {
            InitializeComponent();
            DataContext = this;
            RankValue = 0;
            ReputationValue = 2;
        }

        public void DataBind(int rank, int progress, int reputation)
        {
            RankValue = rank;
            ReputationValue = reputation;
        }

        private static void OnReputationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MajorFactionPanel c)
            {
                c.ReputationText = App.GetResource<string>($"Faction.Reputation.{e.NewValue}.Text");
                c.ReputationBrush =  new SolidColorBrush(e.NewValue switch
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
                c.Image = App.GetResource<ImageSource>(c.RankValue > 0
                    ? $"Faction.{c.Faction}.Navy.Icon"
                    : $"Faction.{c.Faction}.Icon");
                var text = App.GetResource<string>($"Faction.{c.Faction}.{c.RankValue}.Text");
                if (string.IsNullOrWhiteSpace(text))
                {
                    c.RankVisibility = Visibility.Collapsed;
                    c.Image = App.GetResource<ImageSource>($"Faction.{c.Faction}.Icon");
                }
                else
                {
                    c.Image = App.GetResource<ImageSource>($"Faction.{c.Faction}.Navy.Icon");
                    c.RankText = text;
                    c.RankVisibility = Visibility.Visible;
                }
            }
        }
    }
}
