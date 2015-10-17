using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

    //information about the car
    [Serializable()]
    public class Car : ISerializable
    {
        public string make;
        private string model;
        private int year;
        private Owner owner;

        public Car()
        {
        }

        public Car(SerializationInfo info, StreamingContext ctxt)
        {
            this.make = (string)info.GetValue("Make", typeof(string));
            this.model = (string)info.GetValue("Model", typeof(string));
            this.year = (int)info.GetValue("Year", typeof(int));
            this.owner = (Owner)info.GetValue("Owner", typeof(Owner));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Make", this.make);
            info.AddValue("Model", this.model);
            info.AddValue("Year", this.year);
            info.AddValue("Owner", this.owner);
        }
    }

