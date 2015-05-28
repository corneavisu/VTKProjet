using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using iGUI;


public class MobileUnityInterface : MonoBehaviour {

	// Cornea data, Mesh, ColorMapping
	
	private Vector3[] verticesArrayAnt;
	private int[] trianglesArrayAnt;

	// Collider zones
	private GameObject corneaPivot;
	private Vector3 corneaCenter;

	private GameObject zoneTouchable;
	private GameObject BFSCenterAnt;

	public static Quaternion corneaQuaternion;
	public static Vector3 corneaPosition;
	public static Sphere BFSAnt;

	private GameObject selectedCenter;


	// Use this for initialization
	void Awake() {

		// Import data of the anterior surface
		ImportData corneaData = new ImportData ();
		corneaData.readTxtFile ("_ant");

		// Create the mesh vertices and triangles of the cornea
		CorneaManager cornea = new CorneaManager ("ant");

		this.verticesArrayAnt = cornea.getVerticesAnt ().ToArray ();
		this.trianglesArrayAnt = cornea.getTrianglesAnt ().ToArray ();

		// Set the mesh of the cornea 
		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.Clear ();
		mesh.vertices = this.verticesArrayAnt;
		mesh.triangles = this.trianglesArrayAnt;
		
		
		// Bounds allows me to get the right center point
		// Normals give the light to the mesh
		// Optimization of the mesh 
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		mesh.Optimize ();

		BFSCreate sphereMeshAnt = new BFSCreate (mesh.vertices);
		BFSAnt = sphereMeshAnt.BFSCalculation ();



		// Calculate the center point of the mesh
		this.corneaCenter = new Vector3 (mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z - 40f);
		 
		// Set the cornea object at this point
		this.transform.position = new Vector3 (0f, 0f, this.corneaCenter.z);

		this.BFSCenterAnt = GameObject.FindGameObjectWithTag ("BFSAnt");
		this.BFSCenterAnt.transform.position = new Vector3 (BFSAnt.CX, BFSAnt.CY, BFSAnt.CZ + this.corneaCenter.z);
		
		// Determination of the cornea manipulation area
		Camera.main.transform.position = new Vector3(this.corneaCenter.x, 3.1f, 0f);
		Camera.main.orthographicSize = 5f;
		
		// Create the pivot of the cornea (game object parent of the cornea): Now, I only have to rotate thos object to make the right rotation for the cornea

		this.corneaPivot = GameObject.Find ("corneaPivot");

		this.corneaPivot.transform.position = new Vector3(this.corneaCenter.x, this.corneaCenter.y, CorneaPostSurface.corneaCenterPost.z);

		GameObject BFSCenterPost = GameObject.FindGameObjectWithTag ("BFSPost");
		BFSCenterPost.transform.parent = this.corneaPivot.transform;
		GameObject post = GameObject.FindGameObjectWithTag ("corneaPost");
		post.transform.parent = this.corneaPivot.transform;

		this.corneaPivot.transform.position = this.corneaCenter;

		this.BFSCenterAnt.transform.parent = this.corneaPivot.transform;

		this.transform.parent = this.corneaPivot.transform;
		corneaQuaternion = this.corneaPivot.transform.rotation;
		corneaPosition = this.corneaPivot.transform.position;

		// Create the touchable zone 
		this.zoneTouchable = GameObject.Find ("ZoneCollider");
		BoxCollider zoneCollider = zoneTouchable.AddComponent<BoxCollider> ();
		zoneCollider.center = Vector3.zero;
		zoneCollider.size = new Vector3 (25f, 25f, 0.01f);
	}
}