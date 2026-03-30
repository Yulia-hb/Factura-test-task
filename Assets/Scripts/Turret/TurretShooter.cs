using UnityEngine;
using Zenject;

public class TurretShooter : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootForce = 20f;
    [SerializeField] private float _fireRate = 0.2f;

    private BulletPool _bulletPool;

    [Inject]
    public void Construct(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    public void Tick()
    {
        Shoot();
    }

    private void Shoot()
    {
        Bullet bullet = _bulletPool.Spawn();

        bullet.transform.position = _shootPoint.position;
        bullet.transform.rotation = _shootPoint.rotation;

        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();

        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        Vector3 spread = new Vector3(
        Random.Range(-0.05f, 0.05f),
        Random.Range(-0.03f, 0.03f),
        0f
        );

        Vector3 direction = (_shootPoint.forward + spread).normalized;

        rigidbody.linearVelocity = direction * _shootForce;
    }
}
