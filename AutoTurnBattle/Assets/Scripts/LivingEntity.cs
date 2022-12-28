using UnityEngine;
using UnityEngine.Events;

/*
 처음에 Player1, Player2 스크립트를 따로 빼주면서 만들었는데 막상 하다보니
 동작이 비슷해 분리할 이유가 없어져서 애매해진 스크립트...
 */

public class LivingEntity : MonoBehaviour
{
    public float Speed { get; protected set; } // 게이지 속도
    public int AttackPower { get; protected set; } // 공격력
    public int DefensivePower { get; protected set; } // 방어력
    public int CurrentHealth { get; protected set; } // 현재 체력
    public int SkillCool1 { get; protected set; }
    public int SkillCool2 { get; protected set; }
    public int CurrentAttackGauge { get; protected set; } // 게이지 차오른 정도
    public bool IsAlive { get; protected set; } // 플레이어 생존 여부
    public int TotalDamage { get; private set; }

    protected static int InitialGauge = 100; // 게이지 최댓값 초기화용

    // 공격력, 방어력 범위
    private float _minStatus = 30;
    private float _maxStatus = 50;

    // 속도 범위
    private float _minSpeed = 1f;
    private float _maxSpeed = 10f;

    protected virtual void OnEnable()
    {
        IsAlive = true;
        CurrentHealth = InitialGauge;
        CurrentAttackGauge = 0;
        AttackPower = (int)Random.Range(_minStatus, _maxStatus);
        DefensivePower = (int)Random.Range(_minStatus, _maxStatus) - 10;
        Speed = Random.Range(_minSpeed, _maxSpeed) * 10;
    }

    /// <summary>
    /// 피격 시 데미지만큼 HP를 감소하고, 사망을 확인하는 함수
    /// </summary>
    /// <param name="damage">피격 데미지</param>
    public virtual void OnDamaged(int damage)
    {
        TotalDamage = Mathf.Max(damage - DefensivePower, 0);
        Debug.Log($"{transform.name} 피격당함! 받은 데미지: {TotalDamage}");
        CurrentHealth -= TotalDamage;

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