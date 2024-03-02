using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace StitchesLib.Common.Managers.AutoRenderTargets
{
	public class AutoRenderTarget // thanks orange
	{
		public RenderTarget2D renderTarget = new(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, default, default, default, RenderTargetUsage.PreserveContents);

		public void RefreshTarget(int width, int height)
		{
			renderTarget = new(Main.graphics.GraphicsDevice, width, height, false, default, default, default, RenderTargetUsage.PreserveContents);
		}

		public virtual BlendState BlendStateToDrawOnWith => BlendState.AlphaBlend;

		public virtual void DrawOnTarget() { }
	}
}
