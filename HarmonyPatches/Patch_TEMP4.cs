using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace PartySizeReunited.HarmonyPatches
{
    //[HarmonyPatch(typeof(MapEvent), "FinalizeEventAux")]
    class Patch_TEMP4
    {
        private static readonly AccessTools.FieldRef<MapEvent, bool> KeepSiegeEventRef =
        AccessTools.FieldRefAccess<MapEvent, bool>("_keepSiegeEvent");

        private static readonly AccessTools.FieldRef<MapEvent, MapEvent.BattleTypes> MapEventTypeRef =
            AccessTools.FieldRefAccess<MapEvent, MapEvent.BattleTypes>("_mapEventType");

        private static readonly Action<MapEvent, MapEventState> SetState =
            AccessTools.MethodDelegate<Action<MapEvent, MapEventState>>(
                AccessTools.PropertySetter(typeof(MapEvent), nameof(MapEvent.State)));

        private static readonly Action<MapEventComponent> CallBeforeFinalizeComponent =
            AccessTools.MethodDelegate<Action<MapEventComponent>>(
                AccessTools.Method(typeof(MapEventComponent), "BeforeFinalizeComponent"));

        private static readonly Action<MapEventComponent> CallFinalizeComponent =
            AccessTools.MethodDelegate<Action<MapEventComponent>>(
                AccessTools.Method(typeof(MapEventComponent), "FinalizeComponent"));

        [HarmonyPrefix]
        private static bool Prefix(MapEvent __instance)
        {
            if (__instance.IsFinalized)
            {
                return false;
            }

            SetState(__instance, MapEventState.WaitingRemoval);
            CampaignEventDispatcher.Instance.OnMapEventEnded(__instance);

            bool isWin = false;
            bool flag = false;
            bool keepSiegeEvent = KeepSiegeEventRef(__instance);

            if (__instance.MapEventSettlement != null)
            {
                if (__instance.BattleState != BattleState.None &&
                    (__instance.IsSiegeAssault || __instance.IsSiegeOutside || __instance.IsSallyOut || __instance.IsBlockadeSallyOut || __instance.IsBlockade) &&
                    __instance.MapEventSettlement.SiegeEvent != null)
                {
                    __instance.MapEventSettlement.SiegeEvent.OnBeforeSiegeEventEnd(__instance.BattleState, __instance.EventType);
                }

                if (!keepSiegeEvent && (__instance.IsSiegeAssault || __instance.IsSiegeOutside))
                {
                    switch (__instance.BattleState)
                    {
                        case BattleState.AttackerVictory:
                            CampaignEventDispatcher.Instance.SiegeCompleted(__instance.MapEventSettlement, __instance.AttackerSide.LeaderParty.MobileParty, true, __instance.EventType);
                            isWin = true;
                            break;

                        case BattleState.DefenderVictory:
                            __instance.MapEventSettlement.SiegeEvent?.BesiegerCamp.RemoveAllSiegeParties();
                            CampaignEventDispatcher.Instance.SiegeCompleted(__instance.MapEventSettlement, __instance.AttackerSide.LeaderParty.MobileParty, false, __instance.EventType);
                            break;
                    }

                    if (__instance.BattleState == BattleState.AttackerVictory || __instance.BattleState == BattleState.DefenderVictory)
                    {
                        flag = true;
                    }
                }
                else if (__instance.IsSallyOut || __instance.IsBlockadeSallyOut)
                {
                    if (__instance.MapEventSettlement.Town != null &&
                        __instance.MapEventSettlement.Town.GarrisonParty != null &&
                        __instance.MapEventSettlement.Town.GarrisonParty.IsActive)
                    {
                        __instance.MapEventSettlement.Town.GarrisonParty.SetMoveModeHold();
                    }

                    switch (__instance.BattleState)
                    {
                        case BattleState.DefenderVictory:
                            CampaignEventDispatcher.Instance.SiegeCompleted(__instance.MapEventSettlement, __instance.DefenderSide.LeaderParty.MobileParty, true, __instance.EventType);
                            isWin = true;
                            break;

                        case BattleState.AttackerVictory:
                            __instance.MapEventSettlement.SiegeEvent?.BesiegerCamp.RemoveAllSiegeParties();
                            CampaignEventDispatcher.Instance.SiegeCompleted(__instance.MapEventSettlement, __instance.DefenderSide.LeaderParty.MobileParty, false, __instance.EventType);
                            break;
                    }

                    if (__instance.BattleState == BattleState.AttackerVictory || __instance.BattleState == BattleState.DefenderVictory)
                    {
                        flag = true;
                    }
                }
                else if (__instance.IsBlockadeSallyOut || __instance.IsBlockade)
                {
                    if (__instance.BattleState == BattleState.AttackerVictory)
                    {
                        __instance.MapEventSettlement.SiegeEvent?.BesiegerCamp.RemoveAllSiegeParties();
                        CampaignEventDispatcher.Instance.SiegeCompleted(__instance.MapEventSettlement, __instance.DefenderSide.LeaderParty.MobileParty, false, __instance.EventType);
                    }
                }
            }

            if (__instance.Component != null)
            {
                CallBeforeFinalizeComponent(__instance.Component);
            }

            foreach (PartyBase involvedParty in __instance.InvolvedParties)
            {
                if (involvedParty.IsMobile)
                {
                    involvedParty.MobileParty.EventPositionAdder = Vec2.Zero;
                }

                involvedParty.SetVisualAsDirty();

                if (!involvedParty.IsMobile ||
                    involvedParty.MobileParty.Army == null ||
                    involvedParty.MobileParty.Army.LeaderParty != involvedParty.MobileParty)
                {
                    continue;
                }

                foreach (MobileParty attachedParty in involvedParty.MobileParty.Army.LeaderParty.AttachedParties)
                {
                    attachedParty.Party.SetVisualAsDirty();
                }
            }

            if (!keepSiegeEvent)
            {
                MapEventSide[] sides = new[] { __instance.DefenderSide, __instance.AttackerSide };
                for (int i = 0; i < sides.Length; i++)
                {
                    sides[i].HandleMapEventEnd();
                }
            }

            __instance.MapEventVisual?.OnMapEventEnd();

            MapEvent.BattleTypes mapEventType = MapEventTypeRef(__instance);
            if (mapEventType != MapEvent.BattleTypes.Siege &&
                mapEventType != MapEvent.BattleTypes.SiegeOutside &&
                mapEventType != MapEvent.BattleTypes.SallyOut)
            {
                foreach (PartyBase involvedParty2 in __instance.InvolvedParties)
                {
                    if (involvedParty2.IsMobile &&
                        involvedParty2 != PartyBase.MainParty &&
                        involvedParty2.MobileParty.BesiegedSettlement != null &&
                        (involvedParty2.MobileParty.Army == null || involvedParty2.MobileParty.Army.LeaderParty == involvedParty2.MobileParty))
                    {
                        if (involvedParty2.IsActive)
                        {
                            EncounterManager.StartSettlementEncounter(involvedParty2.MobileParty, involvedParty2.MobileParty.BesiegedSettlement);
                        }
                        else
                        {
                            involvedParty2.MobileParty.BesiegerCamp = null;
                        }
                    }
                }
            }

            if (__instance.Component != null)
            {
                CallFinalizeComponent(__instance.Component);
            }

            if (flag)
            {
                CampaignEventDispatcher.Instance.AfterSiegeCompleted(__instance.MapEventSettlement, __instance.AttackerSide.LeaderParty.MobileParty, isWin, mapEventType);
            }

            if (!keepSiegeEvent)
            {
                MapEventSide[] clearSides = new[] { __instance.DefenderSide, __instance.AttackerSide };
                for (int i = 0; i < clearSides.Length; i++)
                {
                    clearSides[i].Clear();
                }
            }

            return false;
        }
    }
}
