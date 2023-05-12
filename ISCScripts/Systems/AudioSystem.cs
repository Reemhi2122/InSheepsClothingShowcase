using InSheepsClothing.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InSheepsClothing.Audio
{
	/*
	 * What does this class do?
	 * 
	 * This class keeps a reference to an audiosource and clip and can be instantiated multiple times so that
	 * there is a collection of channels.
	*/
	[System.Serializable]
	public class Channel
	{
		[SerializeField]
		private InSheepsClothingAudioClip audioClip = null;
		public InSheepsClothingAudioClip AudioClip => audioClip;

		[SerializeField]
		private AudioSource audioSource = null;
		public AudioSource AudioSource => audioSource;

		private int currentSubtitleIndex = 0;
		public int CurrentSubtitleIndex => currentSubtitleIndex;

		/// <summary>
		/// Sets the audio clip of the channel.
		/// </summary>
		/// <param name="clip"></param>
		public void SetAudioClip(InSheepsClothingAudioClip clip)
		{
			audioClip = clip;
			if (audioClip)
				audioSource.clip = audioClip.Clip;
		}

		/// <summary>
		/// Sets the audio source.
		/// </summary>
		/// <param name="audioSource"></param>
		public void SetAudioSource(AudioSource audioSource) => this.audioSource = audioSource;

		/// <summary>
		/// Sets the current subtitle index to display.
		/// </summary>
		/// <param name="index"></param>
		public void SetCurrentSubtitleIndex(int index) => currentSubtitleIndex = index;

		/// <summary>
		/// Returns the audio source.
		/// </summary>
		/// <returns></returns>
		public AudioSource GetAudioSource() => audioSource;
	}

	/*
	 * What does this class do?
	 * 
	 * This class has audio channels that can be played, stopped and paused. It handles CoopScoopAudioClips and is for SFX, Speech and Music.
	*/
	public class AudioSystem : MonoBehaviour
	{
		private List<Channel> channels = new List<Channel>();
		public List<Channel> Channels => channels;

		private string fullText = string.Empty;

		[SerializeField]
		private List<InSheepsClothingAudioClip> clips = new List<InSheepsClothingAudioClip>();
		public List<InSheepsClothingAudioClip> Clips => clips;

		private void Awake()
		{
			for (int i = 0; i < System.Enum.GetNames(typeof(SoundCategory)).Length; i++)
			{
				Channel channel = new Channel();
				channel.SetAudioSource(gameObject.AddComponent<AudioSource>());
				channels.Add(channel);
			}
		}

		private void OnEnable()
		{
			if (EventManager.Instance != null)
            {
				EventManager.Instance.PlayAudioClip += PlayAudioClip;
				EventManager.Instance.PlayAudioClipByType += PlayAudioClipByType;
			}
		}

		private void OnDisable()
		{
			if (EventManager.Instance != null)
            {
				EventManager.Instance.PlayAudioClip += PlayAudioClip;
				EventManager.Instance.PlayAudioClipByType += PlayAudioClipByType;
			}
		}

        /// <summary>
        /// Plays an audio clip.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayAudioClip(object sender, PlayAudioClipEventArgs e) => PlayAudioClip(e.audioClip);

		/// <summary>
		/// Plays an audio clip.
		/// </summary>
		/// <param name="clip"></param>
		private void PlayAudioClip(InSheepsClothingAudioClip clip)
		{
			if (!clip)
				return;

			fullText = string.Empty;

			channels[(int)clip.Category].SetAudioClip(clip);
			channels[(int)clip.Category].SetCurrentSubtitleIndex(0);
			channels[(int)clip.Category].GetAudioSource().Play();

			SubtitlesUpdatedEventArgs evt = new SubtitlesUpdatedEventArgs
			{
				text = fullText
			};
			EventManager.Instance.OnSubtitlesUpdated(this, evt);
		}

		/// <summary>
		/// Checks a channel for subtitles.
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="category"></param>
		private void CheckForSubs(Channel channel, SoundCategory category)
		{
			if (!channel.AudioClip)
				return;

			// If the audio clip is done playing.
			if (channel.AudioSource.timeSamples >= channel.AudioClip.Clip.samples)
			{
				AudioChannelDoneEventArgs evt = new AudioChannelDoneEventArgs
				{
					audioClip = channel.AudioClip,
					category = category
				};
				EventManager.Instance.OnAudioChannelDone(this, evt);
				channel.SetAudioClip(null);
				channel.SetCurrentSubtitleIndex(0);
				return;
			}

			// If the current index is equal or more than the number of subtitles, do not update.
			if (channel.CurrentSubtitleIndex >= channel.AudioClip.Dialogue.Count)
				return;

			// If the position of the audio clip is higher than the subtitle position, update.
			if (channel.AudioSource.timeSamples >= channel.AudioClip.Dialogue[channel.CurrentSubtitleIndex].SamplePosition)
			{
				if (channel.AudioClip.Dialogue[channel.CurrentSubtitleIndex].Clear)
					fullText = string.Empty;
				fullText += channel.AudioClip.Dialogue[channel.CurrentSubtitleIndex].Lines[0].Text;
				SubtitlesUpdatedEventArgs e = new SubtitlesUpdatedEventArgs
				{
					text = fullText
				};
				EventManager.Instance.OnSubtitlesUpdated(this, e);
				channel.SetCurrentSubtitleIndex(channel.CurrentSubtitleIndex + 1);
				CheckForSubs(channel, category);
			}
		}

		private void PlayAudioClipByType(object sender, PlayAudioClipByTypeEventArgs e)
		{
			List<InSheepsClothingAudioClip> eClips = new List<InSheepsClothingAudioClip>();
            for (int i = 0; i < clips.Count; i++)
            {
				if (clips[i].AudioClipType == e.audioClipType)
					eClips.Add(clips[i]);
            }
			InSheepsClothingAudioClip clip = eClips[UnityEngine.Random.Range(0, eClips.Count)];
			if (!clip)
				return;

			fullText = string.Empty;

			channels[(int)clip.Category].SetAudioClip(clip);
			channels[(int)clip.Category].SetCurrentSubtitleIndex(0);
			channels[(int)clip.Category].GetAudioSource().Play();

			SubtitlesUpdatedEventArgs evt = new SubtitlesUpdatedEventArgs
			{
				text = fullText
			};
			EventManager.Instance.OnSubtitlesUpdated(this, evt);
		}

		private void Update()
		{
			for (int i = 0; i < channels.Count; i++)
				CheckForSubs(channels[i], (SoundCategory)i);
		}
	}
}