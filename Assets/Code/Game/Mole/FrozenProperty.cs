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
        if (_freezeState != FreezeState.Unfrozen)
        {
            _stateChanged = true;
            _freezeState--;
        }
    }

    public void Assign(params object[] parameters)
    {
        _freezeState = FreezeState.Frozen;
    }

    public void Reset()
    {
        _freezeState = FreezeState.Unfrozen;
    }

    public void Animate(Animator animator)
    {
        if (_stateChanged)
        {
            animator.SetInteger("Freeze", (int)_freezeState);
            _stateChanged = false;
        }
    }
}
