using InSheepsClothing.Events;
using UnityEngine;
using VRTools.Interaction;

namespace InSheepsClothing.Interaction
{
	public class Touchpad : MonoBehaviour
	{
		private bool canClick = true;

		private Animator animator = null;

		private void Awake() => animator = GetComponent<Animator>();

		private void OnEnable() => EventManager.Instance.NextSlide += NextSlide;
		private void OnDisable() => EventManager.Instance.NextSlide -= NextSlide;

		private void NextSlide(object sender)
		{
			animator.SetBool("Press", true);
			GetComponent<Outline>().enabled = true;
			canClick = true;
		}

		private void OnTriggerEnter(Collider collision)
		{
			if (canClick && collision.GetComponent<Grabber>())
				Press();
		}

		public void Press()
		{
			animator.SetBool("Press", false);
			GetComponent<Outline>().enabled = false;
			canClick = false;
			EventManager.Instance.OnPlayerNextSlide(this);
		}
	}
}