using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGeneratorScaler : MonoBehaviour
{

    public float DefaultScale = 0.7f;
    public float LowerBoundWidth = 10.0f;
    public float UpperBoundHeight = 16.0f;
    public bool Active = true;

    private IIRFilter filter;
    private float scale;

    // Start is called before the first frame update
    void Start()
    {
        filter = new IIRFilter(DefaultScale, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        float scale = DefaultScale;

        if(Active)
        {
            Bounds bounds = GetBounds();

            float width = bounds.extents.x / transform.localScale.x;
            if (width >= LowerBoundWidth)
            {
                scale = DefaultScale * (LowerBoundWidth / width);
            }
            else
            {
                float height = bounds.extents.y / transform.localScale.y;
                if (height <= UpperBoundHeight && height >= 0.1)
                {
                    scale = DefaultScale * (UpperBoundHeight / height);
                }
            }
        }

        scale = filter.Filter(scale);
        transform.localScale = new Vector3(scale, scale, scale);

    }

    // https://forum.unity.com/threads/getting-the-bounds-of-the-group-of-objects.70979/
    Bounds GetBounds()
    {
        Bounds bounds = new Bounds();
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            //Find first enabled renderer to start encapsulate from it
            foreach (Renderer renderer in renderers)
            {

                if (renderer.enabled)
                {
                    bounds = renderer.bounds;
                    break;
                }
            }

            //Encapsulate for all renderers

            foreach (Renderer renderer in renderers)
            {
                if (renderer.enabled)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
        }
        return bounds;
    }
}
