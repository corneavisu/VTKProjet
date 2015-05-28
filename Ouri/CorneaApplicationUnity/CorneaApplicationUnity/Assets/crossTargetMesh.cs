using System.Collections;
using UnityEngine;

public class crossTargetMesh: MonoBehaviour{

	private Vector3[] vertices;
	private int[] triangles;
	private Color32[] colors;

	void Start()
	{
		this.vertices = new Vector3[17]{new Vector3(0.225f, 0.5f), new Vector3(0.275f, 0.5f), new Vector3(0.225f, 0.275f), new Vector3(0.275f, 0.275f), 
										new Vector3(0f, 0.275f), new Vector3(0f, 0.225f), new Vector3(0.225f, 0.225f), new Vector3(0.225f, 0f), 
										new Vector3(0.275f, 0f), new Vector3(0.275f, 0.225f), new Vector3(0.5f, 0.225f), new Vector3(0.5f, 0.275f), 
			new Vector3(0.25f, 0.4f), new Vector3(0.1f, 0.25f), new Vector3(0.25f, 0.1f), new Vector3(0.4f, 0.25f), new Vector3(0.25f, 0.25f)};
		this.triangles = new int[120]{0, 12, 1, 1, 12, 0,  0, 12, 2, 2, 12, 0,  1, 12, 3, 3, 12, 1,  2, 12, 3, 3, 12, 2,
									2, 13, 4, 4, 13, 2,  4, 13, 5, 5, 13, 4,  5, 13, 6, 6, 13, 5,  6, 13, 2, 2, 13, 6,
									6, 14, 7, 7, 14, 6,  7, 14, 8, 8, 14, 7,  8, 14, 9, 9, 14, 8,  9, 14, 6, 6, 14, 9,
									9, 15, 10, 10, 15, 9,  10, 15, 11, 11, 15, 10,  11, 15, 3, 3, 15, 11,  3, 15, 9, 9, 15, 3,
									2, 3, 16, 16, 3, 2,  2, 6, 16, 16, 6, 2,  6, 9, 16, 16, 9, 6,  9, 3, 16, 16, 3, 9};

		Mesh mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.Clear ();

		mesh.vertices = this.vertices;
		mesh.triangles = this.triangles;

		// Bounds allows me to get the right center point
		// Normals give the light to the mesh
		// Optimization of the mesh 
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		mesh.Optimize ();

		this.colors = new Color32[17];
		for(int i = 0; i < this.colors.Length; i++)
			this.colors[i] = new Color32(0, 0, 0, 255);
		mesh.colors32 = this.colors;
	}
}
