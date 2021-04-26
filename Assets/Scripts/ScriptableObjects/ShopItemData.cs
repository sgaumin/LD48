using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ShopItemData", order = 1)]
public class ShopItemData : ScriptableObject
{
	public ShopItemType type;
	public int shopIndex;
	public float value;
	public bool showValueOnButton;
	[Space]
	public Sprite sprite;
	public int coinsCost;
	public int starsCost;
	[TextArea(1, 5)] public string description;
}