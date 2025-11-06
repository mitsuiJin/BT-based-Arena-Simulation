using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;

    void Start() => health = maxHealth;

    void Update()
    {
        if (healthSlider.value != health)
            healthSlider.value = health;
    }

    public void takeDamage(float damage) // ← public 으로 변경해야 외부에서 호출 가능
    {
        health -= damage;
    }
}
