using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnDamage : MonoBehaviour, IDamagable {

    [SerializeField] MeshRenderer render;
    [SerializeField] Color c;

    public void DoDamage(DamageType type, float amaount)
    {
        render.material.color = Color.red;
        StartCoroutine(ChangeColorBack(1.0f));
    }

    // Use this for initialization
    void Start () {
        render = GetComponent<MeshRenderer>();
        c = render.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ChangeColorBack(float time)
    {
        float t = time;
        while(t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

        render.material.color = c;

    }

}
