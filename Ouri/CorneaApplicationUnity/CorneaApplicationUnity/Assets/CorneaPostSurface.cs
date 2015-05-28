using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using iGUI;

public class CorneaPostSurface : MonoBehaviour {

	// Cornea data, Mesh, ColorMapping

	private Vector3[] verticesArrayPost;
	private int[] trianglesArrayPost;
	
	// Collider zones
	private GameObject corneaPivot;
	public static Vector3 corneaCenterPost;
	
	private GameObject zoneTouchable;
	private GameObject BFSCenterPost;

	public static Sphere BFSPost;

	
	// Use this for initialization
	void Awake() {

		// Import data of the anterior surface
		ImportData corneaData = new ImportData ();
		corneaData.readTxtFile ("_pos");
		
		// Create the mesh vertices and triangles of the cornea
		CorneaManager cornea = new CorneaManager ("post");

		this.verticesArrayPost = cornea.getVerticesPost ().ToArray ();
		this.trianglesArrayPost = cornea.getTrianglesPost ().ToArray ();
		
		// Set the mesh of the cornea 
		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.Clear ();
		mesh.vertices = this.verticesArrayPost;
		mesh.triangles = this.trianglesArrayPost;
		
		
		// Bounds allows me to get the right center point
		// Normals give the light to the mesh
		// Optimization of the mesh 
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		mesh.Optimize ();

		BFSCreate sphereMeshPost = new BFSCreate (mesh.vertices);
		BFSPost = sphereMeshPost.BFSCalculation ();
		


		this.renderer.enabled = false;
		// Calculate the center point of the mesh
		corneaCenterPost = new Vector3 (mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z - 40f);
		 
		// Set the cornea object at this point
		this.transform.position = new Vector3 (0f, 0f, corneaCenterPost.z);

		this.BFSCenterPost = GameObject.FindGameObjectWithTag ("BFSPost");
		this.BFSCenterPost.transform.position = new Vector3 (BFSPost.CX, BFSPost.CY, BFSPost.CZ + corneaCenterPost.z);
	}
}
