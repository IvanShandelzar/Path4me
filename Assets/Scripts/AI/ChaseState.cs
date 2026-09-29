using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyAI enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = false;
        enemy.Agent.speed = enemy.ChaseSpeed;
    }

    public override void Update()
    {
        if (!enemy.PlayerInChaseRange)
        {
            enemy.ChangeState(new IdleState(enemy));
            return;
        }

        if (enemy.PlayerInAttackRange)
        {
            enemy.ChangeState(new AttackState(enemy));
            return;
        }

        enemy.Agent.SetDestination(enemy.Player.position);
    }
}