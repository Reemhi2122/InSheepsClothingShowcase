using InSheepsClothing.Events;
using TMPro;
using UnityEngine;

namespace InSheepsClothing.Audio
{
	/*
	 * What does this class do?
	 * 
	 * This class listens to the subtitle updated event and displays the text.
	*/
	public class Subtitle : MonoBehaviour
	{
		private TextMeshProUGUI _text = null;

		private void Awake() => _text = GetComponent<TextMeshProUGUI>();

		private void OnEnable() 
		{
			if (EventManager.Instance != null)
				EventManager.Instance.SubtitlesUpdated += SubtitlesUpdated;
		} 

		private void OnDisable() 
		{
			if (EventManager.Instance != null)
				EventManager.Instance.SubtitlesUpdated -= SubtitlesUpdated;
		}

		private void SubtitlesUpdated(object sender, SubtitlesUpdatedEventArgs e) => _text.text = e.text;
	}
}