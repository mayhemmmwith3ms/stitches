using StitchesLib.Common.Systems.AutoUI;
using StitchesLib.Common.Systems.ParticleSystem;
using StitchesLib.Content.UI.DebugMenuUI;
using Terraria.ModLoader;
using Terraria.UI;

namespace StitchesLib;

public class StitchesLib : Mod
{
	public static StitchesLib Instance { get; private set; }

	public StitchesLib()
	{
		Instance = this;
	}

	public override void Load()
	{
		On_UserInterface.Draw += On_UserInterface_Draw;
	}

	private void On_UserInterface_Draw(On_UserInterface.orig_Draw orig, UserInterface self, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.GameTime time)
	{
		ParticleSystem.Layers.ForEach(x =>
		AutoUILoader.GetAutoUIState<DebugMenuState>().mainPanel.infoList.Add($"{x.name}: {x.particles.Count} particles active")
		);

		orig(self, spriteBatch, time);
	}
}