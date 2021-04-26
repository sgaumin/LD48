using UnityEngine;

public class GrappleHead : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Grapple parent;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Collectible c = collision.GetComponentInParent<Collectible>();
		if (c != null)
		{
			if (c.IsDangerous)
			{
				parent.Hit();
			}
			else
			{
				parent.Collect(c);
			}

			c.DoInteraction();
		}
	}
}
