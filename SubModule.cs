using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace PartySizeReunited
{
	public class SubModule : MBSubModuleBase
	{
		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);
			gameStarterObject.AddModel(
				new PartySize(
					new Compatibilizer(gameStarterObject)
					)
				);
		}

		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			base.OnBeforeInitialModuleScreenSetAsRoot();
		}

		protected void ReplaceModel<TBaseType, TChildType>(IGameStarter gameStarterObject)
			where TBaseType : GameModel
			where TChildType : TBaseType, new()
		{
			IList<GameModel>? models = gameStarterObject.Models as IList<GameModel>;

			if (models == null)
			{
				return;
			}

			bool found = false;

			for (int index = 0; index < models.Count; index++)
			{
				if (models[index] is TBaseType)
				{
					found = true;
					models[index] = new TChildType();
				}
			}

			if (!found)
			{
				gameStarterObject.AddModel(new TChildType());
			}
		}
	}
}

