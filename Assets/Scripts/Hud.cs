using DG.Tweening;
using TMPro;
using UnityEngine;

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

	protected void Start()
	{
		UpdateCoin();
		UpdateStars();

		inputStartText.DOFade(0f, fadDurationInputText).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}

	public void ShowStartButtons()
	{
		buttons.interactable = true;
		buttons.DOFade(1f, fadDuration);
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
