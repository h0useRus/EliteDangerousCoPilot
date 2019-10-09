using System.Windows.Media;
using NSW.EliteDangerous.API;

namespace NSW.EliteDangerous.Copilot.Helpers
{
    public static class ImageHelper
    {
        public static ImageSource GetImage(CqcRank cqcRank) => AppRes.GetResource<ImageSource>($"Rank.Cqc.{(int)cqcRank}.Icon");
        public static ImageSource GetImage(TradeRank tradeRank) => AppRes.GetResource<ImageSource>($"Rank.Trade.{(int)tradeRank}.Icon");
        public static ImageSource GetImage(ExplorationRank explorationRank) => AppRes.GetResource<ImageSource>($"Rank.Exploration.{(int)explorationRank}.Icon");
        public static ImageSource GetImage(CombatRank combatRank) => AppRes.GetResource<ImageSource>($"Rank.Combat.{(int)combatRank}.Icon");
    }
}