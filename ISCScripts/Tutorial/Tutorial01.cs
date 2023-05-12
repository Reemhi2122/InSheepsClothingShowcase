using InSheepsClothing.Events;
using System;
using UnityEngine;

namespace InSheepsClothing.Tutorials
{
	/*
	 * What does this class do?
	 * 
	 * This class describes the tutorial-specific code.
	*/
	public class Tutorial : MonoBehaviour
	{
		protected void OnEnable()
		{
			if (EventManager.Instance != null)
				EventManager.Instance.TutorialActionStarted += TutorialActionStarted;
		}

		protected void OnDisable()
		{
			if (EventManager.Instance != null)
				EventManager.Instance.TutorialActionStarted -= TutorialActionStarted;
		}

		protected virtual void TutorialActionStarted(object sender, TutorialActionStartedEventArgs e)
		{ }
	}

	public class Tutorial01 : Tutorial
	{
		[SerializeField]
		private Presentation presentation = null;

		private void SetOutline(MonoBehaviour[] gameObjects, bool enabled)
		{
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (enabled)
                    gameObjects[i].GetComponent<OutlinedObject>().Select();
                else
                    gameObjects[i].GetComponent<OutlinedObject>().Deselect();
            }
        }

		protected override void TutorialActionStarted(object sender, TutorialActionStartedEventArgs e)
		{
			if (e.action == null)
				return;

			switch (e.action.TutorialCodename)
			{
				case "tutorial_00_start":
				{
					void playerVoted(object sender, PlayerVotedEventArgs e)
					{
						if (e.vote == Vote.Yes)
						{
							GameManager.Instance.VotingSystem.EndVote();
							SetOutline(FindObjectsOfType<Sign>(), false);

							EventManager.Instance.OnNextTutorialItem(this);
							EventManager.Instance.PlayerVoted -= playerVoted;
							GameplaySettings.AmountOfStartingSheep = 4;
						}
                        else
                        {
							EventManager.Instance.OnStopTutorial(this);
							EventManager.Instance.OnRoundStart(this);
							SetOutline(FindObjectsOfType<Sign>(), false);
							GameplaySettings.AmountOfStartingSheep = 8;
							EventManager.Instance.PlayerVoted -= playerVoted;
						}
					}
					EventManager.Instance.PlayerVoted += playerVoted;
					SetOutline(FindObjectsOfType<Sign>(), true);
					GameManager.Instance.PresentationSystem.SetPresentation(presentation);
					GameManager.Instance.PresentationSystem.SetTextures(presentation.Slides[0], presentation.Color);
					break;
				}
				case "tutorial_01_vote_tutorial":
				{
					GameManager.Instance.PresentationSystem.NextSlide();
					break;
				}
				case "tutorial_01_vote":
				{
					void playerVoted(object sender, PlayerVotedEventArgs e)
					{
						if (e.vote == Vote.Yes)
						{
							GameManager.Instance.VotingSystem.EndVote();
							SetOutline(FindObjectsOfType<Sign>(), false);

							EventManager.Instance.OnNextTutorialItem(this);
							EventManager.Instance.PlayerVoted -= playerVoted;
						}
                        else
							EventManager.Instance.OnTutorialWrong(this);
					}
					GameManager.Instance.PresentationSystem.NextSlide();
					EventManager.Instance.PlayerVoted += playerVoted;
					SetOutline(FindObjectsOfType<Sign>(), true);
					break;
				}
				case "tutorial_02_presentation":
				{
					void playerNextSlide(object sender)
					{
						EventManager.Instance.OnNextTutorialItem(this);
						EventManager.Instance.PlayerNextSlide -= playerNextSlide;
					}
					GameManager.Instance.PresentationSystem.NextSlide();

					EventManager.Instance.OnNextSlide(this);
					EventManager.Instance.PlayerNextSlide += playerNextSlide;
					break;
                }
				case "tutorial_03_camera":
				{
					void pictureTakenTargetless(object sender)
					{
						EventManager.Instance.OnTutorialWrong(this);
					}
					void pictureTakenTarget(object sender)
					{
						SetOutline(FindObjectsOfType<InSheepsClothing.Interaction.Camera>(), false);
						EventManager.Instance.PictureTakenTargetless -= pictureTakenTargetless;
						EventManager.Instance.PictureTakenTarget -= pictureTakenTarget;
						GameManager.Instance.TargetSystem.ClearTarget();
						EventManager.Instance.OnNextTutorialItem(this);
					}
					GameManager.Instance.PresentationSystem.NextSlide();

					SetOutline(FindObjectsOfType<InSheepsClothing.Interaction.Camera>(), true);
					GameManager.Instance.TargetSystem.SetTarget();
					EventManager.Instance.PictureTakenTargetless += pictureTakenTargetless;
					EventManager.Instance.PictureTakenTarget += pictureTakenTarget;
					break;
                }
				case "tutorial_04_sending_pictures":
				{
					void pictureInSuitcase(object sender, PictureInSuitcaseEventArgs e)
					{
						EventManager.Instance.PictureInSuitcase -= pictureInSuitcase;
						EventManager.Instance.OnNextTutorialItem(this);
					}
					GameManager.Instance.PresentationSystem.NextSlide();

					EventManager.Instance.PictureInSuitcase += pictureInSuitcase;
					break;
                }
				case "tutorial_03_end_cue":
				{
					EventManager.Instance.OnRoundStart(this);
					break;
                }
			}
		}
    }
}