using UnityEngine;

[CreateAssetMenu(fileName = "New BattleData", menuName = "Battle Data", order = 51)]
public class BattleData : ScriptableObject
{
    public BattleCharacter Player;
    public BattleMob Enemy;
    public HitDirection[] KomboHits; //Комбинация персонажа
    public int FilledCombo; //Текущее заполнение комбинации
    public float PenetrationMultiplier; //Множитель урона за счет пробивания
    public TypeOfHit TypeOfCurrentHit; //Состояние текущего удара (крит, блок или уворот)    
    public Loot Loot = null; //Награда при окончании боя
}
