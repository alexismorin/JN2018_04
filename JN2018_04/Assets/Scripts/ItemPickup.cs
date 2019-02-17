using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public string keyID; // a,b ,c or d
    Interact inventoryScript;
    PrisonManager manager;

    void Start () {
        inventoryScript = GameObject.Find ("Player").transform.GetChild (0).GetComponent<Interact> ();
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
    }

    public void Interact (string heldItem) {

        if (keyID == "e") {
            manager.HideWardenDialog ();
        }

        inventoryScript.Equip (keyID);
        print (gameObject + " was picked up");
        Destroy (gameObject);
    }
}