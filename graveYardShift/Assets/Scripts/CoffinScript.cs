using UnityEngine;
using UnityEngine.InputSystem;

public class CoffinScript : MonoBehaviour
{
    public Charecters charecterInside;
    public float velocity = 0.5f;
    public GraveYardManager graveyardManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(velocity > 0)
        {
            transform.position += (Vector3.up + Vector3.left*0.5f) * velocity * Time.deltaTime;
            velocity -= Time.deltaTime*4;
        }

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
            graveyardManager.HandleCoffinClick(this);
        }




    }



    public void abandonCoffin()
    {
        
    }


    public void DestroyCoffin()
    {
        Destroy(gameObject);
    }


}
