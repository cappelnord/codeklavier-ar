using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {

public class CKARTools
{
    public static float LinLin(float value, float minFrom, float maxFrom, float minTo, float maxTo)
    {
        if (value <= minFrom) return minTo;
        if (value >= maxFrom) return maxTo;

        float normRange = (value - minFrom) / (maxFrom - minFrom);
        return Mathf.Lerp(minTo, maxTo, normRange);
        
    }
}
}