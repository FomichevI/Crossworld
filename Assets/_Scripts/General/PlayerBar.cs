using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBar : MobBar
{
    [SerializeField] private TextMeshProUGUI _expBarText;
    [SerializeField] private Image _expFillerImg;

    private void Start()
    {
        int lvl = PlayerDataLoader.S.GetLevel();
        int currentExp = PlayerDataLoader.S.GetExperience();
        int maxExp = GameDataLoader.S.GetExperienceToLevelUp(lvl);
        RefreshLevel(lvl);
        RefreshExpBar(currentExp, maxExp);
    }

    public void RefreshExpBar(int currentExp, int maxExp)
    {
        _expBarText.text = currentExp + "/" + maxExp;
        _expFillerImg.fillAmount = (float)currentExp / maxExp;
    }
}
