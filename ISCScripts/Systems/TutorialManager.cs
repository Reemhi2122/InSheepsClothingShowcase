using InSheepsClothing.Events;
using System.Collections.Generic;
using UnityEngine;
using InSheepsClothing.Audio;

namespace InSheepsClothing.Tutorials
{
	public class TutorialManager : MonoBehaviour
    {
        public List<TutorialAction> TutorialItems = new List<TutorialAction>();

        private int currentTutorialItem = -1; // Start at -1 because the first Tutorialtem starts at Awake.

        public int CurrentTutorialItem => currentTutorialItem;

		private Timer 
			actionTimer = new Timer(), 
			reminderTimer = new Timer();

		private void OnEnable()
		{
			if (EventManager.Instance != null)
            {
				EventManager.Instance.NextTutorialItem += NextTutorialItem;
				EventManager.Instance.TutorialWrong += TutorialWrong;
				EventManager.Instance.StopTutorial += StopTutorial;
            }
		}

		private void OnDisable()
		{
			if (EventManager.Instance != null)
			{
				EventManager.Instance.NextTutorialItem -= NextTutorialItem;
				EventManager.Instance.TutorialWrong -= TutorialWrong;
				EventManager.Instance.StopTutorial -= StopTutorial;
			}
		}

        private void StopTutorial(object sender)
        {
			actionTimer.Enabled = false;
			reminderTimer.Enabled = false;
			currentTutorialItem = -1;

		}

        private void NextTutorialItem(object sender) => SetNextActionTimer(GameplaySettings.DefaultTutorialActionDelay);

		private void ImmediateActionCallback(object sender, AudioChannelDoneEventArgs e)
		{
			if (e.category != SoundCategory.SoundCategory_Speech)
				return;

			EventManager.Instance.AudioChannelDone -= ImmediateActionCallback;
			SetNextActionTimer(GameplaySettings.DefaultTutorialActionDelay);
		}

		/// <summary>
		/// Sets the next action timer.
		/// </summary>
		private void SetNextActionTimer(float time)
		{
			actionTimer.Enabled = true;
			actionTimer.SetTime(time);
			actionTimer.TickAction = NextAction;
		}

		private void ReminderCallback(object sender, AudioChannelDoneEventArgs e)
		{
			if (e.category != SoundCategory.SoundCategory_Speech)
				return;

			EventManager.Instance.AudioChannelDone -= ReminderCallback;
			SetReminderTimer();
		}

		/// <summary>
		/// Sets the next action timer.
		/// </summary>
		private void SetReminderTimer()
		{
			reminderTimer.Reset();
			reminderTimer.Enabled = true;
			reminderTimer.SetTime(GameplaySettings.TutorialReminderDelay);
			reminderTimer.TickAction = CallReminder;
		}

		private void Awake()
		{
			SetNextActionTimer(GameplaySettings.TutorialStartDelay);
		}

		/// <summary>
		/// Next tutorial item.
		/// </summary>
		/// <param name="seconds">Amount of seconds delay before going to the next action.</param>
		public void NextAction()
		{
			EventManager.Instance.AudioChannelDone -= ImmediateActionCallback;
			EventManager.Instance.AudioChannelDone -= ReminderCallback;

			actionTimer.Enabled = false;
			reminderTimer.Enabled = false;
			reminderTimer.Reset();

			if (TutorialItems.Count <= (currentTutorialItem + 1))
                return;

			currentTutorialItem++;
			TutorialActionStartedEventArgs e = new TutorialActionStartedEventArgs
			{
				action = TutorialItems[currentTutorialItem]
			};
			EventManager.Instance.OnTutorialActionStarted(this, e);

			PerformAction(currentTutorialItem);
		}

        /// <summary>
        /// Displays a tutorial action.
        /// </summary>
        private void PerformAction(int currentTutorialItem)
        {
			if (TutorialItems[currentTutorialItem].TutorialClips.Length > 0)
			{
				InSheepsClothingAudioClip clip =
					TutorialItems[currentTutorialItem]
						.TutorialClips[Random
							.Range(0,
							TutorialItems[currentTutorialItem]
								.TutorialClips
								.Length)];

				PlayAudioClipEventArgs e = new PlayAudioClipEventArgs
				{
					audioClip = clip,
					immediate = true
				};
				EventManager.Instance.OnPlayAudioClip(this, e);
			}

			// TODO: Notify tutorial sound.
			//{
			//	PlayAudioClipEventArgs e = new PlayAudioClipEventArgs
			//	{
			//		soundType = SoundEffectType.SoundEffect_TutorialNotify
			//	};
			//	EventManager.Instance.OnPlayAudioClip(this, e);
			//}

			// If it has reminders, that means the player must do an action to go to the next tutorial item.
			if (TutorialItems[currentTutorialItem].ReminderClips.Length > 0)
				EventManager.Instance.AudioChannelDone += ReminderCallback;
			// Else it means that this action is a tutorial - item -in-between, like a compliment, story or general instruction.
			else
				EventManager.Instance.AudioChannelDone += ImmediateActionCallback;
		}

        /// <summary>
        /// Calls the reminder every few seconds.
        /// </summary>
        /// <returns></returns>
        private void CallReminder()
		{
			reminderTimer.Reset();
			reminderTimer.Enabled = false;

			InSheepsClothingAudioClip clip = TutorialItems[currentTutorialItem].ReminderClips[
				Random.Range(0, TutorialItems[currentTutorialItem].ReminderClips.Length)];
			PlayAudioClipEventArgs e = new PlayAudioClipEventArgs
			{
				audioClip = clip,
				immediate = true
			};
			EventManager.Instance.OnPlayAudioClip(this, e);

			EventManager.Instance.AudioChannelDone += ReminderCallback;
		}

		private void Update()
		{
			reminderTimer.Update();
			actionTimer.Update();
		}

		public void TutorialWrong(object sender)
		{
			InSheepsClothingAudioClip clip = TutorialItems[currentTutorialItem].WrongClips[
				Random.Range(0, TutorialItems[currentTutorialItem].WrongClips.Length)];
			PlayAudioClipEventArgs e = new PlayAudioClipEventArgs
			{
				audioClip = clip,
				immediate = true
			};
			EventManager.Instance.OnPlayAudioClip(this, e);
		}
	}
}
