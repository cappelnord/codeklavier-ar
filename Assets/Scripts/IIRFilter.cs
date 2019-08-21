using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IIRFilter
{
    private float weight;
    private float counterWeight;
    private float state;
    private float target;

    public IIRFilter(float _state, float _weight)
    {
        weight = _weight;
        Init(_state);
    }

    public void Init(float _state)
    {
        state = _state;
        target = _state;
    }

    public void Set(float _target)
    {
        target = _target;
    }

    public float Get()
    {
        return state;
    }

    public float Filter(float _target)
    {
        target = _target;
        return Filter();
    }

    public float Filter()
    {
        Compute();
        return state;
    }

    public void Compute()
    {

        float targetWeight = Mathf.Clamp(weight * 60.0f * Time.deltaTime, 0.0f, 1.0f);
        float stateWeight = 1.0f - targetWeight;

        state = state * stateWeight + target * targetWeight;
    }
}

public class IIRFilter3
{
    private float weight;
    private float counterWeight;
    private Vector3 state;
    private Vector3 target;

    public IIRFilter3(Vector3 _state, float _weight)
    {
        weight = _weight;
        Init(_state);
    }

    public void Init(Vector3 _state)
    {
        state = _state;
        target = _state;
    }

    public void Set(Vector3 _target)
    {
        target = _target;
    }

    public Vector3 Get()
    {
        return state;
    }

    public Vector3 Filter(Vector3 _target)
    {
        target = _target;
        return Filter();
    }

    public Vector3 Filter()
    {
        Compute();
        return state;
    }

    public void Compute()
    {
        float targetWeight = Mathf.Clamp(weight * 60.0f * Time.deltaTime, 0.0f, 1.0f);
        float stateWeight = 1.0f - targetWeight;

        state = state * stateWeight + target * targetWeight;
    }

}