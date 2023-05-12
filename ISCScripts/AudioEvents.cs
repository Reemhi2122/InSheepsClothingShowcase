using InSheepsClothing.Events;
using UnityEngine;

namespace InSheepsClothing.Audio
{
    public class AudioEvents : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.PlayerVoted += PlayerVoted;
        }

        private void OnDisable()
        {
            EventManager.Instance.PlayerVoted -= PlayerVoted;
        }

        private void PlayerVoted(object sender, PlayerVotedEventArgs e)
        {
            if (GameManager.Instance.VotingSystem.VotingState != VotingState.VotingState_Waiting)
                return;

            PlayAudioClipByTypeEventArgs playAudioE = new PlayAudioClipByTypeEventArgs()
            {
                immediate = true,
                audioClipType = e.vote == GameManager.Instance.VotingSystem.ExpectedVote ? AudioClipType.AudioClipType_Correct_Vote : AudioClipType.AudioClipType_Incorrect_Vote
            };
            EventManager.Instance.OnPlayAudioClipByType(this, playAudioE);
        }
    }
}