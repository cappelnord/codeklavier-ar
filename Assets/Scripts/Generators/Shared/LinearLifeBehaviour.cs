using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearLifeBehaviour : LifeBehaviour
{

    override public void Update()
    {
        if (Time.time > GrowStartTime)
        {
            float timeSinceGrowStart = Time.time - GrowStartTime;
            float scale = Mathf.Lerp(0f, 1f, timeSinceGrowStart / GrowTime);
            transform.localScale = TargetScale * scale;
        }
    }
}
