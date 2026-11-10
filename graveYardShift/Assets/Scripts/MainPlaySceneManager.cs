using UnityEngine;

public class MainPlaySceneManager : MonoBehaviour
{
    public GameObject CounterCamera;
    public GameObject[] graveYardEtc;
    public GameObject GraveyardCamera;
    public GameObject[] counterEtc;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwitchToCounterCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SwitchToCounterCamera()
    {
        CounterCamera.SetActive(true);
        GraveyardCamera.SetActive(false);
        for (int i = 0; i < graveYardEtc.Length; i++)
        {
            graveYardEtc[i].SetActive(false);
        }
        for (int i = 0; i < counterEtc.Length; i++)
        {
            counterEtc[i].SetActive(true);
        }
    }    
    public void SwitchToGraveyardCamera()
    {
        CounterCamera.SetActive(false);
        GraveyardCamera.SetActive(true);
        for (int i = 0; i < graveYardEtc.Length; i++)
        {
            graveYardEtc[i].SetActive(true);
        }
        for (int i = 0; i < counterEtc.Length; i++)
        {
            counterEtc[i].SetActive(false);
        }
    }

    //


}
