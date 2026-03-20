using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public void SetRunning(bool value)
    {
        _animator.SetBool("IsRunning", value);
    }

    public void SetAttacking(bool value)
    {
        _animator.SetBool("IsAttacking", value);
    }

    public void Die()
    {
        _animator.SetBool("IsDead", true);
    }
}
