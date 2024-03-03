using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StitchesLib.Common.Systems.AutoUI;
using StitchesLib.Content.UI.DebugMenuUI;
using System;
using Terraria;

namespace StitchesLib.Common.Systems.Primitives;

public class Primitive : IDisposable
{
	public Primitive(int maxVertices, int maxIndices)
	{
		device = Main.graphics.GraphicsDevice;

		if (device != null)
		{
			Main.QueueMainThreadAction(() =>
			{
				vBuffer = new DynamicVertexBuffer(device, typeof(VertexPositionColorTexture), maxVertices, BufferUsage.None);
				iBuffer = new DynamicIndexBuffer(device, IndexElementSize.SixteenBits, maxIndices, BufferUsage.None);
			});
		}
	}

	public void Dispose()
	{
		vBuffer.Dispose();
		iBuffer.Dispose();
		effect = null;
	}

	Matrix world = Matrix.CreateTranslation(0, 0, 0);
	Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
	Matrix projection = Matrix.CreateOrthographic(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height, 0, 1000);

	public GraphicsDevice device;

	private DynamicVertexBuffer vBuffer;
	public DynamicIndexBuffer iBuffer;

	public Effect effect;

	public Mesh mesh;

	public bool active = true;

	public void Render()
	{
		if (!active)
		{
			Dispose();
			return;
		}

		if (AutoUILoader.GetAutoUIState<DebugMenuState>().mainPanel.visualisePrims) DebugVisualiseVertices();

		device = Main.graphics.GraphicsDevice;

		if (effect is not BasicEffect) //i don't think this actually does anything lol
		{
			effect.Parameters["transformMatrix"].SetValue(world * view * projection);
		}
		else
		{
			BasicEffect basicEffect = effect as BasicEffect;
			basicEffect.World = world;
			basicEffect.View = view;
			basicEffect.Projection = projection;
		}

		vBuffer.SetData(0, mesh._vertices, 0, mesh._vertices.Length, VertexPositionColorTexture.VertexDeclaration.VertexStride, SetDataOptions.Discard); //haha! i dont remember how any of this works
		iBuffer.SetData(0, mesh._indices, 0, mesh._indices.Length, SetDataOptions.Discard);

		device.SetVertexBuffer(vBuffer);
		device.Indices = iBuffer;

		device.RasterizerState = new RasterizerState { CullMode = CullMode.None /*TODO fix sword trail breakage*/};

		foreach (EffectPass pass in effect.CurrentTechnique.Passes)
		{
			pass.Apply();
			device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vBuffer.VertexCount, 0, iBuffer.IndexCount / 3);
		}
	}

	private void DebugVisualiseVertices()
	{
		/*MiscDrawManager.debugDrawCalls.Add(() =>
		{
			for (int i = 0; i < mesh._vertices.Length; i++)
			{
				float x = mesh._vertices[i].Position.X + Main.screenPosition.X + Main.graphics.GraphicsDevice.Viewport.Width / 2;
				float y = -mesh._vertices[i].Position.Y + Main.screenPosition.Y + Main.graphics.GraphicsDevice.Viewport.Height / 2;
				DebugUtils.DrawDebugPixel(new Vector2(x, y));
			}
		});*/
	}

	public struct Mesh
	{
		public VertexPositionColorTexture[] _vertices;
		public short[] _indices;

		public Mesh(VertexPositionColorTexture[] vertices, short[] indices)
		{
			_vertices = vertices;
			_indices = indices;
		}
	}
}
