using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StitchesLib.Common.Systems.AutoRenderTargets;
using StitchesLib.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;

namespace StitchesLib.Common.Systems.DrawOnTiles;
public class DrawOnTilesTarget : AutoRenderTarget
{
	public List<Action> DrawCalls { get; set; } = new();

	public RenderTarget2D TileTargetCacheTarget { get; set; }

	public override void DrawOnTarget()
	{
		DrawCalls.ForEach(x => x?.Invoke());
		DrawCalls.Clear();

		DebugUtils.DrawDebugPixel(Main.MouseWorld + new Vector2(0, 40));

		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, GraphicsUtils.ScreenRect, Color.Red * 0.6f);
	}
}
