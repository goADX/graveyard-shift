using UnityEngine;
using System.Collections.Generic;
//using System;
using UnityEngine.InputSystem;
using TMPro;

public class CounterManager : MonoBehaviour
{

    public TheGreatPlayManager theGreatPlayManager;
    public GameObject CharecterPrefabs;
    public Charecters[] AllCharecters;
    public Requests[] RequestsList;
    public static CounterManager instance;

    public GameObject TextBoxObject;
    public TMP_Text TextBoxScript;

    private void Awake()
    {
        instance = this;
        AllCharecters = Resources.LoadAll<Charecters>("Charecters");

        if (AllCharecters.Length == 0)
            Debug.LogWarning("No Charecters found in the folder.");


        RequestsList = Resources.LoadAll<Requests>("Messages");
        if (RequestsList.Length == 0)
            Debug.LogWarning("No Requests found in the folder.");
    }


    public List<Charecters> CharectersToSpawn = new List<Charecters>();
    public int numOfRequests = 5;


    public bool IsThereCharecterOnCounter = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(IsThereCharecterOnCounter == false)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SpawnCharecter();
            }
        }
    }


    public void PickRequest()
    {
        for (int i = 0; i < numOfRequests; i++)
        {
            CharectersToSpawn.Add(AllCharecters[Random.Range(0, AllCharecters.Length)]);            
        }
    }

    public void SpawnCharecter()
    {
        if (CharectersToSpawn.Count > 0)
        {
            
            
            //get the next in queue

            GameObject charecterInstance = Instantiate(CharecterPrefabs, transform.position, Quaternion.identity);
            CharecterInCounterScript charCounterScript = charecterInstance.GetComponent<CharecterInCounterScript>();
            charCounterScript.counterManager = this;
            charCounterScript.myCharecter = CharectersToSpawn[0];
            charCounterScript.TextBox = TextBoxObject;
            charCounterScript.dialogueText = TextBoxScript;
            charCounterScript.currentRequest = GetPossibleRequest(CharectersToSpawn[0]);

            CharectersToSpawn.RemoveAt(0);
            IsThereCharecterOnCounter = true;
        }
        else
        {
            Debug.Log("No more characters to spawn.");
        }
    }


    

    private Requests PickWeightedRequest(List<Requests> requests)
    {
        float totalWeight = 0f;

        foreach (var request in requests)
        {
            if (request.weight > 0)
                totalWeight += request.weight;
        }

        if (totalWeight <= 0f)
        {
            Debug.LogWarning("No valid requests with a positive weight.");
            return null;
        }

        float roll = Random.Range(0f, totalWeight);
        Requests lastPositiveRequest = null;

        foreach (var request in requests)
        {
            if (request.weight <= 0)
                continue;

            lastPositiveRequest = request;
            roll -= request.weight;

            if (roll <= 0f)
                return request;
        }

        return lastPositiveRequest; // Floating-point safety fallback
    }

    public void charecterLeftCounter(Requests request, Charecters charecter )
    {
        IsThereCharecterOnCounter = false;
        theGreatPlayManager.AddRequest(request, charecter);
    }

    public Requests GetPossibleRequest(Charecters charecter)
{
    List<Requests> possibleRequests = new List<Requests>();

    foreach (var request in RequestsList)
    {
        bool matches = request.requester switch
        {
            Requests.Requester.Any => true,
            Requests.Requester.Charecter => request.charecterRequester == charecter,
            Requests.Requester.TypeOfCharecter => request.monsterTypeRequester == charecter.monsterType,
            _ => false
        };

        if (matches)
            possibleRequests.Add(request);
    }

    if (possibleRequests.Count == 0)
    {
        Debug.LogWarning("No possible requests found for the given character.");
        return null;
    }

    return PickWeightedRequest(possibleRequests);
}
}
