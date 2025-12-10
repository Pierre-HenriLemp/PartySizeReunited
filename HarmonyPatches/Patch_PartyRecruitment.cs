using HarmonyLib;
using PartySizeReunited.McMMenu.Options;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(DefaultClanTierModel), "GetPartyLimitForTier")]
    internal class Patch_PartyRecruitment
    {
        public static void Postfix(Clan clan, int clanTierToCheck, ref int __result)
        {
            if (SubModule.partyRecruitmentOptions.IsActivate && clan == Clan.PlayerClan && clan.Leader.IsHumanPlayerCharacter)
            {
                if (SubModule.partyRecruitmentOptions.Type.SelectedValue.SelectedValue == OptionTypeEnum.STATIC)
                {
                    __result = SubModule.partyRecruitmentOptions.Amount;
                }
                else if (SubModule.partyRecruitmentOptions.Type.SelectedValue.SelectedValue == OptionTypeEnum.PROGRESSIVE)
                {
                    __result = clanTierToCheck + SubModule.partyRecruitmentOptions.Amount;
                }
            }
        }
    }
}
