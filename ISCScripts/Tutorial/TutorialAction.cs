using InSheepsClothing.Audio;
using UnityEngine;

namespace InSheepsClothing.Tutorials
{
	[System.Serializable]
	public class TutorialAction
	{
		public string 
			TutorialName, 
			TutorialDescription,
			TutorialCodename;

		public InSheepsClothingAudioClip[] 
			TutorialClips,
			ReminderClips,
			WrongClips;
	}
}