using InSheepsClothing.Audio;
using InSheepsClothing.Interaction;
using InSheepsClothing.Tutorials;
using System;

namespace InSheepsClothing.Events
{
	public class GameOverEventArgs : EventArgs
	{
		public string reason = "";
	}

	public class ScoreUpdatedEventArgs : EventArgs
	{
		public int score = 0;
	}
	public class TutorialActionStartedEventArgs : EventArgs
	{
		public TutorialAction action;
	}

	//public class PlaySpeechClipEventArgs : EventArgs
	//{
	//	public CoopScoopSpeechClip audioClip;
	//	public bool immediate = true;
	//}

	//public class PlaySpeechClipFromCategoryEventArgs : EventArgs
	//{
	//	public VOCategory category;
	//	public bool byChance = false;
	//}

	//public class SpeechClipPlayedEventArgs : EventArgs
	//{
	//	public CoopScoopSpeechClip audioClip;
	//}

	//public class PlayAudioClipEventArgs : EventArgs
	//{
	//	public SoundType soundType;
	//}

	//public class AudioClipPlayedEventArgs : EventArgs
	//{
	//	public CoopScoopAudioClip audioClip;
	//}

	public class SubtitlesUpdatedEventArgs : EventArgs
	{
		public string text = null;
	}

	public class PlayerVotedEventArgs : EventArgs
	{
		public Vote vote = Vote.None;
	}

	public class AudioChannelDoneEventArgs : EventArgs
	{
		public InSheepsClothingAudioClip audioClip;
		public SoundCategory category;
	}

	public class PlayAudioClipEventArgs : EventArgs
	{
		public InSheepsClothingAudioClip audioClip;
		public bool immediate = true;
	}

	public class PlayAudioClipByTypeEventArgs : EventArgs
	{
		public AudioClipType audioClipType;
		public bool immediate = true;
	}

	public class PictureInSuitcaseEventArgs : EventArgs
	{
		public TakenPicture picture;
	}

	public sealed class EventManager : CSingleton<EventManager>
	{
		/* 
		* Name: OnGameOver.
		* Description: Called when the player gets a game over.
			* reason: string, The reason why the player failed.
		*/
		public delegate void GameOverHandler(object sender, GameOverEventArgs e);
		public event GameOverHandler GameOver = delegate { };

		public void OnGameOver(object sender, GameOverEventArgs e)
		{
			GameOver.Invoke(sender, e);
		}

		/* 
		* Name: OnScoreUpdate.
		* Description: Called when the score updates.
			* score: int, the score.
		*/
		public delegate void ScoreUpdatedHandler(object sender, ScoreUpdatedEventArgs e);
		public event ScoreUpdatedHandler ScoreUpdated = delegate { };

		public void OnScoreUpdated(object sender, ScoreUpdatedEventArgs e)
		{
			ScoreUpdated.Invoke(sender, e);
		}

		/* 
		* Name: Vote.
		* Description: When the GameManager wants the voting system to start.
		*/
		public delegate void VoteHandler(object sender);
		public event VoteHandler Vote = delegate { };

		public void OnVote(object sender)
		{
			Vote.Invoke(sender);
		}

		/* 
		* Name: peek.
		* Description: When the GameManager wants the peeksheep to peek.
		*/
		public delegate void PeekHandler(object sender);
		public event PeekHandler Peek = delegate { };

		public void OnPeek(object sender)
		{
			Peek.Invoke(sender);
		}

		/* 
		* Name: PictureTaken.
		* Description: When the player makes a picture with the camera.
		*/
		public delegate void PictureTakenHandler(object sender);
		public event PictureTakenHandler PictureTaken = delegate { };

		public void OnPictureTaken(object sender)
		{
			PictureTaken.Invoke(sender);
		}

		/* 
		* Name: PictureTakenTarget.
		* Description: When the player makes a picture with the camera of a target.
		*/
		public delegate void PictureTakenTargetHandler(object sender);
		public event PictureTakenTargetHandler PictureTakenTarget = delegate { };

		public void OnPictureTakenTarget(object sender)
		{
			PictureTakenTarget.Invoke(sender);
		}

		/* 
		* Name: PictureTakenTargetless.
		* Description: When the player makes a picture with the camera without a target.
		*/
		public delegate void PictureTakenTargetlessHandler(object sender);
		public event PictureTakenTargetlessHandler PictureTakenTargetless = delegate { };

		public void OnPictureTakenTargetless(object sender)
		{
			PictureTakenTargetless.Invoke(sender);
		}

		/* 
		* Name: OnPlayerVoted.
		* Description: When the player picks up a sign.
		*/
		public delegate void PlayerVotedHandler(object sender, PlayerVotedEventArgs e);
		public event PlayerVotedHandler PlayerVoted = delegate { };

		public void OnPlayerVoted(object sender, PlayerVotedEventArgs e)
		{
			PlayerVoted.Invoke(sender, e);
		}

		/* 
		* Name: NextSlide.
		* Description: When the GameManager wants to start the presentation's next slide.
		*/
		public delegate void NextSlideHandler(object sender);
		public event NextSlideHandler NextSlide = delegate { };

		public void OnNextSlide(object sender)
		{
			NextSlide.Invoke(sender);
		}

		/* 
		* Name: NextSlideWarning.
		* Description: When the player is forgetting to go to the next slide.
		*/
		public delegate void NextSlideWarningHandler(object sender);
		public event NextSlideWarningHandler NextSlideWarning = delegate { };

		public void OnNextSlideWarning(object sender)
		{
			NextSlideWarning.Invoke(sender);
		}

		/* 
		* Name: PlayerNextSlide.
		* Description: When the player wants to go to the next slide of the presentation.
		*/
		public delegate void PlayerNextSlideHandler(object sender);
		public event PlayerNextSlideHandler PlayerNextSlide = delegate { };

		public void OnPlayerNextSlide(object sender)
		{
			PlayerNextSlide.Invoke(sender);
		}

		/* 
		* Name: OnRoundStart.
		* Description: When the round starts.
		*/
		public delegate void RoundStartHandler(object sender);
		public event RoundStartHandler RoundStart = delegate { };

		public void OnRoundStart(object sender)
		{
			RoundStart.Invoke(sender);
		}

		/* 
		* Name: OnNextTutorialItem.
		* Description: Calls the tutorial manager to move on to the next item.
		*/
		public delegate void NextTutorialItemHandler(object sender);
		public event NextTutorialItemHandler NextTutorialItem = delegate { };

		public void OnNextTutorialItem(object sender)
		{
			NextTutorialItem.Invoke(sender);
		}

		/* 
		* Name: OnTutorialWrong.
		* Description: Calls the audioclip from the current tutorial item when doing a wrong action.
		*/
		public delegate void TutorialWrongHandler(object sender);
		public event TutorialWrongHandler TutorialWrong = delegate { };

		public void OnTutorialWrong(object sender)
		{
			TutorialWrong.Invoke(sender);
		}

		/* 
		* Name: OnStopTutorial.
		* Description: Stops the tutorial.
		*/
		public delegate void StopTutorialHandler(object sender);
		public event StopTutorialHandler StopTutorial = delegate { };

		public void OnStopTutorial(object sender)
		{
			StopTutorial.Invoke(sender);
		}

		/* 
		* Name: OnTutorialActionStarted.
		* Description: Called when a tutorial action starts.
		*/
		public delegate void TutorialActionStartedHandler(object sender, TutorialActionStartedEventArgs e);
		public event TutorialActionStartedHandler TutorialActionStarted = delegate { };

		public void OnTutorialActionStarted(object sender, TutorialActionStartedEventArgs e)
		{
			TutorialActionStarted.Invoke(sender, e);
		}

		/* 
		* Name: OnSubtitlesUpdated.
		* Description: When the subtitles get updated.
		*/
		public delegate void SubtitlesUpdatedHandler(object sender, SubtitlesUpdatedEventArgs e);
		public event SubtitlesUpdatedHandler SubtitlesUpdated = delegate { };

		public void OnSubtitlesUpdated(object sender, SubtitlesUpdatedEventArgs e)
		{
			if (sender.GetType() != typeof(AudioSystem))
				return;

			SubtitlesUpdated.Invoke(sender, e);
		}

		/* 
		* Name: OnAudioChannelDone.
		* Description: When audio channel is done playing.
		*/
		public delegate void AudioChannelDoneHandler(object sender, AudioChannelDoneEventArgs e);
		public event AudioChannelDoneHandler AudioChannelDone = delegate { };

		public void OnAudioChannelDone(object sender, AudioChannelDoneEventArgs e)
		{
			if (sender.GetType() != typeof(AudioSystem))
				return;

			AudioChannelDone.Invoke(sender, e);
		}

		/* 
		* Name: OnPlayAudioClip.
		* Description: Called when a wanting to call an audio clip.
		*/
		public delegate void PlayAudioClipHandler(object sender, PlayAudioClipEventArgs e);
		public event PlayAudioClipHandler PlayAudioClip = delegate { };

		public void OnPlayAudioClip(object sender, PlayAudioClipEventArgs e)
		{
			PlayAudioClip.Invoke(sender, e);
		}

		/* 
		* Name: OnPlayAudioClipByType.
		* Description: Called when a wanting to call an audio clip by type.
		*/
		public delegate void PlayAudioClipByTypeHandler(object sender, PlayAudioClipByTypeEventArgs e);
		public event PlayAudioClipByTypeHandler PlayAudioClipByType = delegate { };

		public void OnPlayAudioClipByType(object sender, PlayAudioClipByTypeEventArgs e)
		{
			PlayAudioClipByType.Invoke(sender, e);
		}

		/* 
		* Name: OnPictureInSuitcase.
		* Description: Called when a picture gets put in the suitcase.
		*/
		public delegate void PictureInSuitcaseHandler(object sender, PictureInSuitcaseEventArgs e);
		public event PictureInSuitcaseHandler PictureInSuitcase = delegate { };

		public void OnPictureInSuitcase(Suitcase suitcase, PictureInSuitcaseEventArgs e)
        {
			PictureInSuitcase.Invoke(suitcase, e);
        }

        internal void OnPictureInSuitcase(Suitcase suitcase, object e)
        {
            throw new NotImplementedException();
        }
    }
}