using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
