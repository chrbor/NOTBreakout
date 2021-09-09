using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class BallScript : MonoBehaviour, IDestructable
{
    public GameObject hitEffect;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        manager.getActiveBalls += AddBall;
    }

    private void OnDestroy()
    {
        manager.getActiveBalls -= AddBall;
    }
    private void AddBall() => activeBalls++;


    private void FixedUpdate()
    {
        if (gamePause || !gameRun) return;
        rb.velocity += manager.ballGravity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 diff = other.GetContact(0).point - (Vector2)transform.position;
        Vector2 norm = other.GetContact(0).normal;


        //Debug.Log(Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg - 90);
        //Debug.Break();
        Instantiate(hitEffect, other.GetContact(0).point, Quaternion.Euler(Vector3.forward * (Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg - 90)));
        
    }

    public void DestroyObject()
    {
        activeBalls--;
        Destroy(gameObject);
    }
}
