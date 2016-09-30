using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Classes.SoundsAndMusic
{
    class AudioReport
    {
        //READ
        private int _times;//how many times this report has been report to
        //if 3 isHoe, farmers had reported here this value will be 3.
        //is important to know how many objects had reported so we can have
        //an average of the distance and not a SUM of the distance so we can determine
        //the volume of the AudioSource.

        //However this class is better suited for instance for detect how many
        //ocean elements are in it and how many shore elements or forest
        //for people is not well suited while trying to play animation 

        //In the case specific of people they will play it directly from the Body class

        private float _dist;
        private string _key;

        public AudioReport(string key, float dist)
        {
            _key = key;
            _dist = dist;
            _times++;//1st object reporting
        }

        public void NewReport(float dist)
        {
            _dist += dist;
            _times++;//more objects reporting
        }

        public float AverageDistance()
        {
            return _dist/_times;
        }
    }
}
