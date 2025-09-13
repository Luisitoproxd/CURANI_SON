using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    public GameObject[] balls;
    public float transitionSpeed = 6f; // Velocidad de cambio de color (más alto = más rápido)
    public float totalDuration = 10f;  // Tiempo total del efecto
    public float startDelay = 2f;      // Tiempo de espera antes de empezar

    private Color[] colors = new Color[]
    {
        new Color32(0xE9, 0xE8, 0xE8, 0xFF), // #E9E8E8
        new Color32(0xC5, 0xC5, 0xC5, 0xFF), // #C5C5C5
        new Color32(0xAE, 0xAE, 0xAE, 0xFF), // #AEAEAE
        new Color32(0x8F, 0x8F, 0x8F, 0xFF)  // #8F8F8F
    };

    private int[] colorIndices;
    private float elapsedTime = 0f;
    private bool startTransition = false;

    void Start()
    {
        colorIndices = new int[balls.Length];
        for (int i = 0; i < balls.Length; i++)
        {
            colorIndices[i] = i % colors.Length;
            SetColor(balls[i], colors[colorIndices[i]]);
        }

        // Inicia la Coroutine de espera
        StartCoroutine(StartAfterDelay());
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        startTransition = true;
    }

    void Update()
    {
        if (!startTransition) return; // Espera antes de empezar
        if (elapsedTime >= totalDuration) return;

        elapsedTime += Time.deltaTime;

        for (int i = 0; i < balls.Length; i++)
        {
            // Color objetivo (siguiente en reversa)
            int nextIndex = (colorIndices[i] - 1 + colors.Length) % colors.Length;
            Color targetColor = colors[nextIndex];

            // Lerp rápido
            Color currentColor = GetColor(balls[i]);
            SetColor(balls[i], Color.Lerp(currentColor, targetColor, transitionSpeed * Time.deltaTime));

            // Cuando esté cerca del objetivo, actualizamos índice
            if (Vector4.Distance(GetColor(balls[i]), targetColor) < 0.01f)
            {
                colorIndices[i] = nextIndex;
                SetColor(balls[i], targetColor); // asegura exactitud
            }
        }
    }

    void SetColor(GameObject ball, Color color)
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        if (renderer != null) renderer.material.color = color;

        var image = ball.GetComponent<UnityEngine.UI.Image>();
        if (image != null) image.color = color;
    }

    Color GetColor(GameObject ball)
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        if (renderer != null) return renderer.material.color;

        var image = ball.GetComponent<UnityEngine.UI.Image>();
        if (image != null) return image.color;

        return Color.white;
    }
}


