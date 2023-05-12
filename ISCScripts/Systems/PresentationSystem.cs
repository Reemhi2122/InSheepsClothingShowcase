using InSheepsClothing.Events;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace InSheepsClothing
{
	public enum PresentationState
	{
		PresentationState_None,
		PresentationState_Waiting,
	}

	public class PresentationSystem : MonoBehaviour
	{
		private PresentationState presentationState = PresentationState.PresentationState_None;
		public PresentationState PresentationState => presentationState;

		[SerializeField]
		private Renderer laptopRenderer = null;

		[SerializeField]
		private Renderer presentationRenderer = null;

		[SerializeField]
		private DecalProjector decalRenderer = null;

		[SerializeField]
		private Presentation[] presentations;

		private int currentSlide = 0, currentPresentationIndex = 0;

		private Presentation currentPresentation = null;

		private Timer
			presentationTimer = new Timer(),
			presentationUrgentTimer = new Timer();

        private void Awake()
        {
			presentationTimer.Enabled = false;
			presentationUrgentTimer.Enabled = false;

			presentationTimer.SetTime(GameplaySettings.PresentationTimeDelayRoundStart);
			presentationUrgentTimer.SetTime(GameplaySettings.PresentationUrgentTime);
		}

		private void OnEnable()
		{
			EventManager.Instance.RoundStart += RoundStart;
		}

		private void DisableTimer(ref Timer timer, float time)
		{
			timer.Enabled = false;
			timer.Reset();
			timer.SetTime(time);
		}

		private void RoundStart(object sender)
		{
			EventManager.Instance.RoundStart -= RoundStart;

			presentationTimer.Enabled = true;

			presentationTimer.TickAction = PresentationGracePeriodEnded;
			presentationUrgentTimer.TickAction = PresentationWarning;

			EventManager.Instance.NextSlide += NextSlide;
			EventManager.Instance.PlayerNextSlide += PlayerNextSlide;

			currentPresentationIndex = 0;
			currentPresentation = presentations[currentPresentationIndex];
			SetTextures(currentPresentation.Slides[currentSlide], currentPresentation.Color);
		}

		private void PresentationGracePeriodEnded()
		{
			presentationUrgentTimer.Reset();
			presentationUrgentTimer.Enabled = true;

			float time = GameplaySettings.PresentationTimeMax;
			DisableTimer(ref presentationTimer, time);

			presentationTimer.TickAction = PresentationTimerFinished;
			EventManager.Instance.OnNextSlide(this);
		}

		public void SetPresentation(Presentation presentation)
		{
			currentPresentation = presentation;
		}

		private void PresentationWarning()
		{
			GameManager.Instance.SheepSystem.presentationSheep.AddSuspiciousness();
			presentationUrgentTimer.Reset();

			EventManager.Instance.OnNextSlideWarning(this);
		}

		private void OnDisable()
		{
			EventManager.Instance.NextSlide -= NextSlide;
			EventManager.Instance.PlayerNextSlide -= PlayerNextSlide;
		}

		private void Update()
		{
			presentationTimer.Update();
			presentationUrgentTimer.Update();
		}

		private void NextSlide(object sender)
		{
			if (presentationState == PresentationState.PresentationState_Waiting)
				return;

			presentationState = PresentationState.PresentationState_Waiting;
		}

		private void IncrementSlide()
		{
			currentSlide = ++currentSlide % currentPresentation.Slides.Length;
			if (currentSlide == 0)
			{
				currentPresentationIndex = ++currentPresentationIndex % presentations.Length;
				currentPresentation = presentations[currentPresentationIndex];
				currentSlide = 0;
			}
		}

		private void PlayerNextSlide(object sender)
		{
			presentationUrgentTimer.Reset();
			presentationUrgentTimer.Enabled = false;
			presentationTimer.Enabled = true;
			NextSlide();

			if (presentationState != PresentationState.PresentationState_Waiting)
				return;
		}

		public void NextSlide()
		{
			IncrementSlide();
			SetTextures(currentPresentation.Slides[currentSlide], currentPresentation.Color);
			presentationState = PresentationState.PresentationState_None;
		}

		public void SetTextures(Sprite sprite, Color color)
		{
			laptopRenderer.material.SetTexture("_BaseMap", sprite.texture);
			presentationRenderer.material.SetTexture("_Base_Map", sprite.texture);
			decalRenderer.material.SetTexture("_Base_Map", sprite.texture);
		}

		public void PresentationTimerFinished()
		{
			presentationUrgentTimer.Reset();
			presentationUrgentTimer.Enabled = true;

			float time = presentationUrgentTimer.GetTime() * GameplaySettings.PresentationUrgentMultiplier;
			time = Mathf.Clamp(time, GameplaySettings.PresentationUrgentMin, GameplaySettings.PresentationUrgentMax);
			presentationUrgentTimer.SetTime(time);

			time = presentationTimer.GetTime() * GameplaySettings.PresentationMultiplier;
			time = Mathf.Clamp(time, GameplaySettings.PresentationTimeMin, GameplaySettings.PresentationTimeMax);
			DisableTimer(ref presentationTimer, time);

			presentationTimer.TickAction = PresentationTimerFinished;
			EventManager.Instance.OnNextSlide(this);
		}
	}
}