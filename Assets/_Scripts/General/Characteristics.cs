using UnityEngine;

public class Characteristics //Класс, содержащий в себе все основные характеристики моба/персонажа.
                             //Также содержит второстепенные (рассчитанные) характеристики, которые требуются для проведения боя
                             //Все формулы расчета характеристик 
{
    //основные характеристики
    private int _level; public int Level { get { return _level; } }
    private int _strength; public int Strength { get { return _strength; } }
    private int _agility; public int Agility { get { return _agility; } }
    private int _parry; public int Parry { get { return _parry; } }
    private int _rage; public int Rage { get { return _rage; } }
    private int _survivability; public int Survivability { get { return _survivability; } }
    private int _defense; public int Defense { get { return _defense; } }
    private int _penetration; public int Penetration { get { return _penetration; } }
    private int _minDamage; public int MinDamage { get { return _minDamage; } }
    private int _maxDamage; public int MaxDamage { get { return _maxDamage; } }
    //Вторичные характеристики
    private int _maxHp; public int MaxHp { get { return _maxHp; } }
    private int _totalMinDamage; public int TotalMinDamage { get { return _totalMinDamage; } }
    private int _totalMaxDamage; public int TotalMaxDamage { get { return _totalMaxDamage; } }
    private int _chanceToDodge; public int ChanceToDodge { get { return _chanceToDodge; } }
    private int _chanceToBlock; public int ChanceToBlock { get { return _chanceToBlock; } }
    private int _chanceToCrit; public int ChanceToCrit { get { return _chanceToCrit; } }
    private int _multiplierDamage; public int MultiplierDamage { get { return _multiplierDamage; } }
    private int _multiplierDefense; public int MultiplierDefense { get { return _multiplierDefense; } }

    public void SetCharacteristics(string name)
    {
        if (name == "player")
        {
            _level = PlayerDataLoader.S.GetLevel();
            EquipmentItemData[] equip = PlayerDataLoader.S.GetEquip();
            _strength = 5 * _level;
            _agility = 5 * _level;
            _parry = 5 * _level;
            _rage = 5 * _level;
            _survivability = 5 * _level;
            _defense = 5 * _level;
            _penetration = 5 * _level;
            _minDamage = 10;
            _maxDamage = 15;
            foreach(EquipmentItemData item in equip)
            {
                _strength += item.Strength;
                _agility += item.Agility;
                _parry += item.Parry;
                _rage += item.Rage;
                _survivability += item.Survivability;
                _defense += item.Defense;
                _penetration += item.Penetration;
                if (item.MinDamage != 0)
                    _minDamage += item.MinDamage;
                if (item.MaxDamage != 0)
                    _maxDamage += item.MaxDamage;

            }
            _maxHp = 100 + (_level - 1) * 5 + _survivability;
            _totalMinDamage = _minDamage + _strength / 20;
            _totalMaxDamage = _maxDamage + _strength / 20;
        }
        else //Загружаем характеристики из базы данных
        {
            _level = EnemiesDataLoader.S.GetLevel(name);
            EnemiesDataLoader.S.GetChacteristics(name, out _agility, out _parry, out _rage, out _maxHp, out _defense, out _penetration, out _totalMinDamage, out _totalMaxDamage);
        }
        SetMinorCharacteristics();
        
    }

    public void SetMinorCharacteristics()
    {
        float dependingOnLevel = (1 + _level / 50f) / (Mathf.Sqrt(_level) * Mathf.Pow(1.1f, _level));
        int middleValueForLevel = (int)(30 / dependingOnLevel * 2); //Среднее значение характеристик ловкость/парирование/ярость для того, чтобы на определенном уровне эти характеристики
        //давали 30% шанса уворота/блока/крита (на самом деле крит и блок будут давать не 30%, а 60%, но это прописывается в другой формуле)
        _chanceToDodge = (int)(_agility * dependingOnLevel * middleValueForLevel / ((middleValueForLevel + _agility) / 2)/2);
        _chanceToBlock = (int)(_parry * dependingOnLevel * middleValueForLevel / ((middleValueForLevel + _parry) / 2));
        _chanceToCrit = (int)(_rage * dependingOnLevel * middleValueForLevel / ((middleValueForLevel + _rage) / 2));
        _multiplierDamage = (int)(_penetration * Mathf.Sqrt(_level) * 0.01f);
        _multiplierDefense = (int)(_defense * Mathf.Sqrt(_level) * 0.01f);
    }
}
