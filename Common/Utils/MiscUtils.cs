using System.Linq;

namespace StitchesLib.Common.Utils;
public static class MiscUtils
{
	public static string SignedDisplayText(float num) => $"{(num >= 0 ? "+" : "")}{num}";

	public static string SignedDisplayText(int num) => $"{(num >= 0 ? "+" : "")}{num}";

	public static bool CheckAllElementsEqual<T>(this T[] array)
	{
		var first = array.First();
		return array.All(x => x.Equals(first));
	}

	public static T[] FillArray<T>(this T[] array, T value)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = value;
		}
		return array;
	}

	public static T[] ShiftArrayDown<T>(this T[] array, T newValueAtIndex0)
	{
		for (int i = array.Length - 1; i > 0; i--)
		{
			array[i] = array[i - 1];
		}
		array[0] = newValueAtIndex0;
		return array;
	}
}
