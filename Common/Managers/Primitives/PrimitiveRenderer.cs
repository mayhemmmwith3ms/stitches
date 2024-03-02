using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;

namespace StitchesLib.Common.Managers.Primitives;

public class PrimitiveRenderer
{
	public List<PrimitiveDrawLayer> layers = new();

	public void Load(Mod mod)
	{
		On_Main.DoUpdateInWorld += UpdatePrimitives;
	}

	private void UpdatePrimitives(On_Main.orig_DoUpdateInWorld orig, Main self, System.Diagnostics.Stopwatch sw)
	{
		orig(self, sw);

		if (Main.gameMenu)
			return;

		layers.ForEach(l => l.drawFuncs.Clear());

		foreach (var e in Main.projectile)
		{
			if (e.active && e.ModProjectile is IDrawPrimitive c)
			{
				c.UpdatePrim();
			}
		}

		foreach (var e in Main.npc)
		{
			if (e.active && e.ModNPC is IDrawPrimitive c)
			{
				c.UpdatePrim();
			}
		}
	}

	public void UnloadDrawLayers()
	{
		layers.Clear();
	}

	public bool TryGetDrawLayer(string name, out PrimitiveDrawLayer layer)
	{
		layer = GetDrawLayer(name);

		if (layer is not null)
		{
			return false;
		}

		return true;
	}

	public PrimitiveDrawLayer GetDrawLayer(string name) => layers.Find(x => x.name == name);
}
