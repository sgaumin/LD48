using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Level level;
	[SerializeField] private Slider musicVolumeSlider;
	[SerializeField] private Slider grappleVolumeSlider;
	[SerializeField] private Slider uiVolumeSlider;

	private void OnEnable()
	{
		musicVolumeSlider.value = level.GameMusicVolume;
		grappleVolumeSlider.value = level.GameGrappleVolume;
		uiVolumeSlider.value = level.GameUiVolume;
	}
}