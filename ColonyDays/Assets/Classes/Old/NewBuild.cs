using UnityEngine;

public enum BuildType
{
}

public class NewBuild : General
{
    //variables
    private Transform _newPos;

    private int _lot;
    private bool newWasCreated;
    private NewBuild newBuilt;

    #region Properties

    public Transform NewPos
    {
        get { return _newPos; }
        set { _newPos = value; }
    }

    public int Lot
    {
        get { return _lot; }
        set { _lot = value; }
    }

    #endregion Properties

    #region Unity Voids

    // Use this for initialization
    private void Start()
    {
        newBuilt = null;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //MouseControl ();
        if (newBuilt != null)
        {
        }
    }

    #endregion Unity Voids

    #region General Function

    //void MouseControl()
    //{
    //    //hovering will move the new building thru the grid
    //    if(gameObject != null && !this.PositionFixed)
    //    {
    //        if(Program.DETECTSPACE != null)
    //        {
    //            gameObject.transform.position = Program.DETECTSPACE.position;
    //        }
    //    }
    //    //left click
    //    if(Input.GetMouseButtonUp (0))
    //    {
    //        //positioning the new building to the grid in left mouse up...
    //        if(this != null && !this.PositionFixed && Program.DETECTSPACE != null)
    //        {
    //            this.PositionFixed = true;
    //            this.Lot = FindSpace(Program.DETECTSPACE.name, "Space");
    //            this.RenameIt("Building", this.Lot, "NewBuild");
    //            this.collider.enabled = true;
    //            //we are adding this current object to the array
    //            AddInProgramCSArray("NewBuild", this);
    //        }
    //    }
    //    //right click... creates new building
    //    if(Input.GetMouseButtonUp (1))
    //    {
    //        //if this is null ...
    //        if(this == null)
    //        {
    //            newBuilt = (NewBuild)General.Create(Roots.helperNewBuild, Vector3.zero);
    //        }
    //        //if != null and the new obj from this class was not created and if this.PostionFixed ...
    //        else if(this != null && !newWasCreated && this.PositionFixed)
    //        {
    //            newBuilt = (NewBuild)General.Create(Roots.helperNewBuild, Vector3.zero);
    //            //so each new obj of this class can create only one new obj of this class
    //            newWasCreated = true;
    //        }
    //    }
    //}

    ////adds in a list in Program.cs and deals with statics in Program.cs
    //void AddInProgramCSArray(string listName, NewBuild obj)
    //{
    //    //Program.ADDTOARRAY = true;
    //    Program.GENERICOBJ = obj;
    //    Program.ARRAYNAME = listName;
    //    //deactivates the spacelot we built on it
    //    Program.DETECTSPACE.collider.enabled = false;
    //    Program.SPACELOT = FindSpace(Program.DETECTSPACE.name, "Space");
    //}

    //will return int value with space was passed as input
    public int FindSpace(string input, string type)
    {
        int temp = 0;
        int twoDigit = 0;
        int firstDigit = 0;
        int secondDigit = 0;
        if (type == "Space")
        {
            twoDigit = int.Parse(input.Substring(5, 2));
            firstDigit = int.Parse(input.Substring(5, 1));
            secondDigit = int.Parse(input.Substring(6, 1));

            /*
			print ("input:" + input);
			print ("twoDigit:" + twoDigit);
			print ("firstDigit:" + firstDigit);
			print ("secondDigit:" + secondDigit);
			 */

            //if first digit after 'space' = 0...
            if (firstDigit == 0)
            {
                temp = secondDigit;
            }
            //if is bigger than 0...
            else if (firstDigit > 0)
            {
                temp = twoDigit;
            }
        }
        return temp;
    }

    #endregion General Function
}