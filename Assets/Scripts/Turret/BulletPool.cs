using UnityEngine;
using Zenject;

public class BulletPool : MonoMemoryPool<Bullet>
{
    protected override void OnCreated(Bullet bullet)
    {       
        bullet.gameObject.SetActive(false);
    }
    protected override void OnSpawned(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected override void OnDespawned(Bullet item)
    {
        item.gameObject.SetActive(false);
    }
}
