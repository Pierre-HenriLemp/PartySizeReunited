using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using PartySizeReunited.McMMenu.Options;
using PartySizeReunited.Models;

namespace PartySizeReunited.McMMenu
{
    internal static class MCMPartySizeReunitedSettings
    {
        private static readonly string partyImpactedHint = "If check, player's party will be impacted by the amount you have selected in\nthe 'Party multiplicator' setting. If uncheck, player's party will not be impacted whatever option you choose. (Even 'Only player!!')";
        private static readonly string partyBonusHint = "Multiplicator that will be applied to party size.\nIF \"Fixed bonus amount\" IS SET, THIS SETTING IS IGNORED!!!!";
        private static readonly string scopeHint = "Select scope where you want bonus to be applied.\n[Everyone] apply for every parties that have a hero leader.";
        private static readonly string noMoreSupplyHint = "EXPERIMENTAL ! Allow AI parties affected by Party Size Reunited to NEVER have depleted gold and food.";
        private static readonly string influenceCostHint = "Multiply party recruitment cost by this multiplier for anyone (Player and IA)";
        private static readonly string fixedBonusAmntHint = "Add fix bonus to party size.\nIF THIS VALUE IS DIFFERENT THAN 0 THE BONUS MULTIPLIER WILL BE IGNORED!!!!";

        public static ISettingsBuilder AddPartySizeSettings(ISettingsBuilder builder, PartySizeReunitedOptions opt)
        {
            return builder
                .SetSubFolder("PartySizeReunited")
                .CreateGroup("PartySizeReunited", BuildPartySize);

            void BuildPartySize(ISettingsPropertyGroupBuilder builder)
                => builder
                .AddBool(
                    "psr_is_player_party_impacted", "Apply also to player",
                             new ProxyRef<bool>(() => opt.IsPlayerPartyImpacted, value => opt.IsPlayerPartyImpacted = value),
                             propBuilder => propBuilder
                             .SetHintText(partyImpactedHint)
                             .SetRequireRestart(false)
                             .SetOrder(0)
                )
                .AddDropdown("psr_bonus_scope", "Scope", 0,
                             new ProxyRef<Dropdown<ScopeExtension>>(
                                 () => opt.BonusScope, value => opt.BonusScope = value
                                 ),
                             propBuilder => propBuilder
                             .SetHintText(scopeHint)
                             .SetRequireRestart(false)
                             .SetOrder(1)
                             )
                .AddInteger("psr_fixed_bonus_amnt", "Fixed bonus amount", 0, 10000,
                             new ProxyRef<int>(() => opt.FixedBonusAmnt, value => opt.FixedBonusAmnt = value),
                             propBuilder => propBuilder
                             .SetHintText(fixedBonusAmntHint)
                             .SetRequireRestart(false)
                             .SetOrder(2)
                )
                .AddFloatingInteger("psr_party_bonus", "Party multiplicator", 0, 10,
                             new ProxyRef<float>(() => opt.PartyBonusAmnt, value => opt.PartyBonusAmnt = value),
                             propBuilder => propBuilder
                             .SetHintText(partyBonusHint)
                             .SetRequireRestart(false)
                             .SetOrder(3)
                             .AddValueFormat("#0%")
                             )
                .AddBool("psr_no_more_supply", "no more AI supply issue",
                             new ProxyRef<bool>(() => opt.NoMoreSupplyIssues, value => opt.NoMoreSupplyIssues = value),
                             propBuilder => propBuilder
                             .SetHintText(noMoreSupplyHint)
                             .SetRequireRestart(false)
                             .SetOrder(4)
                             )
                .AddFloatingInteger("psr_influence_cost", "Party recruitment cost multiplier", 0, 1,
                             new ProxyRef<float>(() => opt.PartyInfluenceCost, value => opt.PartyInfluenceCost = value),
                             propBuilder => propBuilder
                             .SetHintText(influenceCostHint)
                             .SetRequireRestart(false)
                             .SetOrder(5)
                             .AddValueFormat("#0%")
                             )
                .SetGroupOrder(0);
        }
    }
}
