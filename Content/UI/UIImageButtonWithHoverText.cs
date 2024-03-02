using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria;

namespace StitchesLib.Content.UI;

public class UIImageButtonWithHoverText : UIImageButton
{
	public UIImageButtonWithHoverText(Asset<Texture2D> texture, string hoverText) : base(texture)
	{
		this.hoverText = hoverText;
	}

	public string hoverText;

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		if (IsMouseHovering)
		{
			Main.instance.MouseText(hoverText);
			Main.LocalPlayer.mouseInterface = true;
		}
	}
}
