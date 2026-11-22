using UnityEngine;

public class MoleAnimator
{
    private Animator _animator;

    private Mole.State _state;

    public MoleAnimator(Animator animator)
    {
        _animator = animator;
        _state = Mole.State.Hidden;
    }

    public void Animate(Mole.State state)
    {
        if (_state != state)
        {
            _animator.SetInteger("State", (int)state);
        }
    }
}
