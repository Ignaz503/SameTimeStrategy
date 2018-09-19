using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialButtonLayout : MonoBehaviour {

    [SerializeField] float baseDegreeOffset;
    [SerializeField] float radius = 1f;
    [SerializeField] [Range(0.01f, 360)] float circleAmount; 
    [SerializeField] List<RectTransform> children;
    [SerializeField] bool controlSize;
    [SerializeField] bool fit;
    [SerializeField] float sizeCorrection = 1f;
    [SerializeField] float maxRadAllowed = 100f;


    private void Start()
    {
        circleAmount = circleAmount == 0 ? 0.01f : circleAmount;
        PlaceChildren();
    }

    void PlaceChildren()
    {
        circleAmount = circleAmount == 0 ? 0.01f : circleAmount;
        float stepSize = circleAmount / children.Count;

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] == null)
                continue;
            float rad = (baseDegreeOffset + (i * stepSize))*Mathf.Deg2Rad;
            
            children[i].localPosition = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)).normalized*radius;

        }
    }

    private void OnValidate()
    {
        if (fit)
            FitButtons138CircleAmount();
        PlaceChildren();
        if (controlSize)
            UpdateSize();
    }

    void UpdateSize()
    {
        if (children.Count == 0)
            return;

        Debug.Log(PotentialRadius());

        if(PotentialRadius() < maxRadAllowed)
        {
            radius = PotentialRadius();
            PlaceChildren();
            foreach (RectTransform t in children)
                t.localScale = Vector3.up + Vector3.right;

            return;
        }
        else
        {
            radius = maxRadAllowed;
            PlaceChildren();

            float circlePercent = circleAmount / 360f;

            float preDevisor = 10 * (circlePercent);

            float devisor = preDevisor + ((10 - preDevisor) * (1.0f - circlePercent));

            float scale = ((radius/children.Count)/devisor)* circlePercent;
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] == null)
                    continue;

                children[i].localScale = new Vector3(scale,scale)* sizeCorrection;
            }
        } 

    }

    float PotentialRadius()
    {
        float circlePercent = circleAmount / 360f;

        float preDevisor = 10 * (circlePercent);

        float devisor = preDevisor + ((10 - preDevisor) * (1.0f - circlePercent));

        return (1.0f / circlePercent) * devisor * children.Count;

    }

    private void OnDrawGizmos()
    {
        foreach(RectTransform trans in children)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(trans.position, trans.sizeDelta*trans.localScale);
        }
    }

    void FitButtons138CircleAmount()
    {

            baseDegreeOffset = 122.8433f - (109.664f * children.Count) + (37.36526f * (children.Count * children.Count)) - (6.193161f * (children.Count * children.Count * children.Count)) + (0.4876192f * (children.Count * children.Count * children.Count * children.Count)) - (0.0146282f * (children.Count * children.Count * children.Count * children.Count * children.Count));

    }

}
