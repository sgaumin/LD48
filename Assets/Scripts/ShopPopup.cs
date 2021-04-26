using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : MonoBehaviour
{
	[Header("Animations")]
	[SerializeField] private Color unvalidCostColor;

	[Header("References")]
	[SerializeField] private ShopButton defaultButton;
	[SerializeField] private List<ShopColumnHolder> holders = new List<ShopColumnHolder>();
	[SerializeField] private TextMeshProUGUI description;
	[SerializeField] private TextMeshProUGUI coinsCostText;
	[SerializeField] private TextMeshProUGUI starsCostText;
	[SerializeField] private Button buyButton;

	private ShopButton selectedItem;

	public ShopButton SelectedItem
	{
		get => selectedItem;
		set
		{
			if (selectedItem != null)
			{
				selectedItem.IsSelected = false;
			}

			selectedItem = value;
			buyButton.gameObject.SetActive(GameData.Coins >= selectedItem.Data.coinsCost && GameData.Stars >= selectedItem.Data.starsCost);
			selectedItem.IsSelected = true;
			description.text = selectedItem.Data.description;
			coinsCostText.text = $"{selectedItem.Data.coinsCost}";
			coinsCostText.color = GameData.Coins >= selectedItem.Data.coinsCost ? Color.white : unvalidCostColor;
			starsCostText.gameObject.SetActive(true);
			starsCostText.text = $"{selectedItem.Data.starsCost}";
			starsCostText.color = GameData.Stars >= selectedItem.Data.starsCost ? Color.white : unvalidCostColor;
		}
	}

	protected void Start()
	{
		Refresh();
		buyButton.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		if (selectedItem != null)
		{
			SelectedItem = selectedItem;
		}
	}

	private void Refresh()
	{
		foreach (ShopColumnHolder holder in holders)
		{
			int index = 0;
			switch (holder.Type)
			{
				case ShopItemType.Depth:
					index = GameData.ShopDepth;
					break;
				case ShopItemType.Block:
					index = GameData.ShopBlock;
					break;
				case ShopItemType.Storage:
					index = GameData.ShopStorage;
					break;
				case ShopItemType.GoldMultiplier:
					index = GameData.ShopGoldMultiplier;
					break;
			}

			holder.ShopButtons.ForEach(x => x.ResetState());

			for (int i = 0; i < index; i++)
			{
				holder.ShopButtons[i].HasBeenBought = true;
			}

			for (int i = holder.ShopButtons.Count - 1; i > index; i--)
			{
				holder.ShopButtons[i].IsBlocked = true;
			}
		}
	}

	public void SetDescription(string value)
	{
		description.text = value;
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void DoBuy()
	{
		if (GameData.Coins >= selectedItem.Data.coinsCost && GameData.Stars >= selectedItem.Data.starsCost)
		{
			GameData.Coins -= selectedItem.Data.coinsCost;
			GameData.Stars -= selectedItem.Data.starsCost;

			switch (selectedItem.Data.type)
			{
				case ShopItemType.Depth:
					GameData.ShopDepth = selectedItem.Data.shopIndex;
					GameData.Depth = (int)selectedItem.Data.value;
					break;
				case ShopItemType.Block:
					GameData.ShopBlock = selectedItem.Data.shopIndex;
					GameData.Block = (int)selectedItem.Data.value;
					break;
				case ShopItemType.Storage:
					GameData.ShopStorage = selectedItem.Data.shopIndex;
					GameData.Storage = (int)selectedItem.Data.value;
					break;
				case ShopItemType.GoldMultiplier:
					GameData.ShopGoldMultiplier = selectedItem.Data.shopIndex;
					break;
			}
		}

		Refresh();
	}
}
