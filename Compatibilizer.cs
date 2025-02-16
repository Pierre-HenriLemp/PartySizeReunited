using TaleWorlds.Core;

namespace PartySizeReunited
{
	public class Compatibilizer
	{
		private const string EAGLE_RISING_PARTY_MODEL_NAME = "CA_EagleRising.CA_EagleRisingNotablePowerModel";
		private const string IMPROVE_GARRISON_PARTY_MODEL_NAME = "ImprovedGarrisons.Models.GarrisonpartySizeLimitModel";

		public bool IsEagleRisingMod { get; private set; } = false;
		public bool IsImprovedGarrisonMod { get; private set; } = false;

		public Compatibilizer(IGameStarter gameStarterObject)
		{
			foreach (GameModel model in gameStarterObject.Models)
			{
				if (model.GetType().FullName == EAGLE_RISING_PARTY_MODEL_NAME)
				{
					IsEagleRisingMod = true;
				}

				if (model.GetType().FullName == IMPROVE_GARRISON_PARTY_MODEL_NAME)
				{
					IsImprovedGarrisonMod = true;
				}
			}
		}
	}
}
