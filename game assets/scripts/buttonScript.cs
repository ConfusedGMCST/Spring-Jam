using UnityEngine;

public class buttonScript : MonoBehaviour
{
    public LayerMask airLayer;
    public LayerMask groundLayer;
    public bool buttonType; //false is ground, true is air
    public float checkRadius = 0.75f;
    public GameObject button;
    public GameObject door;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        colCheck();
        //Debug.Log(door.activeSelf);
    }

    void colCheck() {
        bool colDetected = false;
        if (buttonType) {
            if (Physics2D.OverlapCircle(button.transform.position, checkRadius, airLayer))
            {
                colDetected = true;
                Debug.Log("Air detected");
            }
        } else
        {
            if (Physics2D.OverlapCircle(button.transform.position, checkRadius, groundLayer))
            {
                colDetected = true;
                Debug.Log("Ground detected");
            }
        }
        if (colDetected)
        {
            door.SetActive(false);
        } else
        {
            door.SetActive(true);
        }
    }
}
