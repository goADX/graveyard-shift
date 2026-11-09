using UnityEngine;

public class CharecterInCounterScript : MonoBehaviour
{

    public GameObject TextBox;
    public CounterManager counterManager;
    public enum DialogueState
    {
        walking,
        talking,
        waitingForInput,
        finished,
        walkingOut
    }
    public DialogueState currentDialogueState;

    public Vector3 initialPosition;
    public Vector3 targetPosition;
    public Vector3 exitPosition;

    public float walkSpeed = 2f;
    public Requests currentRequest;

    string LineToRead;
    int currentLineNumber = 0;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = initialPosition;
        currentDialogueState = DialogueState.walking;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentDialogueState == DialogueState.walking)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 movement = direction * walkSpeed * Time.deltaTime;
            if(movement.magnitude >= (targetPosition - transform.position).magnitude)
            {
                transform.position = targetPosition;
                currentDialogueState = DialogueState.talking;
                currentLineNumber = 0;
                LineToRead = currentRequest.GetRequestText(currentLineNumber);
            }
            else
            {
                transform.position += movement;
            }
        }else if(currentDialogueState == DialogueState.talking)
        {
            // Show the dialogue box and start the dialogue
            TextBox.SetActive(true);
            // Start the dialogue logic here
        }
        else if(currentDialogueState == DialogueState.waitingForInput)
        {
            // Wait for player input to continue the dialogue
            if(Input.GetKeyDown(KeyCode.Space))
            {
                currentLineNumber++;
                if(currentRequest.GetRequestText(currentLineNumber) == null)
                {
                    currentDialogueState = DialogueState.finished;
                }
                else
                {
                    LineToRead = currentRequest.GetRequestText(currentLineNumber);
                    currentDialogueState = DialogueState.talking;

                }
                // Continue the dialogue logic here
            }
        }
        else if(currentDialogueState == DialogueState.finished)
        {
            // Hide the dialogue box and reset the state
            TextBox.SetActive(false);
            currentDialogueState = DialogueState.walkingOut; 
        }else if(currentDialogueState == DialogueState.walkingOut)
        {
            // Logic for walking out of the scene
            Vector3 direction = (exitPosition - transform.position).normalized;
            Vector3 movement = direction * walkSpeed * Time.deltaTime;
            if(movement.magnitude >= (exitPosition - transform.position).magnitude)
            {
                transform.position = exitPosition;
                // Reset the character to the initial position
                transform.position = initialPosition;
                currentDialogueState = DialogueState.walking;
            }
            else
            {
                transform.position += movement;
            }

        }

    }



    public void Finished()
    {
        


        Destroy(gameObject);
    }

    void LateUpdate()
    {
        
    }

    /*
        first walk to the counter to speak
        dialogue box appears
        stream each line of dialogue
        after each line wait until player presses a key to continue
        after the dialog returns a null the dialogue ends
        then the request will be added to a list in another script


    */
}
