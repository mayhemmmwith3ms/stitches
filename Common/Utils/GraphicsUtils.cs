using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace StitchesLib.Common.Utils;
public static class GraphicsUtils
{
	public static Rectangle screenRect = new(0, 0, Main.screenWidth, Main.screenHeight);

	public static Vector2 WorldToZeroCenteredScreenPos(this Vector2 worldPos)
	{
		Vector2 drawPos;
		drawPos.X = worldPos.X - Main.screenPosition.X - (Main.graphics.GraphicsDevice.Viewport.Width / 2);
		drawPos.Y = -(worldPos.Y - Main.screenPosition.Y - (Main.graphics.GraphicsDevice.Viewport.Height / 2));
		return drawPos;
	}
}
