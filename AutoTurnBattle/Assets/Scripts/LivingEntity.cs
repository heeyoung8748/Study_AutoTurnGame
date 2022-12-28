using UnityEngine;
using UnityEngine.Events;

/*
 ó���� Player1, Player2 ��ũ��Ʈ�� ���� ���ָ鼭 ������µ� ���� �ϴٺ���
 ������ ����� �и��� ������ �������� �ָ����� ��ũ��Ʈ...
 */

public class LivingEntity : MonoBehaviour
{
    public float Speed { get; protected set; } // ������ �ӵ�
    public int AttackPower { get; protected set; } // ���ݷ�
    public int DefensivePower { get; protected set; } // ����
    public int CurrentHealth { get; protected set; } // ���� ü��
    public int SkillCool1 { get; protected set; }
    public int SkillCool2 { get; protected set; }
    public int CurrentAttackGauge { get; protected set; } // ������ ������ ����
    public bool IsAlive { get; protected set; } // �÷��̾� ���� ����
    public int TotalDamage { get; private set; }

    protected static int InitialGauge = 100; // ������ �ִ� �ʱ�ȭ��

    // ���ݷ�, ���� ����
    private float _minStatus = 30;
    private float _maxStatus = 50;

    // �ӵ� ����
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
    /// �ǰ� �� ��������ŭ HP�� �����ϰ�, ����� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="damage">�ǰ� ������</param>
    public virtual void OnDamaged(int damage)
    {
        TotalDamage = Mathf.Max(damage - DefensivePower, 0);
        Debug.Log($"{transform.name} �ǰݴ���! ���� ������: {TotalDamage}");
        CurrentHealth -= TotalDamage;

        if(CurrentHealth <= 0 && IsAlive)
        {
            Debug.Log($"{gameObject}���, ���� ����");
            Die();
        }
    }

    protected virtual void Die()
    {
        IsAlive = false;
        CurrentHealth = 0;
    }
}