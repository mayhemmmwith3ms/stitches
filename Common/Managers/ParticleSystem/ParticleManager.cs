using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace StitchesLib.Common.Managers.ParticleSystem;

public class ParticleManager : ILoadable
{
	public static List<ParticleLayer> Layers { get; private set; }

	public void Load(Mod mod)
	{
		Layers = new();
	}

	public void Unload()
	{
		Layers.Clear();
		Layers = null;
	}

	public static ParticleLayer GetDrawLayer(string name) => Layers.Find(x => x.name == name);

	public static bool TryGetDrawLayer(string name, out ParticleLayer layer)
	{
		layer = GetDrawLayer(name);

		if (layer is not null)
		{
			return false;
		}

		return true;
	}

	public static void DrawAll()
	{
		Layers.ForEach(x => x.DrawAll());
	}
}
