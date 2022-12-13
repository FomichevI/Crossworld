using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapEnemiesList : MonoBehaviour //Отвечает за отображение списка противников на сцене карты
{
    [SerializeField] private Image _simpleEnemyIcon;
    [SerializeField] private Image _middleEnemyIcon;
    [SerializeField] private Image _hardEnemyIcon;
    [SerializeField] private TextMeshProUGUI _simpleEnemyLvlText;
    [SerializeField] private TextMeshProUGUI _middleEnemyLvlText;
    [SerializeField] private TextMeshProUGUI _hardEnemyLvlText;

    public void RefreshEnemiesList(string locationName)
    {
        string currentEnemy = GameDataLoader.S.GetEnemyByType(locationName, "simple");
        if (currentEnemy != "")
        {
            _simpleEnemyIcon.gameObject.SetActive(true);
            _simpleEnemyIcon.sprite = Resources.Load<Sprite>("_Images/Enemies/" + currentEnemy);
            _simpleEnemyLvlText.text = EnemiesDataLoader.S.GetLevel(currentEnemy).ToString();
        }
        currentEnemy = GameDataLoader.S.GetEnemyByType(locationName, "middle");
        if (currentEnemy != "")
        {
            _middleEnemyIcon.gameObject.SetActive(true);
            _middleEnemyIcon.sprite = Resources.Load<Sprite>("_Images/Enemies/" + currentEnemy);
            _middleEnemyLvlText.text = EnemiesDataLoader.S.GetLevel(currentEnemy).ToString();
        }
        currentEnemy = GameDataLoader.S.GetEnemyByType(locationName, "hard");
        if (currentEnemy != "")
        {
            _hardEnemyIcon.gameObject.SetActive(true);
            _hardEnemyIcon.sprite = Resources.Load<Sprite>("_Images/Enemies/" + currentEnemy);
            _hardEnemyLvlText.text = EnemiesDataLoader.S.GetLevel(currentEnemy).ToString();
        }
    }
}
