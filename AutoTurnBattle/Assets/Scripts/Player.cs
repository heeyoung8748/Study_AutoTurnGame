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

    // �ӵ� ����
    private float _minSpeed = 1f;
    private float _maxSpeed = 10f;

    // ���� ������ ����
    private int _minAttackDamage = 1;
    private int _maxAttackDamage = 20;

    // �ൿ������ ���� �� �ӵ��� �ݿ��ϱ� ���� ��Ÿ��
    private float _gaugeCoolTime = 1f;

    protected override void OnEnable()
    {
        base.OnEnable();

        // SerializedField�� ������ UI �ʱ�ȭ
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
    /// �ൿ �������� ������Ű�� �Լ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreaseAttackGauge()
    {
        while (IsAlive)
        {
            CurrentAttackGauge++;
            _attackGauge.value = CurrentAttackGauge;

            // �������� ���� á�� �� �ڽ��� ���̶�� �����Ѵ�
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
    /// ���� �������� �����ϰ� ������ �������� ������ �Լ�
    /// </summary>
    private void Attack()
    {
        int attackDamage = Random.Range(_minAttackDamage, _maxAttackDamage);
        _enemy.OnDamaged(attackDamage);

        // ������ ���� �� ���� �Ѱ��ش�
        _enemy.IsMyTurn = true;
    }

    /// <summary>
    /// �ǰ� �� ��������ŭ HP�� �����ϰ�, ����� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="damage">�ǰ� ������</param>
    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

        // HP�� �ǰ� �������� UI�� �����Ѵ�
        _hp.value = CurrentHealth;
        _damagedUI.text = $"{damage}";
        Invoke("RemoveDamageUI", 1f);
    }

    /// <summary>
    /// ��� �� �ൿ ������ ������ ���ߴ� �Լ�
    /// </summary>
    protected override void Die()
    {
        base.Die();
        StopAllCoroutines();
        _enemy.StopAllCoroutines();
    }

    /// <summary>
    /// ȭ�鿡 ��� �������� ����� �Լ�
    /// </summary>
    private void RemoveDamageUI()
    {
        _damagedUI.text = "";
    }
}