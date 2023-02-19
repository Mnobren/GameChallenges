using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    const float moveSpeed = 3f;
    const float turnSpeed = 2f;
    
    TimeSpan cadency = new TimeSpan(0,0,0,0,500);//0.5 sec

    public GameObject Bar;
    public GameObject Cannonball;

    BarBehaviour healthBar;

    Transform cannon;
    Transform rightCannon;
    Transform leftCannon;

    Animator anim;

    private int health = 20;
    float speed = 0;

    bool moveUp;
    bool shootAhead;
    bool shootAside;

    DateTime cooldown1;
    DateTime cooldown2;

    void Awake()
    {
        GameObject bar = Instantiate(Bar);
        healthBar = bar.GetComponent<BarBehaviour>();
        healthBar.SetShip(gameObject);
        healthBar.Start();

        foreach(Transform t in transform) { if(t.name == "Cannon") { cannon = t; } }
        foreach(Transform t in transform) { if(t.name == "RightCannons") { rightCannon = t; } }
        foreach(Transform t in transform) { if(t.name == "LeftCannons") { leftCannon = t; } }

        anim = gameObject.GetComponent<Animator>();

        cooldown1 = DateTime.Now;
        cooldown2 = DateTime.Now;
    }

    public void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        FixedUpdateInput();
        if(shootAhead) { ShootAhead(); shootAhead = false; }
        if(shootAside) { ShootAside(); shootAside = false; }

        if(moveUp) { speed += (moveSpeed/100); }
        else { speed -= (moveSpeed/100); }
        if(speed > moveSpeed) { speed = moveSpeed; }
        if(speed < 0) { speed = 0; }
        var dir = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.down;
        gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
    }

    private void FixedUpdateInput()
    {
        //Must be called on FixedUpdate()

        moveUp = false; 
        if(Input.GetButton("Move Up"))//W
        {
            moveUp = true;
        }
        if(Input.GetButton("Move Left"))//A
        {
            transform.Rotate(Vector3.forward, turnSpeed);
        }
        if(Input.GetButton("Move Right"))//D
        {
            transform.Rotate(Vector3.forward, -turnSpeed);
        }
        if(Input.GetButton("Shoot Ahead"))//M1
        {
            shootAhead = true;
        }
        if(Input.GetButton("Shoot Aside"))//M2
        {
            shootAside = true;
        }
    }

    void ShootAhead()
    {
        if((DateTime.Now - cooldown1) > cadency)
        {
            Instantiate(Cannonball, cannon.position, Quaternion.identity, cannon);
            cooldown1 = DateTime.Now;
        }
    }

    void ShootAside()
    {
        if((DateTime.Now - cooldown2) > cadency)
        {
            Transform chosenCannon = leftCannon;

            var normal = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.left;
            var mouseRelarivePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var projection = Vector3.Project(mouseRelarivePos, normal);
            if(projection.normalized - normal.normalized != Vector3.zero)//Check if projection and normal are opposed
            {
                chosenCannon = leftCannon;
            }
            else
            {
                chosenCannon = rightCannon;
            }

            foreach(Transform t in chosenCannon)
            {
                Instantiate(Cannonball, t.position, Quaternion.identity, t);
            }
            cooldown2 = DateTime.Now;
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        healthBar.Refresh(health);
        
        if(health <= 0)
        {
            Destroy(healthBar);
            anim.SetTrigger("Explode");
            GameBehaviour.instance.GameOver();
            Destroy(this);
        }
        else if(health <= 10)
        {
            anim.SetTrigger("Damage");
        }
    }
}