using Tools.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CollectibleData", order = 1)]
public class CollectibleData : ScriptableObject
{
	public CollectibleType type;
	public int value;
	public Sprite itemSprite;
	public Sprite dangerousSprite;
	[IntRangeSlider(-100, 0)] public IntRange area = new IntRange(-10, 0);
}