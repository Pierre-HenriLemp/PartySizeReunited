using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace PartySizeReunited
{
	public class PartySize : DefaultPartySizeLimitModel
	{
		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			// Add parameters in order to add value for only player / Allies / All / etc.

			ExplainedNumber result = base.GetPartyMemberSizeLimit(party, includeDescriptions);
			bool flag = party.LeaderHero != null;
			if (flag)
			{
				result.Add(150f, null, null);
			}
			return result;
		}
	}
}

