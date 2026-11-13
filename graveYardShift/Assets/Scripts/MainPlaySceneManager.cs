using UnityEngine;

public class MainPlaySceneManager : MonoBehaviour
{
    public GameObject CounterCamera;
    public GameObject[] counterEtc;
    public GameObject NextButton;
    public GameObject GraveyardCamera;
    public GameObject[] graveYardEtc;

    [System.NonSerialized]

    public bool isAtCounterCamera = true;    

    public static MainPlaySceneManager instance;
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
        isAtCounterCamera = true;
        GraveyardCamera.SetActive(false);
        CounterCamera.SetActive(true);
        TryToShowNextButton();
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
        isAtCounterCamera = false;
        CounterCamera.SetActive(false);
        GraveyardCamera.SetActive(true);
        NextButton.SetActive(false);
        for (int i = 0; i < graveYardEtc.Length; i++)
        {
            graveYardEtc[i].SetActive(true);
        }
        for (int i = 0; i < counterEtc.Length; i++)
        {
            counterEtc[i].SetActive(false);
        }
    }

    public void TryToShowNextButton()
    {
        if(GraveYardManager.instance.CanGoToNextDay()==0 && isAtCounterCamera)
        {
            NextButton.SetActive(true);
        }

    }

    

    //


}
