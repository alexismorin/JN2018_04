using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrisonManager : MonoBehaviour {

    public GameObject[] zones;
    public GameObject[] keysPrefabs; // a b c d
    [Space (10)]
    string[] separatingChars = { "-" };
    public string itemCaseCode;
    public string guardsCaseCode;
    public string[] itemCaseCodeDecrypted;
    public string[] guardsCaseCodeDecrypted;
    [Space (10)]
    public int currentPlayer = 0;
    [Space (10)]
    public string[] openExits;

    void RandomizeCaseCode () { // Debug function that gets a random case code on new game - eventually we keep the last one

        GameObject[] debugZones = GameObject.FindGameObjectsWithTag ("zone");

        foreach (GameObject zone in debugZones) {
            guardsCaseCode += (zone.GetComponent<Zone> ().zoneID + "-");
        }

        itemCaseCode = guardsCaseCode;

        string[] tempDecryptedItemCaseCode = itemCaseCode.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
        string[] tempScrambledItemCaseCode = new string[tempDecryptedItemCaseCode.Length];

        for (int i = 0; i < tempDecryptedItemCaseCode.Length; i++) {
            tempScrambledItemCaseCode[i] = (zones.Length - int.Parse (tempDecryptedItemCaseCode[i]) - 1).ToString ();
        }

        itemCaseCode = "";

        foreach (string character in tempScrambledItemCaseCode) {
            itemCaseCode += (character + "-");
        }

        PlayerPrefs.SetString ("itemCaseCode", itemCaseCode);
        PlayerPrefs.SetString ("guardsCaseCode", itemCaseCode);

    }

    void DecryptItemCaseCode () { // turns string into data we can use for spawners
        itemCaseCodeDecrypted = itemCaseCode.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
    }

    void DecryptGuardsCaseCode () {
        guardsCaseCodeDecrypted = guardsCaseCode.Split (separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
    }

    void Start () {
        zones = GameObject.FindGameObjectsWithTag ("zone");

        StartNewGame ();
    }

    void StartNewGame () {

        openExits[0] = "a";
        openExits[1] = "b";
        openExits[2] = "c";
        openExits[3] = "d";

        itemCaseCode = "";
        guardsCaseCode = "";

        currentPlayer = 1;

        RandomizeCaseCode (); // we should replace this with an actual fetch

        DecryptItemCaseCode ();
        DecryptGuardsCaseCode ();

        SpawnGuards ();
        SpawnItems ();
    }

    public void SpawnItems () { // forgive me justin

        for (int i = 0; i < keysPrefabs.Length; i++) {
            GameObject[] keySpawnLocations = GameObject.FindGameObjectsWithTag ("keySpawnLocation");
            int dice = Random.Range (0, keySpawnLocations.Length);
            keySpawnLocations[dice].transform.parent.GetComponent<Zone> ().SpawnItemInZone (keysPrefabs[i]);
            keySpawnLocations[dice].tag = "keySpawnLocationSpent";
        }

    }

    public void SpawnGuards () {
        foreach (GameObject currentZone in zones) {
            currentZone.GetComponent<Zone> ().ResetGuards ();

            for (int i = 0; i < guardsCaseCodeDecrypted.Length; i++) {
                if (int.Parse (guardsCaseCodeDecrypted[i]) == currentZone.GetComponent<Zone> ().zoneID && currentZone.GetComponent<Zone> ().zoneID % 2 == 0) {
                    currentZone.GetComponent<Zone> ().EnableGuards ();
                }
            }

        }
    }

    public void EndEscape () {
        if (currentPlayer == 4) {
            // end game
        }
    }

}