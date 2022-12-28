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

    // 행동게이지 증가 시 속도를 반영하기 위한 쿨타임
    private float _gaugeCoolTime = 1f;
    private int _initialSkill1Cool = 2;
    private int _initialSkill2Cool = 4;

    protected override void OnEnable()
    {
        base.OnEnable();
        SkillCool1 = _initialSkill1Cool;
        SkillCool2 = _initialSkill2Cool;

        // SerializedField로 적용한 UI 초기화
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
    /// 행동 게이지를 증가시키는 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreaseAttackGauge()
    {
        while (IsAlive)
        {
            CurrentAttackGauge++;
            _attackGauge.value = CurrentAttackGauge;

            // 게이지가 가득 찼을 시 턴 확인
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
    /// 피격 시 데미지만큼 HP를 감소하고, 사망을 확인하는 함수
    /// </summary>
    /// <param name="damage">피격 데미지</param>
    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

        // HP와 피격 데미지를 UI에 적용한다
        _hp.value = CurrentHealth;
        _damagedUI.text = $"{TotalDamage}";
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
