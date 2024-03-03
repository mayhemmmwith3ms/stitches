using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;

namespace StitchesLib.Common.Managers.Primitives;

public class PrimitiveRenderer : ILoadable
{
	public static List<PrimitiveDrawLayer> Layers { get; private set; }

	public void Load(Mod mod)
	{
		On_Main.DoUpdateInWorld += UpdatePrimitives;
	}

	public void Unload()
	{
		Layers.Clear();
		Layers = null;
	}

	private void UpdatePrimitives(On_Main.orig_DoUpdateInWorld orig, Main self, System.Diagnostics.Stopwatch sw)
	{
		orig(self, sw);

		if (Main.gameMenu)
			return;

		Layers.ForEach(l => l.drawFuncs.Clear());

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

	public static bool TryGetDrawLayer(string name, out PrimitiveDrawLayer layer)
	{
		layer = GetDrawLayer(name);

		if (layer is not null)
		{
			return false;
		}

		return true;
	}

	public static PrimitiveDrawLayer GetDrawLayer(string name) => Layers.Find(x => x.name == name);
}
