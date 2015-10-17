using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


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

