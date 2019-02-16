using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public string keyID; // a,b ,c or d
    Interact inventoryScript;

    void Start () {
        inventoryScript = GameObject.Find ("Player").transform.GetChild (0).GetComponent<Interact> ();
    }

    public void Interact (string heldItem) {

        inventoryScript.Equip (keyID);
        Destroy (gameObject);
    }
}