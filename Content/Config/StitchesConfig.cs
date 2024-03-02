using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace StitchesLib.Content.Config;

public class StitchesConfig : ModConfig
{
	public override ConfigScope Mode => ConfigScope.ServerSide;

	[DefaultValue(false)]
	public bool DebugMode { get; set; }

	public static bool DebugModeActive => ModContent.GetInstance<StitchesConfig>().DebugMode;
}
