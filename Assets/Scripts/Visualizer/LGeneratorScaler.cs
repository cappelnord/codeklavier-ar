using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.Visuals;

public class LGeneratorScaler : MonoBehaviour
{

    public float DefaultScale = 0.7f;
    public float LowerBoundHeight = 2.0f;
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

        float lowerBoundWidth;


        // bit a haaaaaack
        if (Camera.main.aspect > 1.7)
        {
            lowerBoundWidth = 9.25f;
        }
        else
        {
            lowerBoundWidth = 8.0f;
        }

        if (Active)
        {
            scale = DefaultScale;

            Bounds bounds = GetBounds();

            float widthScale = scale;
            float width = bounds.extents.x / transform.localScale.x;

            if (width > 0.1)
            {
                widthScale = DefaultScale * (lowerBoundWidth / width);
            }

            float heightScale = scale;
            float height = bounds.extents.y / transform.localScale.y;
            if (height > 0.1)
            {
                heightScale = DefaultScale * (LowerBoundHeight / height);
            }

            scale = widthScale;
            if(heightScale < scale)
            {
                scale = heightScale;
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
