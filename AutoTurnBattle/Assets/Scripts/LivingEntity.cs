using UnityEngine;
using UnityEngine.Events;

/*
 ó���� Player1, Player2 ��ũ��Ʈ�� ���� ���ָ鼭 ������µ� ���� �ϴٺ���
 ������ ����� �и��� ������ �������� �ָ����� ��ũ��Ʈ...
 */

public class LivingEntity : MonoBehaviour
{
    public float Speed { get; protected set; } // ������ �ӵ�
    public int CurrentHealth { get; protected set; } // ���� ü��
    public int CurrentAttackGauge { get; protected set; } // ������ ������ ����
    public bool IsAlive { get; protected set; } // �÷��̾� ���� ����
    public bool IsMyTurn { get; set; } // �÷��̾� ���� ���� ����
    
    protected static int InitialGauge = 100; // ������ �ִ� �ʱ�ȭ��

    protected virtual void OnEnable()
    {
        IsAlive = true;
        CurrentHealth = InitialGauge;
        CurrentAttackGauge = 0;
        IsMyTurn = true;
    }

    /// <summary>
    /// �ǰ� �� ��������ŭ HP�� �����ϰ�, ����� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="damage">�ǰ� ������</param>
    public virtual void OnDamaged(int damage)
    {
        CurrentHealth -= damage;

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