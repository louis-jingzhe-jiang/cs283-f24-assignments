using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Blink the heart image when health is deducted.
/// End the game when the player's health reaches 0
/// </summary>
public class HealthTracker : MonoBehaviour
{
    public Text health;
    public Image img;
    private int _healthNum;

    // Start is called before the first frame update
    void Start()
    {
        int.TryParse(health.text, out _healthNum);
    }

    // Update is called once per frame
    void Update()
    {
        int.TryParse(health.text, out int currHealth);
        if (currHealth < _healthNum) 
        {
            // blink the image
            _healthNum = currHealth;
            StartCoroutine(_Blink());
        }
        if (currHealth <= 0)
        {
            health.text = 0.ToString();
            // go to died scene
            SceneManager.LoadScene("GameOver");
        }
    }

    private IEnumerator _Blink()
    {
        img.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        img.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        img.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        img.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        img.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        img.gameObject.SetActive(true);
    }
}
