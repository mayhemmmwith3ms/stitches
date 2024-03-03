using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StitchesLib.Common.Systems.ParticleSystem;

public class ParticleLayer
{
	public ParticleLayer(string name)
	{
		this.name = name;
		particles = new();
	}

	public string name;

	public List<Particle> particles;

	public T NewParticle<T>(Vector2 position, Vector2 velocity, int lifetime, Color color) where T : Particle
	{
		T p = (T)Activator.CreateInstance(typeof(T));
		p.position = position;
		p.velocity = velocity;
		p.lifetime = lifetime;
		p.color = color;
		p.OnSpawn();
		particles.Add(p);
		return p;
	}

	public void DrawAll()
	{
		particles.RemoveAll(x => !x.active);

		particles.ForEach(x => x.Update());
		particles.ForEach(x => x.Draw());
	}
}
