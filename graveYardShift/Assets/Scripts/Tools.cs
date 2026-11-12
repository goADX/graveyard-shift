using UnityEngine;
using UnityEngine.InputSystem;


public class Tools : MonoBehaviour
{

    public GraveYardManager graveyardManager;

    public GraveYardManager.MouseState currentTool;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
            graveyardManager.SetMouseState(currentTool);
        }
    }

    
}
