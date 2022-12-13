using UnityEngine;

public class BattleHumanoid : BattleMob
{
    [SerializeField] private string _animationType; //�������� ������ ����� ��� ���������� (������� �� ���� ������)

    public override void Initialize(string Name)
    {
        base.Initialize(Name);
        _animator.SetTrigger(_animationType);
    }
}
