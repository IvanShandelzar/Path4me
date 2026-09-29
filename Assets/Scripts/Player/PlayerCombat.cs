using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackCooldown = 0.6f;
    [SerializeField] private LayerMask damageableLayer;

    [Header("References")]
    [SerializeField] private Camera playerCamera;

    private float lastAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, damageableLayer))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null && target.IsAlive)
            {
                target.TakeDamage(damage, gameObject);
                Debug.Log($"Попадание в {hit.collider.name} на {damage} урона");
            }
        }
        else
        {
            Debug.Log("Промах");
        }
    }
}