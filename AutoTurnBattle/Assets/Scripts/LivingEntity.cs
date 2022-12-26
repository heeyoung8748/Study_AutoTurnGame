using UnityEngine;
using UnityEngine.Events;

/*
 처음에 Player1, Player2 스크립트를 따로 빼주면서 만들었는데 막상 하다보니
 동작이 비슷해 분리할 이유가 없어져서 애매해진 스크립트...
 */

public class LivingEntity : MonoBehaviour
{
    public float Speed { get; protected set; } // 게이지 속도
    public int CurrentHealth { get; protected set; } // 현재 체력
    public int CurrentAttackGauge { get; protected set; } // 게이지 차오른 정도
    public bool IsAlive { get; protected set; } // 플레이어 생존 여부
    public bool IsMyTurn { get; set; } // 플레이어 공격 가능 여부
    
    protected static int InitialGauge = 100; // 게이지 최댓값 초기화용

    protected virtual void OnEnable()
    {
        IsAlive = true;
        CurrentHealth = InitialGauge;
        CurrentAttackGauge = 0;
        IsMyTurn = true;
    }

    /// <summary>
    /// 피격 시 데미지만큼 HP를 감소하고, 사망을 확인하는 함수
    /// </summary>
    /// <param name="damage">피격 데미지</param>
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