using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemBehaviour : MonoBehaviour
{
    protected LSystem lsys;
    private string lastLsysState;

    public bool ShouldAct()
    {
        if(lsys == null)
        {
            return false;
        } else
        {
            string thisLsysState = lsys.StateString();
            if(thisLsysState != lastLsysState)
            {
                lastLsysState = thisLsysState;
                return true;
            } else
            {
                return false;
            }
        }
    }

    public void SetLSystemKey(string key)
    {
        SetLSystem(LSystemController.Instance().GetLSystem(key));
    }

    public void SetLSystem(LSystem _lsys)
    {
        lsys = _lsys;

        OnLSystemSet();
    }

    public virtual void OnLSystemSet()
    {

    }
}
