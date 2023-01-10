using UnityEngine;

public class HumanoidNavigator : MobNavigator
{
    [Header("Настройки аниматора")]
    [SerializeField] private string _animationType;

    public override void OnStart()
    {
        _animator.SetTrigger(_animationType); //Определить тип анимации с самого начала (тип анимации есть только у гуманоидов)
        base.OnStart();
    }
}
