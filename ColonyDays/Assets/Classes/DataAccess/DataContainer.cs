using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

[XmlRoot("DataCollection")]
public class DataContainer
{
    [XmlArray("RTSsData")]
    [XmlArrayItem("RTSData")]
    public List<RTSData> SaveInfoRTSs = new List<RTSData>();

    [XmlArrayItem("SubMeshData")]
    public SubMeshData SubMeshData;

    [XmlArrayItem("SpawnedData")]
    public SpawnedData SpawnedData;

    [XmlArrayItem("BuildingData")]
    public BuildingData BuildingData;

    [XmlArrayItem("PersonData")]
    public PersonData PersonData;

    [XmlArrayItem("ProgramData")]
    public ProgramData ProgramData;
    
    [XmlArrayItem("FinalReport")]
    public FinalReport FinalReport;


    //Mods
    [XmlArrayItem("PeopleModData")]
    public PeopleModData PeopleModData;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(DataContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    
    public static DataContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(DataContainer));

        try
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as DataContainer;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("exception: " +ex.Message);
            return null;
        }
    }

    ////Loads the xml directly from the given string. Useful in combination with www.text.
    //public static SaveInfoRTSContainer LoadFromText(string text)
    //{
    //    var serializer = new XmlSerializer(typeof(SaveInfoRTSContainer));
    //    return serializer.Deserialize(new StringReader(text)) as SaveInfoRTSContainer;
    //}
}