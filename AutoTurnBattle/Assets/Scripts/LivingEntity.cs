using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour
{
    public float Speed { get; protected set; }
    public int CurrentHealth { get; protected set; }
    public int CurrentAttackGauge { get; protected set; }
    public bool IsAlive { get; protected set; }
    public bool CanAttack { get; set; }
    
    protected static int InitialGauge = 100;

    protected virtual void OnEnable()
    {
        IsAlive = true;
        CurrentHealth = InitialGauge;
        CurrentAttackGauge = 0;
        CanAttack = true;
    }

    public virtual void OnDamaged(int damage)
    {
        CurrentHealth -= damage;

        if(CurrentHealth <= 0 && IsAlive)
        {
            Debug.Log($"{gameObject}사망, 게임 종료");
            Die();
        }
    }

    protected virtual void Die()
    {
        IsAlive = false;
        CurrentHealth = 0;
    }
}