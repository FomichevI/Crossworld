using UnityEngine;

public class SkillInfo //������ ������ ��� �������������
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
    public HitDirection CurrentDirection; //������� ����������� �����
    public int CurrentHp;
    [HideInInspector] public SkillInfo[] Skills; //������ ��������� �������
    private SkillInfo _currentSkill;

    [SerializeField] private bool _isPlayer; 
    protected bool _isSkill = false; //���� �������� ������� ��� ������� ������
    [SerializeField] private string _skillAnimationTrigger = "JumpAttack";//***********��������� ����������**********

    private BattleData _battleData; //ScriptableObject � ������� �����
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

    public void CheckHit() //��������, �������� ���� �� ����������, ��� ���. ���������� � �������� �� ��������� �����
    {
        BattleController.S.CheckHit(CalculateDamage(), !_isPlayer);
    }

    public void DealDamage() //���������������� ��������� �����. ���������� � ��������
    {
        if (_battleData.TypeOfCurrentHit == TypeOfHit.critical || _battleData.TypeOfCurrentHit == TypeOfHit.simple)
            _mobAudio.PlayHit();
        BattleController.CompleteHitEvent.Invoke(CalculateDamage(), !_isPlayer);
    } 

    public virtual void UseSkill(int skillNumber) //������������� ������
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

    public void GetDamage(int damage) //��������� �����
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
            BattleController.S.FinishFight(!_isPlayer); //****************��������, ����� �������� ��������� ����������**********************
        }
    }

    public int CalculateDamage() //����� ����� ����������� ��� ���� � ������ �����
    {
        int damage = Random.Range(_characteristics.TotalMinDamage, _characteristics.TotalMaxDamage + 1);
        //��������� ������������ �� ����� � �����
        if (_battleData.TypeOfCurrentHit == TypeOfHit.blocked || _battleData.TypeOfCurrentHit == TypeOfHit.criticalBlocked) damage = damage / 2;
        if (_battleData.TypeOfCurrentHit == TypeOfHit.critical || _battleData.TypeOfCurrentHit == TypeOfHit.criticalBlocked) damage = (int)(damage * 1.75f);
        //��������� ����������� �� ���������� � ������
        damage = (int)(damage * _battleData.PenetrationMultiplier);
        //��������� ������������ �� �����-������
        if (_isSkill)
        {
            if (_currentSkill.EffectName == "IncreaseDamage") 
                damage = (int)(damage* _currentSkill.EffectValue); 
        }

        return damage;
    }    

    public void MakeSimpleHit(HitDirection direc) //������� ����
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
