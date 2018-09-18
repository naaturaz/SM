using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using System.Runtime.Serialization.Json;

public class JSONSerie
{



    //// Deserialize a JSON stream to a User object.  
    //static User ReadToObject(string json)
    //{
    //    User deserializedUser = new User();
    //    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
    //    DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
    //    deserializedUser = ser.ReadObject(ms) as User;
    //    ms.Close();
    //    return deserializedUser;
    //}




    //Product product = new Product();
    //product.Name = "Apple";
    //product.Expiry = new DateTime(2008, 12, 28);
    //    product.Price = 3.99M;
    //product.Sizes = new string[] { "Small", "Medium", "Large" };

    //string json = JsonConvert.SerializeObject(product);
    //    //{
    //    //  "Name": "Apple",
    //    //  "Expiry": new Date(1230422400000),
    //    //  "Price": 3.99,
    //    //  "Sizes": [
    //    //    "Small",
    //    //    "Medium",
    //    //    "Large"
    //    //  ]
    //    //}

    //Product deserializedProduct = JsonConvert.DeserializeObject<Product>(json);


}

[DataContract]
internal class User
{
    [DataMember]
    internal string name;

    [DataMember]
    internal int age;
}

