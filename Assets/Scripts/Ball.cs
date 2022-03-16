using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private float maxSpeed;
    private bool reachedMaxSpeed;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        maxSpeed = 2.7f;
        reachedMaxSpeed = false;
    }
    
    private void OnCollisionExit(Collision other)
    {
        var velocity = m_Rigidbody.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        float verticallity = Vector3.Dot(velocity.normalized, Vector3.up);
        if (other.gameObject.tag == "Paddle") {
            if (verticallity > 0.98f)
            {
                velocity += new Vector3(GetRandomForce(velocity.normalized.x > 0), 0, 0);
                
            } else if (verticallity < 0.15f) {
                velocity += new Vector3(0, GetRandomForce(true), 0);
            }
        }

        //max velocity
        if (velocity.magnitude > maxSpeed)
        {
            if (!reachedMaxSpeed) {
                reachedMaxSpeed = true;
            }
            velocity = velocity.normalized * maxSpeed;
        }

        m_Rigidbody.velocity = velocity;
    }

    public void AddMaxSpeed()
    {
        this.maxSpeed += 0.3f;
        this.reachedMaxSpeed = false;
    }

    private float GetRandomForce(bool positive)
    {
        return UnityEngine.Random.Range(0.25f, 0.5f) * (positive ? 1 : -1);
    }
}
