using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StitchesLib.Common.Managers.AutoUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using Terraria.UI;
using Terraria;
using StitchesLib.Common;

namespace StitchesLib.Content.UI.DebugMenuUI;

public class DebugMenuMainPanel : UIElement
{
	public List<string> infoList = new();
	private List<PersistentInfoItem> persistentInfoList = new();
	public UIImageButtonWithHoverText togglePrimitiveVertsButton;

	public bool menuVisible = false;
	public bool visualisePrims = false;

	public override void OnInitialize()
	{
		togglePrimitiveVertsButton = new(ModContent.Request<Texture2D>(Directories.Textures + "GenericUIButton"), "Toggle Primitive Vertices");
		togglePrimitiveVertsButton.SetHoverImage(ModContent.Request<Texture2D>(Directories.Textures + "GenericUIButtonHighlight"));
		togglePrimitiveVertsButton.OnLeftClick += new MouseEvent(PrimVertButtonClicked);
		togglePrimitiveVertsButton.SetVisibility(1f, 0.8f);

		AutoUIState.AddElement(this, togglePrimitiveVertsButton, 42f, 0f, 20f, 0f, 20f, 20f);
	}

	private void PrimVertButtonClicked(UIMouseEvent evt, UIElement listeningElement)
	{
		if (!menuVisible)
			return;

		visualisePrims = !visualisePrims;
	}

	public override void Update(GameTime gameTime)
	{
		if (!menuVisible)
			return;

		base.Update(gameTime);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (!menuVisible)
			return;

		base.Draw(spriteBatch);

		/*if (menuVisible)
		{
			infoList.Add("Particle Layer Info:");

			noitarraria.Instance.particleHandler.layers.ForEach(x =>
			{
				infoList.Add($"{x.name}: {x.particles.Count} particles active");
			});

			DrawInfoList(spriteBatch);
		}
		//Main.NewText(RootRect.TopLeft());
		DebugUtils.DrawDebugPixel(RootRect.TopLeft());*/
	}

	public void DrawInfoList(SpriteBatch spriteBatch)
	{
		Vector2 offset = new(24, 50);

		Vector2 stringPos = new(0, 0);

		foreach (var s in infoList)
		{
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, s, RootRect.TopLeft() + stringPos + offset, Color.White, 0f, Vector2.Zero, Vector2.One);
			stringPos.Y += 20;
		}

		infoList.Clear();

		stringPos = new(300, 0);

		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, "Logged Values:", RootRect.TopLeft() + stringPos + offset, Color.White, 0f, Vector2.Zero, Vector2.One);

		stringPos.Y += 20;

		foreach (var s in persistentInfoList)
		{
			s.UpdateAndDraw(spriteBatch, RootRect.TopLeft() + stringPos + offset);
			stringPos.Y += 20;
		}

		persistentInfoList.RemoveAll(x => !x.active);
	}

	public static void PersistentLog(object value, string key = "unnamed")
	{
		AutoUILoader.GetAutoUIState<DebugMenuState>().mainPanel.AddPersistentInfo(value, key);
	}

	public void AddPersistentInfo(object value, string key = "unnamed")
	{
		persistentInfoList.RemoveAll(x => x.key == key);
		persistentInfoList.Insert(0, new(value, key));
	}

	public Rectangle RootRect => GetInnerDimensions().ToRectangle();

	private class PersistentInfoItem
	{
		public PersistentInfoItem(object value, string key = "unnamed", int timeTillFade = 600)
		{
			info = value.ToString();
			this.key = key;
			fadeTimer = timeTillFade;
		}

		public string info;
		public string key;
		public int fadeTimer;
		public bool active = true;

		public void UpdateAndDraw(SpriteBatch spriteBatch, Vector2 position)
		{
			string text = $"{key}: {info}";

			Color color = Color.Lerp(Color.Transparent, Color.White, Math.Min(1f, fadeTimer / 120f));

			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position, color, 0f, Vector2.Zero, Vector2.One);

			fadeTimer--;

			if (fadeTimer <= 0)
			{
				active = false;
			}
		}
	}
}
