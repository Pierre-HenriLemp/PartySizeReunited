using MCM.Common;
using PartySizeReunited.Models;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace PartySizeReunited
{
	public class PartySize : DefaultPartySizeLimitModel
	{
		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			Dropdown<ScopeExtension> bonusScope = MCMUISettings.Instance.BonusScope;
			IScope selectedScope = bonusScope.SelectedValue.Scope;
			float bonusPercentage = MCMUISettings.Instance.PartyBonusAmnt;
			bool isPlayerImpacted = MCMUISettings.Instance.IsPlayerPartyImpacted;

			ExplainedNumber result = base.GetPartyMemberSizeLimit(party, includeDescriptions);

			float newValue = (float)Math.Round(result.BaseNumber * bonusPercentage);

			switch (selectedScope)
			{
				case IScope.Everyone:
					// Every party who have a leader hero
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter)
					{
						result.Add(ResetValue(result));
						result.Add(newValue);
					}
					break;
				case IScope.Only_player_clan:
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter && party.LeaderHero.Clan == Hero.MainHero.Clan)
					{
						result.Add(ResetValue(result));
						result.Add(newValue);
					}
					break;
				case IScope.Only_player_faction:
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter && party.LeaderHero.Clan.MapFaction.Name == Hero.MainHero.Clan.MapFaction.Name)
					{
						result.Add(ResetValue(result));
						result.Add(newValue);
					}
					break;
				case IScope.Only_ennemies:
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter && party.LeaderHero.Clan.MapFaction.Name != Hero.MainHero.Clan.MapFaction.Name)
					{
						result.Add(ResetValue(result));
						result.Add(newValue);
					}
					break;
			}

			if (party.LeaderHero != null && isPlayerImpacted && party.LeaderHero.IsHumanPlayerCharacter)
			{
				result.Add(ResetValue(result));
				result.Add(newValue);
			}

			return result;
		}

		private float ResetValue(ExplainedNumber valueToReset)
		{
			float val = valueToReset.BaseNumber * -1;
			return val;
		}
	}
}

