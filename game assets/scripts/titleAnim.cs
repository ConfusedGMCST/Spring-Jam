using UnityEngine;
using UnityEngine.UI;

public class MenuIntroAnim : MonoBehaviour
{
    [Header("References")]
    public RawImage titleImage;
    public RectTransform demoButton;

    [Header("Title Fade")]
    public float titleFadeDuration = 0.8f;

    [Header("Button Intro")]
    public float buttonDelay = 0.2f;
    public float buttonFadeDuration = 0.25f;
    public float buttonBoingDuration = 0.7f;

    [Header("Boing Settings")]
    public float overshootScale = 1.25f;
    public float bounceStrength = 0.12f;
    public float bounceFrequency = 10f;

    private CanvasGroup buttonCanvasGroup;

    private Vector3 targetScale;

    private float timer;

    enum State
    {
        TitleFade,
        Wait,
        ButtonIntro,
        Done
    }

    private State state;

    void Start()
    {
        // =====================================
        // TITLE START
        // =====================================
        Color tc = titleImage.color;
        tc.a = 0f;
        titleImage.color = tc;

        // =====================================
        // BUTTON SETUP
        // =====================================

        targetScale = demoButton.localScale;

        // Add CanvasGroup automatically
        buttonCanvasGroup = demoButton.GetComponent<CanvasGroup>();

        if (buttonCanvasGroup == null)
            buttonCanvasGroup = demoButton.gameObject.AddComponent<CanvasGroup>();

        // Start invisible + tiny
        buttonCanvasGroup.alpha = 0f;
        demoButton.localScale = Vector3.zero;

        state = State.TitleFade;
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            // =====================================
            // TITLE FADE
            // =====================================
            case State.TitleFade:
                {
                    float t = Mathf.Clamp01(timer / titleFadeDuration);

                    Color c = titleImage.color;
                    c.a = Mathf.SmoothStep(0f, 1f, t);
                    titleImage.color = c;

                    if (t >= 1f)
                    {
                        timer = 0f;
                        state = State.Wait;
                    }

                    break;
                }

            // =====================================
            // DELAY
            // =====================================
            case State.Wait:
                {
                    if (timer >= buttonDelay)
                    {
                        timer = 0f;
                        state = State.ButtonIntro;
                    }

                    break;
                }

            // =====================================
            // BUTTON BOING
            // =====================================
            case State.ButtonIntro:
                {
                    float t = Mathf.Clamp01(timer / buttonBoingDuration);

                    // Fade in
                    float fadeT = Mathf.Clamp01(timer / buttonFadeDuration);
                    buttonCanvasGroup.alpha = fadeT;

                    // Elastic boing
                    float sin = Mathf.Sin(t * Mathf.PI * bounceFrequency);

                    float scale =
                        Mathf.Lerp(
                            0f,
                            overshootScale,
                            Mathf.SmoothStep(0f, 1f, t)
                        );

                    scale += sin * bounceStrength * (1f - t);

                    // Settle to final size
                    if (t > 0.85f)
                    {
                        scale = Mathf.Lerp(
                            scale,
                            1f,
                            (t - 0.85f) / 0.15f
                        );
                    }

                    demoButton.localScale = targetScale * scale;

                    if (t >= 1f)
                    {
                        demoButton.localScale = targetScale;
                        state = State.Done;
                    }

                    break;
                }
        }
    }
}