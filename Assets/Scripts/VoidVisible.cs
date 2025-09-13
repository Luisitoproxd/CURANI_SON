using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidVisible : MonoBehaviour
{
[Header("Tiempo")]
    [SerializeField] private float startDelay = 1f;     // Espera antes de aplicar visibilidad
    [SerializeField] private float fadeDuration = 0.5f; // 0 = cambio instantáneo, >0 = desvanecido

    [Header("Interacción al finalizar")]
    [SerializeField] private bool enableInteraction = true; // Activa clicks/hover al terminar

    // 🔹 Método público para mostrar un objeto que se pasa como parámetro
    public void MostrarConBoton(GameObject objeto)
    {
        StartCoroutine(ApplyVisibilityAfterDelay(objeto));
    }

    // 🔹 Corrutina que aplica visibilidad al objeto indicado
    private IEnumerator ApplyVisibilityAfterDelay(GameObject objeto)
    {
        CanvasGroup canvasGroup = objeto.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = objeto.AddComponent<CanvasGroup>();

        // Espera previa
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        if (fadeDuration <= 0f)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            float initial = canvasGroup.alpha;
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(t / fadeDuration);
                canvasGroup.alpha = Mathf.Lerp(initial, 1f, p);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        if (enableInteraction)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
 }
