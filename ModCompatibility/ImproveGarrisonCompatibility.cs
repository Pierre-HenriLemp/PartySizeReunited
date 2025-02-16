using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace PartySizeReunited.ModCompatibility
{
	public static class ImproveGarrisonCompatibility
	{
		public static ExplainedNumber GetImproveGarrisonCompatibility(PartyBase party, ExplainedNumber baseValue, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = baseValue;
			ExplainedNumber result;
			try
			{
				bool flag = IsMobileGarrisonParty(party.MobileParty);
				bool flag2 = IsTransferParty(party.MobileParty);
				bool flag3 = IsRecruiterParty(party.MobileParty);
				bool flag4 = flag || flag2 || flag3;
				if (flag4)
				{
					explainedNumber = (result = new ExplainedNumber(200f, true, new TextObject("[IG-cheats] custom party size", null)));
				}
				else
				{
					double num = 1f;
					double num2 = 1f;
					double num3 = 1f;
					double num4 = 1f;
					bool flag5 = party != null && party.MobileParty != null;
					if (flag5)
					{
						bool flag6 = party.MobileParty.IsGarrison && num > 0.0;
						if (flag6)
						{
							float num5 = (float)((double)explainedNumber.ResultNumber * num - (double)explainedNumber.ResultNumber);
							bool flag7 = num5 > 1f;
							if (flag7)
							{
								explainedNumber.Add(num5, new TextObject("[IG-cheats] custom garrison size", null), null);
							}
						}
						bool flag8 = party.MobileParty == MobileParty.MainParty && num2 > 0.0;
						if (flag8)
						{
							float num6 = (float)((double)explainedNumber.ResultNumber * num2 - (double)explainedNumber.ResultNumber);
							bool flag9 = num6 > 1f;
							if (flag9)
							{
								explainedNumber.Add(num6, new TextObject("[IG-cheats] custom player party size", null), null);
							}
						}
						bool flag10 = party.MobileParty != MobileParty.MainParty && party.MobileParty.ActualClan != null && MobileParty.MainParty.ActualClan != null && party.MobileParty.ActualClan == MobileParty.MainParty.ActualClan && num3 > 0.0;
						if (flag10)
						{
							float num7 = (float)((double)explainedNumber.ResultNumber * num3 - (double)explainedNumber.ResultNumber);
							bool flag11 = num7 > 1f;
							if (flag11)
							{
								explainedNumber.Add(num7, new TextObject("[IG-cheats] custom player clan party size", null), null);
							}
						}
						bool flag12 = party.MobileParty.ActualClan != null && MobileParty.MainParty.ActualClan != null && party.MobileParty.ActualClan != MobileParty.MainParty.ActualClan && num3 > 0.0;
						if (flag12)
						{
							float num8 = (float)((double)explainedNumber.ResultNumber * num3 - (double)explainedNumber.ResultNumber);
							bool flag13 = num8 > 1f;
							if (flag13)
							{
								explainedNumber.Add(num8, new TextObject("[IG-cheats] custom ai clan party size", null), null);
							}
						}
					}
					bool flag14 = party != null && party.MobileParty != null && party.MobileParty.StringId == "improvedgarrisons_template_party";
					if (flag14)
					{
						explainedNumber = new ExplainedNumber(100000f, true, new TextObject("Improved Garrison template party size", null));
					}
					result = explainedNumber;
				}
			}
			catch (Exception)
			{
				result = explainedNumber;
			}
			return result;
		}


		private static bool IsMobileGarrisonParty(MobileParty party)
		{
			bool flag = party != null;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = party.StringId != null;
				bool flag4 = flag3;
				if (flag4)
				{
					return party.StringId.Contains("mobilegarrison_");
				}
			}
			return false;
		}
		private static bool IsTransferParty(MobileParty party)
		{
			bool flag = party != null;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = party.StringId != null;
				bool flag4 = flag3;
				if (flag4)
				{
					return party.StringId.Contains("garrisontransferparty_");
				}
			}
			return false;
		}
		private static bool IsRecruiterParty(MobileParty party)
		{
			bool flag = party != null;
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = party.StringId != null;
				bool flag4 = flag3;
				if (flag4)
				{
					return party.StringId.Contains("improvedgarrison_recruiter_");
				}
			}
			return false;
		}
	}
}
