using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using StitchesLib.Common.Systems;
using ReLogic.Graphics;

namespace StitchesLib.Common.Utils;

public static class DebugUtils
{
	public static void DrawDebugText(string text, Vector2 worldPos, Color? color = null)
	{
		color ??= Color.White;

		Main.spriteBatch.DrawString(Terraria.GameContent.FontAssets.MouseText.Value, text.ToString(), (worldPos - Main.screenPosition).VectorRoundToInts(), (Color)color);
	}

	public static void QueueDrawDebugPixel(Vector2 worldPos, Color? color = null)
	{
		DebugDrawManager.DebugDrawQueue.Add(() =>
		{
			DrawDebugPixel(worldPos, color);
		});
	}

	public static void DrawDebugPixel(Vector2 worldPos, Color? color = null)
	{
		color ??= Color.White;

		Main.EntitySpriteDraw(ModContent.Request<Texture2D>(Directories.Textures_DebugPixel).Value, worldPos - Main.screenPosition, null, (Color)color, 0, new Vector2(0.5f), 2, SpriteEffects.None);
	}
}
