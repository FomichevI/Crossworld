using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _positions; //Точки спавна мобов
    [SerializeField] private float _offsetPlayer = 10f; //Минимально расстояние, на котором должен находиться моб от персонажа при спавне
    private string[] _enemiesList; //Список мобов (каждый дубликат конкретного моба имеет свою ячейку)
    private GameObject _currentMob;

    public void SetEnemiesList(string location)
    {        
        string simpleName;
        string middleName;
        string hardName;
        int simpleCount;
        int middleCount;
        int hardCount;
        GameDataLoader.S.GetEnemyByType(location, "simple", out simpleName, out simpleCount);
        GameDataLoader.S.GetEnemyByType(location, "middle", out middleName, out middleCount);
        GameDataLoader.S.GetEnemyByType(location, "hard", out hardName, out hardCount);
        _enemiesList = new string[simpleCount + middleCount + hardCount];
        for (int i = 0; i< _enemiesList.Length; i++) //Поочередно добавляем всех мобов в список
        {
            if(simpleCount>0)
            {
                _enemiesList[i] = simpleName;
                simpleCount--;
            }
            else if (middleCount >0)
            {
                _enemiesList[i] = middleName;
                middleCount--;
            }
            else if (hardCount>0)
            {
                _enemiesList[i] = hardName;
                hardCount--;
            }
        }
    }
    public void SpawnAllMobs()
    {
        {
            System.Random rand = new System.Random();
            //Рандомная сортировка позиций
            for (int i = _positions.Length - 1; i >= 1; i--) 
            {
                int j = rand.Next(i + 1);

                Transform t = _positions[j];
                _positions[j] = _positions[i];
                _positions[i] = t;
            }
        }
        //Непосресдственный спавн мобов
        string lastMobName = "";
        int k = 0;
        for (int i = 0; i < _enemiesList.Length; i++)
        {
            if (k < _positions.Length)
            {
                if (lastMobName != _enemiesList[i]) //Проверка для того, чтобы лишний раз не грузить ресурсы
                {
                    lastMobName = _enemiesList[i];
                    if (lastMobName == "") break;
                    _currentMob = Resources.Load<GameObject>("Prefabs/Enemies/OnMap/" + lastMobName);
                }
                if ((_positions[k].position - LocationController.S.PlayerPosition.position).magnitude > _offsetPlayer) //Создаем экземпляр моба
                {
                    GameObject mob = Instantiate<GameObject>(_currentMob, _positions[k].position, _positions[k].rotation);
                    mob.name = lastMobName;
                }
                else
                {
                    i--;
                }
                k++;
            }
        }
    }
}
