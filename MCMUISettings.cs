using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;
using PartySizeReunited.Models;

namespace PartySizeReunited
{
	internal sealed class MCMUISettings : AttributeGlobalSettings<MCMUISettings> // AttributePerSaveSettings<MCMUISettings> AttributePerCampaignSettings<MCMUISettings>
	{
		private float _partyBonusAmnt = 0f;
		private float _partyInfluenceCost = 0f; // Percentage
		private bool _isPlayerPartyImpacted = true;
		private bool _noMoreSupplyIssue = false;

		public override string Id => "PartySizeReunited";
		public override string DisplayName => "Party Size Reunited";
		public override string FolderName => "PartySizeReunited";
		public override string FormatType => "json";

		[SettingPropertyBool("Also apply to player party", Order = 0, RequireRestart = false, HintText = "If checked, player's party will be impacted by the amount you have selected in\nthe 'Party multiplicator' setting. If unchecked, player's party will not be impacted whatever option you choose. (Even 'Only player!!')")]
		[SettingPropertyGroup("General")]
		public bool IsPlayerPartyImpacted
		{
			get => _isPlayerPartyImpacted;
			set
			{
				if (_isPlayerPartyImpacted != value)
				{
					_isPlayerPartyImpacted = value;
					OnPropertyChanged();
				}
			}
		}

		[SettingPropertyFloatingInteger("Party multiplicator", 0, 10, "#0%", Order = 1, RequireRestart = false, HintText = "Multiplicator that will be applied to the party size.")]
		[SettingPropertyGroup("General")]
		public float PartyBonusAmnt
		{
			get => _partyBonusAmnt;
			set
			{
				if (_partyBonusAmnt != value)
				{
					_partyBonusAmnt = value;
					OnPropertyChanged();
				}
			}
		}

		[SettingPropertyDropdown("Scope", Order = 2, RequireRestart = false, HintText = "Select the scope where you want the bonus to be applied.\n[Everyone] apply for every parties that have a hero leader.")]
		[SettingPropertyGroup("General")]
		public Dropdown<ScopeExtension> BonusScope { get; set; } = new Dropdown<ScopeExtension>(new ScopeExtension[]
		{
			new (IScope.Everyone),
			new (IScope.Only_player),
			new (IScope.Only_player_clan),
			new (IScope.Only_player_faction),
			new (IScope.Only_ennemies)
		}, selectedIndex: 0);



		[SettingPropertyBool("AI no more supplies issue", Order = 3, RequireRestart = false, HintText = "EXPERIMENTAL ! Allow AI parties affected by Party Size Reunited to NEVER have depleted gold and food.")]
		[SettingPropertyGroup("General")]
		public bool NoMoreSupplyIssues
		{
			get => _noMoreSupplyIssue;
			set
			{
				if (_noMoreSupplyIssue != value)
				{
					_noMoreSupplyIssue = value;
					OnPropertyChanged();
				}
			}
		}

		[SettingPropertyFloatingInteger("Party recrutment cost multiplier", 0, 1, "#0%", Order = 4, RequireRestart = false, HintText = "Multiply party recrutment cost by this multiplier for anyone (Player and IA)")]
		[SettingPropertyGroup("General")]
		public float PartyInfluenceCost
		{
			get => _partyInfluenceCost;
			set
			{
				if (_partyInfluenceCost != value)
				{
					_partyInfluenceCost = value;
					OnPropertyChanged();
				}
			}
		}

	}

}
