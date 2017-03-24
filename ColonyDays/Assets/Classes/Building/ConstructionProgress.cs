using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ConstructionProgress
{
    private Building _building;
    Vector3[] _vertexes;
    List<General> _show = new List<General>();
    List<GameObject> _empties = new List<GameObject>();

    GameObject _container;

    int _count;
    Vector3 _diff;

    public ConstructionProgress(Building building)
    {
        _building = building;
        _container = new GameObject();
        _container.transform.position = _building.transform.position;
    }




    public void Update()
    {
        if (_vertexes == null && _building != null && 
            !UMath.nearEqualByDistance(_building.transform.position, new Vector3(), 1f))
        {
            _vertexes = _building.Geometry.GetComponent<MeshFilter>().mesh.vertices;
            _diff = _building.transform.position - new Vector3();
        }

        if (_count < _vertexes.Length)
        {
            var cube = UVisHelp.CreateHelpers(_vertexes[_count] + _diff, Root.cube);
            GameObject go = new GameObject();
            go.transform.SetParent(_container.transform);
            go.transform.localPosition = new Vector3();

            cube.transform.SetParent(go.transform);
            
            go.transform.localScale = _building.Geometry.transform.localScale;
            go.transform.localRotation = _building.Geometry.transform.localRotation;
            go.transform.localPosition = _building.Geometry.transform.localPosition;

            _show.Add(cube);
            _empties.Add(go);

            _count++;
        }
    }

    public void Clean()
    {
        for (int i = 0; i < _show.Count; i++)
        {
            _show[i].Destroy();
        }
        _show.Clear();
    }
}
