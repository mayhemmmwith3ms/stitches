using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.UI;

namespace StitchesLib.Common.Systems.AutoUI;

[Autoload(Side = ModSide.Client)]
public class AutoUILoader : ModSystem
{
	public static List<AutoUIState> UIStates;

	public override void Load()
	{
		UIStates = new();

		foreach (var t in AssemblyManager.GetLoadableTypes(Mod.Code))
		{
			if (!t.IsAbstract && t.IsSubclassOf(typeof(AutoUIState)))
			{
				var currentUIState = (AutoUIState)Activator.CreateInstance(t, null);
				//currentUIState.Activate();
				var ui = new UserInterface();
				ui.SetState(currentUIState);
				currentUIState.UI = ui;

				UIStates.Add(currentUIState);
			}
		}
	}

	public override void Unload()
	{
		UIStates.Clear();
		UIStates = null;
	}

	public override void UpdateUI(GameTime gameTime)
	{
		UIStates.ForEach(x =>
		{
			if (x.Visible)
			{
				x.UI.Update(gameTime);
			}
		});
	}

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		foreach (var currentUIState in UIStates)
		{
			int layerIndex = layers.FindIndex(l => l.Name.Equals(currentUIState.InterfaceLayerToInsertAt));
			if (layerIndex != -1)
			{
				layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
					$"noitarraria: {currentUIState}",
					delegate
					{
						if (currentUIState.Visible)
						{
							currentUIState.UI.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI
					));
			}
		}
	}

	public static T GetAutoUIState<T>() where T : AutoUIState
	{
		foreach (var state in UIStates)
		{
			if (state.GetType() == typeof(T))
			{
				return (T)state;
			}
		}
		return null;
	}
}
