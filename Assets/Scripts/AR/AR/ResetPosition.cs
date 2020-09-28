using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class ResetPosition : MonoBehaviour, IPointerClickHandler
{

    public bool Active = false;
    public bool Visible = false;

    public GameObject PositionCube;
    public GameObject ARWorld;
    public float DefaultScale = 0.25f;

    public float MinimumScale = 0.05f;
    public float MaximumScale = 1.0f;

    private Image button;

    private bool registeredTouchBegan = false;
    private float currentScale = 0.0f;
    private float lastScaleTouchDistance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Image>();
        Hide();
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active && Visible)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Began)
                {
                    registeredTouchBegan = true;
                }

                if (touch.phase == TouchPhase.Ended && registeredTouchBegan)
                {
                    Deactivate();
                    Position();
                    registeredTouchBegan = false;
                }
            }
        } else
        {
            if(Input.touchCount == 2)
            {
                Vector2 screen = new Vector2(Screen.width, Screen.height);
                Vector2 positionA = Input.GetTouch(0).position / screen;
                Vector2 positionB = Input.GetTouch(1).position / screen;
                float distance = Vector2.Distance(positionA, positionB);
                if(lastScaleTouchDistance == 0.0f)
                {
                    lastScaleTouchDistance = distance;
                }

                float deltaDistance = distance - lastScaleTouchDistance;
                float newScale = currentScale * (1.0f + deltaDistance);
                ScaleWorld(newScale);

                lastScaleTouchDistance = distance;

            } else
            {
                lastScaleTouchDistance = 0.0f;
            }
        }
    }

    void ScaleWorld(float scale)
    {
        scale = Mathf.Clamp(scale, MinimumScale, MaximumScale);
        ARWorld.transform.localScale = new Vector3(scale, scale, scale);
        currentScale = scale;
    }

    void Position()
    {
        ARWorld.transform.localPosition = PositionCube.transform.localPosition;
        ARWorld.transform.localRotation = PositionCube.transform.localRotation;
        ScaleWorld(DefaultScale);
    }

    void UnPosition()
    {
        ARWorld.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Activate()
    {
        Active = true;
        registeredTouchBegan = false;
        if (Visible)
        {
            Show();
        }
        UnPosition();
    }

    void Deactivate()
    {
        Active = false;
        if (Visible)
        {
            Show();
        }
    }

    // Careful: Show and Hide are called every frame atm.
    public void Show()
    {
        if (Active)
        {
            button.enabled = false;
            PositionCube.SetActive(true);
        } else
        {
            button.enabled = true;
            PositionCube.SetActive(false);
        }
        Visible = true;
    }

    public void Hide()
    {
        button.enabled = false;
        PositionCube.SetActive(false);
        Visible = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!Active)
        {
            Activate();
        }
    }
}
