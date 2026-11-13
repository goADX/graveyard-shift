using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class Grave : MonoBehaviour, IPointerClickHandler
{
    public GraveYardManager graveyardManager()
    {
        return GraveYardManager.instance;
    }
    public bool isOccupied;
    public bool isDug;
    public Vector2Int gravePosition;
    public Sprite graveSprite;
    public Sprite dugGraveSprite;
    public Charecters OccupyingCharecter;
    public GameObject coffinPrefab;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(screenPosition);

        Collider2D hit = Physics2D.OverlapPoint(worldPosition);

        if (hit != null && hit.gameObject == gameObject)
        {
            graveyardManager().HandleGraveClick(this);
        }
    }

    public void DigUpGrave()
    {
        isDug = true;
        GetComponent<SpriteRenderer>().sprite = dugGraveSprite;
        if(OccupyingCharecter != null)
        {
            SpawnCoffin();
        }
    }
    public void FillGrave()
    {
        isDug = false;
        GetComponent<SpriteRenderer>().sprite = graveSprite;
    }
    public bool OccupyGrave(Charecters charecter)
    {
        if(isDug && !isOccupied)
        {
            isOccupied = true;
            OccupyingCharecter = charecter;
            return true;
        }
        return false;
    }
    public void freeGrave()
    {
        isOccupied = false;
        OccupyingCharecter = null;
    }

     public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            graveyardManager().HandleGraveClick(this);
            
        }
    }
    public void SpawnCoffin()
    {
        GameObject coffin = Instantiate(coffinPrefab, transform.position, Quaternion.identity);
        CoffinScript coffinScript = coffin.GetComponent<CoffinScript>();
        coffinScript.charecterInside = OccupyingCharecter;
        OccupyingCharecter = null;
        coffin.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        coffinScript.graveyardManager = graveyardManager();
        isOccupied = false;
        graveyardManager().coffinsInScene.Add(coffinScript);
    }
    

    public bool AmIReady()
    {
        return !isDug;
    }


}
