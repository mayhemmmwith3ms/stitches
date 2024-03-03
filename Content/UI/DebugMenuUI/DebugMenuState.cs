using Microsoft.Xna.Framework.Graphics;
using StitchesLib.Common;
using StitchesLib.Common.Systems.AutoUI;
using StitchesLib.Content.Config;
using Terraria.ModLoader;
using Terraria.UI;

namespace StitchesLib.Content.UI.DebugMenuUI;

public class DebugMenuState : AutoUIState
{
	public override bool Visible => StitchesConfig.DebugModeActive;

	public UIElement root;
	public UIImageButtonWithHoverText toggleMenuButton;
	public DebugMenuMainPanel mainPanel;

	public override void OnInitialize()
	{
		root = new();
		SetBounds(root, 40f, 0f, 20f, 0.3f, 200f, 200f);
		mainPanel = new();
		AddElement(root, mainPanel, 0f, 0f, 200f, 200f);

		toggleMenuButton = new(ModContent.Request<Texture2D>(Directories.Textures + "GenericUIButton"), "Toggle Debug Menu");
		toggleMenuButton.SetHoverImage(ModContent.Request<Texture2D>(Directories.Textures + "GenericUIButtonHighlight"));
		toggleMenuButton.OnLeftClick += new MouseEvent(ToggleButtonClicked);
		toggleMenuButton.SetVisibility(1f, 0.8f);

		AddElement(root, toggleMenuButton, 20f, 0f, 20f, 0f, 20f, 20f);

		Append(root);
	}

	private void ToggleButtonClicked(UIMouseEvent evt, UIElement listeningElement)
	{
		mainPanel.menuVisible = !mainPanel.menuVisible;
	}
}
