using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShipBehaviour : MonoBehaviour
{
    public GameObject Pirate;

    GameObject MainShip;

    void Start()
    {
        Instantiate(Pirate, transform.position, Quaternion.identity, transform);
    }

    void FixedUpdate()
    {
        if(MainShip != null)
        {
            gameObject.transform.position = MainShip.transform.position;
            gameObject.transform.rotation = MainShip.transform.rotation;
        }
    }

    public void SetMainShip(GameObject mainShip)
    {
        this.MainShip = mainShip;
    }
}
