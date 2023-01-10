public enum HitDirection { up, midle, down}

public class BattleCharacter : BattleMob
{
    public HitDirection[] KomboDirection; //*****************В будущем будет подгружаться из PlayerData********************

    private void OnEnable()
    {
        BattleController.MakeHitPlayer.AddListener(MakeSimpleHit);        
    }

    private void Start()
    {
        //*************Скиллы в разработке, пока что будет так ****************
        Skills = new SkillInfo[7];
        for (int i = 0; i < 7; i++)
            Skills[i] = new SkillInfo();
        Skills[1].HitName = "JumpAttack";
        Skills[1].EffectName = "IncreaseDamage";
        Skills[1].EffectValue = 1.4f;
    }

    private void OnDisable()
    {
        BattleController.MakeHitPlayer.RemoveListener(MakeSimpleHit);
    }
}
