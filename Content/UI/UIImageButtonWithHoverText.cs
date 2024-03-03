using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;

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
