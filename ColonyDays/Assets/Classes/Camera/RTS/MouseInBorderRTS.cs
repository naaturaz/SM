using UnityEngine;

public class MouseInBorderRTS : GenericCameraComponent {

    public static Dir GlobalDir;
    private int gUIsize = 10;

    private int cornerSize = 3;

    public int GUIsize
    {
        get { return gUIsize; }
        set { gUIsize = value; }
    }
    Dir direction;

    public Dir Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    //public static MouseInBorderRTS Create(string root, Vector3 origen, Vector3 moveTo, Transform cam)
    //{
    //    MouseInBorderRTS obj = null;
    //    obj = (MouseInBorderRTS)Resources.Load(root, typeof(MouseInBorderRTS));
    //    obj = (MouseInBorderRTS)Instantiate(obj, origen, Quaternion.identity);
    //    return obj;
    //}

    public Dir ReturnMouseDirection()
    {
        direction = Dir.None;

        //create single dir rectagles
        var recdown = new Rect(0, 0, Screen.width, gUIsize);
        var recup = new Rect(0, Screen.height - gUIsize, Screen.width, gUIsize);
        var recleft = new Rect(0, 0, gUIsize, Screen.height);
        var recright = new Rect(Screen.width - gUIsize, 0, gUIsize, Screen.height);

        //create composite dir rectagles
        var rectDownLeft = new Rect(0, 0, cornerSize, cornerSize);
        var rectDownRight = new Rect(Screen.width - cornerSize, 0, cornerSize, cornerSize);
        var rectUpLeft = new Rect(0, Screen.height - cornerSize, cornerSize, cornerSize);
        var rectUpRight = new Rect(Screen.width - cornerSize, Screen.height - cornerSize, cornerSize, cornerSize);

        //if mouse is not on top of gui ...
        if (!MiniMapRTS.isMouseOnGUI)
        {
            direction = ReturnComposeDirection(rectDownLeft, rectDownRight,
                rectUpLeft, rectUpRight);

            if (direction == Dir.None)
            {
                direction = ReturnSingleDirection(recdown, recup, recleft, recright);

                if(UInput.IfCursorKeyIsPressed())
                {
                    direction = ReturnComposeDirectionWithKeyBoard(direction);
                }
            }
        }
        GlobalDir = direction;
        return direction;
    }

    /// <summary>
    /// return         //mouse and key directions only if they dont conflict...
    /// if mouse going right and keyboard up will return an 'UpRight' direction
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    Dir ReturnComposeDirectionWithKeyBoard(Dir dir)
    {
        if(UInput.IfHorizontalKeysIsPressed() && dir == Dir.Up)
        {
            if(UInput.HorizVal()> 0)
            {
                dir = Dir.UpRight;
            }
            else if (UInput.HorizVal() < 0)
            {
                dir = Dir.UpLeft;
            }
        }
        else if (UInput.IfHorizontalKeysIsPressed() && dir == Dir.Down)
        {
            if (UInput.HorizVal() > 0)
            {
                dir = Dir.DownRight;
            }
            else if (UInput.HorizVal() < 0)
            {
                dir = Dir.DownLeft;
            }
        }

        if(UInput.IfVerticalKeyIsPressed() && dir == Dir.Right)
        {
            if(UInput.VertiVal() > 0)
            {
                dir = Dir.UpRight;
            }
            else if (UInput.VertiVal() < 0)
            {
                dir = Dir.DownRight;
            }
        }
        else if (UInput.IfVerticalKeyIsPressed() && dir == Dir.Left)
        {
            if (UInput.VertiVal() > 0)
            {
                dir = Dir.UpLeft;
            }
            else if (UInput.VertiVal() < 0)
            {
                dir = Dir.DownLeft;
            }
        }
        return dir;
    }

    Dir ReturnComposeDirection(Rect rectDownLeft, Rect rectDownRight,
        Rect rectUpLeft, Rect rectUpRight)
    {
        direction = Dir.None;
        Vector3 mP = Input.mousePosition;
        if (rectDownLeft.Contains(Input.mousePosition))
        {
            direction = Dir.DownLeft;
        }
        else if (rectDownRight.Contains(Input.mousePosition))
        {
            direction = Dir.DownRight;
        }
        else if (rectUpLeft.Contains(Input.mousePosition))
        {
            direction = Dir.UpLeft;
        }
        else if (rectUpRight.Contains(Input.mousePosition))
        {
            direction = Dir.UpRight;
        }
        return direction;
    }

    Dir ReturnSingleDirection(Rect recdown, Rect recup, Rect recleft, Rect recright)
    {
        direction = Dir.None;
        if (recdown.Contains(Input.mousePosition))
        {
            direction = Dir.Down;
            //direction = D.DownRight;
        }
        if (recup.Contains(Input.mousePosition))
        {
            direction = Dir.Up;
        }

        if (recleft.Contains(Input.mousePosition))
        {
            direction = Dir.Left;
        }
        if (recright.Contains(Input.mousePosition))
        {
            direction = Dir.Right;
        }
        return direction;
    }
}