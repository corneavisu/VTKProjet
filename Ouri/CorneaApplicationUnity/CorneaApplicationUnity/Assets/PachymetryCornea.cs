using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using iGUI;

public class PachymetryCornea : MonoBehaviour {

	// Cornea data, Mesh, ColorMapping

	private Vector3[] verticesArrayPach;
	private int[] trianglesArrayPach;
	public static float[] elevationArrayPach;
	
	// Collider zones
	private GameObject corneaPivot;
	private Vector3 corneaCenter;
	
	private GameObject zoneTouchable;


	
	// Use this for initialization
	void Awake() {

		// Import data of the anterior surface
		ImportData corneaData = new ImportData ();
		corneaData.readTxtFile ("_pk");
		
		// Create the mesh vertices and triangles of the cornea
		CorneaManager cornea = new CorneaManager ("pach");
		
		this.verticesArrayPach = cornea.getVerticesPach ().ToArray ();
		this.trianglesArrayPach = cornea.getTrianglesPach ().ToArray ();
		elevationArrayPach = cornea.getElevationPach ().ToArray ();
		
		// Set the mesh of the cornea 
		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.Clear ();
		mesh.vertices = this.verticesArrayPach;
		mesh.triangles = this.trianglesArrayPach;
		
		
		// Bounds allows me to get the right center point
		// Normals give the light to the mesh
		// Optimization of the mesh 
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		mesh.Optimize ();

		
		this.renderer.enabled = false;
		// Calculate the center point of the mesh
		this.corneaCenter = new Vector3 (mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z - 40f);
		
		// Set the cornea object at this point
		this.transform.position = new Vector3 (0f, 0f, this.corneaCenter.z);
		
		
		// Create the pivot of the cornea (game object parent of the cornea): Now, I only have to rotate thos object to make the right rotation for the cornea
		
		this.corneaPivot = GameObject.Find ("corneaPivot");
		this.corneaPivot.transform.position = this.corneaCenter;
		this.transform.parent = this.corneaPivot.transform;
	}
}
