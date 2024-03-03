using Terraria.UI;

namespace StitchesLib.Common.Systems.AutoUI;

public abstract class AutoUIState : UIState
{
	public UserInterface UI;

	public virtual string InterfaceLayerToInsertAt => "Vanilla: Resource Bars";

	public virtual bool Visible => true;

	public static void SetBounds(UIElement element, float left, float top, float width, float height)
	{
		element.Left.Set(left, 0f);
		element.Top.Set(top, 0f);
		element.Width.Set(width, 0f);
		element.Height.Set(height, 0f);
	}

	public static void SetBounds(UIElement element, float left, float leftPercent, float top, float topPercent, float width, float height)
	{
		element.Left.Set(left, leftPercent);
		element.Top.Set(top, topPercent);
		element.Width.Set(width, 0f);
		element.Height.Set(height, 0f);
	}

	public static void AddElement(UIElement parent, UIElement element, float left, float top, float width, float height)
	{
		SetBounds(element, left, top, width, height);
		parent.Append(element);
	}

	public static void AddElement(UIElement parent, UIElement element, float left, float leftPercent, float top, float topPercent, float width, float height)
	{
		SetBounds(element, left, leftPercent, top, topPercent, width, height);
		parent.Append(element);
	}
}
