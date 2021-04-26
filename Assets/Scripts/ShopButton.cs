using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
	[SerializeField] private ShopItemData data;

	[Header("References")]
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI value;
	[SerializeField] private Image selectedImage;
	[SerializeField] private Image boughtImage;
	[SerializeField] private Image blockImage;
	[SerializeField] private Button button;

	private ShopPopup shop;
	private bool hasBeenBought;
	private bool isBlocked;
	private bool isSelected;

	public bool HasBeenBought
	{
		get => hasBeenBought;
		set
		{
			hasBeenBought = value;
			boughtImage.gameObject.SetActive(value);
			button.interactable = !value;
		}
	}

	public bool IsBlocked
	{
		get => isBlocked;
		set
		{
			isBlocked = value;
			blockImage.gameObject.SetActive(value);
			button.interactable = !value;
		}
	}

	public bool IsSelected
	{
		get => isSelected;
		set
		{
			isSelected = value;
			selectedImage.gameObject.SetActive(value);
		}
	}

	public ShopItemData Data => data;

	protected void Awake()
	{
		shop = GetComponentInParent<ShopPopup>();
	}

	protected void Start()
	{
		image.sprite = Data.sprite;
		if (Data.showValueOnButton)
		{
			value.text = $"{Data.value}";
		}
	}

	public void Select()
	{
		shop.SelectedItem = this;
	}

	public void ResetState()
	{
		HasBeenBought = false;
		IsBlocked = false;
		IsSelected = false;
	}
}
