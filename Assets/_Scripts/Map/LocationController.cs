using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationController : MonoBehaviour
{
    public static LocationController S;
    [SerializeField] private MobSpawner _mobSpawner;
    [SerializeField] private Transform _playerPosition; public Transform PlayerPosition { get { return _playerPosition; } }
    private Characteristics _playerCharacteristics; public Characteristics PlayerCharacteristics { get { return _playerCharacteristics; } }

    [Header("Настройки локации")]
    [SerializeField] private string _locationName = "Village"; public string LocationName { get { return _locationName; } }

    private void Awake()
    {
        S = this;
        transform.position = _playerPosition.position = PlayerDataLoader.S.GetMapPosition();
        _playerCharacteristics = new Characteristics();
        _playerCharacteristics.SetCharacteristics("player");
    }
    private void Start()
    {
        _mobSpawner.SetEnemiesList(_locationName);
        _mobSpawner.SpawnAllMobs();
    }

    public void StartFight(string enemyName)
    {
        PlayerDataSaver.S.SetCurrentEnemy(enemyName);
        PlayerDataSaver.S.SetCurrentMapPosition(_playerPosition.position);
        PlayerPrefs.SetInt("SceneToLoad", 1);
        SceneManager.LoadScene("LoadingScene");
    }
    public void OpenInventory()
    {
        PlayerDataSaver.S.SetCurrentMapPosition(_playerPosition.position);
        PlayerPrefs.SetInt("SceneToLoad", 2);
        SceneManager.LoadScene("LoadingScene");
    }
}
