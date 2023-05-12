using InSheepsClothing.Events;
using UnityEngine;

namespace InSheepsClothing
{
	public class Player : MonoBehaviour
    {
        private Vote currentVote;

        public Vote GetCurrentVote()
        {
            return currentVote;
        }

        public void Detected()
        {
            Debug.Log("Detected! You have been seen! ");
        }
    }
}