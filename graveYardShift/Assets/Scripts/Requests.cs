using UnityEngine;
using UnityEditor;
using System;

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
        MoveOut
    }
    [Header("Request Type")]
    [SerializeField]
    public RequestType requestType;
    public int LineNumber;
    public Vector2Int exactLocation;
    public Charecters charecterRelatedToRequest;


    public float weight = 1f;


    [Header("you need to enable at least on stayer for the duration to work")]
    public int requestDuration = 0;//in whole days
    public bool StayWhenDone = false;
    public bool StayWhenFailed = false;

    [Header("Scores updates")]
    public double scoreToGainOnDone = 0;
    public double scoreToLoseOnFail = 0;

    [Header("Message")]
    
    public string requestText;

    #nullable enable
    public string? GetRequestText(int stringLineNumber)
    {
        string[] lines = requestText.Split(
    new[] { "\r\n", "\r", "\n" },
    StringSplitOptions.RemoveEmptyEntries
);
        return lines.Length > stringLineNumber ? lines[stringLineNumber] : null;
    }
}


public struct RequestData
{
    public Requests request;
    public Charecters PersonMakingRequest;
    public int requestDuration;//in whole days
    
    public bool StayWhenDone() {return request.StayWhenDone;}
    public bool StayWhenFailed() {return request.StayWhenFailed;}

    public RequestData(Requests request, Charecters personMakingRequest)
    {
        this.request = request;
        this.PersonMakingRequest = personMakingRequest;
        this.requestDuration = request.requestDuration;
    }


    public string getDescription()
    {
        string description = PersonMakingRequest.charecterName + "wants";

        switch (request.requestType)
        {
            case Requests.RequestType.NoneOnlyTalk:
                description += " to talk to you.";
                break;
            case Requests.RequestType.MoveToExactLocation:
                description += " to move to location " + request.exactLocation + ".";
                break;
            case Requests.RequestType.MoveToHorizontalLine:
                description += " to move to horizontal line " + request.LineNumber + ".";
                break;
            case Requests.RequestType.MoveToVerticalLine:
                description += " to move to vertical line " + request.LineNumber + ".";
                break;
            case Requests.RequestType.MoveNextToCharecter:
                description += " to move next to " + request.charecterRelatedToRequest.charecterName + ".";
                break;
            case Requests.RequestType.MoveAwayFromCharecter:
                description += " to move away from " + request.charecterRelatedToRequest.charecterName + ".";
                break;
            case Requests.RequestType.MoveToAnyLocation:
                description += " to move to change location.";
                break;
            default:
                description += " an unknown request.";
                break;
        }
        
        return description;
    }
}
