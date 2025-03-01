using HarmonyLib;
using MCM.Common;
using PartySizeReunited.Models;
using StoryMode.GameComponents;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace PartySizeReunited
{
	public class PartySize : DefaultPartySizeLimitModel
	{
		public PartySize() { }

		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			ExplainedNumber basePartySize = base.GetPartyMemberSizeLimit(party, includeDescriptions);
			float totalBonus = 0;

			// Get all classes that implement DefaultPartySizeLimitModel but are not StoryModePartySizeLimitModel and PartySize
			var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t =>
					t.IsSubclassOf(typeof(DefaultPartySizeLimitModel))
					&& !t.IsInstanceOfType(this)
					&& !t.IsAssignableFrom(typeof(StoryModePartySizeLimitModel))
					&& !t.IsAbstract
					)
				.ToList();

			// Call all childs and retrieve bonus value to apply 
			foreach (var type in derivedTypes)
			{
				var method = AccessTools.Method(type, "GetPartyMemberSizeLimit");
				Console.WriteLine($"Executing override from {method.DeclaringType.Name}");
				// Instancier la classe correspondante et appeler la méthode avec les bons paramètres
				var instance = Activator.CreateInstance(method.DeclaringType);
				ExplainedNumber returnedValue = (ExplainedNumber)method.Invoke(instance, new object[] { party, includeDescriptions });

				totalBonus += CalculBonus(basePartySize.BaseNumber, returnedValue.BaseNumber);
			}

			// get party size with mods modifiers applied
			var partySizeWithModsApplied = new ExplainedNumber(basePartySize.BaseNumber + totalBonus, includeDescriptions, null);

			// Apply PartySizeReunited bonus on the total
			ExplainedNumber finalPartySize = GetPartySizeUpdatedByPartySizeMod(party, partySizeWithModsApplied);

			return finalPartySize;
		}

		private float CalculBonus(float baseValue, float newValue)
		{
			return newValue - baseValue;
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

