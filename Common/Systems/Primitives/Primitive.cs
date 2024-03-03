using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StitchesLib.Common.Systems.AutoUI;
using StitchesLib.Common.Utils;
using StitchesLib.Content.UI.DebugMenuUI;
using System;
using Terraria;

namespace StitchesLib.Common.Systems.Primitives;

public class Primitive : IDisposable
{
	public Primitive(int maxVertices, int maxIndices)
	{
		_gd = Main.graphics.GraphicsDevice;

		if (_gd != null)
		{
			Main.QueueMainThreadAction(() =>
			{
				_vBuffer = new DynamicVertexBuffer(_gd, typeof(VertexPositionColorTexture), maxVertices, BufferUsage.None);
				_iBuffer = new DynamicIndexBuffer(_gd, IndexElementSize.SixteenBits, maxIndices, BufferUsage.None);
			});
		}
	}

	public void Dispose()
	{
		_vBuffer.Dispose();
		_iBuffer.Dispose();
		effect = null;
	}

	public Effect effect;

	public PrimitiveMesh mesh;

	public bool active = true;

	private Matrix _world = Matrix.CreateTranslation(0, 0, 0);
	private Matrix _view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
	private Matrix _projection = Matrix.CreateOrthographic(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height, 0, 1000);

	private readonly GraphicsDevice _gd;

	private DynamicVertexBuffer _vBuffer;
	private DynamicIndexBuffer _iBuffer;

	public void Render()
	{
		if (!active)
		{
			Dispose();
			return;
		}

		if (AutoUILoader.GetAutoUIState<DebugMenuState>().mainPanel.visualisePrims) DebugVisualiseVertices();

		//_gd = Main.graphics.GraphicsDevice;

		if (effect is not BasicEffect) //i don't think this actually does anything lol
		{
			effect.Parameters["transformMatrix"].SetValue(_world * _view * _projection);
		}
		else
		{
			BasicEffect basicEffect = effect as BasicEffect;
			basicEffect.World = _world;
			basicEffect.View = _view;
			basicEffect.Projection = _projection;
		}

		_vBuffer.SetData(0, mesh.vertices, 0, mesh.vertices.Length, VertexPositionColorTexture.VertexDeclaration.VertexStride, SetDataOptions.Discard); //haha! i dont remember how any of this works
		_iBuffer.SetData(0, mesh.indices, 0, mesh.indices.Length, SetDataOptions.Discard);

		_gd.SetVertexBuffer(_vBuffer);
		_gd.Indices = _iBuffer;

		_gd.RasterizerState = new RasterizerState { CullMode = CullMode.None /*TODO fix sword trail breakage*/};

		foreach (EffectPass pass in effect.CurrentTechnique.Passes)
		{
			pass.Apply();
			_gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vBuffer.VertexCount, 0, _iBuffer.IndexCount / 3);
		}
	}

	private void DebugVisualiseVertices()
	{
		DebugDrawManager.DebugDrawQueue.Add(() =>
		{
			for (int i = 0; i < mesh.vertices.Length; i++)
			{
				float x = mesh.vertices[i].Position.X + Main.screenPosition.X + Main.graphics.GraphicsDevice.Viewport.Width / 2;
				float y = -mesh.vertices[i].Position.Y + Main.screenPosition.Y + Main.graphics.GraphicsDevice.Viewport.Height / 2;
				DebugUtils.DrawDebugPixel(new Vector2(x, y));
			}
		});
	}
}
