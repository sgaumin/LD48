using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Tools.Utils;
using UnityEngine;

public class Grapple : MonoBehaviour
{
	[Header("Inputs")]
	[SerializeField] private string startButtonName = "Submit";

	[Header("Vertical Movements")]
	[SerializeField] private float moveSpeedY = 1f;
	[SerializeField] private float moveSpeedYEmpty = 2f;
	[SerializeField] private float headSmoothTimeY = 0.3f;

	[Header("Horizontal Movements")]
	[SerializeField] private float baseSmoothTimeX = 0.15f;
	[SerializeField] private float headSmoothTimeX = 0.3f;
	[SerializeField] private float borderLimitX = 0.8f;

	[Header("Animations")]
	[SerializeField] private float shieldDestructionDuration = 0.6f;

	[Header("Audio")]
	[SerializeField] private AudioExpress grappleMoveSound;
	[SerializeField] private AudioExpress grappleFallSound;
	[SerializeField] private AudioExpress pickupSound;

	[Header("Sprite")]
	[SerializeField] private Sprite headClose;
	[SerializeField] private Sprite headOpen;

	[Header("References")]
	[SerializeField] private List<SpriteRenderer> shields = new List<SpriteRenderer>();
	[SerializeField] private List<SpriteRenderer> storages = new List<SpriteRenderer>();
	[SerializeField] private Level level;
	[SerializeField] private LineRenderer line;
	[SerializeField] private SpriteRenderer baseGrapple;
	[SerializeField] private Transform baseLinePoint;
	[SerializeField] private SpriteRenderer head;
	[SerializeField] private Transform headLinePoint;
	[SerializeField] private SpriteRenderer depthLimit;
	[SerializeField] private Transform collectPoint;
	[SerializeField] private CinemachineImpulseSource cinemachineImpulse;

	public GrappleStatus Status { get; set; }

	private AudioUnit grappleMoveUnitSound;
	private AudioUnit grappleFallUnitSound;
	private List<Collectible> collectibles = new List<Collectible>();
	private bool hasEnded;
	private Vector2 destinationX;
	private Vector2 destinationY;
	private float moveX;
	private Vector2 baseCurrentSpeed = Vector2.zero;
	private Vector2 headCurrentSpeedX = Vector2.zero;
	private Vector2 headCurrentSpeedY = Vector2.zero;
	private Vector2 baseTargetDestination;
	private Vector2 headTargetDestinationX;
	private Vector2 headTargetDestinationY;
	private int currentBlockPoints;
	private int currentStoragePoints;
	private Vector2 startPosition;
	private float currentMoveSpeedY;
	private bool fastClearDone;

	public bool IsGoingBackToBaseEmpty() => Status == GrappleStatus.Up && collectibles.IsEmpty();

	protected void Start()
	{
		Status = GrappleStatus.Base;
		startPosition = head.transform.position;
		line.positionCount = 2;
		fastClearDone = false;

		grappleMoveUnitSound = grappleMoveSound.Play();
		grappleFallUnitSound = grappleFallSound.Play();

		Init();
	}

	public void Init()
	{
		StopAllCoroutines();

		currentBlockPoints = GameData.Block;
		currentStoragePoints = GameData.Storage;

		collectibles.Clear();

		head.sprite = headOpen;

		Refresh();
	}

	public void Refresh()
	{
		RefreshShieldDisplay();
		RefreshStorageDisplay();
	}

	private void Update()
	{
		if (Status == GrappleStatus.Base)
		{
			headTargetDestinationY = Vector2.zero;
			if (Input.GetButtonDown(startButtonName))
			{
				GoDown();
			}
		}
		else
		{
			// Movements Head Y
			currentMoveSpeedY = Status == GrappleStatus.Down ? -moveSpeedY : collectibles.IsEmpty() ? moveSpeedYEmpty : moveSpeedY;
			destinationY = head.transform.position.withY(head.transform.position.y + currentMoveSpeedY);
			headTargetDestinationY = Vector2.SmoothDamp(head.transform.position, head.transform.position.withY(destinationY.y), ref headCurrentSpeedY, headSmoothTimeY);
			headTargetDestinationY = headTargetDestinationY.withY(Mathf.Clamp(headTargetDestinationY.y, GameData.Depth + startPosition.y - 0.2f, startPosition.y));
			head.transform.position = head.transform.position.withY(headTargetDestinationY.y);

			if (head.transform.position.y <= GameData.Depth + startPosition.y)
			{
				ReturnToBase();
			}
			else if (head.transform.position.y == startPosition.y && hasEnded)
			{
				ArriveAtBase();
			}

			if (IsGoingBackToBaseEmpty() && !fastClearDone)
			{
				fastClearDone = true;
				level.ClearCollectibles();
			}

		}

		moveX = Input.GetAxisRaw("Horizontal");
		destinationX = baseGrapple.transform.position.withX(baseGrapple.transform.position.x + moveX);

		// Movements Base
		baseTargetDestination = Vector2.SmoothDamp(baseGrapple.transform.position, destinationX, ref baseCurrentSpeed, baseSmoothTimeX);
		baseTargetDestination = baseTargetDestination.withX(Mathf.Clamp(baseTargetDestination.x, -borderLimitX, borderLimitX));
		baseGrapple.transform.position = baseTargetDestination;

		// Movements Head X
		headTargetDestinationX = Vector2.SmoothDamp(head.transform.position, head.transform.position.withX(destinationX.x), ref headCurrentSpeedX, headSmoothTimeX);
		headTargetDestinationX = headTargetDestinationX.withX(Mathf.Clamp(headTargetDestinationX.x, -borderLimitX, borderLimitX));
		head.transform.position = head.transform.position.withX(headTargetDestinationX.x);

		// Line Connection
		line.SetPosition(0, baseLinePoint.transform.position);
		line.SetPosition(1, headLinePoint.transform.position);

		// HUD
		Hud.Instance.UpdateDepth(Mathf.Abs(Mathf.FloorToInt(head.transform.position.y - startPosition.y)));

		// Music
		grappleMoveUnitSound.volume = Mathf.Clamp01((float)Mathf.Abs(baseCurrentSpeed.x));
		grappleFallUnitSound.volume = Mathf.Clamp01((float)Mathf.Abs(headTargetDestinationY.y));
	}

	public void Collect(Collectible collectible)
	{
		if (collectibles.Contains(collectible))
			return;

		head.sprite = headClose;

		collectible.HasBeenCollected = true;
		collectibles.Add(collectible);

		collectible.transform.SetParent(collectPoint);
		collectible.Target = collectPoint;

		currentStoragePoints--;

		pickupSound.Play();

		if (collectibles.Count > GameData.Storage)
		{
			ReturnToBase();
		}

		Refresh();
	}

	public void Hit()
	{
		cinemachineImpulse.GenerateImpulse();

		if (currentBlockPoints > 0)
		{
			currentBlockPoints--;
		}
		else
		{
			if (!collectibles.IsEmpty())
			{
				Collectible remove = collectibles.Random();
				collectibles.Remove(remove);
				remove.gameObject.SetActive(false);
			}
		}

		for (int i = shields.Count - 1; i >= 0; i--)
		{
			if (shields[i].gameObject.activeSelf)
			{
				shields[i].GetComponent<Animator>()?.SetTrigger("Destruction");
				StartCoroutine(DeactivteAfterDuration(shields[i].gameObject, shieldDestructionDuration));
				break;
			}
		}
	}

	private void ArriveAtBase()
	{
		ComputeTreasure();
		hasEnded = false;
		Status = GrappleStatus.Base;
		level.RefreshLevel();
		Hud.Instance.ShowStartButtons();
		Init();
	}

	private void GoDown()
	{
		Status = GrappleStatus.Down;

		Init();
		Hud.Instance.HideStartButtons();

		// Depth limit setup
		depthLimit.transform.position = depthLimit.transform.position.withY(GameData.Depth + startPosition.y);
	}

	private void RefreshShieldDisplay()
	{
		shields.ForEach(x => x.gameObject.SetActive(false));
		for (int i = 0; i < currentBlockPoints; i++)
		{
			shields[i].gameObject.SetActive(true);
		}
	}

	private IEnumerator DeactivteAfterDuration(GameObject gameObject, float duration)
	{
		yield return new WaitForSeconds(duration);
		gameObject.SetActive(false);
	}

	private void RefreshStorageDisplay()
	{
		storages.ForEach(x => x.gameObject.SetActive(false));
		for (int i = 0; i < currentStoragePoints; i++)
		{
			storages[i].gameObject.SetActive(true);
		}
	}

	private void ComputeTreasure()
	{
		foreach (Collectible collectible in collectibles)
		{
			switch (collectible.Data.type)
			{
				case CollectibleType.Coins:
					GameData.Coins += Mathf.FloorToInt(collectible.Data.value * GameData.CoinsMulitplier);
					break;
				case CollectibleType.Star:
					GameData.Stars += collectible.Data.value;

					switch (collectible.DepthLevel)
					{
						case DepthLevels.Level1:
							GameData.StarLevel1 = true;
							break;
						case DepthLevels.Level2:
							GameData.StarLevel2 = true;
							break;
						case DepthLevels.Level3:
							GameData.StarLevel3 = true;
							break;
						case DepthLevels.Level4:
							GameData.StarLevel4 = true;
							break;
					}
					break;
				case CollectibleType.Final:
					GameData.Final = true;
					break;
			}
		}
		collectibles.Clear();
	}

	private void ReturnToBase()
	{
		hasEnded = true;
		Status = GrappleStatus.Up;
		level.MakeCollectiblesDangerous();

		if (GameData.BestDepth > head.transform.position.y)
		{
			GameData.BestDepth = head.transform.position.y;
		}
	}
}
