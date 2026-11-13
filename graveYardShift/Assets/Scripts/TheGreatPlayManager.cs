using UnityEngine;
using System.Collections.Generic;
public class TheGreatPlayManager : MonoBehaviour
{

    public bool isNightTime = false;

    [System.NonSerialized]
    public List<RequestData> RequestsList = new List<RequestData>();
    public List<Charecters> CharectersList = new List<Charecters>();

    public GameObject NightShade;
    private double Score;
    #region Score getters and setters
    public double GetScore()
    {
        return Score;
    }
    public double SetScore(double value)
    {
        Score = value;
        return Score;
    }
    public double SetScore(float value)
    {
        Score = (double)value;
        return Score;
    }

    public double AddScore(double value)
    {
        Score += value;
        return Score;
    }
    public double AddScore(float value)
    {
        Score += (double)value;
        return Score;
    }
    #endregion


    public static TheGreatPlayManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AllRequestsDone()
    {
        return RequestsList.Count == 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetDayTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRequest(Requests request, Charecters personMakingRequest)
    {
        RequestsList.Add(new RequestData(request, personMakingRequest));
    }

    public bool CheckRequest(RequestData request)
    {
        return GraveYardManager.instance.isRequestFulfilled(request);
    }

    public void CheckAllRequests()
    {
        List<int> RequestsToRemove = new List<int>();
        for(int i = 0; i<RequestsList.Count; i++)
        {
            
            if(CheckRequest(RequestsList[i]))
            {
                if(RequestDone(RequestsList[i]))
                {
                    RequestsToRemove.Add(i);
                }
               

            }else
            {
                if(RequestFailed(RequestsList[i]))
                {
                    RequestsToRemove.Add(i);             
                }

                
            }
        }
        RequestsToRemove.Reverse();
        foreach (int request in RequestsToRemove)
        {
            RequestsList.RemoveAt(request);
        }
    }


    public void newCharecterAdd(Charecters charecter)
    {
        CharectersList.Add(charecter);
    }
    public void removeCharecter(Charecters charecter)
    {
        CharectersList.Remove(charecter);
    }

    public void SetDayTime()//at day you will fullfil requests
    {
        isNightTime = false;
        NightShade.SetActive(false);
    }
    public void SetNightTime()//at night you will set at counter to talk with ghosts
    {
        isNightTime = true;
        NightShade.SetActive(true);
    }
    public void NextTimeState()
    {
        if (isNightTime)
        {

            
            SetDayTime();
        }
        else
        {
            //the day gets next when it goes from day to night
            NextDay();
            SetNightTime();
        }
    }

    public void NextDay()
    {
        CheckAllRequests();
        GraveYardManager.instance.NewDay();
        CounterManager.instance.PickRequest();
    }



    public bool RequestFailed(RequestData request)
    {
        // Handle the failed request, e.g., show a message to the player, log it, etc.




        if(request.StayWhenFailed())
        {
            request.requestDuration--;
            return request.requestDuration < 0;
        }else
        {
            return true;
        }

    }

    public bool RequestDone(RequestData request)
    {
        // Handle the completed request, e.g., show a message to the player, log it, etc.



        if (request.StayWhenDone())
        {
            request.requestDuration--;
            return request.requestDuration < 0;
        }else
        {
            return true;
        }
    }

    

}