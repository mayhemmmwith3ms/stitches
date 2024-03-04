using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StitchesLib.Common.Systems.AutoRenderTargets;
using StitchesLib.Common.Utils;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace StitchesLib.Common.Systems.DrawOnTiles;

public class DrawOnTilesManager : ILoadable
{
	public void Load(Mod mod)
	{
		On_Main.DrawDust += On_Main_DrawDust;
	}

	private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
	{
		orig(self);

		if (AutoRenderTargetManager.TryGetRenderTarget<DrawOnTilesTarget>(out var target))
		{
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect tileAlphaClipEffect = GameShaders.Misc["StitchesLib:DrawOnTiles"].Shader;

			tileAlphaClipEffect.Parameters["tileTexture"].SetValue(Main.instance.tileTarget);

			tileAlphaClipEffect.CurrentTechnique.Passes[0].Apply();

			Vector2 zoom = new(1f / Main.GameViewMatrix.Zoom.X, 1f / Main.GameViewMatrix.Zoom.Y);

			Main.spriteBatch.Draw(Main.instance.tileTarget, GraphicsUtils.ScreenRect.Center(), null, Color.White, 0f, Main.instance.tileTarget.Size() / 2, zoom, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(target.renderTarget, GraphicsUtils.ScreenRect.Center(), null, Color.White, 0f, target.renderTarget.Size() / 2, zoom, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
		}
	}

	public void Unload()
	{
	}
}
