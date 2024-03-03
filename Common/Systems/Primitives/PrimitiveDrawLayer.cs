using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace StitchesLib.Common.Systems.Primitives;

public class PrimitiveDrawLayer
{
	public PrimitiveDrawLayer(string name)
	{
		this.name = name;
	}

	public string name;

	public RenderTarget2D renderTargetToDrawTo = null;

	public List<Action> drawFuncs = new();

	public void Add(Action func)
	{
		drawFuncs.Add(func);
	}

	public void RenderAll()
	{
		GraphicsDevice gd = Main.graphics.GraphicsDevice;

		gd.BlendState = BlendState.AlphaBlend;

		if (renderTargetToDrawTo is not null)
		{
			gd.SetRenderTarget(renderTargetToDrawTo); // OH BOY I HOPE THIS WORKS
		}

		foreach (Action func in drawFuncs)
		{
			func?.Invoke();
		}

		if (renderTargetToDrawTo is not null)
		{
			gd.SetRenderTarget(null);
		}
	}
}
