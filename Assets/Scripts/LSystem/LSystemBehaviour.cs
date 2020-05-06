using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemBehaviour : MonoBehaviour
{
    protected LSystem lsys;
    private string lastLsysState;
    private bool first = true;

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
                if (first)
                {
                    first = false;
                    return true;
                }
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
        lastLsysState = null;
        OnLSystemSet();
    }

    public virtual void OnLSystemSet()
    {

    }
}
