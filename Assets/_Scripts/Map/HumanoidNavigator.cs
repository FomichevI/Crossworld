using UnityEngine;

public class HumanoidNavigator : MobNavigator
{
    [Header("��������� ���������")]
    [SerializeField] private string _animationType;

    public override void OnStart()
    {
        _animator.SetTrigger(_animationType); //���������� ��� �������� � ������ ������ (��� �������� ���� ������ � ����������)
        base.OnStart();
    }
}
