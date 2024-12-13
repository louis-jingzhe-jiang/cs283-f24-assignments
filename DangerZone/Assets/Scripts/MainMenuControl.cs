using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public GameObject tipsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Methods to handle buttons
    public void OnStartButtonPressed()
    {
        // Load the main scene
        SceneManager.LoadScene("Main");
    }

    public void OnTipsButtonPressed()
    {
        // make the tips panel active
        tipsPanel.SetActive(true);
    }

    public void OnCloseButtonPressed()
    {
        // make the tips panel inactive
        tipsPanel.SetActive(false);
    }
}
