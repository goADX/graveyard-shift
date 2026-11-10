using UnityEngine;
using System.Collections.Generic;
//using System;
using UnityEngine.InputSystem;

public class CounterManager : MonoBehaviour
{

    public GameObject[] CharecterPrefabs;
    public Requests[] RequestsList;

    private void Awake()
    {
        CharecterPrefabs = Resources.LoadAll<GameObject>("Prefabs/Characters");

        if (CharecterPrefabs.Length == 0)
            Debug.LogWarning("No prefabs found in the folder.");


        RequestsList = Resources.LoadAll<Requests>("Messages");
        if (RequestsList.Length == 0)
            Debug.LogWarning("No Requests found in the folder.");
    }


    public List<GameObject> CharectersToSpawn = new List<GameObject>();
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
            CharectersToSpawn.Add(CharecterPrefabs[Random.Range(0, CharecterPrefabs.Length)]);            
        }
    }

    public void SpawnCharecter()
    {
        if (CharectersToSpawn.Count > 0)
        {
            GameObject charecterPrefab = CharectersToSpawn[0];
            CharectersToSpawn.RemoveAt(0);
            //get the next in queue

            GameObject charecterInstance = Instantiate(charecterPrefab, transform.position, Quaternion.identity);
            charecterInstance.GetComponent<CharecterInCounterScript>().counterManager = this;
            IsThereCharecterOnCounter = true;
        }
        else
        {
            Debug.Log("No more characters to spawn.");
        }
    }


    public Requests GetValidRequest(Charecters charecter)
    {
        List<Requests> validRequests = new List<Requests>();
        //weights = request.weight;
        foreach (var request in RequestsList)
        {
            if (request.requester == Requests.Requester.Charecter && request.charecterRequester == charecter)
            {
                validRequests.Add(request);
            }
            
        }

        if (validRequests.Count > 0)
        {
            return PickWeightedRequest(validRequests);        }
        else
        {
            Debug.LogWarning("No valid requests found for the given character.");
            return null;
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

    public void charecterLeftCounter()
    {
        IsThereCharecterOnCounter = false;
    }
}
