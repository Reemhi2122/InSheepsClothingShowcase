using UnityEngine;
using VRTools.Interaction;

public class AnimatedHand : MonoBehaviour
{
    [SerializeField]
    private Hand hand;

    private FloatInputAction input = null;

    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        input = VRControls.Instance.GetHandGrip(hand);
    }

    private void Update()
    {
        animator.SetFloat("Grab", input.Value);
    }
}
