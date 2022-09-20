﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.Visuals;
using ARquatic.LSystem;

namespace ARquatic.OldVisuals {

public class PlanetsGenerator : LGenerator
{

    public GameObject ProtoPlanet;
    public GameObject ProtoEmpty;

    protected override void Start()
    {
        base.Start();
    }

    public override void Generate()
    {
        PreGenerate();

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child == transform) continue;

            // Destroy(child.gameObject);
            PlanetsBehaviour behave = child.gameObject.GetComponent<PlanetsBehaviour>();
            if(behave != null)
            {
                behave.Die();
            } else
            {
                Destroy(child.gameObject);
            }
        }

        Grow(gameObject.transform, lsys.Units[0], 0);
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation)
    {
        float outwardRadius = 1.0f - (generation * RandomRange(0.02f, 0.08f));

        // add an empty in center if not-singular axiom
        if(generation == 0 && children.Count == 1) {
            outwardRadius = 0.0f;
        }
        if (generation == 0 && children.Count > 1)
        {
            GameObject empty = Instantiate(ProtoEmpty, parent);
            parent = empty.transform;
        }

        float phase = RandomRange(0.0f, 360.0f);
        float angle = 0.0f;
        float deltaAngle = 360.0f / children.Count;

        float skew = RandomRange(-50.0f, 50.0f);

        float rotationSpeed = RandomRange(2.0f, 2.0f + (2.0f * generation));

        foreach(ProcessUnit unit in children)
        {
            GameObject obj;
            if (VisitUnit(unit) && unit.Content != '0')
            {
                obj = Spawn(parent, unit);
                obj.GetComponent<PlanetsBehaviour>().DegreesPerSecond = rotationSpeed;
                obj.GetComponent<PlanetsBehaviour>().Gen = this;
            }
            else
            {
                obj = Instantiate(ProtoEmpty, parent);
            }

            Transform tra = obj.transform;

            tra.localEulerAngles = new Vector3(0.0f, angle + phase, skew);
            tra.Translate(outwardRadius, 0.0f, 0.0f);
            float newScale = parent.localScale[0] * 0.97f;
            tra.localScale = new Vector3(newScale, newScale, newScale);


            angle += deltaAngle;
            Grow(tra, unit.Children, generation + 1);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit)
    {
        GameObject obj =  Object.Instantiate(ProtoPlanet, parent);

        PlanetStripesCycler psc = obj.transform.GetChild(0).GetComponent<PlanetStripesCycler>();

        psc.PhaseFreqeuncy = new Vector4(RandomRange(1, 3), RandomRange(1, 5), RandomRange(1, 3), RandomRange(1, 5));
        psc.PhaseOffset = new Vector4(RandomRange(-3, 3), RandomRange(-3, 3), RandomRange(-3, 3), RandomRange(-3, 3));
        psc.Offset = RandomRange(0.0f, 10.0f);

        Color col = materialLookup.GetColor(unit.Content);

        psc.Color = col;
        float bgDarken = RandomRange(0.15f, 0.3f);
        psc.BackgroundColor = new Color(col.r * bgDarken, col.g * bgDarken, col.b * bgDarken);

        return obj;
    }
}
}