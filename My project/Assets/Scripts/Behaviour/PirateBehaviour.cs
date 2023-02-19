using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateBehaviour : MonoBehaviour
{
    const float maxSpeed = 5000f;
    Vector3 dir;

    System.DateTime set;
    Vector3 start;
    Vector3 finish;
    int delay;

    void Start()
    {
        StartCoroutine(Aim(1,5));
    }

    void FixedUpdate()
    {
        gameObject.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        if(dir != Vector3.zero) { Walk(); }
    }

    IEnumerator Aim(int minDelay, int maxDelay)
    {
        while(true)
        {
            int r = Random.Range(0,1);
            if(r == 0)
            {
                r = Random.Range(0,4);
                if(r == 0) { dir = Vector3.up; }
                if(r == 1) { dir = Vector3.left; }
                if(r == 2) { dir = Vector3.down; }
                if(r == 3) { dir = Vector3.right; }
                set = System.DateTime.Now;
                start = transform.localPosition;
                finish = transform.localPosition + (dir*(float)(Random.Range(maxSpeed/2,maxSpeed)/10));
            }
            else
            {
                dir = Vector3.zero;
                set = System.DateTime.Now;
                start = transform.localPosition;
                finish = transform.localPosition;
            }
            delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);        }
    }

    void Walk()
    {
        //gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized * maxSpeed;
        var delta = System.DateTime.Now - set;
        gameObject.transform.localPosition = Vector3.Lerp(start, finish, delta.Seconds/delay);
        Debug.Log(gameObject.transform.localPosition+" = "+Vector3.Lerp(start, finish, delta.Seconds/delay)+" ?");
        CheckFlip();
    }
    void CheckFlip()
    {
        //var normal = Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.left;
        //var projection = Vector3.Project(dir, normal);
        //if(projection.normalized - normal.normalized != Vector3.zero)//Check if projection and normal are opposed
        if(dir.x > 0) { gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true; }
        else { gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false; }
    }
}
