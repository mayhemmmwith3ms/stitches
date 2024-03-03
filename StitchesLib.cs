using StitchesLib.Common.Managers.ParticleSystem;
using StitchesLib.Common.Managers.Primitives;
using Terraria.ModLoader;

namespace StitchesLib;

public class StitchesLib : Mod
{
	public static StitchesLib Instance { get; private set; }

	public StitchesLib()
	{
		Instance = this;
	}
}