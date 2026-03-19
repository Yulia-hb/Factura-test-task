using UnityEngine;
using Zenject;

public class TurretShooter : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootForce = 20f;
    [SerializeField] private float _fireRate = 0.2f;
    private float _timer;

    private BulletPool _bulletPool;

    [Inject]
    public void Construct(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }

    public void Tick()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fireRate)
        {
            Shoot();
            _timer = 0f;
        }
    }

    private void Shoot()
    {
        Bullet bullet = _bulletPool.Spawn();

        bullet.transform.position = _shootPoint.position;
        bullet.transform.rotation = _shootPoint.rotation;

        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();

        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.linearVelocity = _shootPoint.forward * _shootForce;
    }
}
