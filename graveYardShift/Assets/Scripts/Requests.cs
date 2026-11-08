using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Requests", menuName = "Scriptable Objects/Requests")]
public class Requests : ScriptableObject
{
    [System.Serializable]
    public enum Requester
    {
        Any,
        TypeOfCharecter,
        Charecter,
        
    }
    [Header("Message Requester")]
    public Requester requester;
    public Charecters charecterRequester;
    public MonsterTypes monsterTypeRequester;


    





    [System.Serializable]
    public enum RequestType
    {
        NoneOnlyTalk,
        MoveToExactLocation,
        MoveToHorizontalLine,
        MoveToVerticalLine,
        MoveNextToCharecter,
        MoveAwayFromCharecter,
        
        MoveToAnyLocation,
    }
    [Header("Request Type")]
    [SerializeField]
    public RequestType requestType;
    public int LineNumber;
    public Vector2Int exactLocation;
    public Charecters charecterRelatedToRequest;



    [Header("Message")]
    
    public string requestText;
}
