using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventWitBool : UnityEvent<bool> { }
public class EventWithIntBool : UnityEvent<int, bool> { }
public class EventWithHitDirection : UnityEvent<HitDirection> { }
public enum TypeOfHit { simple, miss, blocked, critical, criticalBlocked }

public class BattleController : MonoBehaviour
{
    public static BattleController S;
    public static EventWithIntBool CompleteHitEvent = new EventWithIntBool();
    public static UnityEvent ShowHitButtonsEvent = new UnityEvent();
    public static EventWitBool FinishFightEvent = new EventWitBool();
    public static EventWithHitDirection MakeHitPlayer = new EventWithHitDirection();
    private BattleData _battleData;
    [SerializeField] private Transform _playerTrans;
    [SerializeField] private Transform _enemyTrans;
    [Header("Настройки боя")]
    [SerializeField] private float _delayBetweenTurns = 1f;
    private bool _isPlayerTurn = true;
    private bool _isFighting = true;

    private void OnEnable()
    {
        CompleteHitEvent.AddListener(DealDamage);
    }

    private void Awake()
    {
        if (S == null)
            S = this;
        _battleData = Resources.Load<BattleData>("ScriptableObjects/BattleData");
        //Создаем и инициализируем персонажа
        GameObject playerGo = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/BattlePlayer"), _playerTrans.position, _playerTrans.rotation);
        _battleData.Player = playerGo.GetComponent<BattleCharacter>();
        _battleData.KomboHits = _battleData.Player.KomboDirection;
        _battleData.FilledCombo = 0;
        _battleData.Player.Initialize("player");
        //Создаем и инициализируем противника
        string enemyName = PlayerDataLoader.S.GetCurrentEnemy();
        GameObject enemyGo = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Enemies/Battle/" + enemyName), _enemyTrans.position, _enemyTrans.rotation);
        _battleData.Enemy = enemyGo.GetComponent<BattleMob>();
        _battleData.Enemy.Initialize(enemyName);
    }

    private IEnumerator StartNewTurn()
    {
        yield return new WaitForSeconds(_delayBetweenTurns);
        if (!_isFighting) yield break;
        if (_isPlayerTurn) //Если ход игрока, то активируем все, что может использовать игрок в свой ход
        {
            ShowHitButtonsEvent.Invoke();
        }
        else //Если ход противника, то просто наносим удал (в самом классе противника будет возможность использовать способность, в зависимости от условий)
        {
            HitDirection dir = (HitDirection)Random.Range(0, System.Enum.GetValues(typeof(HitDirection)).Length);
            _battleData.Enemy.MakeSimpleHit(dir);
        }
    }

    public void FinishFight(bool isWin)
    {
        _isFighting = false;
        if (isWin)
        {
            LootRamdomizer lootRamdomizer = new LootRamdomizer();
            EquipmentItemData[] items = lootRamdomizer.GetLoot(_battleData.Enemy.Name);
            float gold = EnemiesDataLoader.S.GetDropGold(_battleData.Enemy.Name);
            int exp = EnemiesDataLoader.S.GetExperience(_battleData.Enemy.Name);
            _battleData.Loot = new Loot(items, exp, gold);
            if (items != null)
                foreach (EquipmentItemData item in items)
                    InventoryDataSaver.S.AddEquipmentItem(item);
            PlayerDataSaver.S.AddExperience(exp);
        }
        FinishFightEvent.Invoke(isWin);
    }

    public void CheckHit(int damage, bool forPlayer)
    {
        int chanceToDodge;
        int chanceToBlock;
        int chanceToCrit;
        BattleMob mob;
        if (forPlayer)
        {
            chanceToDodge = _battleData.Player.Characteristics.ChanceToDodge;
            chanceToBlock = _battleData.Player.Characteristics.ChanceToBlock;
            chanceToCrit = _battleData.Enemy.Characteristics.ChanceToCrit;
            mob = _battleData.Player;
            //Считаем мултипликатор пробивания
            _battleData.PenetrationMultiplier = 1 + _battleData.Player.Characteristics.MultiplierDamage / 100f - _battleData.Enemy.Characteristics.MultiplierDefense / 100f;
        }
        else
        {
            chanceToDodge = _battleData.Enemy.Characteristics.ChanceToDodge;
            chanceToBlock = _battleData.Enemy.Characteristics.ChanceToBlock;
            chanceToCrit = _battleData.Player.Characteristics.ChanceToCrit;
            mob = _battleData.Enemy;
            //Считаем мултипликатор пробивания
            _battleData.PenetrationMultiplier = 1 + _battleData.Enemy.Characteristics.MultiplierDamage / 100f - _battleData.Player.Characteristics.MultiplierDefense / 100f;
        }
        //Проверка на попадание
        int random = Random.Range(0, 100);
        if (random < chanceToDodge)
        {
            _battleData.TypeOfCurrentHit = TypeOfHit.miss;
            mob.DodgeDamage();
        }

        else if (random >= chanceToDodge && random < (chanceToDodge + chanceToBlock))
        {
            _battleData.TypeOfCurrentHit = TypeOfHit.blocked;
            mob.BlockDamage();
        }
        else
        {
            _battleData.TypeOfCurrentHit = TypeOfHit.simple;
        }
        //Проверка на крит
        random = Random.Range(0, 100);
        if (random < chanceToCrit)
        {
            if (_battleData.TypeOfCurrentHit == TypeOfHit.simple) _battleData.TypeOfCurrentHit = TypeOfHit.critical;
            else if (_battleData.TypeOfCurrentHit == TypeOfHit.blocked) _battleData.TypeOfCurrentHit = TypeOfHit.criticalBlocked;
        }
    }

    private void DealDamage(int damage, bool forPlayer) //Непосредственное нанесение урона
    {
        if (_battleData.TypeOfCurrentHit != TypeOfHit.miss)
        {
            if (!forPlayer)
            {
                _battleData.Enemy.GetDamage(damage);
                if (_battleData.FilledCombo < 7 && _battleData.KomboHits[_battleData.FilledCombo] == _battleData.Player.CurrentDirection) //Заполняем счетчик комбо
                    ChangeFilledCombo(1);
            }
            else
            {
                _battleData.Player.GetDamage(damage);
            }
        }
        _isPlayerTurn = !_isPlayerTurn; //Даем ход аппоненту
        StartCoroutine("StartNewTurn");
    }

    public void ChangeFilledCombo(int value)
    {
        if (_battleData.FilledCombo + value > 7)
            value = 7 - _battleData.FilledCombo;
        _battleData.FilledCombo += value;
    }    

    private void OnDisable()
    {
        CompleteHitEvent.RemoveListener(DealDamage);
    }
}
