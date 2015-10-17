//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Serializer
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        List<Car> cars = new List<Car>();
//        ObjectToSerialize objectToSerialize;
//        SerializerClass serializer;

//        private void Form1_Load(object sender, EventArgs e)
//        {
            


//        }

//        private void btnSave_Click(object sender, EventArgs e)
//        {
//            cars.Add(new Car());
//            cars[0].make = textBox1.Text;

//            //save the car list to a file
//            objectToSerialize = new ObjectToSerialize();
//            objectToSerialize.Cars = cars;

//            serializer = new SerializerClass();
//            serializer.SerializeObject("outputFile.bin", objectToSerialize);

//            //the car list has been saved to outputFile.txt
//        }

//        private void btnLoad_Click(object sender, EventArgs e)
//        {
//            //read the file back from outputFile.txt

//            objectToSerialize = serializer.DeSerializeObject("outputFile.bin");
//            cars = objectToSerialize.Cars;
//            textBox1.Text = "loaded: " + cars[0].make;
//        }
//    }
//}
