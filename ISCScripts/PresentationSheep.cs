using InSheepsClothing.Events;
using UnityEngine;

namespace InSheepsClothing
{
    public class PresentationSheep : Sheep
    {
        [SerializeField]
        private GameObject exclamationMark = null;

        private Animator animator = null;

        private void Awake() => animator = GetComponentInChildren<Animator>();

        private new void OnEnable()
        {
            base.OnEnable();
            PlayerNextSlide(this);

            EventManager.Instance.NextSlide += NextSlide;
            EventManager.Instance.NextSlideWarning += NextSlideWarning;
            EventManager.Instance.PlayerNextSlide += PlayerNextSlide;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            EventManager.Instance.NextSlide -= NextSlide;
            EventManager.Instance.NextSlideWarning -= NextSlideWarning;
            EventManager.Instance.PlayerNextSlide -= PlayerNextSlide;
        }

        private void NextSlide(object sender)
        {
            animator.SetBool("NextSlide", true);
            exclamationMark.SetActive(true);
        }

        private void PlayerNextSlide(object sender)
        {
            animator.SetBool("Alarm", false);
            animator.SetBool("NextSlide", false);
            exclamationMark.SetActive(false);
        }

        private void NextSlideWarning(object sender)
        {
            animator.SetBool("Alarm", true);
        }
    }
}