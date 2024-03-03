using Microsoft.Xna.Framework;
using System;

namespace StitchesLib.Common.Systems.ParticleSystem;
public abstract class Particle : IDisposable
{
	public Color color;

	public Vector2 position;
	public Vector2 velocity;

	public int lifetime;

	public bool active = true;

	public virtual void OnSpawn() { }

	public virtual void Update()
	{
		position += velocity;
		lifetime--;

		if (lifetime <= 0)
		{
			Dispose();
		}
	}

	public virtual void Draw() { }

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		active = false;
	}
}
