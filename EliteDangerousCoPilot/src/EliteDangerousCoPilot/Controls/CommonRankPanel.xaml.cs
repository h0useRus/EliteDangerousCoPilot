using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NSW.EliteDangerous.Copilot.Helpers;

namespace NSW.EliteDangerous.Copilot.Controls
{
    public enum CommonRank
    {
        Unknown,
        Combat,
        Exploration,
        Trade,
        Cqc
    }

    public partial class CommonRankPanel : UserControl
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(CommonRankPanel));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(CommonRankPanel));
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(int), typeof(CommonRankPanel));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(int), typeof(CommonRankPanel),
            new FrameworkPropertyMetadata(-1,
                FrameworkPropertyMetadataOptions.AffectsRender, 
                OnValueChanged
            ));
        public static readonly DependencyProperty RankProperty = DependencyProperty.Register(nameof(Rank), typeof(CommonRank), typeof(CommonRankPanel),
            new FrameworkPropertyMetadata(CommonRank.Unknown,
                FrameworkPropertyMetadataOptions.AffectsRender, 
                OnValueChanged
            ));

        public CommonRank Rank
        {
            get => (CommonRank)GetValue(RankProperty);
            set => SetValue(RankProperty, value);
        }

        protected int Value
        {
            get => (int)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }

        protected int Progress
        {
            get => (int)GetValue(ProgressProperty);
            private set => SetValue(ProgressProperty, value);
        }

        protected ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            private set => SetValue(ImageProperty, value);
        }

        protected string Text
        {
            get => (string)GetValue(TextProperty);
            private set => SetValue(TextProperty, value);
        }

        public CommonRankPanel()
        {
            InitializeComponent();
            DataContext = this;
            Value = 0;
        }

        public void DataBind(int rankValue, int progress)
        {
            Value = rankValue;
            Progress = progress;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CommonRankPanel control)
            {
                control.Text = AppRes.GetResource<string>($"Rank.{control.Rank.ToString()}.{control.Value}.Text");
                control.Image = AppRes.GetResource<ImageSource>($"Rank.{control.Rank.ToString()}.{control.Value}.Icon");
            }
        }
    }
}
