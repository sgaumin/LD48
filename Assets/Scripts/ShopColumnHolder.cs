using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopColumnHolder : MonoBehaviour
{
	[SerializeField] private ShopItemType type;

	public ShopItemType Type => type;
	public List<ShopButton> ShopButtons { get; set; } = new List<ShopButton>();

	protected void Awake()
	{
		ShopButtons = GetComponentsInChildren<ShopButton>().ToList();
	}
}
