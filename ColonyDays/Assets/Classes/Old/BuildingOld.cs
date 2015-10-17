using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingOld : MonoBehaviour {

	private Vector3 p0 = new Vector3();
	private Vector3 p1 = new Vector3();
	private Vector3 p2 = new Vector3();

	private Mesh mesh = null;
	private Vector3[] vertices = null;
	private Vector3[] normals = null;
	private int[] triangles = null;
	
	private List<int> vertexToMoveListIndex = new List<int>();
	
	private List<Vector3> AllVertexInXPos = new List<Vector3>();
	private List<int> AllVertexInXIndex = new List<int>();
	
	private bool raySwitch = true;
	
	//algorith to find the building base //building must have a rectangular collider to work
	
	//x
	private int xLeft = 0;
	private int xRight = 0; 
	private float xValue = 0;
	//z
	private int zUp = 0;
	private int zDown = 0;
	private float zValue = 0;
	//y
	private float yValue = -0.51f;
	//other vars
	private Vector3 origin = new Vector3();
	private string currentStep = "";
	private int listCounter = 0;
	private int zVertexCounter = 0;

	void FindRemaindingOnes()
	{
		
	}
	
	/*Carlos Pseudo Code
	point pInicio = 0,0;
	point pFinal = 5,5;

	for(int x= pInicio.X; x < pfinal.x+1; i++)
	{
	  for(int y = pInicio.Y; y<pFinal.Y+1; y=y+2.6)
		{
	   	 (x, y)
		}
	}
	*/

	//find the amount of items in x  lenght for have the size of the buildling base
	//
	void UpdateXLenght(Vector3 originPass, int index)
	{
		//step 1 : xGoToLeft
		if(currentStep == "" || currentStep == "xGoToLeft")
		{
			xLeft++;
			AllVertexInXPos.Add(originPass);//will hold the position
			AllVertexInXIndex.Add (index);
		}
		//step 2 : xGoToRight
		else if(currentStep == "xGoToRight")
		{
			xRight++;
			AllVertexInXPos.Add(originPass);//will hold the position
			AllVertexInXIndex.Add (index);
		}
	}

	private Vector3 MoveOriginOfRayCastOneXRow1stThenAllZ(Vector3 originPass)
	{
		float checkIntervalDistance = 0.2f;
		Vector3 origenHere = new Vector3();
		xValue = originPass.x;
		yValue = originPass.y;
		zValue = originPass.z;
		
		//step 1 : xGoToLeft
		if(currentStep == "" || currentStep == "xGoToLeft")
		{
			xValue = xValue + checkIntervalDistance;
			currentStep = "xGoToLeft";
			origenHere = new Vector3(xValue, yValue, zValue);
		}
		//step 2 : xGoToRight
		else if(currentStep == "xGoToRight")
		{
			xValue = xValue - checkIntervalDistance;
			origenHere = new Vector3(xValue, yValue, zValue);
		}
		//step 3  : zGoUp
		else if(currentStep == "zGoUp")
		{
			zValue = zValue + checkIntervalDistance;
			origenHere = new Vector3(xValue, yValue, zValue);
		}
		//step 4  : zGoDown
		else if(currentStep == "zGoDown")
		{
			print ("zGoDown: zValue: "+zValue);
			zValue = zValue - checkIntervalDistance;
			origenHere = new Vector3(xValue, yValue, zValue);
		}

		return origenHere;
	}
	
	void FindAllVertexBelowBulding()
	{
		Vector3 direction = new Vector3();

		//switch ray direction to top and down
		if(raySwitch)
		{
			raySwitch = false;
			direction = Vector3.up;
		}
		else if(!raySwitch)
		{
			raySwitch = true;
			direction = Vector3.down;
		}

		//if ray hits something on top and down...
		if(RayCastFindDrawSelectVertex(direction, origin))
		{
			//print("mesh: " + mesh);
		
			//if direction = Vector3.down...
			if(direction == Vector3.down)
			{
				FindVerticeIndex(p0, "p0");
				FindVerticeIndex(p1, "p1");
				FindVerticeIndex(p2, "p2");
				origin = MoveOriginOfRayCastOneXRow1stThenAllZ(origin);
			}
		}
		//if ray doesnt hits something on top or down...
		else if(!RayCastFindDrawSelectVertex(direction, origin))
		{
			if(currentStep == "xGoToLeft")
			{
				currentStep = "xGoToRight";
				//and reset origin to center of gameobj
				origin = gameObject.transform.GetChild(1).transform.position;
			}
			else if(currentStep == "xGoToRight")
			{
				currentStep = "zGoUp";
				//and reset origin to center of gameobj
				origin = gameObject.transform.GetChild(1).transform.position;
				
				for (int i = 0; i < AllVertexInXPos.Count; i++) 
				{
					CreateHelper("yellowSphereHelp", AllVertexInXPos[i], AllVertexInXIndex[i]);
				}
			}
			else if(currentStep == "zGoUp")
			{
				//if zVertexCounter == AllVertexInXPos.Count...
				if(zVertexCounter == AllVertexInXPos.Count)
				{
					currentStep = "zGoDown";
					//and reset origin to center of gameobj
					origin = gameObject.transform.GetChild(1).transform.position;
					zVertexCounter = 0;
				}
				//otherwise
				else if(zVertexCounter < AllVertexInXPos.Count)
				{
					origin = AllVertexInXPos[zVertexCounter];
					zVertexCounter++;
				}
			}
			else if(currentStep == "zGoDown")
			{
				//print("in zGoDown: zVertexCounter: "+zVertexCounter+" AllVertexInXPos.Count: "+AllVertexInXPos.Count);
				//print ("AllVertexInXPos[zVertexCounter]: "+AllVertexInXPos[zVertexCounter]);
			
				//if zVertexCounter == AllVertexInXPos.Count...
				if(zVertexCounter == AllVertexInXPos.Count)
				{
					currentStep = "GotDimensions";
					
				}
				//otherwise
				else if(zVertexCounter < AllVertexInXPos.Count)
				{
					origin = AllVertexInXPos[zVertexCounter];
					zVertexCounter++;
				}
			}
		}
	}

	//will return true if hits a collider, otherwise false.
	//will draw the triangle and find the 3 vertex too
	private bool RayCastFindDrawSelectVertex(Vector3 directionPass, Vector3 originPass)
	{
		bool geometryHit = false;
		Vector3 origin = new Vector3();
		
		if(originPass == null || originPass == Vector3.zero)
		{
			origin = gameObject.transform.GetChild(1).gameObject.transform.position;
		}	
		else if(originPass != null)
		{
			origin = originPass;
		}
		
		//print("origin: " + origin.name);
	
		//raycast
		RaycastHit hit;
		//only works if hits something... other wise below if() is null
		if (Physics.Raycast(origin, directionPass, out hit))
		{
			//as long hits something and we are inside this if() this whole method will return true
			geometryHit = true;

			//Draw Ray
			Debug.DrawRay(origin, directionPass, Color.red);
			
			if(currentStep == "zGoDown")
			{
				CreateHelper("rayCastTrace", origin, 0);
				print ( "hit.transform.name: " + hit.transform.name );
			}

			float distance = Vector3.Distance(transform.position, hit.transform.position);
			//print ( "distance: " + distance ) ;
			
			//Draw Triangle
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider == null || meshCollider.sharedMesh == null)
				return geometryHit;

			mesh = meshCollider.sharedMesh;
			vertices = mesh.vertices;
			triangles = mesh.triangles;

			p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
			p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
			p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

			Debug.DrawLine(p0, p1, Color.red);
			Debug.DrawLine(p1, p2, Color.red);
			Debug.DrawLine(p2, p0, Color.red);
		}
		
		return geometryHit;
	}

	void FindVerticeIndex(Vector3 vertex, string name)
	{
		//loop thru all vertices in the mesh
		for (int i = 0; i < vertices.Length; i++)
		{
			//if one vertex in the mesh is the same was passed as parameter 'vertex' then...
			if(vertices[i] == vertex)
			{
				//if list has less than 3 vertex...
				if(vertexToMoveListIndex.Count < 3)
				{
					AddToList(vertex, i);
					CreateHelper("redSphereHelp",vertex, i);
				}
				//other wise
				else if(vertexToMoveListIndex.Count > 2)
				{	//loop thru list to only add a new vertex 
					for (int j = 0; j < vertexToMoveListIndex.Count; j++) 
					{
						//if current vertex in list is not the same that current vertex in mesh
						if(vertexToMoveListIndex[j] != i)
						{
							//we add to list counter
							listCounter++;
						}
						//if listCounter was added the same amount of time tht vertexToMoveList.Count means
						//that was a brand new vertex then ....
						if(listCounter == vertexToMoveListIndex.Count)
						{
							AddToList(vertex, i);
							CreateHelper("redSphereHelp",vertex, i);
						}
					}
					//will reset it to zero for every new vertex in mesh
					listCounter=0;
				}
			}
		}
	}
	
	void AddToList(Vector3 vertex, int index)
	{
		//print (name + " found in vert[]: " + i.ToString());
		vertexToMoveListIndex.Add(index);//will hold the vertex index
		UpdateXLenght(vertex, index);
	}
	
	void CreateHelper(string helperType, Vector3 vertex, int index)
	{
		GameObject redSphereHelp = (GameObject)Resources.Load("Prefab/Helper/RedSphereHelp", typeof(GameObject));
		GameObject yellowSphereHelp = (GameObject)Resources.Load("Prefab/Helper/YellowSphereHelp", typeof(GameObject));
		GameObject rayCastTrace = (GameObject)Resources.Load("Prefab/Helper/RayCastTrace", typeof(GameObject));
		GameObject clone = null;
		
		if(helperType == "redSphereHelp")
		{
			clone = (GameObject)Instantiate(redSphereHelp, vertex, Quaternion.identity);
		}
		else if(helperType == "yellowSphereHelp")
		{
			clone = (GameObject)Instantiate(yellowSphereHelp, vertex, Quaternion.identity);
		}
		else if(helperType == "rayCastTrace")
		{
			clone = (GameObject)Instantiate(rayCastTrace, vertex, Quaternion.identity);
		}
		
		clone.name = helperType  + ". in vertex: " + index.ToString();
	}
	
	//unity 
	// Use this for initialization
	void Start () 
	{
		//origin to center of gameobj
		origin = gameObject.transform.GetChild(1).transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		print ("currentStep: "+currentStep);
	
		if(currentStep != "GotDimensions")
		{
			FindAllVertexBelowBulding();
		}
		else if(currentStep == "GotDimensions")
		{
			FindRemaindingOnes();
		}
		
		if(Input.GetMouseButtonUp(0) || Input.GetKeyUp("f"))
		{
			FindAllVertexBelowBulding();
		}
		
		if(Input.GetMouseButtonUp(1))
		{
			print("zVertexCounter: "+zVertexCounter +" AllVertexInXPos.Count: "+ AllVertexInXPos.Count);
			print ("currentStep: " + currentStep);
			print ("origin: " + origin);
			//print ("listCounter: "  + listCounter);
			print ("vertexToMoveList.Count: "+vertexToMoveListIndex.Count);
			print ("xLeft: " + xLeft + " xRight: "+xRight + " zUp: " + zUp + " zDown: " + zDown);
			for (int i = 0; i < AllVertexInXIndex.Count; i++) 
			{
				print ("AllVertexInXIndex["+i+"]:"+AllVertexInXIndex[i]);
			}
		}
	}
	
	
	//old methods
	/*
	
	private Vector3 MoveOriginOfRayCastInCross(Vector3 originPass)
	{
		float checkIntervalDistance = 0.2f;
		Vector3 origenHere = new Vector3();
		xValue = originPass.x;
		yValue = originPass.y;
		zValue = originPass.z;
		
		//step 1 : xGoToLeft
		if(currentStep == "" || currentStep == "xGoToLeft")
		{
			
			xValue = xValue + checkIntervalDistance;
			currentStep = "xGoToLeft";
			origenHere = new Vector3(xValue, yValue, zValue);
		}
		//step 2 : xGoToRight
		else if(currentStep == "xGoToRight")
		{

			xValue = xValue - checkIntervalDistance;
			origenHere = new Vector3(xValue, yValue, zValue);
		}
		//step 3  : zGoUp
		else if(currentStep == "zGoUp")
		{

			zValue = zValue + checkIntervalDistance;
			origenHere = new Vector3(xValue, yValue, zValue);
		}
		//step 4  : zGoDown
		else if(currentStep == "zGoDown")
		{

			zValue = zValue - checkIntervalDistance;
			origenHere = new Vector3(xValue, yValue, zValue);
		}

		return origenHere;
	}
	
		
	private Vector3 hit1 = new Vector3();
	private Vector3 hit2 = new Vector3();
	private Vector3 hit3 = new Vector3();
	
	private int vertexIndex = 0;
	
	void CheckHit()
	{	
		RaycastHit hit;
		GameObject origin = gameObject.transform.GetChild(1).gameObject;
		
		if (!Physics.Raycast(origin.transform.position, Vector3.down, out hit))
			return;
		
		MeshCollider MC = hit.collider as MeshCollider;
		if (MC != null)
		{
			Mesh mesh = MC.sharedMesh;
			vertexIndex = hit.triangleIndex * 3;
			
			hit1 = mesh.vertices[mesh.triangles[vertexIndex    ]];
			hit2 = mesh.vertices[mesh.triangles[vertexIndex + 1]];
			hit3 = mesh.vertices[mesh.triangles[vertexIndex + 2]];
			
			print ("hit1:" + hit1.ToString());
			print ("hit2:" + hit2.ToString());
			print ("hit3:" + hit3.ToString());
			
			print ("vertexIndex1: " + vertexIndex);
		}
	}
	*/
	
	
}
