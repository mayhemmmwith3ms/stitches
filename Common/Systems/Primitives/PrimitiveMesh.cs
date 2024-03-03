using Microsoft.Xna.Framework.Graphics;

namespace StitchesLib.Common.Systems.Primitives;

public struct PrimitiveMesh
{
	public VertexPositionColorTexture[] vertices;
	public short[] indices;

	public PrimitiveMesh(VertexPositionColorTexture[] vertices, short[] indices)
	{
		this.vertices = vertices;
		this.indices = indices;
	}
}
