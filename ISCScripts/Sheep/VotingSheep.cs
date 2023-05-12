using InSheepsClothing.Events;
using System.Collections;
using UnityEngine;
using VRTools.Interaction;

namespace InSheepsClothing
{
	public class VotingSheep : Sheep
	{
		private Quaternion PresentationRotation = new Quaternion();
		private Transform PlayerTransform;
		private bool IsVoting = false, IsLookingPresentation = false;

#pragma warning disable 0414
		LTDescr tween;
#pragma warning restore 0414

		public Animator animator = null;

		private Timer
			peekTimer = new Timer(),
			peekDelayTimer = new Timer();

		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
			animator.ForceStateNormalizedTime(UnityEngine.Random.Range(0.0f, 1.0f));

			peekDelayTimer.SetTime(Random.Range(GameplaySettings.VotingSheepPeekDelayTimeMin, GameplaySettings.VotingSheepPeekDelayTimeMax));
			peekDelayTimer.Enabled = false;
			peekDelayTimer.TickAction = OnPeek;
		}

		private void Start()
		{
			GameManager gameManager = GameManager.Instance;

			Vector3 PresentationSheepTransform = gameManager.PresentationSheepTransform.position;
			Transform MyTransform = this.transform;
			MyTransform.LookAt(new Vector3(PresentationSheepTransform.x, this.transform.position.y, PresentationSheepTransform.z), Vector3.up);
			PresentationRotation = MyTransform.rotation;

			PlayerTransform = gameManager.Player.transform;
		}

		private new void OnEnable()
		{
			base.OnEnable();
			EventManager.Instance.NextSlideWarning += NextSlideWarning;
			EventManager.Instance.PlayerNextSlide += PlayerNextSlide;

			peekDelayTimer.Enabled = true;
		}

		private new void OnDisable()
		{
			base.OnDisable();
			EventManager.Instance.NextSlideWarning -= NextSlideWarning;
			EventManager.Instance.PlayerNextSlide -= PlayerNextSlide;
		}

		private void NextSlideWarning(object sender)
		{
			IsLookingPresentation = true;
			LookAtPlayer();
		}

		private void PlayerNextSlide(object sender)
		{
			IsLookingPresentation = false;
			LookAtPresentationSheep();
		}

		public override bool IsSheepLooking()
		{
			return base.IsSheepLooking() || IsVoting || IsLookingPresentation;
		}

		private void OnPeek()
		{
			peekDelayTimer.Reset();
			peekDelayTimer.Enabled = false;

			float suspiciousness = GetSuspiciousness() / 100.0f;
			float halfpeekdelaymax = GameplaySettings.VotingSheepPeekDelayTimeMax;
			float peeksussubraction = halfpeekdelaymax * suspiciousness;

			peekDelayTimer.SetTime(Random.Range(GameplaySettings.VotingSheepPeekDelayTimeMin, GameplaySettings.VotingSheepPeekDelayTimeMax - peeksussubraction));

			peekTimer.SetTime(Random.Range(GameplaySettings.VotingSheepPeekTimeMin, GameplaySettings.VotingSheepPeekTimeMax));
			peekTimer.Enabled = true;
			peekTimer.TickAction = OnPeekEnd;

			IsLooking = true;
			LookAtPlayer();
		}

		private void OnPeekEnd()
		{
			peekTimer.Reset();
			peekTimer.Enabled = false;

			IsLooking = false;
			LookAtPresentationSheep();
		}

		private void LookAtPlayer()
		{
			Vector3 relativePos = new Vector3(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z) - transform.position;
			Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
			tween = LeanTween.rotate(this.gameObject, rotation.eulerAngles, 0.75f).setEase(LeanTweenType.easeOutSine);
		}

		private void LookAtPresentationSheep()
		{
			if (!IsLooking && !IsVoting && !IsLookingPresentation)
				tween = LeanTween.rotate(this.gameObject, PresentationRotation.eulerAngles, 0.75f).setEase(LeanTweenType.easeOutSine);
		}

		private new void Update()
		{
			base.Update();
			peekDelayTimer.Update();
			peekTimer.Update();

			if (Input.GetKeyDown(KeyCode.E))
				LookAtPlayer();
		}

		public void Vote(Vote a_Vote)
		{
			IsVoting = true;
			LookAtPlayer();
			animator.SetInteger("Vote", (int)a_Vote);
			animator.SetBool("IsVoting", true);
		}

		public void EndVote()
		{
			animator.SetBool("IsVoting", false);
			IsVoting = false;
			LookAtPresentationSheep();
		}
	}
}