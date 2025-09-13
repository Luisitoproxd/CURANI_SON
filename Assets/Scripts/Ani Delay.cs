using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniDelay : MonoBehaviour
{
 public Animator animator;
    public string animationName = "Bar";
    public float delay = 2f;
    public bool playOnStart = true; // 🔹 Marca si quieres que se ejecute sola al iniciar

    private bool isPlaying = false;

    void Start()
    {
        if (playOnStart) // Si está marcado, se ejecuta sola
            StartCoroutine(PlayAfterDelay());
    }

    // 🔹 Método público para llamarlo desde un botón
    public void PlayWithDelay()
    {
        if (!isPlaying)
            StartCoroutine(PlayAfterDelay());
    }

    IEnumerator PlayAfterDelay()
    {
        isPlaying = true;
        yield return new WaitForSeconds(delay);
        animator.Play(animationName);
        isPlaying = false;
    }
}
