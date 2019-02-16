using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PrisonManager : MonoBehaviour {

    public GameObject[] zones;
    public GameObject[] spawnPoints;
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
    [Space (10)]
    public string pendingNewCaseCode;
    public float alarmTimer = 200f;
    public bool isTimerDecreasing;

    void Update () {

        if (alarmTimer <= 0f) {
            if (isTimerDecreasing) {
                Alarm ();
                isTimerDecreasing = false;
            }

        }

        if (isTimerDecreasing == true && alarmTimer >= 0f) {
            alarmTimer -= 1f * Time.deltaTime;
        }
    }

    void Alarm () {
        GameObject[] guards = GameObject.FindGameObjectsWithTag ("guard");
        for (int i = 0; i < guards.Length; i++) {
            guards[i].SendMessage ("Alarm", SendMessageOptions.DontRequireReceiver);
        }
    }

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

    void GetLatestCaseCode () {
        guardsCaseCode = PlayerPrefs.GetString ("guardsCaseCode", "0-1-2-3-");
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
        spawnPoints = GameObject.FindGameObjectsWithTag ("jailSpawnPoint");

        StartNewGame ();
    }

    void StartNewGame () {

        openExits[0] = "a";
        openExits[1] = "b";
        openExits[2] = "c";
        openExits[3] = "d";

        itemCaseCode = "";
        guardsCaseCode = "";
        pendingNewCaseCode = "";

        currentPlayer = 1;
        alarmTimer = 200f;
        //    RandomizeCaseCode (); // we should replace this with an actual fetch
        GetLatestCaseCode ();

        DecryptItemCaseCode ();
        DecryptGuardsCaseCode ();

        SpawnGuards ();
        SpawnItems ();

        MovePlayer ();
    }

    void StartNewEscape () {

        GetLatestCaseCode ();
        pendingNewCaseCode = "";

        currentPlayer++;
        alarmTimer = 200f - (currentPlayer * 25f);
        //    RandomizeCaseCode (); // we should replace this with an actual fetch

        DecryptItemCaseCode ();
        DecryptGuardsCaseCode ();

        SpawnGuards ();
        //   SpawnItems ();

        MovePlayer ();
    }

    public void MovePlayer () {
        GameObject playerObject = GameObject.Find ("Player");
        playerObject.GetComponent<FirstPersonController> ().enabled = false;
        playerObject.transform.GetChild (0).GetComponent<Interact> ().Void ();
        playerObject.transform.position = spawnPoints[Random.Range (0, spawnPoints.Length)].transform.position;
        Invoke ("GivePlayerControl", 2f);
    }

    public void GivePlayerControl () {
        GameObject playerObject = GameObject.Find ("Player");
        playerObject.GetComponent<FirstPersonController> ().enabled = true;
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

    public void AddToCurrentCaseCode (int zoneCode) {
        pendingNewCaseCode += zoneCode.ToString () + "-";
    }

    public void EndEscape () {

        isTimerDecreasing = false;
        PlayerPrefs.SetFloat ("PlayerTime" + currentPlayer.ToString (), alarmTimer);

        if (currentPlayer == 4) {
            // end game
        } else {
            PlayerPrefs.SetString ("guardsCaseCode", pendingNewCaseCode);
            StartNewEscape ();
        }
    }

}