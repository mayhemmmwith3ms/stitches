using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace StitchesLib.Common.Systems.ScreenTargets;
public class ScreenTarget
{
	public ScreenTarget(Action _drawTo, Func<bool> _isVisible, Action<Vector2> _onResize = null, float _priority = 0)
	{
		drawTo = _drawTo;
		isVisible = _isVisible;
		onResize = _onResize;
		Priority = _priority;

		Vector2 dimensions = Utils.GraphicsUtils.ScreenRect.Size();

		Main.QueueMainThreadAction(() =>
		{
			Target = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y, false, default, default, default, RenderTargetUsage.PreserveContents);
		});

		onResize?.Invoke(dimensions);

		ScreenTargetManager.RegisterTarget(this);
	}

	public Action drawTo;

	public Func<bool> isVisible;

	public Action<Vector2> onResize;

	public float Priority { get; set; } = 0;

	public RenderTarget2D Target { get; set; }

	public void ResizeTarget(Vector2 newSize)
	{
		Target?.Dispose();
		Target = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)newSize.X, (int)newSize.Y, false, default, default, default, RenderTargetUsage.PreserveContents);
	}
}
