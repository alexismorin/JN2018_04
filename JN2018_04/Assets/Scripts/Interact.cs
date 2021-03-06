﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Interact : MonoBehaviour {

    public string currentItem = "a";
    public LayerMask interactMask;
    public GameObject[] keyPrefabs;
    public GameObject[] handPrefabs;
    public string[] keyPrefabNames;
    Vector3 discardPosition;

    public void Equip (string newItem) {
        if (currentItem != "none") {
            Discard (transform.parent.position);
        }
        currentItem = newItem;
        this.GetComponentInParent<FirstPersonController> ().PlayPickupSound ();

        for (int i = 0; i < keyPrefabNames.Length; i++) {
            if (keyPrefabNames[i] == currentItem) {
                handPrefabs[i].SetActive (true);
            }
        }

    }

    public void Discard (Vector3 discPos) {

        RaycastHit hit;

        if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 3f, interactMask)) {

            print (gameObject + " was dropped");
            this.GetComponentInParent<FirstPersonController> ().PlayDropSound ();

            for (int i = 0; i < keyPrefabNames.Length; i++) {
                if (keyPrefabNames[i] == currentItem) {
                    GameObject.Instantiate (keyPrefabs[i], hit.point, Quaternion.identity);
                }
            }

            for (int i = 0; i < handPrefabs.Length; i++) {
                handPrefabs[i].SetActive (false);
                currentItem = "none";
            }

        }
    }

    public void Void () {

        for (int i = 0; i < handPrefabs.Length; i++) {
            handPrefabs[i].SetActive (false);
            currentItem = "none";
        }

    }

    void Update () {
        if (Input.GetKeyDown ("e")) {
            RaycastHit hit;

            if (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out hit, 3f, interactMask)) {

                if (hit.transform.gameObject.tag == "Untagged") {
                    if (currentItem != "none") {
                        print ("dropping");
                        Discard (hit.point);
                    }
                } else {
                    //                    print ("casting interact to item");
                    hit.transform.gameObject.SendMessage ("Interact", currentItem, SendMessageOptions.DontRequireReceiver);

                }

            }
        }
    }
}