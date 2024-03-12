using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StitchesLib.Common.Systems.ScreenTargets;
using StitchesLib.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace StitchesLib.Common.Systems.DrawOnTiles;

public class DrawOnTilesManager : ILoadable
{
	public static ScreenTarget DrawOnTilesTarget = new(DoDrawCalls, () => !Main.gameMenu);
	public static ScreenTarget TilesTargetCache = new(DrawTilesTargetCache, () => !Main.gameMenu, _priority:1);

	public static List<Action> DrawCalls { get; private set; } = new();

	public void Load(Mod mod)
	{
		On_Main.DrawDust += On_Main_DrawDust;
	}

	public void Unload()
	{
	}

	private static void DrawTilesTargetCache()
	{
		Main.graphics.GraphicsDevice.Clear(Color.Transparent);

		//Main.spriteBatch.Draw(Main.instance.blackTarget, Main.sceneTilePos - Main.screenPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(Main.instance.tileTarget, Main.sceneTilePos - Main.screenPosition, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
	}

	private static void DoDrawCalls()
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

		DrawCalls.ForEach(x =>
		{
			x?.Invoke();
		});

		DrawCalls.Clear();

		//DebugUtils.DrawDebugPixel(Main.MouseWorld + new Vector2(0, 40));

		//Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, GraphicsUtils.ScreenRect, Color.Red * 0.6f);
	}

	private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
	{
		orig(self);

		if (TilesTargetCache.Target is null)
			return;

		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

		Effect tileAlphaClipEffect = GameShaders.Misc["StitchesLib:DrawOnTiles"].Shader;
		tileAlphaClipEffect.Parameters["tileTexture"].SetValue(TilesTargetCache.Target);
		tileAlphaClipEffect.CurrentTechnique.Passes[0].Apply();

		Main.spriteBatch.Draw(DrawOnTilesTarget.Target, Vector2.Zero.VectorRoundToInts(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

		Main.spriteBatch.End();
	}
}
