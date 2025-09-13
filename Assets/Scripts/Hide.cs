using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{

    [Header("Tiempo")]
    [SerializeField] private float startDelay = 1f;     // Espera antes de aplicar transparencia
    [SerializeField] private float fadeDuration = 0.5f; // 0 = cambio instantáneo, >0 = desvanecido

    [Header("Interacción al finalizar")]
    [SerializeField] private bool disableInteraction = true; // Deshabilita clicks/hover al terminar

    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Asegura un CanvasGroup para controlar alpha de todo el árbol UI
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        StartCoroutine(ApplyTransparencyAfterDelay());
    }

    private IEnumerator ApplyTransparencyAfterDelay()
    {
        // Espera previa
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        // Si no hay duración, hazlo instantáneo
        if (fadeDuration <= 0f)
        {
            canvasGroup.alpha = 0f;
        }
        else
        {
            // Desvanece desde el alpha actual hasta 0
            float initial = canvasGroup.alpha;
            float t = 0f;

            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime; // usa tiempo no escalado por si hay pausa
                float p = Mathf.Clamp01(t / fadeDuration);
                canvasGroup.alpha = Mathf.Lerp(initial, 0f, p);
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }

        // Opcional: quitar interacción y raycasts cuando ya es invisible
        if (disableInteraction)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Métodos útiles por si luego quieres revertir desde código/botones:
    public void MakeVisible(float fadeInDuration = 0.5f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(1f, fadeInDuration, enableInteraction: true));
    }

    public void MakeInvisible(float delay = 0f, float fadeOutDuration = 0.5f)
    {
        StopAllCoroutines();
        startDelay = delay;
        fadeDuration = fadeOutDuration;
        OnEnable();
    }

    private IEnumerator FadeTo(float target, float duration, bool enableInteraction)
    {
        if (duration <= 0f)
        {
            canvasGroup.alpha = target;
        }
        else
        {
            float start = canvasGroup.alpha;
            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, target, t / duration);
                yield return null;
            }
            canvasGroup.alpha = target;
        }

        canvasGroup.interactable = enableInteraction;
        canvasGroup.blocksRaycasts = enableInteraction;
    }
}
