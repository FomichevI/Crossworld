using UnityEngine;

public class LocationInterface : MonoBehaviour
{
    public static LocationInterface S;
    [SerializeField] private PlayerBar _playerBar;
    [SerializeField] private MapEnemiesList _enemiesList;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        _playerBar.RefreshHpBar(LocationController.S.PlayerCharacteristics.MaxHp);
        _enemiesList.RefreshEnemiesList(LocationController.S.LocationName);
    }
}
