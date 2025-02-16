using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace PartySizeReunited.ModCompatibility
{
	public static class EagleRisingCompatibility
	{
		public static ExplainedNumber GetEagleRisingCompatibility(PartyBase party, ExplainedNumber baseValue, bool includeDescriptions = false)
		{
			bool flag = !party.IsMobile;
			ExplainedNumber result2;
			if (flag)
			{
				result2 = new ExplainedNumber(0f, includeDescriptions, null);
			}
			else
			{
				result2 = baseValue;
				bool isGarrison = party.MobileParty.IsGarrison;
				if (isGarrison)
				{
					ExplainedNumber result = baseValue;
					result.Add(20 * party.MobileParty.CurrentSettlement.Town.GetWallLevel(), new TextObject("Military Infrastructure Level", null), null);
					result2 = result;
				}
			}
			return result2;
		}
	}
}
