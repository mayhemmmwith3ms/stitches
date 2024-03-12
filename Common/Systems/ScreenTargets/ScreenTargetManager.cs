using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace StitchesLib.Common.Systems.ScreenTargets;
public class ScreenTargetManager : ILoadable
{
	public static List<ScreenTarget> Targets { get; private set; } = new();

	public void Load(Mod mod)
	{
		On_Main.CheckMonoliths += On_Main_CheckMonoliths;
		Main.OnResolutionChanged += Main_OnResolutionChanged;
	}

	public void Unload()
	{
		On_Main.CheckMonoliths -= On_Main_CheckMonoliths;
		Main.OnResolutionChanged -= Main_OnResolutionChanged;

		Main.QueueMainThreadAction(() =>
		{
			Targets.ForEach(t => t.Target?.Dispose());
			Targets.Clear();
			Targets = null;
		});
	}

	public static void RegisterTarget(ScreenTarget target)
	{
		Targets.Add(target);
		Targets.Sort((a, b) => a.Priority < b.Priority ? 1 : -1);
	}

	private void On_Main_CheckMonoliths(On_Main.orig_CheckMonoliths orig)
	{
		orig();

		if (Main.dedServ)
			return;

		RenderTargetBinding[] bindingsCache = Main.graphics.GraphicsDevice.GetRenderTargets();

		foreach(var tgt in Targets)
		{
			Main.spriteBatch.Begin();

			Main.graphics.GraphicsDevice.SetRenderTarget(tgt.Target);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			if (tgt.isVisible())
			{
				tgt.drawTo();
			}
			
			Main.spriteBatch.End();
		}

		Main.graphics.GraphicsDevice.SetRenderTargets(bindingsCache);
	}

	private void Main_OnResolutionChanged(Vector2 obj)
	{
		if (Main.dedServ)
			return;

		foreach (var tgt in Targets)
		{
			tgt.ResizeTarget(obj);
		}
	}
}
