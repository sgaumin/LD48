using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
	public string characterName;
	public Color characterNameColor;
	[TextArea(2, 5)] public string content;
}