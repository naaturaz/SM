/*This is the object will be saved on HardDisk to hold all the info
 * of the subMesh fake elements. Subpolygons. It hold a List of Lots
 * and the total number of how many subvertices has
 */

using System.Collections.Generic;
using UnityEngine;

public class SubMeshData
{
    public int amountOfSubVertices;// how many subvertices has

    //hard coded bz an island the mostcomom value is 2.5f bz is the ground of the floor
    public float mostCommonYValue;//basically the ground level

    public List<Lot> AllSubMeshedLots = new List<Lot>();
    //public Grid grid = new Grid();

    //the marine bounds ...are saved then dont need to find them again
    public List<Vector3> MarineBounds = new List<Vector3>();

    public List<Vector3> MountainBounds = new List<Vector3>();

    private CrystalManager _crystalManager = new CrystalManager();
    private LandZoneManager _landZoneManager = new LandZoneManager();

    public CrystalManager CrystalManager1
    {
        get { return _crystalManager; }
        set { _crystalManager = value; }
    }

    public LandZoneManager LandZoneManager1
    {
        get { return _landZoneManager; }
        set { _landZoneManager = value; }
    }

    public SubMeshData()
    {
    }
}