using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;

namespace PartySizeReunited
{
	internal sealed class MCMUISettings : AttributeGlobalSettings<MCMUISettings> // AttributePerSaveSettings<MCMUISettings> AttributePerCampaignSettings<MCMUISettings>
	{
		private float _partyBonusAmnt = 150f;

		public override string Id => "PartySizeReunited";
		public override string DisplayName => $"Party Size Reunited";
		public override string FolderName => "PartySizeReunited";
		public override string FormatType => "json";

		[SettingPropertyFloatingInteger("Party value", 0f, 5000f, "0", Order = 0, RequireRestart = false, HintText = "How much party point you want to add to parties.\nYOU NEED TO RESTART YOUR SAVE IN ORDER TO APPLY CHANGES IF YOU UPDATE IT IN-GAME !")]
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

		[SettingPropertyDropdown("Scope", Order = 1, RequireRestart = false, HintText = "Select the scope where you want the bonus to be applied.\n[Everyone] mean that every parties that have an hero leader.\nAll options always impact player party.\nYOU NEED TO RESTART YOUR SAVE IN ORDER TO APPLY CHANGES IF YOU UPDATE IT IN-GAME !")]
		[SettingPropertyGroup("General")]
		public Dropdown<string> BonusScope { get; set; } = new Dropdown<string>(new string[]
		{
			"Everyone",
			"Only player",
			"Only player clan",
			"Only player faction"
		}, selectedIndex: 0);
	}

}
