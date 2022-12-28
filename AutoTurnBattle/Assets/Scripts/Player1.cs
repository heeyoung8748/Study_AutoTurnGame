using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player1 : LivingEntity
{
    [SerializeField] private Slider _hp;
    [SerializeField] private Player2 _enemy;
    [SerializeField] private Slider _attackGauge;
    [SerializeField] private TextMeshProUGUI _damagedUI;
    [SerializeField] private TextMeshProUGUI _skillCoolTimeInfoUI;

    // �ൿ������ ���� �� �ӵ��� �ݿ��ϱ� ���� ��Ÿ��
    private float _gaugeCoolTime = 1f;
    private int _initialSkillCool = 3;

    protected override void OnEnable()
    {
        base.OnEnable();
        SkillCool1 = _initialSkillCool;
        SkillCool2 = _initialSkillCool;

        // SerializedField�� ������ UI �ʱ�ȭ
        _hp.maxValue = InitialGauge;
        _hp.value = CurrentHealth;
        _attackGauge.maxValue = InitialGauge;
        _attackGauge.value = CurrentAttackGauge;
        _damagedUI.text = "";
        _skillCoolTimeInfoUI.text = $"Turn: {SkillCool1} / {SkillCool2}";

        Debug.Log($"Name: {transform.name} / ATK: {AttackPower} / DEF: {DefensivePower}");
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

            // �������� ���� á�� �� �� Ȯ��
            if (CurrentAttackGauge >= _attackGauge.maxValue)
            {
                CurrentAttackGauge = 0;
                _attackGauge.value = CurrentAttackGauge;

                if (SkillCool2 <= 0)
                {
                    Skill2();
                    SkillCool1 = Mathf.Max(0, --SkillCool1);
                }
                else if (SkillCool1 <= 0)
                {
                    Skill1();
                    SkillCool2 = Mathf.Max(0, --SkillCool2);
                }
                else
                {
                    _enemy.OnDamaged((int)AttackPower);
                    SkillCool1 = Mathf.Max(0, --SkillCool1);
                    SkillCool2 = Mathf.Max(0, --SkillCool2);
                }

                _skillCoolTimeInfoUI.text = $"Turn: {SkillCool1} / {SkillCool2}";
            }

            yield return new WaitForSeconds(_gaugeCoolTime / Speed);
        }
    }

    private void Skill1()
    {
        CurrentHealth = (int)Mathf.Min(CurrentHealth + (InitialGauge * 0.2f), _hp.maxValue);
        _hp.value = CurrentHealth;
        SkillCool1 = _initialSkillCool;
    }

    private void Skill2()
    {
        int damage = 50 + (int)Mathf.Round(DefensivePower * 0.2f);
        Debug.Log(damage);
        _enemy.OnDamaged(damage);
        SkillCool2 = _initialSkillCool;
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
        _damagedUI.text = $"{TotalDamage}";
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