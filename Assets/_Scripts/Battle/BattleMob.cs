using UnityEngine;

public class SkillInfo //Данные навыка для использования
{
    public string HitName;
    public int ValueToUse;
    public string EffectName;
    public float EffectValue;
    public float EffectDuration;

    public SkillInfo()
    {
        HitName = "";
        ValueToUse = 0;
        EffectName = "";
        EffectValue = 0;
        EffectDuration = 0;
    }
}

public class BattleMob : MonoBehaviour
{
    public HitDirection CurrentDirection; //Текущее направление удара
    public int CurrentHp;
    [HideInInspector] public SkillInfo[] Skills; //Список доступных скиллов
    private SkillInfo _currentSkill;

    [SerializeField] private bool _isPlayer; 
    protected bool _isSkill = false; //Удар является скиллом или обычной атакой
    [SerializeField] private string _skillAnimationTrigger = "JumpAttack";//***********Временная переменная**********

    private BattleData _battleData; //ScriptableObject с данными битвы
    protected Animator _animator;
    private MobAudio _mobAudio;
    private Characteristics _characteristics; public Characteristics Characteristics { get { return _characteristics; } } 
    private string _name; public string Name { get { return _name; } }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mobAudio = GetComponent<MobAudio>();
        _characteristics = new Characteristics();
        _battleData = Resources.Load<BattleData>("ScriptableObjects/BattleData");
    }

    public virtual void Initialize(string Name)
    {
        _name = Name;
        _characteristics.SetCharacteristics(Name);
        CurrentHp = _characteristics.MaxHp;
    }

    public void CheckHit() //Проверка, попадает удар по противнику, или нет. Вызывается в анимации до нанесения урона
    {
        BattleController.S.CheckHit(CalculateDamage(), !_isPlayer);
    }

    public void DealDamage() //Непосредственное нанесение урона. Вызывается в анимации
    {
        if (_battleData.TypeOfCurrentHit == TypeOfHit.critical || _battleData.TypeOfCurrentHit == TypeOfHit.simple)
            _mobAudio.PlayHit();
        BattleController.CompleteHitEvent.Invoke(CalculateDamage(), !_isPlayer);
    } 

    public virtual void UseSkill(int skillNumber) //Использование навыка
    {
        _isSkill = true;
        _animator.SetTrigger(_skillAnimationTrigger);
        if (Skills[skillNumber - 1] != null)
            _currentSkill = Skills[skillNumber - 1];
    }

    public void DodgeDamage()
    {
        _animator.SetTrigger("Dodge");
        _mobAudio.PlayDodge();
    }
    public void BlockDamage()
    {
        _animator.SetTrigger("Block");
        _mobAudio.PlayBlock();
    }

    public void GetDamage(int damage) //Получение урона
    {
        CurrentHp -= damage;
        if (CurrentHp > 0)
        {
            if (_battleData.TypeOfCurrentHit != TypeOfHit.blocked && _battleData.TypeOfCurrentHit != TypeOfHit.criticalBlocked)
            {
                _animator.SetTrigger("GetDamage");
                _mobAudio.PlayGetDamage();
            }
        }
        else
        {
            _animator.SetTrigger("Death");
            _mobAudio.PlayGetDamage();
            CurrentHp = 0;
            BattleController.S.FinishFight(!_isPlayer); //****************Изменить, когда появится несколько соперников**********************
        }
    }

    public int CalculateDamage() //Сдесь будут учитываться все бафы и дебафы урона
    {
        int damage = Random.Range(_characteristics.TotalMinDamage, _characteristics.TotalMaxDamage + 1);
        //Добавляем модификаторы от блока и крита
        if (_battleData.TypeOfCurrentHit == TypeOfHit.blocked || _battleData.TypeOfCurrentHit == TypeOfHit.criticalBlocked) damage = damage / 2;
        if (_battleData.TypeOfCurrentHit == TypeOfHit.critical || _battleData.TypeOfCurrentHit == TypeOfHit.criticalBlocked) damage = (int)(damage * 1.75f);
        //Добавляем модификатор от пробивания и защиты
        damage = (int)(damage * _battleData.PenetrationMultiplier);
        //Добавляем модификаторы от супер-ударов
        if (_isSkill)
        {
            if (_currentSkill.EffectName == "IncreaseDamage") 
                damage = (int)(damage* _currentSkill.EffectValue); 
        }

        return damage;
    }    

    public void MakeSimpleHit(HitDirection direc) //Обычный удар
    {        
        _isSkill = false;
        if (direc == HitDirection.up)
            _animator.SetTrigger("AttackUp");
        else if (direc == HitDirection.midle)
            _animator.SetTrigger("AttackMiddle");
        else if (direc == HitDirection.down)
            _animator.SetTrigger("AttackDown");
        CurrentDirection = direc;
    }
}
