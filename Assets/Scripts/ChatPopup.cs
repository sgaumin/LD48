using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools.Utils;
using UnityEngine;

public class ChatPopup : MonoBehaviour
{
	[SerializeField] private List<DialogueData> dialogues = new List<DialogueData>();

	[Header("Animations")]
	[SerializeField] private float fadDuration = 0.15f;
	[SerializeField] private float durationDisplayLetter = 0.02f;
	[SerializeField] private float dialogueDisplayDuration = 1.5f;
	[SerializeField, FloatRangeSlider(0f, 10f)] private FloatRange timeBetweenDialogue = new FloatRange(1f, 3f);

	[Header("Audio")]
	[SerializeField] private AudioExpress letterDisplaySound;

	[Header("References")]
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private TextMeshProUGUI characterName;
	[SerializeField] private TextMeshProUGUI content;

	private DialogueData currentDialogue;

	protected void Start()
	{
		StartCoroutine(Run());
		canvasGroup.alpha = 0f;
	}

	private IEnumerator Run()
	{
		while (true)
		{
			yield return ShowContent();
			yield return new WaitForSeconds(timeBetweenDialogue.RandomValue);
		}
	}

	private IEnumerator ShowContent()
	{
		canvasGroup.DOFade(1f, fadDuration);
		currentDialogue = dialogues.Random();
		characterName.text = currentDialogue.characterName.ToUpper();
		characterName.color = currentDialogue.characterNameColor;

		content.text = "";
		foreach (char letter in currentDialogue.content.ToUpper())
		{
			yield return new WaitForSeconds(durationDisplayLetter);
			content.text += letter;
			letterDisplaySound.Play();
		}

		yield return new WaitForSeconds(dialogueDisplayDuration);
		canvasGroup.DOFade(0f, fadDuration);
	}
}