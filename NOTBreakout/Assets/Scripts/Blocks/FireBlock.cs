using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        IDestructable destructable = other.gameObject.GetComponent<IDestructable>();
        if (destructable == null) return;
        destructable.DestroyObject();

    }
}
