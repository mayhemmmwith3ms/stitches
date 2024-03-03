using Microsoft.Xna.Framework;
using Terraria;

namespace StitchesLib.Common.Utils;
public static class EntityUtils
{
	public static int ProjectileOwnerIndexFromEntity(Entity entity) => (entity is Player) ? entity.whoAmI : -1;

	public static bool GetClosestNPCTo(Vector2 toPoint, out NPC closest, out float distanceTo)
	{
		float dist = float.MaxValue;
		int closestIndex = -1;

		foreach (var e in Main.npc)
		{
			if (Vector2.Distance(e.Center, toPoint) < dist)
			{
				dist = Vector2.Distance(e.Center, toPoint);
				closestIndex = e.whoAmI;
			}
		}

		distanceTo = dist;

		if (closestIndex != -1)
		{
			closest = Main.npc[closestIndex];
			return true;
		}
		else
		{
			closest = null;
			return false;
		}
	}

	public static bool GetClosestProjectileTo(Vector2 toPoint, out Projectile closest, out float distanceTo)
	{
		float dist = float.MaxValue;
		int closestIndex = -1;

		foreach (var e in Main.projectile)
		{
			if (Vector2.Distance(e.Center, toPoint) < dist)
			{
				dist = Vector2.Distance(e.Center, toPoint);
				closestIndex = e.whoAmI;
			}
		}

		distanceTo = dist;

		if (closestIndex != -1)
		{
			closest = Main.projectile[closestIndex];
			return true;
		}
		else
		{
			closest = null;
			return false;
		}
	}

	public static bool GetClosestPlayerTo(Vector2 toPoint, out Player closest, out float distanceTo)
	{
		float dist = float.MaxValue;
		int closestIndex = -1;

		foreach (var e in Main.player)
		{
			if (Vector2.Distance(e.Center, toPoint) < dist)
			{
				dist = Vector2.Distance(e.Center, toPoint);
				closestIndex = e.whoAmI;
			}
		}

		distanceTo = dist;

		if (closestIndex != -1)
		{
			closest = Main.player[closestIndex];
			return true;
		}
		else
		{
			closest = null;
			return false;
		}
	}
}
