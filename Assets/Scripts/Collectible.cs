using DG.Tweening;
using System.Collections;
using Tools.Utils;
using UnityEngine;

public class Collectible : MonoBehaviour
{
	[SerializeField] private CollectibleData data;
	[SerializeField, FloatRangeSlider(0f, 1f)] private FloatRange smoothTime = new FloatRange(0.1f, 0.3f);

	[Header("Animations")]
	[SerializeField, FloatRangeSlider(1f, 1.5f)] private FloatRange strecthMultiplier = new FloatRange(1.05f, 1.1f);
	[SerializeField, FloatRangeSlider(0f, 2f)] private FloatRange stretchDuration = new FloatRange(1f, 2f);
	[SerializeField] private Ease stretchEase = Ease.InOutCubic;

	[Header("Audio")]
	[SerializeField] private AudioExpress explosion;

	[Header("References")]
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private GameObject explosionEffect;

	private bool isDangerous;
	private Vector2 currentSpeed = Vector2.zero;
	private float currentSmoothTime;
	private Coroutine stretchCoroutine;
	private Transform target;

	public DepthLevels DepthLevel { get; set; }

	public Transform Target
	{
		get => target;
		set
		{
			target = value;
			if (target != null && stretchCoroutine != null)
			{
				StopCoroutine(stretchCoroutine);
			}
		}
	}

	public bool HasBeenCollected { get; set; }

	public bool IsDangerous
	{
		get => isDangerous;
		set
		{
			isDangerous = value;
			spriteRenderer.sprite = isDangerous ? data.dangerousSprite : data.itemSprite;
		}
	}
	public CollectibleData Data => data;

	protected void Start()
	{
		IsDangerous = false;
		currentSmoothTime = smoothTime.RandomValue;

		stretchCoroutine = StartCoroutine(DoStrechtAnimation());
	}

	private void Update()
	{
		if (Target != null)
		{
			transform.position = Vector2.SmoothDamp(transform.position, Target.position, ref currentSpeed, currentSmoothTime);
		}
	}

	private IEnumerator DoStrechtAnimation()
	{
		while (true)
		{
			float currentStrecthFactor = strecthMultiplier.RandomValue;
			float currentStrecthDuration = stretchDuration.RandomValue;

			transform.DOScaleX(transform.localScale.x * currentStrecthFactor, currentStrecthDuration).SetEase(stretchEase).SetLoops(2, LoopType.Yoyo);

			yield return new WaitForSeconds(currentStrecthDuration * 2);
		}
	}

	public void DoInteraction()
	{
		if (IsDangerous)
		{
			explosion.Play();
			gameObject.SetActive(false);

			GameObject o = Instantiate(explosionEffect);
			o.transform.position = transform.position;
		}
	}
}
