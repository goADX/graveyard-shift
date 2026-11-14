using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ListUI : MonoBehaviour
{
    public Transform requestsContent;//a list of all requests that are still active
    public Transform charactersContent;//a list of all charecters who are in the graveyard and their burial locations
    public TMP_Text rowPrefab;
    public static ListUI instance;

    public void Awake()
    {
        instance = this;
    }

    public void RefreshRequests()
    {
        Clear(requestsContent);

        foreach (RequestData request in TheGreatPlayManager.instance.RequestsList)
        {
            Instantiate(rowPrefab, requestsContent).text = request.getDescription();
        }
    }

    public void RefreshCharacters()
    {
        Clear(charactersContent);

        foreach (KeyValuePair<Charecters, Vector2Int> entry in GraveYardManager.instance.charecterCurrentPositions)
        {
            Instantiate(rowPrefab, charactersContent).text =
                entry.Key.charecterName + " - X: " + entry.Value.x + ", Y: " + entry.Value.y;
        }
    }

    void Clear(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}