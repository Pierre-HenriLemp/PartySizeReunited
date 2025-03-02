using MCM.Common;
using PartySizeReunited.Models;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace PartySizeReunited
{
	public class PartySize : DefaultPartySizeLimitModel
	{
		private readonly List<DefaultPartySizeLimitModel> _partySizeModels;

		public PartySize(List<DefaultPartySizeLimitModel> partySizeModels = null)
		{
			_partySizeModels = partySizeModels ?? new List<DefaultPartySizeLimitModel>();
		}

		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			ExplainedNumber partySize = base.GetPartyMemberSizeLimit(party, includeDescriptions);

			// Appliquer tous les modèles de taille découverts
			foreach (var model in _partySizeModels)
			{
				try
				{
					Console.WriteLine($"Applying party size model from {model.GetType().Name}");
					partySize = model.GetPartyMemberSizeLimit(party, includeDescriptions);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error applying model {model.GetType().Name}: {ex.Message}");
				}
			}

			// Appliquer le bonus de PartySizeReunited
			ExplainedNumber finalPartySize = GetPartySizeUpdatedByPartySizeMod(party, partySize);
			return finalPartySize;
		}

		private ExplainedNumber GetPartySizeUpdatedByPartySizeMod(PartyBase party, ExplainedNumber basePartySize)
		{
			Dropdown<ScopeExtension> bonusScope = MCMUISettings.Instance.BonusScope;
			IScope selectedScope = bonusScope.SelectedValue.Scope;
			float bonusPercentage = MCMUISettings.Instance.PartyBonusAmnt;
			bool isPlayerImpacted = MCMUISettings.Instance.IsPlayerPartyImpacted;
			TextObject partySizeBonusText = new TextObject("Party Size modifier");

			ExplainedNumber result = basePartySize;
			float newValue = (float)Math.Round(result.ResultNumber * bonusPercentage);
			float valueToApply = newValue - result.ResultNumber;

			if (party.LeaderHero != null)
			{
				switch (selectedScope)
				{
					case IScope.Everyone:
						if (!party.LeaderHero.IsHumanPlayerCharacter)
						{
							result.Add(valueToApply, partySizeBonusText);
						}
						break;

					case IScope.Only_player_clan:
						if (!party.LeaderHero.IsHumanPlayerCharacter &&
							party.LeaderHero.Clan != null &&
							Hero.MainHero?.Clan != null &&
							party.LeaderHero.Clan == Hero.MainHero.Clan)
						{
							result.Add(valueToApply, partySizeBonusText);
						}
						break;

					case IScope.Only_player_faction:
						if (!party.LeaderHero.IsHumanPlayerCharacter &&
							party.LeaderHero.Clan?.MapFaction != null &&
							Hero.MainHero?.Clan?.MapFaction != null &&
							party.LeaderHero.Clan.MapFaction.Name == Hero.MainHero.Clan.MapFaction.Name)
						{
							result.Add(valueToApply, partySizeBonusText);
						}
						break;

					case IScope.Only_ennemies:
						if (!party.LeaderHero.IsHumanPlayerCharacter &&
							party.LeaderHero.Clan?.MapFaction != null &&
							Hero.MainHero?.Clan?.MapFaction != null &&
							party.LeaderHero.Clan.MapFaction.Name != Hero.MainHero.Clan.MapFaction.Name)
						{
							result.Add(valueToApply, partySizeBonusText);
						}
						break;
				}
			}

			if (isPlayerImpacted && party.LeaderHero?.IsHumanPlayerCharacter == true)
			{
				result.Add(valueToApply, partySizeBonusText);
			}

			return result;
		}
	}
}
