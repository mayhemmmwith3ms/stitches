using StitchesLib.Common.Systems.AutoUI;
using StitchesLib.Common.Systems.ParticleSystem;
using StitchesLib.Content.UI.DebugMenuUI;
using Terraria;
using Terraria.ModLoader;

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
		On_Main.CheckMonoliths += On_Main_CheckMonoliths;
	}

	private void On_Main_CheckMonoliths(On_Main.orig_CheckMonoliths orig)
	{
		orig();

		AutoUILoader.GetAutoUIState<DebugMenuState>().mainPanel.infoList.Add("Particle Layer Info:");

		ParticleSystem.Layers.ForEach(x =>
		AutoUILoader.GetAutoUIState<DebugMenuState>().mainPanel.infoList.Add($"{x.name}: {x.particles.Count} particles active")
		);
	}
}