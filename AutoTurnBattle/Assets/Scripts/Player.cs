using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : LivingEntity
{
    [SerializeField] private Slider _hp;
    [SerializeField] private Player _enemy;
    [SerializeField] private Slider _attackGauge;
    [SerializeField] private TextMeshProUGUI _damagedUI;

    // 속도 범위
    private float _minSpeed = 1f;
    private float _maxSpeed = 10f;

    // 공격 데미지 범위
    private int _minAttackDamage = 1;
    private int _maxAttackDamage = 20;

    // 행동게이지 증가 시 속도를 반영하기 위한 쿨타임
    private float _gaugeCoolTime = 1f;

    protected override void OnEnable()
    {
        base.OnEnable();

        // SerializedField로 적용한 UI 초기화
        _hp.maxValue = InitialGauge;
        _hp.value = CurrentHealth;
        _attackGauge.maxValue = InitialGauge;
        _attackGauge.value = CurrentAttackGauge;
        _damagedUI.text = "";

        Speed = Random.Range(_minSpeed, _maxSpeed) * 10;
    }

    private void Start()
    {
        StartCoroutine(IncreaseAttackGauge());
    }

    /// <summary>
    /// 행동 게이지를 증가시키는 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreaseAttackGauge()
    {
        while (IsAlive)
        {
            CurrentAttackGauge++;
            _attackGauge.value = CurrentAttackGauge;

            // 게이지가 가득 찼을 시 자신의 턴이라면 공격한다
            if (CurrentAttackGauge >= _attackGauge.maxValue)
            {
                if (IsMyTurn)
                {
                    Attack();

                    CurrentAttackGauge = 0;
                    _attackGauge.value = CurrentAttackGauge;
                    IsMyTurn = false;
                }
                else
                {
                    yield return null;
                }

            }

            yield return new WaitForSeconds(_gaugeCoolTime / Speed);
        }
    }

    /// <summary>
    /// 랜덤 데미지를 결정하고 적에게 데미지를 입히는 함수
    /// </summary>
    private void Attack()
    {
        int attackDamage = Random.Range(_minAttackDamage, _maxAttackDamage);
        _enemy.OnDamaged(attackDamage);

        // 공격이 끝난 후 턴을 넘겨준다
        _enemy.IsMyTurn = true;
    }

    /// <summary>
    /// 피격 시 데미지만큼 HP를 감소하고, 사망을 확인하는 함수
    /// </summary>
    /// <param name="damage">피격 데미지</param>
    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

        // HP와 피격 데미지를 UI에 적용한다
        _hp.value = CurrentHealth;
        _damagedUI.text = $"{damage}";
        Invoke("RemoveDamageUI", 1f);
    }

    /// <summary>
    /// 사망 시 행동 게이지 증가를 멈추는 함수
    /// </summary>
    protected override void Die()
    {
        base.Die();
        StopAllCoroutines();
        _enemy.StopAllCoroutines();
    }

    /// <summary>
    /// 화면에 띄운 데미지를 지우는 함수
    /// </summary>
    private void RemoveDamageUI()
    {
        _damagedUI.text = "";
    }
}