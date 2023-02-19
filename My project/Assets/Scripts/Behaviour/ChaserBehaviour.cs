using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBehaviour : EnemyBehaviour
{
    protected bool boom;
    protected bool sinking;

    protected override void Awake()
    {
        moveSpeed = 2.5f;
        turnSpeed = 1f;
        award = 30;

        base.Awake();
    }

    public override void Update()
    {
        target = player.transform.position - gameObject.transform.position;

        if((AngleVector().normalized - target.normalized) != Vector3.zero) { spin = true; }
        else { spin = false; }

        if(!sinking)
        {
            if(target.magnitude > 1) { move = true; }
            else { move = false; boom = true; }
        }
    }

    public override void FixedUpdate()
    {
        try{
            
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        if(move)
        {
            var dir = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.down;
            gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized * moveSpeed;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        if(spin)
        {
            var speed = turnSpeed;
            var normal = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.left;
            var projection = Vector3.Project(target, normal);
            if(projection.normalized - normal.normalized == Vector3.zero)//Check if projection and normal are opposed
            {
                speed = -turnSpeed;
            }
            transform.Rotate(Vector3.forward, speed);
        }

        if(boom)
        {
            Destroy(barInstance);
            anim.SetTrigger("Explode");
            sinking = true;
            boom = false;
            player.GetComponent<PlayerBehaviour>().Damage(4);
        }

        }catch(MissingReferenceException e){}
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
