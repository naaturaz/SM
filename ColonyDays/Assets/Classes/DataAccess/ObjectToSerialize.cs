using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable()]
public class ObjectToSerialize : ISerializable
{
    private List<Car> cars;
    private List<RTSData> saveInfo;

    public List<RTSData> SaveInfo
    {
        get { return saveInfo; }
        set { saveInfo = value; }
    }

    public List<Car> Cars
    {
        get { return this.cars; }
        set { this.cars = value; }
    }

    public ObjectToSerialize()
    {
    }

    public ObjectToSerialize(SerializationInfo info, StreamingContext ctxt)
    {
        this.cars = (List<Car>)info.GetValue("Cars", typeof(List<Car>));
        this.saveInfo = (List<RTSData>)info.GetValue("saveInfo", typeof(List<RTSData>));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("Cars", this.cars);
        info.AddValue("saveInfo", this.saveInfo);
    }
}