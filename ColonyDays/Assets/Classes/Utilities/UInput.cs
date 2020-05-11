using UnityEngine;

public class UInput : MonoBehaviour
{
    private static SMe m = new SMe();

    private static Vector3 _transformedMPos;

    public static Vector3 TransformedMPos
    {
        get { return _transformedMPos; }
    }

    public static bool IfCursorKeyIsPressed()
    {
        if (Dialog.IsActive() || BuildingPot.InputMode != Mode.None)
        {
            return false;
        }

        bool result = false;
        if (Input.GetKey(KeyCode.W) || Input.GetKey("up"))
        {
            result = true;
        }
        else if ((Input.GetKey(KeyCode.S)) || Input.GetKey("down"))
        {
            result = true;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey("right"))
        {
            result = true;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey("left"))
        {
            result = true;
        }
        else { result = false; }

        return result;
    }

    public static bool IfHorizontalKeysIsPressed()
    {
        if (Dialog.IsActive())
        {
            return false;
        }

        bool result = false;

        if (Input.GetKey(KeyCode.D) || Input.GetKey("right"))
        {
            result = true;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey("left"))
        {
            result = true;
        }
        else { result = false; }

        return result;
    }

    public static bool IfVerticalKeyIsPressed()
    {
        if (Dialog.IsActive())
        {
            return false;
        }

        bool result = false;
        if (Input.GetKey(KeyCode.W) || Input.GetKey("up"))
        {
            result = true;
        }
        else if ((Input.GetKey(KeyCode.S)) || Input.GetKey("down"))
        {
            result = true;
        }
        else { result = false; }

        return result;
    }

    public static float HorizVal()
    {
        //if mouse out of screen return

        return Input.GetAxis("Horizontal");
    }

    public static float VertiVal()
    {
        return Input.GetAxis("Vertical");
    }

    // Use this for initialization
    private void Start()
    { }

    // Update is called once per frame
    public static void Update()
    {
        if (Program.gameScene.controllerMain != null)
        {
            if (Program.gameScene.controllerMain.MeshController != null)
            {
                _transformedMPos = CamControl.CAMRTS.GetComponent<Camera>().WorldToScreenPoint(m.HitMouseOnTerrain.point);
            }
        }
    }
}