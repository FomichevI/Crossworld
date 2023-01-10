using UnityEngine;

public class BattleHumanoid : BattleMob
{
    [SerializeField] private string _animationType; //Анимация разных стоек для гуманойдов (зависит от типа оружия)

    public override void Initialize(string Name)
    {
        base.Initialize(Name);
        _animator.SetTrigger(_animationType);
    }
}
