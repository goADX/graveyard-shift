using UnityEngine;

public class MainPlaySceneManager : MonoBehaviour
{
    public GameObject CounterCamera;
    public GameObject GraveyardCamera;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SwitchToCounterCamera()
    {
        CounterCamera.SetActive(true);
        GraveyardCamera.SetActive(false);
    }
    public void SwitchToGraveyardCamera()
    {
        CounterCamera.SetActive(false);
        GraveyardCamera.SetActive(true);
    }

    


}
