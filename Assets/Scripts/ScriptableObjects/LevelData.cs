using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CollectibleStats
{
	public Collectible collectiblePrefab;
	public int count;
}

[CreateAssetMenu(fileName = "Data", menuName = "LevelData", order = 0)]
public class LevelData : ScriptableObject
{
	public GameObject levelPrefab;
	public Collectible finalPrefab;
	public Collectible starPrefab;
	public List<CollectibleStats> collectibleStats = new List<CollectibleStats>();
}