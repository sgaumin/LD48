using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
	public static Hud Instance { get; private set; }

	protected void Awake() => Instance = this;

	[Header("Animations")]
	[SerializeField] private float fadDuration = 0.15f;
	[SerializeField] private float fadDurationInputText = 0.5f;

	[Header("References")]
	[SerializeField] private TextMeshProUGUI depth;
	[SerializeField] private TextMeshProUGUI coin;
	[SerializeField] private TextMeshProUGUI stars;
	[SerializeField] private CanvasGroup stats;
	[SerializeField] private CanvasGroup buttons;
	[SerializeField] private TextMeshProUGUI inputStartText;
	[SerializeField] private Image wonImage;

	protected void Start()
	{
		Refresh();
		inputStartText.DOFade(0f, fadDurationInputText).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}

	public void Refresh()
	{
		UpdateCoin();
		UpdateStars();
		wonImage.gameObject.SetActive(GameData.Final);
	}

	public void ShowStartButtons()
	{
		buttons.interactable = true;
		buttons.DOFade(1f, fadDuration);
		wonImage.gameObject.SetActive(GameData.Final);
	}

	public void HideStartButtons()
	{
		buttons.interactable = false;
		buttons.DOFade(0f, fadDuration);
	}

	public void UpdateDepth(int value)
	{
		depth.text = $"{Mathf.Max(value, 0)}m";
	}

	public void UpdateCoin()
	{
		coin.text = $"{Mathf.Max(GameData.Coins, 0)}";
	}

	public void UpdateStars()
	{
		stars.text = $"{Mathf.Max(GameData.Stars, 0)}";
	}
}
