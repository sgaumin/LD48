using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
	[SerializeField] private CollectibleType type;
	[SerializeField] private DepthLevels level;

	public CollectibleType Type => type;
	public DepthLevels Level => level;
}