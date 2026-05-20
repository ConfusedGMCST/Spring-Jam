using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class charSwitch : MonoBehaviour
{
    public bool character; //General rule of thumb is that !character is earthBoy, character is airGirl.

    public float switchDbTime = 0.25f;
    public float tweenSpeed = 2f;

    private bool debounce = false;

    public AudioSource switchSound;

    public GameObject earthBoy;
    public GameObject airGirl;
    public GameObject gameCamera;
    public Image swapUI;
    private Color disabledColor = new Color32(161, 161, 161, 255);

    private float tweenProg = 0;

    IEnumerator switchDb()
    {
        swapUI.color = disabledColor;
        yield return new WaitForSeconds(switchDbTime);
        swapUI.color = Color.white;
        debounce = false;
    }

    private void FixedUpdate()
    {
        float target = character ? 1f : 0f;

        tweenProg = Mathf.MoveTowards(tweenProg, target, tweenSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        float eased = Mathf.Sin(tweenProg * Mathf.PI * 0.5f);

        Vector3 earthPos = earthBoy.transform.position;
        Vector3 airPos = airGirl.transform.position;

        Vector3 camPos = Vector3.Lerp(earthPos, airPos, eased);

        camPos.z = gameCamera.transform.position.z;

        gameCamera.transform.position = camPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !debounce)
        {
            switchSound.Play();
            character = !character;
            debounce = true;
            StartCoroutine(switchDb());
        }
    }
}