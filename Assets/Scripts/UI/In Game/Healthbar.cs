using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour
{
    ///////////////////////////////
    ////////// VARIABLES //////////
    ///////////////////////////////
    
    // UI related components
    public Slider slider;
    public GameObject player;
    private CanvasGroup canvasGroup;

    private Vector3 offset = new Vector3(0, 0.9f, 0);
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float fadeSpeed = 0.5f;
    private float prevHealth;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        slider.maxValue = player.GetComponent<PlayerVals>().getHealth();
        slider.value = player.GetComponent<PlayerVals>().getHealth();
        prevHealth = slider.value;

        // Get or add CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Hide initially
        canvasGroup.alpha = 0f;
    }

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.transform.position + offset;

        slider.value = player.GetComponent<PlayerVals>().getHealth();

        // If health changed
        if (slider.value != prevHealth)
        {
            ShowHealthBar();
        }

        prevHealth = slider.value;
    }

    public void ShowHealthBar()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Show the health bar
        canvasGroup.alpha = 1f;

        // Start new fade
        fadeCoroutine = StartCoroutine(FadeOutHealthBar());
    }

    private IEnumerator FadeOutHealthBar()
    {

        yield return new WaitForSeconds(displayTime);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

    }
}
