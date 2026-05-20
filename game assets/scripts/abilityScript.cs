using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class abilityScript : MonoBehaviour
{
    public charSwitch charSwitchScript;
    public AudioSource platformSound;
    public int maxPlatforms = 3;
    public GameObject platformPrefab;
    public float platformDuration = 5f;
    public float platformDbTime = 0.125f;
    public GameObject earthBoy;
    public GameObject airGirl;
    private Transform playerTransform;
    public Image platformUI;
    private bool curPlr = false;
    private bool platformDebounce = false;
    private bool resettingPlatforms = false;
    private int platforms = 0;
    private Color disabledColor = new Color32(161, 161, 161, 255);

    void Start()
    {
        
    }

    void Update()
    {
        curPlr = charSwitchScript.character;
        if (!curPlr) { 
            playerTransform = earthBoy.transform;
        } else
        {
            playerTransform = airGirl.transform;
        }
        if (platforms < maxPlatforms && Input.GetKeyDown(KeyCode.E) && !platformDebounce && !curPlr) {
            newGround();
        }
        if (platformDebounce || platforms >= maxPlatforms || curPlr) {
            platformUI.color = disabledColor;
        } else
        {
            platformUI.color = Color.white;
        }

        if (platforms >= maxPlatforms && !resettingPlatforms)
        {
            resettingPlatforms = true;
            StartCoroutine(ResetPlatforms());
        }
    }

    IEnumerator RemovePlatformAfterDelay(GameObject platform) { 
        yield return new WaitForSeconds(platformDuration);
        Destroy(platform);
    }

    IEnumerator ResetPlatforms() {
        platformDebounce = true;
        yield return new WaitForSeconds(platformDuration);
        platforms = 0;
        platformDebounce = false;
        resettingPlatforms = false;

    }
    
    IEnumerator PlatformDebounce() {
        if (platforms >= maxPlatforms)
        {
            yield break;
        }
        platformDebounce = true;
        yield return new WaitForSeconds(platformDbTime);
        platformDebounce = false;
    }

    void newGround() {
        platformSound.Play();
        platforms++;
        GameObject newPlatform = Instantiate(platformPrefab, new Vector2(playerTransform.position.x + platformPrefab.transform.position.x, playerTransform.position.y + platformPrefab.transform.position.y), Quaternion.identity);
        StartCoroutine(RemovePlatformAfterDelay(newPlatform));
        StartCoroutine(PlatformDebounce());
    }
}
