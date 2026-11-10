using UnityEngine;
using System.Collections.Generic;
public class TheGreatPlayManager : MonoBehaviour
{



    public List<Requests> RequestsList = new List<Requests>();





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void AddRequest(Requests request)
    {
        RequestsList.Add(request);
    }




    public bool CheckRequest(Requests request)
    {
        //todo: check if request is done
        return true;
    }




    public void CheckAllRequests()
    {
        int i = 0;
        while( RequestsList.Count > 0)
        {
            i++;
            if(i > 100)
            {
                Debug.LogError("infinite loop in CheckAllRequests");
                break;
            }
            if(CheckRequest(RequestsList[0]))
            {
                RequestsList.RemoveAt(0);



            }else
            {
                

                RequestsList.RemoveAt(0);
            }
        }
    }
}
