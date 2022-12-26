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

    private float _minSpeed = 1f;
    private float _maxSpeed = 10f;
    private int _minAttackDamage = 1;
    private int _maxAttackDamage = 20;
    private float _gaugeCoolTime = 1f;

    protected override void OnEnable()
    {
        base.OnEnable();

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

    private IEnumerator IncreaseAttackGauge()
    {
        while (IsAlive)
        {
            CurrentAttackGauge++;
            _attackGauge.value = CurrentAttackGauge;

            if (CurrentAttackGauge >= _attackGauge.maxValue)
            {
                if (CanAttack)
                {
                    Attack();

                    CurrentAttackGauge = 0;
                    _attackGauge.value = CurrentAttackGauge;
                    CanAttack = false;
                }
                else
                {
                    yield return null;
                }

            }

            yield return new WaitForSeconds(_gaugeCoolTime / Speed);
        }
    }

    private void Attack()
    {
        int attackDamage = Random.Range(_minAttackDamage, _maxAttackDamage);
        _enemy.OnDamaged(attackDamage);
        _enemy.CanAttack = true;
    }

    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);
        _hp.value = CurrentHealth;
        _damagedUI.text = $"{damage}";
        Invoke("RemoveDamageUI", 1f);
    }

    protected override void Die()
    {
        base.Die();
        StopAllCoroutines();
        _enemy.StopAllCoroutines();
    }

    private void RemoveDamageUI()
    {
        _damagedUI.text = "";
    }
}