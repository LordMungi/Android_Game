using UnityEngine;

public class FrozenProperty : IMoleProperty
{
    private enum FreezeState
    {
        Frozen,
        Cracked1,
        Cracked2,
        Cracked3,
        Unfrozen
    }

    private FreezeState _freezeState;

    public bool CanKill()
    {
        return _freezeState == FreezeState.Unfrozen;
    }

    public void Shake()
    {
        if (_freezeState != FreezeState.Unfrozen)
           _freezeState++;
    }

    public void Tap()
    {
        if (_freezeState != FreezeState.Unfrozen)
            _freezeState++;
        Debug.Log("State:" +  _freezeState);
    }

    public void Assign(params object[] parameters)
    {
        _freezeState = FreezeState.Frozen;
    }

    public void Reset()
    {
        _freezeState = FreezeState.Frozen;
    }
}
