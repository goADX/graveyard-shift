using UnityEngine;

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

                if (grave == null)
                {
                    Debug.LogError("Grave prefab has no Grave component.");
                    continue;
                }

                grave.graveyardManager = this;

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
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Grave grave = graves[row, column];
                if (grave != null && grave.isOccupied && grave.OccupyingCharecter == charecter)
                {
                    return new Vector2Int(row, column);
                }
            }
        }

        Debug.LogWarning("Character not found in any grave.");
        return new Vector2Int(-1, -1); // Not found
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


    public bool isRequestFulfilled(Requests request)
    {
        switch (request.requester)
        {
            case Requests.Requester.Charecter:
                

            default:
                Debug.LogWarning("Request type not handled.");
                return false;
        }
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
                        charecterHeld = null; // Clear the held character after placing
                        coffinHeld.DestroyCoffin();
                        coffinHeld = null;
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
        
    }
}