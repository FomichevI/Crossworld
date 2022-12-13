using UnityEngine;

[CreateAssetMenu(fileName = "New BattleData", menuName = "Battle Data", order = 51)]
public class BattleData : ScriptableObject
{
    public BattleCharacter Player;
    public BattleMob Enemy;
    public HitDirection[] KomboHits; //���������� ���������
    public int FilledCombo; //������� ���������� ����������
    public float PenetrationMultiplier; //��������� ����� �� ���� ����������
    public TypeOfHit TypeOfCurrentHit; //��������� �������� ����� (����, ���� ��� ������)    
    public Loot Loot = null; //������� ��� ��������� ���
}
