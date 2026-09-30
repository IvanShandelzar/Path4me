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

        // Лёгкая тряска при самом взмахе (ещё без попадания)
        if (CameraShake.Instance != null)
         CameraShake.Instance.Shake(0.08f, 0.04f);

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, damageableLayer))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null && target.IsAlive)
            {
                target.TakeDamage(damage, gameObject);

                Debug.Log($"HitStop.Instance = {(HitStop.Instance != null ? "OK" : "NULL")}");
                Debug.Log($"CameraShake.Instance = {(CameraShake.Instance != null ? "OK" : "NULL")}");

                if (HitStop.Instance != null)
                    HitStop.Instance.Stop(Random.Range(0.06f, 0.10f), 0.05f);

                if (CameraShake.Instance != null)
                    CameraShake.Instance.Shake(
                        Random.Range(0.15f, 0.22f),
                        Random.Range(0.10f, 0.15f));

                Debug.Log($"Попадание в {hit.collider.name} на {damage} урона");
            }
        }
    }
}