using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    protected float moveSpeed;
    protected float turnSpeed;
    protected int award;
    
    TimeSpan cadency = new TimeSpan(0,0,0,0,900);//0.5 sec

    public GameObject Cannonball;
    public GameObject Bar;
    protected GameObject barInstance;

    protected BarBehaviour healthBar;

    Transform cannon;
    Transform rightCannon;
    Transform leftCannon;

    protected Animator anim;

    protected GameObject player;

    protected Vector3 target;

    protected int health = 10;

    protected DateTime cooldown1;
    protected DateTime cooldown2;

    protected bool move;
    protected bool spin;
    protected bool shoot;

    protected virtual void Awake()
    {
        barInstance = Instantiate(Bar);
        healthBar = barInstance.GetComponent<BarBehaviour>();
        healthBar.SetShip(gameObject);
        healthBar.Start();

        player = GameObject.FindGameObjectWithTag("Player");

        foreach(Transform t in transform) { if(t.name == "Cannon") { cannon = t; } }

        anim = gameObject.GetComponent<Animator>();

        cooldown1 = DateTime.Now;
        cooldown2 = DateTime.Now;
    }

    public virtual void Update()
    {
        target = player.transform.position - gameObject.transform.position;

        if(target.magnitude > 4.8f) { move = true; }
        else { move = false; }

        if((AngleVector().normalized - target.normalized) != Vector3.zero) { spin = true; }
        else { spin = false; }

        if(!move) { shoot = true; }
        else { shoot = false; }
    }

    public virtual void FixedUpdate()
    {
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

        if(shoot)
        {
            if((DateTime.Now - cooldown1) > cadency)
            {
                Instantiate(Cannonball, cannon.position, Quaternion.identity, cannon);
                cooldown1 = DateTime.Now;
            }
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        healthBar.Refresh(health);
         
        if(health <= 0)
        {
            Destroy(barInstance);
            anim.SetTrigger("Explode");
            GameBehaviour.instance.IncrementScore(award);
        }
        else if(health <= 10)
        {
            anim.SetTrigger("Damage");
        }
    }

    protected Vector3 AngleVector()
    {
        return (Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.down);
    }

    void Destroy()
    {
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(this);
    }
}