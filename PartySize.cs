using MCM.Common;
using PartySizeReunited.ModCompatibility;
using PartySizeReunited.Models;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace PartySizeReunited
{
	public class PartySize : DefaultPartySizeLimitModel
	{
		private readonly Compatibilizer compatibilizer;

		public PartySize(Compatibilizer compatibilizer)
		{
			this.compatibilizer = compatibilizer;
		}

		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			ExplainedNumber basePartySize = base.GetPartyPrisonerSizeLimit(party, includeDescriptions);

			if (compatibilizer.IsEagleRisingMod)
				basePartySize = EagleRisingCompatibility.GetEagleRisingCompatibility(party, basePartySize, includeDescriptions);

			if (compatibilizer.IsImprovedGarrisonMod)
				basePartySize = ImproveGarrisonCompatibility.GetImproveGarrisonCompatibility(party, basePartySize, includeDescriptions);

			return GetPartySizeUpdatedByPartySizeMod(party, basePartySize);
		}

		private ExplainedNumber GetPartySizeUpdatedByPartySizeMod(PartyBase party, ExplainedNumber basePartySize)
		{
			Dropdown<ScopeExtension> bonusScope = MCMUISettings.Instance.BonusScope;
			IScope selectedScope = bonusScope.SelectedValue.Scope;
			float bonusPercentage = MCMUISettings.Instance.PartyBonusAmnt;
			bool isPlayerImpacted = MCMUISettings.Instance.IsPlayerPartyImpacted;

			ExplainedNumber result = basePartySize;

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

