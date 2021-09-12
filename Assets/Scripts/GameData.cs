using UnityEngine;

public static class GameData
{
	private const int MIN_DEPTH = -25;

	private const string FINAL = "final";
	private const string COIN = "coin";
	private const string COIN_MULTIPLIER = "coin_multiplier";
	private const string STAR = "star";
	private const string STAR_LEVEL1 = "star_level1";
	private const string STAR_LEVEL2 = "star_level2";
	private const string STAR_LEVEL3 = "star_level3";
	private const string STAR_LEVEL4 = "star_level4";
	private const string BLOCK = "block";
	private const string SHOP_BLOCK = "shop_block";
	private const string STORAGE = "storage";
	private const string SHOP_STORAGE = "shop_storage";
	private const string DEPTH = "depth";
	private const string BEST_DEPTH = "best_depth";
	private const string SHOP_DEPTH = "shop_depth";
	private const string SHOP_SPECIAL = "shop_special";

	public static bool Final
	{
		get
		{
			return PlayerPrefs.HasKey(FINAL) ? bool.Parse(PlayerPrefs.GetString(FINAL)) : false;
		}
		set
		{
			PlayerPrefs.SetString(FINAL, value.ToString());
			PlayerPrefs.Save();
		}
	}

	public static int Coins
	{
		get
		{
			return PlayerPrefs.HasKey(COIN) ? PlayerPrefs.GetInt(COIN) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(COIN, value);
			PlayerPrefs.Save();
			Hud.Instance.UpdateCoin();
		}
	}

	public static float CoinsMulitplier
	{
		get
		{
			return PlayerPrefs.HasKey(COIN_MULTIPLIER) ? PlayerPrefs.GetFloat(COIN_MULTIPLIER) : 1f;
		}
		set
		{
			PlayerPrefs.SetFloat(COIN_MULTIPLIER, value);
			PlayerPrefs.Save();
		}
	}

	public static int Stars
	{
		get
		{
			return PlayerPrefs.HasKey(STAR) ? PlayerPrefs.GetInt(STAR) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(STAR, value);
			PlayerPrefs.Save();
			Hud.Instance.UpdateStars();
		}
	}

	public static bool StarLevel1
	{
		get
		{
			return PlayerPrefs.HasKey(STAR_LEVEL1) ? bool.Parse(PlayerPrefs.GetString(STAR_LEVEL1)) : false;
		}
		set
		{
			PlayerPrefs.SetString(STAR_LEVEL1, value.ToString());
			PlayerPrefs.Save();
		}
	}

	public static bool StarLevel2
	{
		get
		{
			return PlayerPrefs.HasKey(STAR_LEVEL2) ? bool.Parse(PlayerPrefs.GetString(STAR_LEVEL2)) : false;
		}
		set
		{
			PlayerPrefs.SetString(STAR_LEVEL2, value.ToString());
			PlayerPrefs.Save();
		}
	}

	public static bool StarLevel3
	{
		get
		{
			return PlayerPrefs.HasKey(STAR_LEVEL3) ? bool.Parse(PlayerPrefs.GetString(STAR_LEVEL3)) : false;
		}
		set
		{
			PlayerPrefs.SetString(STAR_LEVEL3, value.ToString());
			PlayerPrefs.Save();
		}
	}

	public static bool StarLevel4
	{
		get
		{
			return PlayerPrefs.HasKey(STAR_LEVEL4) ? bool.Parse(PlayerPrefs.GetString(STAR_LEVEL4)) : false;
		}
		set
		{
			PlayerPrefs.SetString(STAR_LEVEL4, value.ToString());
			PlayerPrefs.Save();
		}
	}

	public static int Block
	{
		get
		{
			return PlayerPrefs.HasKey(BLOCK) ? PlayerPrefs.GetInt(BLOCK) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(BLOCK, value);
			PlayerPrefs.Save();
		}
	}

	public static int ShopBlock
	{
		get
		{
			return PlayerPrefs.HasKey(SHOP_BLOCK) ? PlayerPrefs.GetInt(SHOP_BLOCK) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(SHOP_BLOCK, value);
			PlayerPrefs.Save();
		}
	}

	public static int Storage
	{
		get
		{
			return PlayerPrefs.HasKey(STORAGE) ? PlayerPrefs.GetInt(STORAGE) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(STORAGE, value);
			PlayerPrefs.Save();
		}
	}

	public static int ShopStorage
	{
		get
		{
			return PlayerPrefs.HasKey(SHOP_STORAGE) ? PlayerPrefs.GetInt(SHOP_STORAGE) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(SHOP_STORAGE, value);
			PlayerPrefs.Save();
		}
	}

	public static int Depth
	{
		get
		{
			return PlayerPrefs.HasKey(DEPTH) ? PlayerPrefs.GetInt(DEPTH) : MIN_DEPTH;
		}
		set
		{
			PlayerPrefs.SetInt(DEPTH, value);
			PlayerPrefs.Save();
		}
	}

	public static float BestDepth
	{
		get
		{
			return PlayerPrefs.HasKey(BEST_DEPTH) ? PlayerPrefs.GetFloat(BEST_DEPTH) : 0f;
		}
		set
		{
			PlayerPrefs.SetFloat(BEST_DEPTH, value);
			PlayerPrefs.Save();
		}
	}

	public static int ShopDepth
	{
		get
		{
			return PlayerPrefs.HasKey(SHOP_DEPTH) ? PlayerPrefs.GetInt(SHOP_DEPTH) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(SHOP_DEPTH, value);
			PlayerPrefs.Save();
		}
	}

	public static int ShopGoldMultiplier
	{
		get
		{
			return PlayerPrefs.HasKey(SHOP_SPECIAL) ? PlayerPrefs.GetInt(SHOP_SPECIAL) : 0;
		}
		set
		{
			PlayerPrefs.SetInt(SHOP_SPECIAL, value);
			PlayerPrefs.Save();
		}
	}
}