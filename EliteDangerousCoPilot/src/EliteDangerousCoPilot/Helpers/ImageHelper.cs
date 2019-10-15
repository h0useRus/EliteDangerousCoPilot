using System.Windows.Media;
using Accessibility;
using NSW.EliteDangerous.API;

namespace NSW.EliteDangerous.Copilot.Helpers
{
    public static class ImageHelper
    {
        public static DrawingImage GetVector(ShipModel shipModel) => App.GetResource<DrawingImage>($"Ship.{shipModel}.DrawingImage");
        public static DrawingImage GetVector(CombatRank combatRank) => App.GetResource<DrawingImage>($"Rank.Combat.{(int)combatRank}.DrawingImage");
        public static DrawingImage GetVector(ExplorationRank explorationRank) => App.GetResource<DrawingImage>($"Rank.Exploration.{(int)explorationRank}.DrawingImage");
        public static DrawingImage GetVector(TradeRank tradeRank) => App.GetResource<DrawingImage>($"Rank.Trade.{(int)tradeRank}.DrawingImage");
        public static DrawingImage GetVector(CqcRank cqcRank) => App.GetResource<DrawingImage>($"Rank.Cqc.{(int)cqcRank}.DrawingImage");

        public static DrawingImage GetVector(StationType stationType)
        {
            var imageKey = stationType switch
            {
                StationType.CoriolisStarport => "CoriolisStarport",
                StationType.OcellusStarport => "OcellusStarport",
                StationType.OrbisStarport => "OrbisStarport",
                StationType.PlanetaryOutpost => "PlanetaryOutpost",
                StationType.PlanetaryPort => "PlanetaryPort",
                StationType.AsteroidBase => "AsteroidBase",
                StationType.MegaShip => "MegaShip",
                StationType.CivilianMegaShip => "MegaShip",
                _ => "Outpost"
            };
            return App.GetResource<DrawingImage>($"Station.{imageKey}.DrawingImage");
        }
    }
}