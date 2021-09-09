using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    const int destroyMask = (1 << 9) | (1 << 10);
    const float timeStep = 0.04f;
    bool destroyed;

    public GameObject effectPrefab;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Hit");

        if (((1 << other.gameObject.layer) & destroyMask) == 0 || destroyed) return;
        destroyed = true;

        //Zerstöre den Block:
        if(effectPrefab != null) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        StartCoroutine(DestroyBlock());

    }


    IEnumerator DestroyBlock()
    {
        //Erstelle losgelöste Instanz des Materials:
        Material mat = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = mat;

        //Randomisiere Pattern:
        mat.SetVector("_Tiling", 50 * new Vector2(Random.value > .5f ? 1 : -1, Random.value > .5f ? 1 : -1));
        mat.SetVector("_Offset", Random.insideUnitCircle);

        for(float count = 0; count < 1; count += timeStep)
        {
            mat.SetFloat("_Step", count);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
