using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;  // TextMeshPro text component
    public string[] dialogueLines; // Array of dialogue lines
    public float textSpeed = 0.05f; // Speed of text appearing

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool dialogueActive = false;

    public GameObject player; // Reference to the player GameObject
    private MovementController playerMovement; // Reference to movement script

    void Start()
    {
        // Automatically find the player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); 
        }

        // Get movement script if player exists
        if (player != null)
        {
            playerMovement = player.GetComponent<MovementController>();
        }

        StartDialogue(); // Automatically start dialogue
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        Time.timeScale = 0f; // Pause game time
        if (playerMovement != null)
        {
            playerMovement.isFrozen = true; // Freeze movement
        }

        currentLineIndex = 0;
        dialogueText.text = "";
        gameObject.SetActive(true);
        DisplayNextLine();
    }

    void DisplayNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogueLines[currentLineIndex]));
            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed); // Use unscaled time so it works when paused
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueActive = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume game time

        if (playerMovement != null)
        {
            playerMovement.isFrozen = false; // Unfreeze movement
        }
    }
}


