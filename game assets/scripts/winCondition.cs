using UnityEngine;
using UnityEngine.SceneManagement;

public class winCondition : MonoBehaviour
{
    public LayerMask airLayer;
    public LayerMask groundLayer;
    public float checkRadius = 2.25f;
    public GameObject button;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        colCheck();
        //Debug.Log(door.activeSelf);
    }

    void colCheck()
    {
        if (Physics2D.OverlapCircle(button.transform.position, checkRadius, airLayer) && Physics2D.OverlapCircle(button.transform.position, checkRadius, groundLayer))
        {
            SceneManager.LoadScene(2);
        }
    }
}
