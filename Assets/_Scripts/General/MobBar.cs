using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hpBarText;
    [SerializeField] private Image _hpFillerImg;
    [SerializeField] private TextMeshProUGUI _levelText;

    public void RefreshHpBar(int currentUp, int maxHp)
    {
        _hpBarText.text = currentUp + "/" + maxHp;
        _hpFillerImg.fillAmount = (float)currentUp / maxHp;
    }
    public void RefreshHpBar(int maxHp)
    {
        _hpBarText.text = maxHp + "/" + maxHp;
        _hpFillerImg.fillAmount = 1;
    }
    public void RefreshLevel(int level)
    {
        _levelText.text = level.ToString();
    }
}
