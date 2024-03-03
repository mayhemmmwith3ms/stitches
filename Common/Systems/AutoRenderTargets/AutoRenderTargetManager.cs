using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.Core;
using Microsoft.Xna.Framework;

namespace StitchesLib.Common.Systems.AutoRenderTargets;

public class AutoRenderTargetManager : ILoadable
{
	public static List<AutoRenderTarget> Targets { get; private set; }

	public void Load(Mod mod)
	{
		Targets = new();

		Main.QueueMainThreadAction(() =>
		{
			foreach (var t in AssemblyManager.GetLoadableTypes(mod.Code))
			{
				if (!t.IsAbstract && t.IsSubclassOf(typeof(AutoRenderTarget)))
				{
					Targets.Add(Activator.CreateInstance(t, null) as AutoRenderTarget);
				}
			}
		});

		On_Main.SetDisplayMode += RefreshTargets;
		On_Main.CheckMonoliths += DrawOnTargets;
	}

	public void Unload()
	{
		Targets.Clear();
		Targets = null;
	}

	private void RefreshTargets(On_Main.orig_SetDisplayMode orig, int width, int height, bool fullscreen)
	{
		if (width != Main.screenWidth || height != Main.screenHeight)
		{
			Targets.ForEach(t => t.RefreshTarget(width, height));
		}

		orig(width, height, fullscreen);
	}

	private void DrawOnTargets(On_Main.orig_CheckMonoliths orig)
	{
		if (!Main.gameMenu)
		{
			foreach (var e in Main.projectile)
			{
				if (e.active && e.ModProjectile is IDrawToRenderTarget c)
				{
					c.QueueDrawAction();
				}
			}

			foreach (var e in Main.npc)
			{
				if (e.active && e.ModNPC is IDrawToRenderTarget c)
				{
					c.QueueDrawAction();
				}
			}

			GraphicsDevice gd = Main.graphics.GraphicsDevice;

			foreach (var t in Targets)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, t.BlendStateToDrawOnWith, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

				gd.SetRenderTarget(t.renderTarget);
				gd.Clear(Color.Transparent);

				t.DrawOnTarget();

				Main.spriteBatch.End();
				gd.SetRenderTarget(null);
			}
		}

		orig();
	}

	public static bool TryGetRenderTarget<T>(out AutoRenderTarget result) where T : AutoRenderTarget
	{
		result = GetRenderTarget<T>();
		if (result is not null)
			return true;
		return false;
	}

	public static T GetRenderTarget<T>() where T : AutoRenderTarget
	{
		foreach (var t in Targets)
		{
			if (t.GetType() == typeof(T))
			{
				return (T)t;
			}
		}
		return null;
	}
}
