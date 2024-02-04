using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour
{
    public Image currentHealthBar;
    public Image delayedHealthBar;

    public float updateSpeed = 0.5f;

    private float currentHealth = 1f;
    private float delayedHealth = 1f;

    private void Start()
    {
        currentHealthBar.fillAmount = currentHealth;
        delayedHealthBar.fillAmount = delayedHealth;
    }
    
    public void UpdateHP(float hp)
    {
        currentHealth = hp;
        currentHealthBar.fillAmount = currentHealth;
    }

    private void Update()
    {
        if (delayedHealth > currentHealth)
        {
            delayedHealth = Mathf.Lerp(delayedHealth, currentHealth, updateSpeed * Time.deltaTime);
            delayedHealthBar.fillAmount = delayedHealth;
        }
    }
}
