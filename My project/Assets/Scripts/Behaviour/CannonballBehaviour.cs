using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballBehaviour : MonoBehaviour
{
    const float speed = 4f;

    protected Animator anim;

    void Start()
    {
        var dir = Quaternion.AngleAxis(transform.parent.eulerAngles.z-270, Vector3.forward) * Vector3.down;
        gameObject.GetComponent<Rigidbody2D>().velocity = dir * speed;
        gameObject.GetComponent<Rigidbody2D>().velocity += transform.root.GetComponent<Rigidbody2D>().velocity;
        gameObject.transform.parent = null;
        anim = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        try{

        if(col.gameObject.name != this.gameObject.transform.root.name)
        {
            if(col.gameObject.name == "Player")
            {
                col.GetComponent<PlayerBehaviour>().Damage(1);
            }
            if(col.gameObject.name == "Shooter(Clone)")
            {
                col.GetComponent<EnemyBehaviour>().Damage(1);
            }
            if(col.gameObject.name == "Chaser(Clone)")
            {
                col.GetComponent<EnemyBehaviour>().Damage(1);
            }
            anim.SetTrigger("Hit");
        }

        }catch(NullReferenceException e){}
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
