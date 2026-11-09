using UnityEngine;

public class CounterManager : MonoBehaviour
{

    public GameObject CharecterPrefabs;

    private void Awake()
    {
        CharecterPrefabs = Resources.LoadAll<GameObject>("Prefabs/Characters");

        if (CharecterPrefabs.Length == 0)
            Debug.LogWarning("No prefabs found in the folder.");
    }


    public bool IsThereCharecterOnCounter = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void charecterLeftCounter()
    {
        IsThereCharecterOnCounter = false;
    }
}
