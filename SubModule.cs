using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace PartySizeReunited
{
	public class SubModule : MBSubModuleBase
	{
		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			base.OnGameStart(game, gameStarterObject);
			this.AddModels(gameStarterObject);
		}

		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			base.OnBeforeInitialModuleScreenSetAsRoot();
		}

		protected virtual void AddModels(IGameStarter gameStarterObject)
		{
			this.ReplaceModel<DefaultPartySizeLimitModel, PartySize>(gameStarterObject);
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

			// Parcourt les modèles existants
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

