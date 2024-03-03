using Microsoft.Xna.Framework.Graphics;

namespace StitchesLib.Common.Systems.Primitives;

public struct PrimitiveMesh
{
	public VertexPositionColorTexture[] _vertices;
	public short[] _indices;

	public PrimitiveMesh(VertexPositionColorTexture[] vertices, short[] indices)
	{
		_vertices = vertices;
		_indices = indices;
	}
}
