using UnityEngine;
using USubtitles;

namespace InSheepsClothing.Audio
{
	public enum AudioClipType
	{
		AudioClipType_None,
		AudioClipType_Voting,
		AudioClipType_Incorrect_Vote,
		AudioClipType_Correct_Vote
	}

	/*
	 * What does this class do?
	 * 
	 * This class inherits from the UAudioclip framework and adds extra InSheepsClothing-specific info.
	 * The UAudioClip is a class with a reference to an audioclip and a list of subtitles.
	*/
	[CreateAssetMenu(fileName = "CoopScoopAudioClip", menuName = "InSheepsClothing/InSheepsClothingAudioClip", order = 1)]
	public class InSheepsClothingAudioClip : UAudioClip
	{
		[SerializeField]
		private SoundCategory category = SoundCategory.SoundCategory_SoundEffect;
		public SoundCategory Category => category;

		[SerializeField]
		private AudioClipType audioClipType = AudioClipType.AudioClipType_None;
		public AudioClipType AudioClipType => audioClipType;
	}
}