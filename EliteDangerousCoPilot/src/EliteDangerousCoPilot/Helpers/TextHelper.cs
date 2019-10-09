using NSW.EliteDangerous.API;

namespace NSW.EliteDangerous.Copilot.Helpers
{
    public static class TextHelper
    {
        public static string Yes => AppRes.GetResource<string>("Strings.Yes");
        public static string No => AppRes.GetResource<string>("Strings.No");

        public static string GetText(ApiStatus apiStatus) => AppRes.GetResource<string>($"Api.Status.{(int)apiStatus}.Text");
        public static string GetText(BodyType bodyType) => AppRes.GetResource<string>($"Api.BodyType.{(int)bodyType}.Text");
        public static string GetText(CqcRank cqcRank) => AppRes.GetResource<string>($"Rank.Cqc.{(int)cqcRank}.Text");
        public static string GetText(TradeRank tradeRank) => AppRes.GetResource<string>($"Rank.Trade.{(int)tradeRank}.Text");
        public static string GetText(ExplorationRank explorationRank) => AppRes.GetResource<string>($"Rank.Exploration.{(int)explorationRank}.Text");
        public static string GetText(CombatRank combatRank) => AppRes.GetResource<string>($"Rank.Combat.{(int)combatRank}.Text");
        public static string GetText(FederationRank federationRank) => AppRes.GetResource<string>($"Faction.Federation.{(int)federationRank}.Text");
        public static string GetText(EmpireRank empireRank) => AppRes.GetResource<string>($"Faction.Empire.{(int)empireRank}.Text");
    }
}