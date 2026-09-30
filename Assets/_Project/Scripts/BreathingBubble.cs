using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BreathingBubble : MonoBehaviour
{
    [Header("Configurações Visuais")]
    public RectTransform bubbleTransform;
    public Text instructionText;

    [Header("Tamanhos da Bolha")]
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Header("Tempos do Ciclo (em segundos)")]
    public float inhaleDuration = 4.0f;
    public float holdDuration = 2.0f;
    public float exhaleDuration = 4.0f;

    private bool isBreathing = false;

    void Start()
    {
        if (bubbleTransform == null)
            bubbleTransform = GetComponent<RectTransform>();

        StartBreathing();
    }

    public void StartBreathing()
    {
        if (!isBreathing)
        {
            isBreathing = true;
            StartCoroutine(BreathingCycle());
        }
    }

    public void StopBreathing()
    {
        isBreathing = false;
        StopAllCoroutines();
    }

    private IEnumerator BreathingCycle()
    {
        while (isBreathing)
        {
            if (instructionText != null) instructionText.text = "Inspire devagar...";
            yield return StartCoroutine(AnimateScale(minScale, maxScale, inhaleDuration));

            if (instructionText != null) instructionText.text = "Segure o ar...";
            yield return new WaitForSeconds(holdDuration);

            if (instructionText != null) instructionText.text = "Solte o ar...";
            yield return StartCoroutine(AnimateScale(maxScale, minScale, exhaleDuration));

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator AnimateScale(Vector3 start, Vector3 end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            bubbleTransform.localScale = Vector3.Lerp(start, end, smoothProgress);
            yield return null;
        }
        bubbleTransform.localScale = end;
    }
}