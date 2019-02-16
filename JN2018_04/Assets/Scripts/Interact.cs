using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public string currentItem = "a";
    public LayerMask interactMask;

    void Update () {
        if (Input.GetKeyDown ("e")) {
            RaycastHit hit;

            if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 3f, interactMask)) {
                hit.transform.gameObject.SendMessage ("Interact", currentItem, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}