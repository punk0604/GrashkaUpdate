using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class TextBoxRotateIntro : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI textBox;

    // Array of strings for different lines
    public string[] textBlocks = new string[] {
        "Princess Flay: Ugh! Mom I’m so bored. Tell me the story about how you got your axe again!",
        "Queen Grashka: You would prefer boredom to war, my child. This peace is why I ventured for the axe in the first place…",
        "Princess Flay: Pleeeeease!!!",
        "Queen Grashka: It would serve you well to use manners, small one. As you know The Devil himself once roamed this realm like you and I, despite my warning him to stick to his own domain."
    };

    private int currentTextIndex = 0;

    // Reference to the Devil_still GameObject
    public GameObject Devil_still;

    public float typingSpeed = 0.001f; // Delay between each letter
    private bool isTyping = false;   // To prevent skipping while typing
    private Coroutine typingCoroutine; // To handle the current typing process

    void Start()
    {
        if (textBlocks.Length > 0)
        {
            StartTypingEffect();  // Start typing the first line
        }
    }

    void Update()
    {
        if (currentTextIndex > 9)
        {
            Devil_still.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isTyping) // Proceed to the next line
        {
            if (currentTextIndex < textBlocks.Length - 1)
            {
                currentTextIndex++;
                StartTypingEffect(); // Start typing the next line
            }
            else // No more text; transition to the next scene
            {
                SceneManager.LoadScene("Hell_1");
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && isTyping) // Skip the current typing effect
        {
            SkipTyping();
        }
    }

    void StartTypingEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop any previous coroutine
        }
        typingCoroutine = StartCoroutine(TypeText(textBlocks[currentTextIndex]));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textBox.text = ""; // Clear the text before typing
        foreach (char letter in text.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false; // Typing is done
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop the current typing coroutine
        }
        textBox.text = textBlocks[currentTextIndex]; // Display the full text
        isTyping = false; // Allow moving to the next line
    }
}

