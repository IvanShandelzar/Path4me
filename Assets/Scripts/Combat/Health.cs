using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private bool destroyOnDeath = false;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsAlive => currentHealth > 0;

    public event Action<GameObject> OnDamaged;
    public event Action OnDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount, GameObject source)
    {
        if (!IsAlive) return;
        if (amount <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        OnDamaged?.Invoke(source);

        if (!IsAlive)
        {
            OnDeath?.Invoke();
            if (destroyOnDeath) Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        if (!IsAlive) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}