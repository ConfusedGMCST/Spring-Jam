using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFcns : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnToMenu() {
        SceneManager.LoadScene(0);
    }

    public void demoButton()
    {
        SceneManager.LoadScene(1);
    }
}
