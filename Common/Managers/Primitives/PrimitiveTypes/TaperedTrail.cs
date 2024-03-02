using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using StitchesLib.Common.Utils;

namespace StitchesLib.Common.Managers.Primitives.PrimitiveTypes;
public class TaperedTrail : IDisposable
{
	public TaperedTrail(int _maxLength, widthFunc _widthFunc, colorFunc _colourFunc)
	{
		prim = new Primitive(_maxLength * 2, 6 * (_maxLength - 1));

		path = new Vector2[_maxLength];

		width = _widthFunc;
		color = _colourFunc;
	}

	public void Dispose()
	{
		prim.active = false;
	}

	public Effect Effect { get => prim.effect; set => prim.effect = value; }

	public Primitive prim;

	public Vector2[] path;

	public delegate float widthFunc(float progress);
	public delegate Color colorFunc(Vector2 texCoord);

	public widthFunc width;
	public colorFunc color;

	public void Render()
	{
		prim.mesh = GenerateMesh();
		prim.Render();
	}

	Primitive.Mesh GenerateMesh()
	{
		VertexPositionColorTexture[] tempVertices = new VertexPositionColorTexture[(path.Length * 2)];
		List<short> tempIndices = new();

		int length = path.Length;

		for (int t = 0; t < length; t++)
		{
			float trailProgress = (float)t / (length - 1);

			float segmentWidth = width?.Invoke(trailProgress) ?? 12;

			Vector2 toNextPoint = t == length - 1 ? path[t] - path[t - 1] : path[t + 1] - path[t];
			Vector2 toLastPoint = t == 0 ? path[t + 1] - path[t] : path[t] - path[t - 1];

			Vector2 perpendicularToNextPoint = toNextPoint.SafeNormalize(Vector2.Zero).RotatedBy(-MathHelper.PiOver2);
			Vector2 perpendicularToLastPoint = toLastPoint.SafeNormalize(Vector2.Zero).RotatedBy(-MathHelper.PiOver2);

			Vector2 avgFirstLast = Vector2.Normalize((perpendicularToNextPoint + perpendicularToLastPoint) / 2);

			Vector2 top = path[t] + avgFirstLast * segmentWidth;
			Vector2 bottom = path[t] - avgFirstLast * segmentWidth;
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

		return new Primitive.Mesh(tempVertices, tempIndices.ToArray());
	}
}
