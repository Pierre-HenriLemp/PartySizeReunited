using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace PartySizeReunited.HarmonyPatches
{
    [HarmonyPatch(typeof(BesiegerCamp), "StartingAssaultOnBesiegedSettlementIsLogical")]
    class Patch_TEMP6
    {
        [HarmonyPrefix]
        private static void Postfix(BesiegerCamp __instance, ref bool __result)
        {
            float num = 0f;
            float num2 = ((MobileParty.MainParty.CurrentSettlement == __instance.SiegeEvent.BesiegedSettlement) ? 0.5f : 1f);
            foreach (PartyBase item in __instance.SiegeEvent.BesiegedSettlement.GetInvolvedPartiesForEventType())
            {
                if (item.IsMobile && item.MobileParty.CurrentSettlement == __instance.SiegeEvent.BesiegedSettlement && (item.MobileParty.Aggressiveness > 0.01f || item.MobileParty.IsMilitia || item.MobileParty.IsGarrison))
                {
                    num += num2 * item.CalculateCurrentStrength();
                }
            }
            float num3 = 0f;
            foreach (PartyBase item2 in __instance.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType())
            {
                num3 += item2.CalculateCurrentStrength();
            }
            bool flag = false;
            bool flag2 = false;
            foreach (SiegeEvent.SiegeEngineConstructionProgress item3 in __instance.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).SiegeEngines.AllSiegeEngines())
            {
                if (item3.IsConstructed)
                {
                    if (item3.SiegeEngine == DefaultSiegeEngineTypes.Ram || item3.SiegeEngine == DefaultSiegeEngineTypes.ImprovedRam)
                    {
                        flag = true;
                    }
                    else if (item3.SiegeEngine == DefaultSiegeEngineTypes.SiegeTower)
                    {
                        flag2 = true;
                    }
                }
            }
            float settlementAdvantage = Campaign.Current.Models.CombatSimulationModel.GetSettlementAdvantage(__instance.SiegeEvent.BesiegedSettlement);
            float num4 = (float)CampaignTime.HoursInDay * 4f;
            float num5 = 0.8f - ((__instance.SiegeEvent.SiegeStartTime.ElapsedHoursUntilNow > num4) ? ((__instance.SiegeEvent.SiegeStartTime.ElapsedHoursUntilNow - num4) * 0.02f) : 0f);
            if (!flag)
            {
                num5 *= 1.25f;
            }
            if (!flag2)
            {
                num5 *= 1.25f;
            }
            float num6 = num3 / (num * MathF.Pow(settlementAdvantage, num5));
            bool result = false;
            float num7 = 1f;
            if (num6 > num7)
            {
                int numberOfEquipmentsBuilt = Campaign.Current.Models.CombatSimulationModel.GetNumberOfEquipmentsBuilt(__instance.SiegeEvent.BesiegedSettlement);
                num6 *= (float)MathF.Min(3, numberOfEquipmentsBuilt) / 3f;
                float num8 = Campaign.Current.Models.CombatSimulationModel.GetMaximumSiegeEquipmentProgress(__instance.SiegeEvent.BesiegedSettlement) + 0.25f * (float)(5 - numberOfEquipmentsBuilt);
                num6 *= 1f - 0.85f * (num8 * num8);
                result = num == 0f || MBRandom.RandomFloat < num6;
            }
            __result = result;
        }
    }
}
