using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        Material mat = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = mat;


        float timeStep = Time.fixedDeltaTime / .2f;
        Color colorStep = (Color.white - Color.black) * timeStep * 2;
        Color colorActive = mat.GetColor("_Color");
        for(float count = 0; count < 1; count += timeStep)
        {
            colorActive += colorStep;
            mat.SetColor("_Color", colorActive);
            mat.SetFloat("_range", -.8f + count);
            yield return new WaitForFixedUpdate();
        }
        mat.SetFloat("_range", .2f);

        yield return new WaitForSeconds(.1f);

        timeStep /= 5;
        for (float count = 0; count < 1; count += timeStep)
        {
            colorActive -= colorStep;
            mat.SetFloat("_pScale", -.35f + count);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
        yield break;
    }
}
