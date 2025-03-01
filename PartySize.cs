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
			ExplainedNumber partySize = base.GetPartyMemberSizeLimit(party, includeDescriptions);

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
				partySize = (ExplainedNumber)method.Invoke(instance, new object[] { party, includeDescriptions });

			}

			// Apply PartySizeReunited bonus on the total
			ExplainedNumber finalPartySize = GetPartySizeUpdatedByPartySizeMod(party, partySize);

			return finalPartySize;
		}

		private ExplainedNumber GetPartySizeUpdatedByPartySizeMod(PartyBase party, ExplainedNumber basePartySize)
		{
			Dropdown<ScopeExtension> bonusScope = MCMUISettings.Instance.BonusScope;
			IScope selectedScope = bonusScope.SelectedValue.Scope;
			float bonusPercentage = MCMUISettings.Instance.PartyBonusAmnt;
			bool isPlayerImpacted = MCMUISettings.Instance.IsPlayerPartyImpacted;

			ExplainedNumber result = basePartySize;

			float newValue = (float)Math.Round(result.ResultNumber * bonusPercentage);
			float valueToApply = newValue - result.ResultNumber;

			switch (selectedScope)
			{
				case IScope.Everyone:
					// Every party who have a leader hero
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter)
					{
						result.Add(valueToApply, new TaleWorlds.Localization.TextObject("Party Size bonus"));
					}
					break;
				case IScope.Only_player_clan:
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter && party.LeaderHero.Clan == Hero.MainHero.Clan)
					{
						result.Add(valueToApply, new TaleWorlds.Localization.TextObject("Party Size bonus"));
					}
					break;
				case IScope.Only_player_faction:
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter && party.LeaderHero.Clan.MapFaction.Name == Hero.MainHero.Clan.MapFaction.Name)
					{
						result.Add(valueToApply, new TaleWorlds.Localization.TextObject("Party Size bonus"));
					}
					break;
				case IScope.Only_ennemies:
					if (party.LeaderHero != null && !party.LeaderHero.IsHumanPlayerCharacter && party.LeaderHero.Clan.MapFaction.Name != Hero.MainHero.Clan.MapFaction.Name)
					{
						result.Add(valueToApply, new TaleWorlds.Localization.TextObject("Party Size bonus"));
					}
					break;
			}

			if (party.LeaderHero != null && isPlayerImpacted && party.LeaderHero.IsHumanPlayerCharacter)
			{
				result.Add(valueToApply, new TaleWorlds.Localization.TextObject("Party Size bonus"));
			}

			return result;
		}
	}
}

