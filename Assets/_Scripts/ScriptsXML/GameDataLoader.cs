using UnityEngine;
using System.Xml;

public class GameDataLoader : MonoBehaviour
{
    static public GameDataLoader S;
    private XmlDocument _gameDataXml;
    private string _path;

    private void Awake()
    {
        S = this;
        _path = Application.dataPath + "/StreamingAssets/Data/GameData.xml";
    }

    public int GetExperienceToLevelUp(int currentLevel) //¬озвращает количество очков опыта, необходимое дл€ следующего лвлапа
    {
        _gameDataXml = new XmlDocument();
        _gameDataXml.Load(_path);
        XmlNode xml = _gameDataXml.SelectSingleNode("xml");
        XmlNode expNode = xml.SelectSingleNode("experiencePerLevel");
        XmlNode levelNode = expNode.SelectSingleNode("level" + currentLevel);
        return int.Parse(levelNode.Attributes["value"].Value);
    }
    public void GetEnemyByType(string location, string type, out string name, out int count) //¬озвращает им€ и количество противников конкретного типа на конкретной локации
    {
        _gameDataXml = new XmlDocument();
        _gameDataXml.Load(_path);
        XmlNode xml = _gameDataXml.SelectSingleNode("xml");
        XmlNode locationsNode = xml.SelectSingleNode("locations");
        XmlNode locationNode = locationsNode.SelectSingleNode(location);
        XmlNode enemiesNode = locationNode.SelectSingleNode("enemies");
        XmlNode mobNode = enemiesNode.SelectSingleNode(type);
        if (mobNode != null)
        {
            name = mobNode.Attributes["name"].Value;
            count = int.Parse(mobNode.Attributes["count"].Value);
        }
        else
        {
            name = "";
            count = 0;
        }
    }
    public string GetEnemyByType(string location, string type) //¬озвращает только им€ противников конкретного типа на конкретной локации
    {
        _gameDataXml = new XmlDocument();
        _gameDataXml.Load(_path);
        XmlNode xml = _gameDataXml.SelectSingleNode("xml");
        XmlNode locationsNode = xml.SelectSingleNode("locations");
        XmlNode locationNode = locationsNode.SelectSingleNode(location);
        XmlNode enemiesNode = locationNode.SelectSingleNode("enemies");
        XmlNode mobNode = enemiesNode.SelectSingleNode(type);
        if (mobNode != null) return mobNode.Attributes["name"].Value;
        else return "";
    }    
}
