using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidHide : MonoBehaviour
{
    [Header("Tiempo")]
    [SerializeField] private float startDelay = 1f;     // Espera antes de aplicar transparencia
    [SerializeField] private float fadeDuration = 0.5f; // 0 = cambio instantáneo, >0 = desvanecido

    [Header("Interacción al finalizar")]
    [SerializeField] private bool disableInteraction = true; // Deshabilita clicks/hover al terminar

    // 🔹 Método público para ocultar un objeto que se pasa como parámetro
    public void OcultarConBoton(GameObject objeto)
    {
        StartCoroutine(ApplyTransparencyAfterDelay(objeto));
    }

    // 🔹 Corrutina que aplica transparencia al objeto indicado
    private IEnumerator ApplyTransparencyAfterDelay(GameObject objeto)
    {
        CanvasGroup canvasGroup = objeto.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = objeto.AddComponent<CanvasGroup>();

        // Espera previa
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        if (fadeDuration <= 0f)
        {
            canvasGroup.alpha = 0f;
        }
        else
        {
            float initial = canvasGroup.alpha;
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(t / fadeDuration);
                canvasGroup.alpha = Mathf.Lerp(initial, 0f, p);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

        if (disableInteraction)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    ///Colores
    
}
