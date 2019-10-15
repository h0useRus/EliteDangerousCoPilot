using NSW.EliteDangerous.API;

namespace NSW.EliteDangerous.Copilot.Helpers
{
    public static class TextHelper
    {
        public static string Yes => App.GetResource<string>("Strings.Yes");
        public static string No => App.GetResource<string>("Strings.No");
        public static string NoInformation => App.GetResource<string>("Strings.NoInformation");

        public static string GetText(ApiStatus apiStatus) => App.GetResource<string>($"Api.Status.{(int)apiStatus}.Text");
        public static string GetText(BodyType bodyType) => App.GetResource<string>($"Api.BodyType.{(int)bodyType}.Text");
        public static string GetText(LegalState legalState) => App.GetResource<string>($"Api.LegalState.{(int)legalState}.Text");
        public static string GetText(CqcRank cqcRank) => App.GetResource<string>($"Rank.Cqc.{(int)cqcRank}.Text");
        public static string GetText(TradeRank tradeRank) => App.GetResource<string>($"Rank.Trade.{(int)tradeRank}.Text");
        public static string GetText(ExplorationRank explorationRank) => App.GetResource<string>($"Rank.Exploration.{(int)explorationRank}.Text");
        public static string GetText(CombatRank combatRank) => App.GetResource<string>($"Rank.Combat.{(int)combatRank}.Text");
        public static string GetText(FederationRank federationRank) => App.GetResource<string>($"Faction.Federation.{(int)federationRank}.Text");
        public static string GetText(EmpireRank empireRank) => App.GetResource<string>($"Faction.Empire.{(int)empireRank}.Text");
    }
}