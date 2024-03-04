using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace StitchesLib.Common.Systems;

public class DebugDrawManager : ILoadable
{
    public static List<Action> DebugDrawQueue { get; set; }

    public void Load(Mod mod)
    {
        DebugDrawQueue = new();

        On_Main.DrawDust += On_Main_DrawDust;
    }

    public void Unload() { }

    private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
    {
        orig(self);

		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

		DebugDrawQueue.ForEach(x => x.Invoke());
        DebugDrawQueue.Clear();

        Main.spriteBatch.End();
    }
}
