using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;
using PartySizeReunited.Models;

namespace PartySizeReunited
{
	internal sealed class MCMUISettings : AttributeGlobalSettings<MCMUISettings> // AttributePerSaveSettings<MCMUISettings> AttributePerCampaignSettings<MCMUISettings>
	{
		private float _partyBonusAmnt = 0;
		private bool _isPlayerPartyImpacted = true;

		public override string Id => "PartySizeReunited";
		public override string DisplayName => $"Party Size Reunited";
		public override string FolderName => "PartySizeReunited";
		public override string FormatType => "json";

		[SettingPropertyBool("Is applied to player party ?", Order = 0, RequireRestart = false, HintText = "If checked, player's party will be impacted by the amount you have selected in\nthe 'Party multiplicator' setting. If unchecked, player's party will not be impacted whatever option you choose. (Even 'Only player!!')\nYOU NEED TO RELOAD YOUR SAVE IN ORDER TO APPLY CHANGES IF YOU UPDATE IT IN-GAME!")]
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

		[SettingPropertyFloatingInteger("Party multiplicator", 0, 10, "#0%", Order = 1, RequireRestart = false, HintText = "Multiplicator that will be applied to the party size.\nYOU NEED TO RELOAD YOUR SAVE IN ORDER TO APPLY CHANGES IF YOU UPDATE IT IN-GAME!")]
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

		[SettingPropertyDropdown("Scope", Order = 2, RequireRestart = false, HintText = "Select the scope where you want the bonus to be applied.\n[Everyone] apply for every parties that have a hero leader.\nYOU NEED TO RELOAD YOUR SAVE IN ORDER TO APPLY CHANGES IF YOU UPDATE IT IN-GAME!")]
		[SettingPropertyGroup("General")]
		public Dropdown<ScopeExtension> BonusScope { get; set; } = new Dropdown<ScopeExtension>(new ScopeExtension[]
		{
			new (IScope.Everyone),
			new (IScope.Only_player),
			new (IScope.Only_player_clan),
			new (IScope.Only_player_faction),
			new (IScope.Only_ennemies)
		}, selectedIndex: 0);
	}

}
