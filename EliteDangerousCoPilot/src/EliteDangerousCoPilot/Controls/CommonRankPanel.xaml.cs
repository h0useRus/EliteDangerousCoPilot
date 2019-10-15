using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NSW.EliteDangerous.Copilot.Helpers;

namespace NSW.EliteDangerous.Copilot.Controls
{
    public enum CommonRank
    {
        Combat,
        Exploration,
        Trade,
        Cqc
    }

    public partial class CommonRankPanel : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(RankValue), typeof(int), typeof(CommonRankPanel),
            new FrameworkPropertyMetadata(-1,
                FrameworkPropertyMetadataOptions.AffectsRender, 
                OnValueChanged
            ));
        public static readonly DependencyProperty RankProperty = DependencyProperty.Register(nameof(Rank), typeof(CommonRank), typeof(CommonRankPanel),
            new FrameworkPropertyMetadata(CommonRank.Combat,
                FrameworkPropertyMetadataOptions.AffectsRender, 
                OnValueChanged
            ));

        public CommonRank Rank
        {
            get => (CommonRank)GetValue(RankProperty);
            set => SetValue(RankProperty, value);
        }

        protected int RankValue
        {
            get => (int)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }

        public CommonRankPanel()
        {
            InitializeComponent();
            DataContext = this;
            RankValue = 0;
        }

        public void DataBind(int rankValue, int progress)
        {
            RankValue = rankValue;
            RankProgress.Value = progress;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CommonRankPanel control)
            {
                control.RankText.Text = App.GetResource<string>($"Rank.{control.Rank.ToString()}.{control.RankValue}.Text");
                control.RankIcon.Source = App.GetResource<DrawingImage>($"Rank.{control.Rank.ToString()}.{control.RankValue}.DrawingImage");
            }
        }
    }
}
