using System;
using System.Runtime.Serialization;

//information about the car's owner
[Serializable()]
public class Owner : ISerializable
{
    private string firstName;
    private string lastName;

    public Owner()
    {
    }

    public Owner(SerializationInfo info, StreamingContext ctxt)
    {
        this.firstName = (string)info.GetValue("FirstName", typeof(string));
        this.lastName = (string)info.GetValue("LastName", typeof(string));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
        info.AddValue("FirstName", this.firstName);
        info.AddValue("LastName", this.lastName);
    }
}