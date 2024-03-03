using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StitchesLib.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;

namespace StitchesLib.Common.Systems.Primitives.PrimitiveWrappers;

public sealed class TaperedTrail : IDisposable
{
	public TaperedTrail(int maxLength, widthFunc widthFunc, colorFunc colourFunc)
	{
		_primitive = new Primitive(maxLength * 2, 6 * (maxLength - 1));

		PathPoints = new Vector2[maxLength];

		width = widthFunc;
		color = colourFunc;
	}

	public void Dispose()
	{
		_primitive.active = false;
	}

	public delegate float widthFunc(float progress);
	public delegate Color colorFunc(Vector2 texCoord);

	public Effect Effect { get => _primitive.effect; set => _primitive.effect = value; }

	public Vector2[] PathPoints { get; set; }

	public widthFunc width;
	public colorFunc color;

	private readonly Primitive _primitive;

	public void Render()
	{
		_primitive.mesh = GenerateMesh();
		_primitive.Render();
	}

	PrimitiveMesh GenerateMesh()
	{
		VertexPositionColorTexture[] tempVertices = new VertexPositionColorTexture[(PathPoints.Length * 2)];
		List<short> tempIndices = new();

		int length = PathPoints.Length;

		for (int t = 0; t < length; t++)
		{
			float trailProgress = (float)t / (length - 1);

			float segmentWidth = width?.Invoke(trailProgress) ?? 12;

			Vector2 toNextPoint = t == length - 1 ? PathPoints[t] - PathPoints[t - 1] : PathPoints[t + 1] - PathPoints[t];
			Vector2 toLastPoint = t == 0 ? PathPoints[t + 1] - PathPoints[t] : PathPoints[t] - PathPoints[t - 1];

			Vector2 perpendicularToNextPoint = toNextPoint.SafeNormalize(Vector2.Zero).RotatedBy(-MathHelper.PiOver2);
			Vector2 perpendicularToLastPoint = toLastPoint.SafeNormalize(Vector2.Zero).RotatedBy(-MathHelper.PiOver2);

			Vector2 avgFirstLast = Vector2.Normalize((perpendicularToNextPoint + perpendicularToLastPoint) / 2);

			Vector2 top = PathPoints[t] + avgFirstLast * segmentWidth;
			Vector2 bottom = PathPoints[t] - avgFirstLast * segmentWidth;
			Vector2 topTexCoord = new(trailProgress, 0);
			Vector2 bottomTexCoord = new(trailProgress, 1);

			Color topVertexColor = color?.Invoke(topTexCoord) ?? Color.White;
			Color bottomVertexColor = color?.Invoke(bottomTexCoord) ?? Color.White;

			tempVertices[t] = new VertexPositionColorTexture(new Vector3(top.WorldToZeroCenteredScreenPos(), 0), topVertexColor, topTexCoord);
			tempVertices[t + length] = new VertexPositionColorTexture(new Vector3(bottom.WorldToZeroCenteredScreenPos(), 0), bottomVertexColor, bottomTexCoord);
		}

		for (short i = 0; i < length - 1; i++)
		{
			short[] segmentIndices = new short[]
			{
					i,
					(short)(i + length),
					(short)(i + length + 1),

					(short)(i + length + 1),
					(short)(i + 1),
					i
			};

			tempIndices.AddRange(segmentIndices);
		}

		Vector2 zoom = Main.GameViewMatrix.Zoom;

		for (int v = 0; v < tempVertices.Length; v++)
		{
			tempVertices[v].Position.X *= zoom.X;
			tempVertices[v].Position.Y *= zoom.Y;
		}

		return new PrimitiveMesh(tempVertices, tempIndices.ToArray());
	}
}
