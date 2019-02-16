using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Animator rig;
    bool open;

    public void Interact (string currentHeldItem) {

        if (open != true) {
            rig.SetTrigger ("openDoor");
            Destroy (GetComponent<BoxCollider> ());
            open = true;
        }

    }
}