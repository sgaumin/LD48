using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using Tools.Utils;
using UnityEngine;
using UnityEngine.Audio;

public class Level : GameSystem
{
	public const string GAME_MUSIC_VOLUME = "musicVolume";
	public const string GAME_GRAPPLE_VOLUME = "grappleVolume";
	public const string GAME_UI_VOLUME = "uiVolume";

	public delegate void GameEventHandler();
	public event GameEventHandler OnStart;
	public event GameEventHandler OnGameOver;
	public event GameEventHandler OnPause;

	[Header("Level Parameters")]
	[SerializeField] private LevelData data;

	[Header("Audio")]
	[SerializeField] private AudioMixer mixer;
	[SerializeField] private AudioExpress gameMusic;
	[SerializeField] private AudioExpress click;

	[Header("References")]
	[SerializeField] private Transform levelHolder;
	[SerializeField] private FadScreen fader;
	[SerializeField] private SpriteRenderer bestLine;
	[SerializeField] private GameObject cameraObject;


	private GameStates gameState;
	private Coroutine loadingLevel;
	private List<Collectible> collectibles = new List<Collectible>();
	private GameObject currentlevel;
	private float gameMusicVolume;
	private float gameGrappleVolume;
	private float gameUiVolume;

	public GameStates GameState
	{
		get => gameState;
		set
		{
			gameState = value;

			switch (value)
			{
				case GameStates.Play:
					OnStart?.Invoke();
					break;

				case GameStates.GameOver:
					OnGameOver?.Invoke();
					break;

				case GameStates.Pause:
					OnPause?.Invoke();
					break;
			}
		}
	}
	public float GameMusicVolume => Mathf.InverseLerp(-60f, 0f, gameMusicVolume);
	public float GameGrappleVolume => Mathf.InverseLerp(-30f, 0f, gameGrappleVolume);
	public float GameUiVolume => Mathf.InverseLerp(-25f, 0f, gameUiVolume);

	#region Unity Callbacks
	protected override void Awake()
	{
		base.Awake();
	}

	protected void Start()
	{
		GameState = GameStates.Play;
		fader.FadIn();

		mixer.GetFloat(GAME_MUSIC_VOLUME, out gameMusicVolume);
		mixer.GetFloat(GAME_GRAPPLE_VOLUME, out gameGrappleVolume);
		mixer.GetFloat(GAME_UI_VOLUME, out gameUiVolume);

		gameMusic.Play();

		RefreshLevel();
	}

	protected override void Update()
	{
		base.Update();

		cameraObject.transform.position = cameraObject.transform.position.withX(0f);
	}
	#endregion

	public void UpdateGameMusicVolume(float percentage)
	{
		gameMusicVolume = Mathf.Lerp(-60f, 0f, percentage);
		mixer.SetFloat(GAME_MUSIC_VOLUME, gameMusicVolume);
	}

	public void UpdateGameGrappleVolume(float percentage)
	{
		gameGrappleVolume = Mathf.Lerp(-30f, 0f, percentage);
		mixer.SetFloat(GAME_GRAPPLE_VOLUME, gameGrappleVolume);
	}

	public void UpdateGameUiVolume(float percentage)
	{
		gameUiVolume = Mathf.Lerp(-25f, 0f, percentage);
		mixer.SetFloat(GAME_UI_VOLUME, gameUiVolume);
	}

	public void ClickSound()
	{
		click.Play();
	}

	public void ClearCollectibles()
	{
		collectibles.ForEach(x => Destroy(x.gameObject));
		collectibles.Clear();
	}

	public void RefreshLevel()
	{
		ClearCollectibles();

		List<Collectible> collectibleToAssign = new List<Collectible>();
		foreach (CollectibleStats stat in data.collectibleStats)
		{
			for (int i = 0; i < stat.count; i++)
			{
				collectibleToAssign.Add(stat.collectiblePrefab);
			}
		}

		if (currentlevel != null)
		{
			Destroy(currentlevel);
		}

		currentlevel = Instantiate(data.levelPrefab, levelHolder);
		List<CollectibleSpawner> spawners = new List<CollectibleSpawner>();
		foreach (Transform spawn in currentlevel.transform)
		{
			if (spawn == currentlevel.transform)
				continue;

			spawners.Add(spawn.GetComponent<CollectibleSpawner>());
		}

		// Spawn Final
		if (data.finalPrefab != null)
		{
			List<CollectibleSpawner> finalSpawns = spawners.Where(x => x.Type == data.finalPrefab.Data.type).ToList();
			if (!finalSpawns.IsEmpty())
			{
				foreach (CollectibleSpawner spawner in finalSpawns)
				{
					Collectible currentCollectible = Instantiate(data.finalPrefab, spawner.transform);
					collectibles.Add(currentCollectible);
					spawners.Remove(spawner);
				}
			}
		}

		// Spawn Star
		if (data.starPrefab != null)
		{
			List<CollectibleSpawner> starsSpawns = spawners.Where(x => x.Type == data.starPrefab.Data.type).ToList();
			if (!starsSpawns.IsEmpty())
			{
				foreach (CollectibleSpawner spawner in starsSpawns)
				{
					bool hasBeenCollected = false;
					switch (spawner.Level)
					{
						case DepthLevels.Level1:
							hasBeenCollected = GameData.StarLevel1;
							break;
						case DepthLevels.Level2:
							hasBeenCollected = GameData.StarLevel2;
							break;
						case DepthLevels.Level3:
							hasBeenCollected = GameData.StarLevel3;
							break;
						case DepthLevels.Level4:
							hasBeenCollected = GameData.StarLevel4;
							break;
					}

					if (!hasBeenCollected)
					{
						Collectible currentCollectible = Instantiate(data.starPrefab, spawner.transform);
						currentCollectible.DepthLevel = spawner.Level;
						collectibles.Add(currentCollectible);
						spawners.Remove(spawner);
					}
				}
			}
		}

		// Spawn Collectibles
		foreach (Collectible collectible in collectibleToAssign)
		{
			if (spawners.Count == 0)
				continue;

			List<CollectibleSpawner> tempList = spawners.Where(x => x.Type == collectible.Data.type && collectible.Data.area.Contains(Mathf.FloorToInt(x.transform.position.y))).ToList();
			if (tempList.IsEmpty())
				continue;

			CollectibleSpawner currentSpawn = tempList.Random();
			if (currentSpawn != null)
			{
				Collectible currentCollectible = Instantiate(collectible, currentSpawn.transform);
				collectibles.Add(currentCollectible);
				spawners.Remove(currentSpawn);
			}
		}

		// Best line setup
		if (GameData.BestDepth != 0)
		{
			bestLine.gameObject.SetActive(true);
			bestLine.transform.position = bestLine.transform.position.withY(GameData.BestDepth);
		}
		else
		{
			bestLine.gameObject.SetActive(false);
		}
	}

	public void DeleteAllSave()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();

		Hud.Instance.Refresh();

		RefreshLevel();
	}

	public void MakeCollectiblesDangerous()
	{
		collectibles.Where(x => x.Data.type == CollectibleType.Coins && !x.HasBeenCollected).ForEach(x => x.IsDangerous = true);
	}

	#region Level Loading Methods
	public void ReloadLevel()
	{
		if (loadingLevel == null)
		{
			loadingLevel = StartCoroutine(LoadLevelCore(

			content: () =>
			{
				LevelLoader.ReloadLevel();
			}));
		}
	}

	public void LoadNextLevel()
	{
		if (loadingLevel == null)
		{
			loadingLevel = StartCoroutine(LoadLevelCore(

			content: () =>
			{
				LevelLoader.LoadNextLevel();
			}));
		}
	}

	public void LoadMenu()
	{
		if (loadingLevel == null)
		{
			loadingLevel = StartCoroutine(LoadLevelCore(

			content: () =>
			{
				LevelLoader.LoadLevelByName(Constants.MENU_SCENE);
			}));
		}
	}

	public void LoadSceneByName(string sceneName)
	{
		if (loadingLevel == null)
		{
			loadingLevel = StartCoroutine(LoadLevelCore(

			content: () =>
			{
				LevelLoader.LoadLevelByName(sceneName);
			}));
		}
	}

	public void QuitGame()
	{
		if (loadingLevel == null)
		{
			loadingLevel = StartCoroutine(LoadLevelCore(

			content: () =>
			{
				LevelLoader.QuitGame();
			}));
		}
	}

	private IEnumerator LoadLevelCore(Action content = null)
	{
		Time.timeScale = 1f;
		yield return fader.FadOutCore();
		content?.Invoke();
	}
	#endregion
}