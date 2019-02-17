using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class TutorialCellDoor : MonoBehaviour {

    PrisonManager manager;
    Interact inventory;
    public string exitType = "e";
    public GameObject destroyTarget;
    public GameObject player;

    void Start () {
        manager = GameObject.Find ("GameStateManager").GetComponent<PrisonManager> ();
        inventory = GameObject.Find ("FirstPersonCharacter").GetComponent<Interact> ();
        player = GameObject.Find("Player");
    }

    public void Interact (string interactedItem) {
        if (interactedItem == exitType) {
            player.GetComponent<FirstPersonController>().PlayOpenCellSound();
            inventory.Void ();
            Destroy (gameObject);
        }
    }

}