using UnityEngine;
using UnityEngine.UI;

public class anim : MonoBehaviour
{
    [Header("Target")]
    public RawImage target;

    [Header("Fade In")]
    public float fadeDuration = 0.5f;

    [Header("Boing Animation")]
    public float boingDuration = 1.2f;
    public float scaleAmount = 0.4f;
    public float bounceHeight = 40f;
    public float rotationAmount = 20f;
    public float bounceFrequency = 8f;

    private RectTransform rect;

    private Vector3 startScale;
    private Vector2 startPos;
    private Quaternion startRot;

    private float timer;
    private bool boingStarted = false;

    void Start()
    {
        if (target == null)
            target = GetComponent<RawImage>();

        rect = target.rectTransform;

        startScale = rect.localScale;
        startPos = rect.anchoredPosition;
        startRot = rect.localRotation;

        // Start invisible and slightly smaller
        Color c = target.color;
        c.a = 0f;
        target.color = c;

        rect.localScale = startScale * 0.7f;
    }

    void Update()
    {
        // -------------------------
        // FADE IN
        // -------------------------
        if (!boingStarted)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / fadeDuration);

            // Fade alpha
            Color c = target.color;
            c.a = t;
            target.color = c;

            // Slight grow while fading in
            rect.localScale = Vector3.Lerp(
                startScale * 0.7f,
                startScale,
                Mathf.SmoothStep(0, 1, t)
            );

            // Start boing after fade
            if (t >= 1f)
            {
                boingStarted = true;
                timer = 0f;
            }

            return;
        }

        // -------------------------
        // BOING
        // -------------------------
        timer += Time.deltaTime;

        float bt = Mathf.Clamp01(timer / boingDuration);

        float damping = 1f - bt;

        // Bounce motion
        float bounce = Mathf.Sin(bt * Mathf.PI * bounceFrequency) * damping;

        // Main scale pulse
        float scalePulse = Mathf.Sin(bt * Mathf.PI);

        float scale =
            1f +
            (scalePulse * scaleAmount) +
            (bounce * 0.08f);

        rect.localScale = startScale * scale;

        // Vertical movement
        rect.anchoredPosition =
            startPos + Vector2.up * bounce * bounceHeight;

        // Rotation wobble
        float rot =
            Mathf.Sin(bt * Mathf.PI * 4f) *
            rotationAmount *
            damping;

        rect.localRotation = Quaternion.Euler(0, 0, rot);

        // Clean finish
        if (bt >= 1f)
        {
            rect.localScale = startScale;
            rect.anchoredPosition = startPos;
            rect.localRotation = startRot;

            enabled = false;
        }
    }
}