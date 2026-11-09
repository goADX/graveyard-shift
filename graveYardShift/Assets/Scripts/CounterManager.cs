using UnityEngine;
using System.Collections.Generic;
//using System;

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
            if (Input.GetKeyDown(KeyCode.Space))
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
            else if (request.requester == Requests.Requester.TypeOfCharecter && request.monsterTypeRequester == charecter.charecterType)
            {
                validRequests.Add(request);
            }
        }

        if (validRequests.Count > 0)
        {
            return validRequests[Random.Range(0, validRequests.Count)];
        }
        else
        {
            Debug.LogWarning("No valid requests found for the given character.");
            return null;
        }
    }

    public void charecterLeftCounter()
    {
        IsThereCharecterOnCounter = false;
    }
}
