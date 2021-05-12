using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(100)]
public class RuleFeeder : MonoBehaviour
{

    private  LSystemController controller;
    private bool didDispatch = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = LSystemController.Instance();
    }

    void Update()
    {
        if(!didDispatch)
        {
            LSystemController c = controller;

            // public void DispatchShape(string key, string shape)
            // public void DispatchTransform(string key, TransformSpec ts)

            // position, scale, rotation

            c.Dispatch("1@g.5,*.121,1.23,2.51,3.1");
            c.DispatchShape("1", "1");

            c.Dispatch("2@g.5,*.103,1.43,4.3,3.11");
            c.DispatchShape("2", "2");
            c.DispatchTransform("2", new TransformSpec(new float[] { 0f, -3f, 0f }, new float[] { 2f, 2f, 2f }, new float[] { 0f, 10f, 0f }));


            c.Dispatch("3@g.4,*.123,1.431,4.32,3.11,2.3");
            c.DispatchShape("3", "3");
            c.DispatchTransform("3", new TransformSpec(new float[] { -1f, 1f, 1f }, new float[] { 1.2f, 1.2f, 1.2f }, new float[] { 45f, 56f, 90f }));

            c.Dispatch("4@g.3,*.123,1.431,4.32,3.11,2.3");
            c.DispatchShape("4", "4");
            c.DispatchTransform("4", new TransformSpec(new float[] { 1f, 0.2f, 1f }, new float[] { 1f, 1f, 1f }, new float[] { 45f, 56f, 90f }));



            c.Dispatch("5@g.4,*.131,1.21,4.41,3.11,2.31");
            c.DispatchShape("5", "3");
            c.DispatchTransform("5", new TransformSpec(new float[] { 1f, -1f, 7f }, new float[] { 4f, 4f, 4f }, new float[] { 45f, 56f, 90f }));


            c.Dispatch("6@g.2,*.123,1.41,4.32,3.11,2.3");
            c.DispatchShape("6", "4");
            c.DispatchTransform("6", new TransformSpec(new float[] { -1f, 0.7f, 1f }, new float[] { 1f, 1f, 1f }, new float[] { 45f, 56f, 90f }));




            didDispatch = true;
        }
    }
}
