using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using PartySizeReunited.McMMenu.Options;

namespace PartySizeReunited.McMMenu
{
    internal static class McMWarSails
    {
        public static readonly string moreBoatHint = "Increase ship deployment limit by the amount selected.\n(You can NOT have more than 8 ships at once in battle)";
        public static readonly string onlyApplyToPlayerHint = "Should bonus only given to player?";

        public static ISettingsBuilder AddWarsailsSettings(ISettingsBuilder builder, WarSailsOptions opt)
        {
            return builder
                .SetSubFolder("WarSails")
                .CreateGroup("WarSails", BuildWarSails);

            void BuildWarSails(ISettingsPropertyGroupBuilder builder)
                => builder
                .AddBool("psr_only_apply_to_player", "Only apply to player",
                             new ProxyRef<bool>(() => opt.OnlyApplyToPlayer, value => opt.OnlyApplyToPlayer = value),
                             propBuilder => propBuilder
                             .SetRequireRestart(false)
                             .SetHintText(onlyApplyToPlayerHint)
                             .SetOrder(0)
                             )
                .AddInteger("psr_more_boat",
                            "Bonus boat",
                            0,
                            8,
                            new ProxyRef<int>(() => opt.BonusBoats, value => opt.BonusBoats = value),
                            propBuilder => propBuilder
                            .SetRequireRestart(false)
                            .SetHintText(moreBoatHint)
                            .SetOrder(1)
                            )
                    .SetGroupOrder(1);
        }
    }
}
