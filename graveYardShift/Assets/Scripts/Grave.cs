using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class Grave : MonoBehaviour, IPointerClickHandler
{
    public GraveYardManager graveyardManager;
    public bool isOccupied;
    public bool isDug;

    public Sprite graveSprite;
    public Sprite dugGraveSprite;
    public Charecters OccupyingCharecter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DigUpGrave()
    {
        isDug = true;
        GetComponent<SpriteRenderer>().sprite = dugGraveSprite;
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
            graveyardManager.HandleGraveClick(this);
            
        }
    }

}
