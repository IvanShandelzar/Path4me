using UnityEngine;

public class DeadState : EnemyState
{
    public DeadState(EnemyAI enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
        enemy.Agent.enabled = false;

        // Падаем на бок — визуальный признак смерти
        enemy.transform.Rotate(90f, 0f, 0f);

        // Уничтожаем через 2 секунды
        Object.Destroy(enemy.gameObject, 2f);
    }

    public override void Update() { }
}