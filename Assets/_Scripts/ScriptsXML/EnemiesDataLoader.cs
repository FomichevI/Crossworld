using UnityEngine;
using System.Xml;


public class EnemiesDataLoader : MonoBehaviour
{
    static public EnemiesDataLoader S;
    private XmlDocument _enemyDataXml;
    private string _path;

    private void Awake()
    {
        S = this;
        _path = Application.dataPath + "/StreamingAssets/Data/EnemiesData.xml";
    }

    public int GetLevel(string name)
    {
        _enemyDataXml = new XmlDocument();
        _enemyDataXml.Load(_path);
        XmlNode xml = _enemyDataXml.SelectSingleNode("xml");
        XmlNode enemyNode = xml.SelectSingleNode(name);
        XmlNode statsNode = enemyNode.SelectSingleNode("stats");
        return int.Parse(statsNode.Attributes["level"].Value);
    }
    public int GetType(string name)
    {
        _enemyDataXml = new XmlDocument();
        _enemyDataXml.Load(_path);
        XmlNode xml = _enemyDataXml.SelectSingleNode("xml");
        XmlNode enemyNode = xml.SelectSingleNode(name);
        XmlNode statsNode = enemyNode.SelectSingleNode("stats");
        return int.Parse(statsNode.Attributes["type"].Value);
    }
    public int GetDropChance(string name) //¬озвращает шанс дропа одной вещи
    {
        _enemyDataXml = new XmlDocument();
        _enemyDataXml.Load(_path);
        XmlNode xml = _enemyDataXml.SelectSingleNode("xml");
        XmlNode enemyNode = xml.SelectSingleNode(name);
        XmlNode dropListNode = enemyNode.SelectSingleNode("dropList");
        return int.Parse(dropListNode.Attributes["chance"].Value);
    }
    public float GetDropGold(string name) //¬озвращает количество золота, которое дропает моб
    {
        _enemyDataXml = new XmlDocument();
        _enemyDataXml.Load(_path);
        XmlNode xml = _enemyDataXml.SelectSingleNode("xml");
        XmlNode enemyNode = xml.SelectSingleNode(name);
        XmlNode dropListNode = enemyNode.SelectSingleNode("dropList");

        System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ",";
        return float.Parse(dropListNode.Attributes["gold"].Value, System.Globalization.NumberStyles.Any, ci);
    }
    public int GetExperience(string name) //¬озвращает количество опыта, получаемое при победе над мобом
    {
        _enemyDataXml = new XmlDocument();
        _enemyDataXml.Load(_path);
        XmlNode xml = _enemyDataXml.SelectSingleNode("xml");
        XmlNode enemyNode = xml.SelectSingleNode(name);
        XmlNode dropListNode = enemyNode.SelectSingleNode("dropList");
        return int.Parse(dropListNode.Attributes["experience"].Value);
    }
    public void GetDropItems(string name, out string rare, out int minLvl, out int maxLvl) //¬озвращает рамки дл€ дальнейшего поска вещей, которые могут выпасть с моба
    {
        _enemyDataXml = new XmlDocument();
        _enemyDataXml.Load(_path);
        XmlNode xml = _enemyDataXml.SelectSingleNode("xml");
        XmlNode enemyNode = xml.SelectSingleNode(name);
        XmlNode dropListNode = enemyNode.SelectSingleNode("dropList");
        rare = dropListNode.Attributes["rare"].Value;
        string[] lvls = dropListNode.Attributes["itemLevel"].Value.Split('-');
        minLvl = int.Parse(lvls[0]);
        maxLvl = int.Parse(lvls[1]);
    }
    public void GetChacteristics(string name, out int agility, out int parry, out int rage, out int hitPoints, out int defense, out int penetration, out int minDamage, out int maxDamage)
    {
        _enemyDataXml = new XmlDocument();
        _enemyDataXml.Load(_path);
        XmlNode xml = _enemyDataXml.SelectSingleNode("xml");
        XmlNode enemyNode = xml.SelectSingleNode(name);
        XmlNode statsNode = enemyNode.SelectSingleNode("stats");
        agility = parry = rage = hitPoints = defense = penetration = minDamage = maxDamage = 0;
        agility = int.Parse(statsNode.Attributes["agility"].Value);
        parry = int.Parse(statsNode.Attributes["parry"].Value);
        rage = int.Parse(statsNode.Attributes["rage"].Value);
        hitPoints = int.Parse(statsNode.Attributes["hitPoints"].Value);
        defense = int.Parse(statsNode.Attributes["defense"].Value);
        penetration = int.Parse(statsNode.Attributes["penetration"].Value);
        minDamage = int.Parse(statsNode.Attributes["minDamage"].Value);
        maxDamage = int.Parse(statsNode.Attributes["maxDamage"].Value);
    }
}
