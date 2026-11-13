using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class GraveYardManager : MonoBehaviour
{
    public GameObject gravePrefab;
    public int rows = 5;
    public int columns = 5;
    public Vector3 initialPosition;

    public float graveSpacingX = 1f;
    public float graveSpacingY = 1f;

    [System.NonSerialized]
    public GameObject[,] gravesObjects;
    [System.NonSerialized]
    public Grave[,] graves;
    public GameObject graveParentObject;
    public GameObject poofPrefab;

    public Vector3 coffinInitSpawnPositions;
    public Vector3 coffinSpawnSpacing;
    public int coffinSpawnNumber = 0;

    public List<CoffinScript> coffinsInScene = new List<CoffinScript>();
    [NonSerialized]
    public Dictionary<Charecters, Vector2Int> charecterYesterdayPositions = new Dictionary<Charecters, Vector2Int>();
    [NonSerialized]
    public Dictionary<Charecters, Vector2Int> charecterCurrentPositions = new Dictionary<Charecters, Vector2Int>();

    public static GraveYardManager instance;
    public GameObject coffinPrefab;
    

    public enum MouseState
    {
        None,
        Shovel,
        dirtBag,
        Character
        
    }
    public MouseState currentMouseState = MouseState.None;
    public Charecters charecterHeld;

    public CoffinScript coffinHeld;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GenerateGraveyard();
    }

    public void GenerateGraveyard()
    {
        if (gravePrefab == null || rows <= 0 || columns <= 0)
        {
            Debug.LogWarning("Graveyard settings are invalid.");
            return;
        }

        gravesObjects = new GameObject[rows, columns];
        graves = new Grave[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 spawnPosition = initialPosition + new Vector3(
                    column * graveSpacingX,
                    -row * graveSpacingY, // Rows go downward
                    0f
                );

                GameObject graveObject = Instantiate(
                    gravePrefab,
                    spawnPosition,
                    Quaternion.identity,
                    transform
                );
                graveObject.transform.SetParent(graveParentObject.transform);

                graveObject.name = $"Grave_{row}_{column}";
                gravesObjects[row, column] = graveObject;

                Grave grave = graveObject.GetComponent<Grave>();
                grave.gravePosition = new Vector2Int(row, column);

                if (grave == null)
                {
                    Debug.LogError("Grave prefab has no Grave component.");
                    continue;
                }
                graves[row, column] = grave;
            }
        }
    }

    public Charecters GetCharecterInGrave(int row, int column)
    {
        if (row < 0 || row >= rows || column < 0 || column >= columns)
        {
            Debug.LogWarning("Invalid grave coordinates.");
            return null;
        }

        Grave grave = graves[row, column];
        if (grave != null && grave.isOccupied)
        {
            return grave.OccupyingCharecter;
        }

        return null;
    }

    public Vector2Int GetMeCharecterPosition(Charecters charecter)
    {
        return charecterCurrentPositions.ContainsKey(charecter) ? charecterCurrentPositions[charecter] : new Vector2Int(-1, -1);
    }


    public bool isGraveOccupied(int row, int column)
    {
        if (row < 0 || row >= rows || column < 0 || column >= columns)
        {
            Debug.LogWarning("Invalid grave coordinates.");
            return false;
        }

        Grave grave = graves[row, column];
        return grave != null && grave.isOccupied;
    }


    public void HandleGraveClick(Grave grave)
    {
        switch (currentMouseState)
        {
            case MouseState.Shovel:
                grave.DigUpGrave();
                break;
            case MouseState.dirtBag:
                grave.FillGrave();
                break;
            case MouseState.Character:
                if (charecterHeld != null)
                {
                    if (grave.OccupyGrave(charecterHeld))
                    {
                        
                        coffinsInScene.Remove(coffinHeld);
                        charecterCurrentPositions[charecterHeld] = grave.gravePosition; // Update the character's current position
                        charecterHeld = null; // Clear the held character after placing
                        coffinHeld.DestroyCoffin();
                        coffinHeld = null;
                        //spawn poof
                        GameObject poof = Instantiate(poofPrefab, grave.transform.position, Quaternion.identity);
                        poof.transform.position = new Vector3(grave.transform.position.x, grave.transform.position.y, grave.transform.position.z - 0.5f);
                    }
                }
                break;
            default:
                break;
        }
    }


    public bool HandleCoffinClick(CoffinScript coffin)
    {
        if(coffinHeld != null)
        {
            coffinHeld.abandonCoffin();
        }
        coffinHeld = coffin;
        charecterHeld = coffin.charecterInside;
        currentMouseState = MouseState.Character;
        return true;
    }

    public void SetMouseState(MouseState newState)
    {
        if(coffinHeld != null)
        {
            coffinHeld.abandonCoffin();
            coffinHeld = null;
            charecterHeld = null;
        }
        currentMouseState = newState;
    }

    private void OnDrawGizmos() {
        //draw coffin grid
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 spawnPosition = initialPosition + new Vector3(
                    column * graveSpacingX,
                    -row * graveSpacingY, // Rows go downward
                    0f
                );
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(spawnPosition, new Vector3(1, 1, 0));
            }
        }

    }
    public void NewDay()
    {
        coffinSpawnNumber = 0;
    }



    public void SpawnNewCoffin(Charecters charecter)
    {
        GameObject coffin = Instantiate(coffinPrefab, transform.position, Quaternion.identity);
        coffin.transform.position = coffinInitSpawnPositions + new Vector3(coffinSpawnNumber * coffinSpawnSpacing.x, coffinSpawnNumber * coffinSpawnSpacing.y, 0);
        coffinSpawnNumber++;
        CoffinScript coffinScript = coffin.GetComponent<CoffinScript>();
        coffinScript.charecterInside = charecter;
        coffinScript.graveyardManager = this;
        coffinsInScene.Add(coffinScript);
    }

    public enum NextDayError
    {
        NoError,
        graveOpen,
        CoffinOut,
    }
    public NextDayError CanGoToNextDay()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Grave grave = graves[row, column];
                if(grave != null && grave.isDug)
                {
                    return NextDayError.graveOpen;
                }
            }
        }
        if(coffinsInScene.Count > 0)
        {
            return NextDayError.CoffinOut;
        }
        return NextDayError.NoError;
    }

    public void SaveCharecterPositions()
    {
        charecterYesterdayPositions.Clear();
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Grave grave = graves[row, column];
                if (grave != null && grave.isOccupied)
                {
                    charecterYesterdayPositions[grave.OccupyingCharecter] = new Vector2Int(row, column);
                }
            }
        }
    }

    public void GoToNextDay()
    {
        SaveCharecterPositions();
        charecterCurrentPositions = charecterYesterdayPositions.ToDictionary(entry => entry.Key, entry => entry.Value);
    }


    //Requests Handlers
    public bool isRequestFulfilled(RequestData request)
    {
        switch (request.request.requestType)
        {   
            case Requests.RequestType.NoneOnlyTalk:
                return true; // No specific action required
            case Requests.RequestType.MoveNextToCharecter:
                return areTwoCharectersNextTwoEachOther(request.PersonMakingRequest, request.request.charecterRelatedToRequest);
            case Requests.RequestType.MoveAwayFromCharecter:
                return areTwoCharecterNotNextTwoEachOther(request.PersonMakingRequest, request.request.charecterRelatedToRequest);
            case Requests.RequestType.MoveToVerticalLine:
                return isCharecterInVerticalLine(request.PersonMakingRequest, request.request.LineNumber);
            case Requests.RequestType.MoveToHorizontalLine:
                return isCharecterInHorizontalLine(request.PersonMakingRequest, request.request.LineNumber);
            case Requests.RequestType.MoveToExactLocation:
                return isAtExactLocation(request.PersonMakingRequest, request.request.exactLocation);
            case Requests.RequestType.MoveToAnyLocation:
                return charecterYesterdayPositions.ContainsKey(request.PersonMakingRequest)? GetMeCharecterPosition(request.PersonMakingRequest) != charecterYesterdayPositions[request.PersonMakingRequest]:false ;
            default:
                Debug.LogWarning("Request type not handled.");
                return false;
        }
    }
    #region Request Type Checkers
    public bool areTwoCharectersNextTwoEachOther(Charecters charecter1, Charecters charecter2)
    {
        Vector2Int pos1 = GetMeCharecterPosition(charecter1);
        Vector2Int pos2 = GetMeCharecterPosition(charecter2);

        if (pos1.x == -1 || pos2.x == -1)
        {
            Debug.LogWarning("One or both characters not found in any grave.");
            return false;
        }

        // Check if they are adjacent (next to each other)
        return (Mathf.Abs(pos1.x - pos2.x) == 1 && pos1.y == pos2.y) || (Mathf.Abs(pos1.y - pos2.y) == 1 && pos1.x == pos2.x);
    }

    public bool areTwoCharecterNotNextTwoEachOther(Charecters charecter1, Charecters charecter2)
    {
        return !areTwoCharectersNextTwoEachOther(charecter1, charecter2);
    }

    public bool isCharecterInVerticalLine(Charecters charecter1, int column)
    {
        Vector2Int pos1 = GetMeCharecterPosition(charecter1);

        if (pos1.x == -1)
        {
            Debug.LogWarning("Character not found in any grave.");
            return false;
        }

        // Check if the character is in the specified column
        return pos1.y == column;
    }
    public bool isCharecterInHorizontalLine(Charecters charecter1, int row)
    {
        Vector2Int pos1 = GetMeCharecterPosition(charecter1);

        if (pos1.x == -1)
        {
            Debug.LogWarning("Character not found in any grave.");
            return false;
        }

        // Check if the character is in the specified row
        return pos1.x == row;
    }
    public bool isAtExactLocation(Charecters charecter1, Vector2Int location)
    {
        Vector2Int pos1 = GetMeCharecterPosition(charecter1);

        if (pos1.x == -1)
        {
            Debug.LogWarning("Character not found in any grave.");
            return false;
        }

        // Check if the character is at the specified location
        return pos1 == location;
    }
    #endregion 
}