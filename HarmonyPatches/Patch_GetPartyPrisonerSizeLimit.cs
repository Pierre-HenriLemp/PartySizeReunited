using HarmonyLib;
using PartySizeReunited.Models;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace PartySizeReunited.HarmonyPatches
{
    /// <summary>
    /// This patch try to add a custom party size mutliplier after the execution of "GetPartyPrisonerSizeLimit" that will define the default party size limit of a garrison.
    /// </summary>
    [HarmonyPatch(typeof(DefaultPartySizeLimitModel), nameof(DefaultPartySizeLimitModel.GetPartyPrisonerSizeLimit))]
    class GetPartyPrisonerSizeLimit
    {
        static void Postfix(PartyBase party, ref ExplainedNumber __result)
        {
            if (ShouldApplyPatch(party))
            {
                var options = SubModule.PartySizeReunitedOptions;
                TextObject description = new("PartySizeReunited modifier");
                if (options.PartyGarrisonFixedBonus != 0)
                {
                    __result.Add(
                        options.PartyGarrisonFixedBonus,
                        description
                    );
                }
                else
                {
                    __result.AddFactor(
                        options.PartyGarrisonMultiBonus,
                        description
                        );
                }
            }
        }

        private static bool ShouldApplyPatch(PartyBase party)
        {
            var options = SubModule.PartySizeReunitedOptions;
            if (!options.IsActivate)
            {
                return false;
            }

            return options.BonusScope.SelectedValue.Scope switch
            {
                IScope.Everyone => true,
                IScope.Only_player => options.IsPlayerPartyImpacted && party.Owner.IsHumanPlayerCharacter,
                IScope.Only_player_clan => ScopeExtension.IsOnlyPlayerClan(party),
                IScope.Only_player_faction => ScopeExtension.IsOnlyPlayerFaction(party.MapFaction),
                IScope.Only_ennemies => ScopeExtension.IsOnlyEnnemies(party.MapFaction),
                _ => false,
            };
        }
    }
}
