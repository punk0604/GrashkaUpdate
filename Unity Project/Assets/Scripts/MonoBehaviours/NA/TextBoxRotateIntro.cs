using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TextBoxRotateIntro : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public string[] textBlocks = new string[] {
        "Princess Flay: Ugh! Mom I’m so bored. Tell me the story about how you got your axe again!",
        "Queen Grashka: You would prefer boredom to war, my child. This peace is why I ventured for the axe in the first place…",
        "Princess Flay: Pleeeeease!!!",
        "Queen Grashka: It would serve you well to use manners, small one. As you know The Devil himself once roamed this realm like you and I, despite my warning him to stick to his own domain."
    };

    private int currentTextIndex = 0;
    public GameObject Devil_still;
    public float typingSpeed = 0.001f;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    
    public Image fadePanel; // UI Panel for fading
    public float fadeDuration = 1.5f;

    void Start()
    {
        fadePanel.gameObject.SetActive(false);
        if (textBlocks.Length > 0)
        {
            StartTypingEffect();
        }
    }

    void Update()
    {
        if (currentTextIndex > 9)
        {
            Devil_still.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            if (currentTextIndex < textBlocks.Length - 1)
            {
                currentTextIndex++;
                StartTypingEffect();
            }
            else
            {
                StartCoroutine(FadeToBlack());
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && isTyping)
        {
            SkipTyping();
        }
    }

    void StartTypingEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(textBlocks[currentTextIndex]));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textBox.text = "";
        foreach (char letter in text.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textBox.text = textBlocks[currentTextIndex];
        isTyping = false;
    }

    IEnumerator FadeToBlack()
    {
        fadePanel.gameObject.SetActive(true);

        float elapsedTime = 0;
        Color panelColor = fadePanel.color;
        
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(2); // Wait before loading next scene
        SceneManager.LoadScene("Hell_1");
    }
}

