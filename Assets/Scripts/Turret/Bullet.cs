using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private float _timer;
    private BulletPool _pool;

    [Inject]
    public void Construct(BulletPool pool)
    {
        _pool = pool;
    }

    private void OnEnable()
    {
        _timer = 0f;
    }

    private void OnDisable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= lifeTime)
        {
            _pool.Despawn(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(10);
        }
    }
}