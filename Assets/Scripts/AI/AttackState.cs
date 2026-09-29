using UnityEngine;

public class AttackState : EnemyState
{
    private float lastAttackTime;

    public AttackState(EnemyAI enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
        lastAttackTime = 0f;
    }

    public override void Update()
    {
        if (!enemy.PlayerInAttackRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        // Смотрим на игрока
        Vector3 look = enemy.Player.position - enemy.transform.position;
        look.y = 0;
        enemy.transform.rotation = Quaternion.LookRotation(look);

        // Атака по кулдауну
        if (Time.time - lastAttackTime >= enemy.AttackCooldown)
        {
            lastAttackTime = Time.time;
            enemy.Attack();
        }
    }
}