using UnityEngine;


[RequireComponent(typeof(Animator))]
public class AnimationInteraction : MonoBehaviour, IInteraction
{
    private Animator _animator;
    public string InteractionAnimationName = "Interaction";
    public string IdleAnimationName = "Idle";

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        _animator.Play(InteractionAnimationName, -1, 0);
    }

    public void ResetInteraction()
    {
    }
}
