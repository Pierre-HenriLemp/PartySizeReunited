using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using PartySizeReunited.McMMenu.Options;
using PartySizeReunited.Models;

namespace PartySizeReunited.McMMenu
{
    internal static class McMPartySizeReunitedSettings
    {
        private static readonly string isActivateHint = "Should party size options be activated?";
        private static readonly string partyImpactedHint = "If check, player's party will be impacted by the amount you have selected in\nthe 'Party multiplicator' setting. If uncheck, player's party will not be impacted whatever option you choose. (Even 'Only player!!')";
        private static readonly string scopeHint = "Select scope where you want bonuses to be applied.\n[Everyone] apply for every parties that have a hero leader.";
        private static readonly string noMoreSupplyHint = "EXPERIMENTAL ! Allow AI parties affected by Party Size Reunited to NEVER have depleted gold and food.";
        private static readonly string influenceCostHint = "Multiply party recruitment cost by this multiplier for anyone (Player and IA)";

        // Party size
        private static readonly string fixedBonusAmntHint = "Add fix bonus to party size.\nIF THIS VALUE IS DIFFERENT THAN 0 THE BONUS MULTIPLIER WILL BE IGNORED!!!!";
        private static readonly string partyBonusMultiHint = "Multiplicator that will be applied to party size.\nIF \"Fixed bonus amount\" IS SET, THIS SETTING IS IGNORED!!!!";

        // Caravan
        private static readonly string fixedCaravanFixedHint = "RELOAD YOUR SAVE TO APPLY CHANGES!!\nAdd fix bonus to caravan party size.\nIF THIS VALUE IS DIFFERENT THAN 0 THE BONUS MULTIPLIER WILL BE IGNORED!!!!";
        private static readonly string partyCaravanMultiHint = "RELOAD YOUR SAVE TO APPLY CHANGES!!\nMultiplicator that will be applied to caravan party size.\nIF \"Fixed bonus amount\" IS SET, THIS SETTING IS IGNORED!!!!";

        // Wage
        private static readonly string fixedWageFixedHint = "Add fix bonus reduction to wage party.\nIF THIS VALUE IS DIFFERENT THAN 0 THE BONUS MULTIPLIER WILL BE IGNORED!!!!";
        private static readonly string partyWageMultiHint = "Multiplicator that will be applied to wage party.\nIF \"Fixed bonus amount\" IS SET, THIS SETTING IS IGNORED!!!!";

        // Player speed
        private static readonly string fixedSpeedFixedHint = "MAX BOAT SPEED IS 10, you can't go any faster than 10 on sea.\nAdd fix bonus to player speed party.\nIF THIS VALUE IS DIFFERENT THAN 0 THE BONUS MULTIPLIER WILL BE IGNORED!!!!";
        private static readonly string partySpeedMultiHint = "MAX BOAT SPEED IS 10, you can't go any faster than 10 on sea.\nMultiplicator that will be applied player's party speed.\nIF \"Fixed bonus amount\" IS SET, THIS SETTING IS IGNORED!!!!";

        // Garrison
        private static readonly string fixedGarrisonFixedHint = "RELOAD YOUR SAVE TO APPLY CHANGES!!\nAdd fix bonus to garrison party size.\nIF THIS VALUE IS DIFFERENT THAN 0 THE BONUS MULTIPLIER WILL BE IGNORED!!!!";
        private static readonly string partyGarrisonMultiHint = "RELOAD YOUR SAVE TO APPLY CHANGES!!\nMultiplicator that will be applied to garrison party size.\nIF \"Fixed bonus amount\" IS SET, THIS SETTING IS IGNORED!!!!";

        // Prisoner
        private static readonly string fixedPrisonerFixedHint = "RELOAD YOUR SAVE TO APPLY CHANGES!!\nAdd fix bonus to prisoner party size.\nIF THIS VALUE IS DIFFERENT THAN 0 THE BONUS MULTIPLIER WILL BE IGNORED!!!!";
        private static readonly string partyPrisonerMultiHint = "RELOAD YOUR SAVE TO APPLY CHANGES!!\nMultiplicator that will be applied to prisoner party size.\nIF \"Fixed bonus amount\" IS SET, THIS SETTING IS IGNORED!!!!";


        public static ISettingsBuilder AddPartySizeSettings(ISettingsBuilder builder, PartySizeReunitedOptions opt)
        {
            return builder
                .SetSubFolder("PartySizeReunited")
                .CreateGroup("Main", MainOption)
                .CreateGroup("Wage", WageOption)
                .CreateGroup("Prisoner", PrisonerOption)
                .CreateGroup("Garrison", GarrisonOption)
                .CreateGroup("Caravan", CaravanOption)
                ;

            void MainOption(ISettingsPropertyGroupBuilder builder)
                => builder
                .AddBool("psr_party_size_activate", "Activate?",
                             new ProxyRef<bool>(() => opt.IsActivate, value => opt.IsActivate = value),
                             propBuilder => propBuilder
                             .SetHintText(isActivateHint)
                             .SetRequireRestart(false)
                             .SetOrder(0)
                )
                .AddBool(
                    "psr_is_player_party_impacted", "Apply also to player",
                             new ProxyRef<bool>(() => opt.IsPlayerPartyImpacted, value => opt.IsPlayerPartyImpacted = value),
                             propBuilder => propBuilder
                             .SetHintText(partyImpactedHint)
                             .SetRequireRestart(false)
                             .SetOrder(1)
                )
                .AddDropdown("psr_bonus_scope", "Scope", 0,
                             new ProxyRef<Dropdown<ScopeExtension>>(
                                 () => opt.BonusScope, value => opt.BonusScope = value
                                 ),
                             propBuilder => propBuilder
                             .SetHintText(scopeHint)
                             .SetRequireRestart(false)
                             .SetOrder(2)
                             )
                .AddInteger("psr_fixed_bonus_amnt", "Fixed party size bonus amount", 0, 10000,
                             new ProxyRef<int>(() => opt.FixedBonusAmnt, value => opt.FixedBonusAmnt = value),
                             propBuilder => propBuilder
                             .SetHintText(fixedBonusAmntHint)
                             .SetRequireRestart(false)
                             .SetOrder(3)
                )
                .AddFloatingInteger("psr_party_bonus", "Party size multiplicator", 0, 10,
                             new ProxyRef<float>(() => opt.PartyBonusAmnt, value => opt.PartyBonusAmnt = value),
                             propBuilder => propBuilder
                             .SetHintText(partyBonusMultiHint)
                             .SetRequireRestart(false)
                             .SetOrder(4)
                             .AddValueFormat("#0%")
                             )
                .AddBool("psr_no_more_supply", "no more AI supply issue",
                             new ProxyRef<bool>(() => opt.NoMoreSupplyIssues, value => opt.NoMoreSupplyIssues = value),
                             propBuilder => propBuilder
                             .SetHintText(noMoreSupplyHint)
                             .SetRequireRestart(false)
                             .SetOrder(5)
                             )
                .AddFloatingInteger("psr_influence_cost", "Party recruitment cost multiplier", 0, 1,
                             new ProxyRef<float>(() => opt.PartyInfluenceCost, value => opt.PartyInfluenceCost = value),
                             propBuilder => propBuilder
                             .SetHintText(influenceCostHint)
                             .SetRequireRestart(false)
                             .SetOrder(6)
                             .AddValueFormat("#0%")
                             )
                .AddInteger("psr_player_party_speed_fixed_amnt", "Fixed player's party speed amount", 0, 30,
                             new ProxyRef<int>(() => opt.PlayerPartySpeedFixedBonus, value => opt.PlayerPartySpeedFixedBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(fixedSpeedFixedHint)
                             .SetRequireRestart(false)
                             .SetOrder(7)
                )
                .AddFloatingInteger("psr_player_party_speed_multi_amnt", "player's party speed multiplicator", 0, 10,
                             new ProxyRef<float>(() => opt.PlayerPartySpeedMultiBonus, value => opt.PlayerPartySpeedMultiBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(partySpeedMultiHint)
                             .SetRequireRestart(false)
                             .SetOrder(8)
                             .AddValueFormat("#0%")
                             )
                .SetGroupOrder(0);
            void WageOption(ISettingsPropertyGroupBuilder builder)
                => builder
                .AddInteger("psr_wage_fixed_amnt", "Fixed wage bonus reducer amount", -10000, 0,
                             new ProxyRef<int>(() => opt.PartyWageFixedBonus, value => opt.PartyWageFixedBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(fixedWageFixedHint)
                             .SetRequireRestart(false)
                             .SetOrder(9)
                )
                .AddFloatingInteger("psr_wage_multi_amnt", "Party wage multiplicator", 0, 1,
                             new ProxyRef<float>(() => opt.PartyWageMultiBonus, value => opt.PartyWageMultiBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(partyWageMultiHint)
                             .SetRequireRestart(false)
                             .SetOrder(10)
                             .AddValueFormat("#0%")
                             )
                .SetGroupOrder(1);
            void PrisonerOption(ISettingsPropertyGroupBuilder builder)
                => builder
                .AddInteger("psr_prisoner_fixed_amnt", "Fixed prisoner bonus amount", 0, 2000,
                             new ProxyRef<int>(() => opt.PartyPrisonerFixedBonus, value => opt.PartyPrisonerFixedBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(fixedPrisonerFixedHint)
                             .SetRequireRestart(false)
                             .SetOrder(14)
                )
                .AddFloatingInteger("psr_prisoner_multi_amnt", "Party prisoner multiplicator", 0, 10,
                             new ProxyRef<float>(() => opt.PartyPrisonerMultiBonus, value => opt.PartyPrisonerMultiBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(partyPrisonerMultiHint)
                             .SetRequireRestart(false)
                             .SetOrder(15)
                             .AddValueFormat("#0%")
                             )
                .SetGroupOrder(2);
            void GarrisonOption(ISettingsPropertyGroupBuilder builder)
                => builder

                .AddInteger("psr_garrison_fixed_amnt", "Fixed garrison bonus amount", 0, 2000,
                             new ProxyRef<int>(() => opt.PartyGarrisonFixedBonus, value => opt.PartyGarrisonFixedBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(fixedGarrisonFixedHint)
                             .SetRequireRestart(false)
                             .SetOrder(12)
                )
                .AddFloatingInteger("psr_garrison_multi_amnt", "Party garrison multiplicator", 0, 10,
                             new ProxyRef<float>(() => opt.PartyGarrisonMultiBonus, value => opt.PartyGarrisonMultiBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(partyGarrisonMultiHint)
                             .SetRequireRestart(false)
                             .SetOrder(13)
                             .AddValueFormat("#0%")
                             )
                .SetGroupOrder(3);
            void CaravanOption(ISettingsPropertyGroupBuilder builder)
                => builder
                .AddInteger("psr_caravan_fixed_amnt", "Fixed caravan bonus amount", 0, 10000,
                             new ProxyRef<int>(() => opt.CaravanFixedBonus, value => opt.CaravanFixedBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(fixedCaravanFixedHint)
                             .SetRequireRestart(false)
                             .SetOrder(7)
                )
                .AddFloatingInteger("psr_caravan_multi_amnt", "Party caravan multiplicator", 0, 10,
                             new ProxyRef<float>(() => opt.CaravanMultiBonus, value => opt.CaravanMultiBonus = value),
                             propBuilder => propBuilder
                             .SetHintText(partyCaravanMultiHint)
                             .SetRequireRestart(false)
                             .SetOrder(8)
                             .AddValueFormat("#0%")
                             )
                .SetGroupOrder(4);
        }
    }
}
