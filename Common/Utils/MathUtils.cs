using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace StitchesLib.Common.Utils;
public static class MathUtils
{
	public static Vector2 ConeSpread(this Vector2 vector, float arc, float minVelocityMul = 1f, float maxVelocityMul = 1f) => vector.RotatedByRandom(arc) * Main.rand.NextFloat(minVelocityMul, maxVelocityMul);

	public static Vector2 VectorRoundToInts(this Vector2 vector) => new((int)vector.X, (int)vector.Y);

	public static Point ConvertToPoint(this Vector2 vector) => new((int)vector.X, (int)vector.Y);

	public static double Sin01(double a) => (Math.Sin(a) + 1) / 2;

	public static double Cos01(double a) => (Math.Cos(a) + 1) / 2;
}
