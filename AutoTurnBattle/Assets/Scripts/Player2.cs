using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player2 : LivingEntity
{
    [SerializeField] private Slider _hp;
    [SerializeField] private Player1 _enemy;
    [SerializeField] private Slider _attackGauge;
    [SerializeField] private TextMeshProUGUI _damagedUI;
    [SerializeField] private TextMeshProUGUI _skillCoolTimeInfoUI;

    // �ൿ������ ���� �� �ӵ��� �ݿ��ϱ� ���� ��Ÿ��
    private float _gaugeCoolTime = 1f;
    private int _initialSkill1Cool = 2;
    private int _initialSkill2Cool = 4;

    protected override void OnEnable()
    {
        base.OnEnable();
        SkillCool1 = _initialSkill1Cool;
        SkillCool2 = _initialSkill2Cool;

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
        int damage = (int)Mathf.Round(AttackPower + AttackPower * 0.3f);
        Debug.Log(damage);
        _enemy.OnDamaged(damage);
        SkillCool1 = _initialSkill1Cool;
    }

    private void Skill2()
    {
        int damage = (int)Mathf.Round(AttackPower * 0.9f);
        Debug.Log(damage);
        _enemy.OnDamaged(damage);
        _enemy.OnDamaged(damage);
        SkillCool2 = _initialSkill2Cool;
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
