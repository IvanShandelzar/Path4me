using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(EnemyAI enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
    }

    public override void Update()
    {
        if (enemy.PlayerInSightRange && enemy.PlayerInChaseRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
    }
}