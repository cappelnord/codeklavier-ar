using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class LinearLifeBehaviour : LifeBehaviour
{

    public Vector3 OutputScale = new Vector3(0f, 0f, 0f);

    override public void Update()
    {
        if (Time.time > GrowStartTime)
        {
            float timeSinceGrowStart = Time.time - GrowStartTime;
            float scale = Mathf.Lerp(0f, 1f, timeSinceGrowStart / GrowTime);
            // transform.localScale = TargetScale * scale;
            OutputScale = TargetScale * scale;
        } 
    }
}
}
