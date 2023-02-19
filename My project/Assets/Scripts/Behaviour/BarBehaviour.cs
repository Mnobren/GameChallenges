using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarBehaviour : MonoBehaviour
{
    public GameObject ship;

    float maxSize;

    public void Start()
    {
        this.gameObject.transform.position = ship.transform.position;
        Transform health = gameObject.GetComponentInChildren<Transform>();
        maxSize = health.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.gameObject.transform.position = ship.transform.position;
    }

    public void Refresh(int health)
    {
        try{

        float aux = maxSize*(health/20f);
        transform.localScale = new Vector3(aux, transform.localScale.y, transform.localScale.z);

        }catch(MissingReferenceException e){}
    }

    public void SetShip(GameObject ship)
    {
        this.ship = ship;
    }
}
