using InSheepsClothing.Events;
using System.Collections.Generic;
using UnityEngine;

public enum VotingState
{
	VotingState_None,
	VotingState_Waiting,
}

namespace InSheepsClothing
{
	public enum Vote
	{
		Yes,
		No,
		None
	}

	public class VotingSystem : MonoBehaviour
	{
		private VotingState votingState;
		public VotingState VotingState => votingState;
		public List<VotingSheep> votingSheep => GameManager.Instance.SheepSystem.VotingSheepList;
		public SheepSystem sheepSystem;

		private Vote expectedVote;
		public Vote ExpectedVote => expectedVote;

		private void DisableTimer(ref Timer timer, float time)
		{
			timer.Enabled = false;
			timer.Reset();
			timer.SetTime(time);
		}

		private Timer
			voteTimer = new Timer(),
			votingTimer = new Timer();

		public Timer VoteTimer => voteTimer;
		public Timer VotingTimer => votingTimer;

		private void Awake()
		{
			voteTimer.Enabled = false;
			votingTimer.Enabled = false;

			voteTimer.SetTime(GameplaySettings.TimeToVoteMax);
			votingTimer.SetTime(GameplaySettings.TimeTillVotingDelayRoundStart);
		}

        private void OnEnable()
		{
			EventManager.Instance.RoundStart += RoundStart;
		}

        private void RoundStart(object sender)
		{
			EventManager.Instance.RoundStart -= RoundStart;

			EventManager.Instance.Vote += StartVote;
			EventManager.Instance.PlayerVoted += PlayerVoted;

			votingTimer.Enabled = true;
			votingTimer.TickAction = VotingGracePeriodEnded;

			voteTimer.SetTime(GameplaySettings.TimeToVoteMax);
			votingTimer.SetTime(GameplaySettings.TimeTillVotingDelayRoundStart);
		}

        private void OnDisable()
		{
			EventManager.Instance.Vote -= StartVote;
			EventManager.Instance.PlayerVoted -= PlayerVoted;
		}

		private void VotingGracePeriodEnded()
		{
			float time = GameplaySettings.TimeTillVotingMax;
			DisableTimer(ref votingTimer, time);

			votingTimer.TickAction = VotingTimerFinished;
			EventManager.Instance.OnVote(this);
		}

		private void VotingTimerFinished()
		{
			float time = votingTimer.GetTime() * GameplaySettings.TimeTillVotingMultiplier;
			time = Mathf.Clamp(time, GameplaySettings.TimeTillVotingMin, GameplaySettings.TimeTillVotingMax);
			DisableTimer(ref votingTimer, time);

			EventManager.Instance.OnVote(this);
		}

		private void Update()
		{
			voteTimer.Update();
			votingTimer.Update();
		}

		public void StartVote(object sender)
		{
			if (votingState == VotingState.VotingState_Waiting)
				return;

			votingState = VotingState.VotingState_Waiting;

			expectedVote = Random.Range(0, 2) == 0 ? Vote.No : Vote.Yes;
			Debug.Log(expectedVote);

			List<int> SheepLeft = new List<int>();

			Debug.Log(votingSheep.Count + " count of voting sheep");
			for (int i = 0; i < votingSheep.Count; i++)
				SheepLeft.Add(i);

			int minimumVotingCorrect = (votingSheep.Count / 2) + 1;
			int maxVotingCorrect = votingSheep.Count + 1;
			int amountOfCorrectSheep = Random.Range(minimumVotingCorrect, maxVotingCorrect);

			Debug.Log(amountOfCorrectSheep + " correct sheep");

			for (int i = 0; i < amountOfCorrectSheep; i++)
			{
				int currentSheep = Random.Range(0, SheepLeft.Count);
				votingSheep[SheepLeft[currentSheep]].Vote(expectedVote);
				SheepLeft.RemoveAt(currentSheep);
			}

			for (int i = 0; i < SheepLeft.Count; i++)
				votingSheep[SheepLeft[i]].Vote(expectedVote == Vote.No ? Vote.Yes : Vote.No);

			voteTimer.Enabled = true;
			voteTimer.TickAction = OnVoteEnd;
		}

		private void OnVoteEnd()
		{
			CheckVote(Vote.None);
		}

		private void PlayerVoted(object sender, PlayerVotedEventArgs e)
		{
			if (votingState == VotingState.VotingState_None)
				return;

			CheckVote(e.vote);
		}

		public void EndVote()
        {
			votingState = VotingState.VotingState_None;
			for (int i = 0; i < votingSheep.Count; i++)
				votingSheep[i].EndVote();
		}

		private void CheckVote(Vote vote)
		{
			// Start delay timer again.
			votingTimer.Enabled = true;

			// Reset and disable actual voting timer.
			voteTimer.Reset();
			voteTimer.Enabled = false;

			EndVote();

			if (vote == expectedVote)
				return;

			for (int i = 0; i < votingSheep.Count; i++)
				votingSheep[i].AddSuspiciousness();
			
			GameManager.Instance.Player.Detected();

			float time = voteTimer.GetTime() * GameplaySettings.TimeToVoteMultiplier;
			time = Mathf.Clamp(time, GameplaySettings.TimeToVoteMin, GameplaySettings.TimeToVoteMax);
			voteTimer.SetTime(time);
		}
	}
}