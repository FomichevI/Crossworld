using UnityEngine;
using System.Xml;

public class PlayerDataSaver : MonoBehaviour
{
    static public PlayerDataSaver S;
    private XmlDocument _playerDataXml;
    private string _path;

    void Awake()
    {
        S = this;
        _path = Application.dataPath + "/StreamingAssets/Data/PlayerData.xml";
    }

    public void SetNewItem(EquipmentItemData newItem) //Заменить текущий эквип определенного типа на другой
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode equipmentNode = xml.SelectSingleNode("equipment");
        XmlNodeList itemsNodeList = equipmentNode.SelectNodes("item");
        for (int i = 0; i < itemsNodeList.Count; i++)
        {
            if (itemsNodeList[i].Attributes["type"].Value == newItem.Type)
            {
                itemsNodeList[i].Attributes["name"].Value = newItem.Name;
                itemsNodeList[i].Attributes["set"].Value = newItem.Set;
                break;
            }
        }
        _playerDataXml.Save(_path);
    }
    public void SetLevel(int level) //Установить новый уровень персонажа
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode statsNode = xml.SelectSingleNode("stats");
        statsNode.Attributes["level"].Value = level.ToString();
        _playerDataXml.Save(_path);
    }
    public void AddExperience(int exp) //Добавить очки опыта. Если их достаточно, то повысить уровень
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode statsNode = xml.SelectSingleNode("stats");
        int newExp = int.Parse(statsNode.Attributes["experience"].Value) + exp;
        int experienceToLevelUp = GameDataLoader.S.GetExperienceToLevelUp(int.Parse(statsNode.Attributes["level"].Value));
        if (experienceToLevelUp <= newExp)
        {
            newExp = newExp - experienceToLevelUp;
            SetLevel(int.Parse(statsNode.Attributes["level"].Value) + 1);            
        }
        statsNode.Attributes["experience"].Value = newExp.ToString();
        _playerDataXml.Save(_path);
    }
    public void SetCurrentEnemy(string enemy) //Установить противника, с которым начинается сражение
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode currentFightNode = xml.SelectSingleNode("currentFight");
        currentFightNode.Attributes["enemy"].Value = enemy;
        _playerDataXml.Save(_path);
    }
    public void SetCurrentMapPosition(Vector3 v) //Сохранить позицию на карте, когда мы уходим с карты
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode mapNode = xml.SelectSingleNode("map");
        mapNode.Attributes["positionX"].Value = v.x.ToString();
        mapNode.Attributes["positionY"].Value = v.y.ToString();
        mapNode.Attributes["positionZ"].Value = v.z.ToString();
        _playerDataXml.Save(_path);
    }
}
