using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace StitchesLib.Common;
// this could probably be redone eventually but it doesnt really matter
internal class BuiltInEffectsLoader : ILoadable
{
	public void Load(Mod mod)
	{
		LoadMiscShader(Directories.Effects + "DrawOnTilesEffect", "DrawOnTiles");
	}

	public void Unload() { }

	public static void LoadMiscShader(string path, string name)
	{
		GameShaders.Misc[$"{Directories.ModName}:{name}"] = new MiscShaderData(new Ref<Effect>(ModContent.Request<Effect>(path, AssetRequestMode.ImmediateLoad).Value), name + "Pass");
	}
}
