using MCM.Common;
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

			Dropdown<string> bonusScope = MCMUISettings.Instance.BonusScope;
			float bonusPartySize = MCMUISettings.Instance.PartyBonusAmnt;

			ExplainedNumber result = base.GetPartyMemberSizeLimit(party, includeDescriptions);

			switch (bonusScope.SelectedValue)
			{
				case "Everyone":
					// Everyone party who have a leader hero
					if (party.LeaderHero != null)
					{
						result.Add(bonusPartySize, null, null);
					}
					break;
				case "Only player":
					if (party.LeaderHero != null && party.LeaderHero.IsHumanPlayerCharacter)
					{
						result.Add(bonusPartySize, null, null);
					}
					break;
				case "Only player clan":
					if (party.LeaderHero != null && (party.LeaderHero.IsHumanPlayerCharacter || party.LeaderHero.Clan == Hero.MainHero.Clan))
					{
						result.Add(bonusPartySize, null, null);
					}
					break;
				case "Only player faction":
					if (party.LeaderHero != null && (party.LeaderHero.IsHumanPlayerCharacter || party.LeaderHero.Clan.MapFaction.Name == Hero.MainHero.Clan.MapFaction.Name))
					{
						result.Add(bonusPartySize, null, null);
					}
					break;
			}
			return result;
		}
	}
}

