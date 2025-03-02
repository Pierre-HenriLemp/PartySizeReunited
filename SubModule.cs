using PartySizeReunited.Services;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace PartySizeReunited
{
	public class SubModule : MBSubModuleBase
	{
		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);

			// Découvrir tous les modèles de taille de parti
			var discoveryService = new PartySizeModelDiscoveryService();
			var partySizeModels = discoveryService.DiscoverPartySizeModels();

			// Injecter les modèles dans notre PartySize
			gameStarterObject.AddModel(new PartySize(partySizeModels));
		}
	}
}

