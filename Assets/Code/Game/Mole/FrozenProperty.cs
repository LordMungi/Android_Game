using UnityEngine;

public class FrozenProperty : IMoleProperty
{
    private enum FreezeState
    {
        Unfrozen,
        Cracked3,
        Cracked2,
        Cracked1,
        Frozen
    }

    private FreezeState _freezeState = FreezeState.Frozen;
    private bool _stateChanged = false;

    private Animator _animator;

    public bool CanKill()
    {
        return _freezeState == FreezeState.Unfrozen;
    }

    public void Shake()
    {
        if (_freezeState != FreezeState.Unfrozen)
        {
            _stateChanged = true;
           _freezeState--;
        }
    }

    public void Tap()
    {
        //if (_freezeState != FreezeState.Unfrozen)
        //{
        //    _stateChanged = true;
        //    _freezeState--;
        //}
    }

    public FrozenProperty(Animator animator)
    {
        _freezeState = FreezeState.Frozen;
        _animator = animator;
        _animator.SetInteger("Freeze", (int)FreezeState.Frozen);
    }

    ~FrozenProperty()
    {
        _animator.SetInteger("Freeze", (int)FreezeState.Unfrozen);
    }

    public void Animate()
    {
        if (_stateChanged)
        {
            _animator.SetInteger("Freeze", (int)_freezeState);
            _stateChanged = false;
        }
    }
}
