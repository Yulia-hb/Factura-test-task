using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletConfig _config;

    private float _timer;
    private bool _isDespawned;
    private BulletPool _pool;

    [Inject]
    public void Construct(BulletPool pool)
    {
        _pool = pool;
    }

    private void OnEnable()
    {
        _timer = 0f;
        _isDespawned = false;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _config.lifetime && !_isDespawned)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        _isDespawned = true;
        _pool.Despawn(this);
    }
    private void OnDisable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponentInParent<Health>();

        if (health != null)
        {
            health.TakeDamage(_config.damage);
        }

        if (!_isDespawned)
        {
            Despawn();
        }
        
    }
}