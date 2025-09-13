using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transp : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 2f;  // Duración del fade-in
    [SerializeField] private float startDelay = 2f;     // Tiempo de espera antes de iniciar

    void Awake()
    {
        // Obtenemos el CanvasGroup o lo agregamos si no existe
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Empezamos totalmente transparente
        canvasGroup.alpha = 0f;
    }

    void Start()
    {
        // Iniciamos la Coroutine que espera y luego hace el fade
        StartCoroutine(FadeInWithDelay());
    }


    IEnumerator FadeInWithDelay()
    {
        // Espera inicial
        yield return new WaitForSeconds(startDelay);

        // Fade-in
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // Asegura que quede completamente visible al final
    }
}
