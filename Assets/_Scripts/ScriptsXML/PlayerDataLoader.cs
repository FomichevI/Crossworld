using UnityEngine;
using System.Xml;

public class EquipmentItemData //Данные экипировки
{
    //Существуют во всех файлах данных (data)
    public string Name;
    public string Type;
    public string Set;

    //Существуют только в ItemsData
    public string Rare;
    public string Class;
    public int Level;
    public int Strength;
    public int Agility;
    public int Parry;
    public int Rage;
    public int Survivability;
    public int Defense;
    public int Penetration;
    public int MinDamage;
    public int MaxDamage;

    public EquipmentItemData(string name, string type, string set)
    {
        Name = name;
        Type = type;
        Set = set;
        ItemDataLoader.S.SetAllCharacteristics(this);
    }

    public EquipmentItemData(string type)
    {
        Name = "none";
        Type = type;
        Set = "none";
    }
}

public class PlayerDataLoader : MonoBehaviour
{
    static public PlayerDataLoader S;
    private XmlDocument _playerDataXml;
    private string _path;

    private void Awake()
    {
        S = this;
        _path = Application.dataPath + "/StreamingAssets/Data/PlayerData.xml";
    }

    public int GetLevel()
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode statsNode = xml.SelectSingleNode("stats");
        int level = int.Parse(statsNode.Attributes["level"].Value);
        return level;
    }
    public int GetExperience()
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode statsNode = xml.SelectSingleNode("stats");
        int exp = int.Parse(statsNode.Attributes["experience"].Value);
        return exp;
    }
    public EquipmentItemData[] GetEquip() //Возвращает весь список экипированных вещей
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode equipmentNode = xml.SelectSingleNode("equipment");
        XmlNodeList itemsNodeList = equipmentNode.SelectNodes("item");
        EquipmentItemData[] items = new EquipmentItemData[itemsNodeList.Count];
        for (int i = 0; i < itemsNodeList.Count; i++)
        {
            items[i] = new EquipmentItemData(itemsNodeList[i].Attributes["name"].Value, itemsNodeList[i].Attributes["type"].Value, itemsNodeList[i].Attributes["set"].Value);
        }
        return items;
    }

    public EquipmentItemData GetEquippedItem(string type) //Возвращает вещь, экипированную в конкретном слоте (при смене эквипа)
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode equipmentNode = xml.SelectSingleNode("equipment");
        XmlNodeList itemsNodeList = equipmentNode.SelectNodes("item");
        EquipmentItemData item = new EquipmentItemData("");
        for (int i = 0; i < itemsNodeList.Count; i++)
        {
            if (itemsNodeList[i].Attributes["type"].Value == type)
            {
                item = new EquipmentItemData(itemsNodeList[i].Attributes["name"].Value, type, itemsNodeList[i].Attributes["set"].Value);
                break;
            }
        }
        return item;
    }

    public string GetCurrentEnemy() //Возвращает противника, с которым будет происходить следующее сражение
    {
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode currentFightNode = xml.SelectSingleNode("currentFight");
        return currentFightNode.Attributes["enemy"].Value;
    }
    public Vector3 GetMapPosition() //Возвращает позицию персонажа на карте
    {
        System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ",";
        _playerDataXml = new XmlDocument();
        _playerDataXml.Load(_path);
        XmlNode xml = _playerDataXml.SelectSingleNode("xml");
        XmlNode mapNode = xml.SelectSingleNode("map");
        Vector3 v = new Vector3();
        v.x = float.Parse(mapNode.Attributes["positionX"].Value, System.Globalization.NumberStyles.Any, ci);
        v.y = float.Parse(mapNode.Attributes["positionY"].Value, System.Globalization.NumberStyles.Any, ci);
        v.z = float.Parse(mapNode.Attributes["positionZ"].Value, System.Globalization.NumberStyles.Any, ci);
        return v;
    }
}
