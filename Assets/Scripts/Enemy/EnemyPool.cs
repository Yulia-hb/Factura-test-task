using Zenject;
using UnityEngine;

public class EnemyPool : MonoMemoryPool<Vector3, Enemy>
{
    protected override void Reinitialize(Vector3 position, Enemy enemy)
    {
        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
    }

    protected override void OnDespawned(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}