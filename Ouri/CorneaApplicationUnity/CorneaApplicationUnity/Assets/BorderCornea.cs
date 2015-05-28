using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class BorderCornea : MonoBehaviour {

	private Vector3[] vertices;
	private List<int> trianglesUnion;
	private List<int> trianglesIntersect;
	private Vector3 corneaCenter;
	private GameObject corneaPivot;
	private GameObject antBorderLine;
	private GameObject postBorderLine;
	private GameObject corneaAntIntersect;

	private List<float> antElevationMap;
	private List<float> postElevationMap;
	private Vector3[] volumeAntDeformationVertices;
	private List<Vector3> antBorderLineIntersect;
	private Vector3[] postVertices;
	public static List<float> volumeFactorDeformation1;
	public static List<float> volumeFactorDeformation2;
	public static List<float> borderVolumeFactorDeformation;
	public static Color32[] elevationColorMapAnt;
	public static Color32[] elevationColorMapPost;
	public static Color32[] elevationSmoothColorMapAnt;
	public static Color32[] elevationSmoothColorMapPost;

	private const int NBSommets = 101;
	private int[,] indexesAnt;
	private int[,] indexesPost;
	private String filename;
	private float centerDifference;

	//public static Vector3[] borderAntIntersectVertices;
	//public static Vector3[] borderPostIntersectVertices;
	
	// Use this for initialization
	void Start () 
	{
		//
		List<Vector3> borderVerticesAnt = CorneaMeshCreate.borderVerticesAnt;
		GameObject ant = GameObject.FindGameObjectWithTag ("corneaCollider");
		Vector3 centerAnt = ant.GetComponent<MeshFilter> ().mesh.bounds.center;


		borderVerticesAnt.Sort(delegate(Vector3 x, Vector3 y) {
			return less(x, y, centerAnt);
		});

		List<Vector3> borderVerticesPost = CorneaMeshCreate.borderVerticesPost;
		GameObject post = GameObject.FindGameObjectWithTag ("corneaPost");
		this.postVertices = post.GetComponent<MeshFilter>().mesh.vertices;
		int[] antVolumeTriangles = post.GetComponent<MeshFilter>().mesh.triangles;
		this.volumeAntDeformationVertices = new Vector3[this.postVertices.Length];

		Vector3 centerPost = post.GetComponent<MeshFilter> ().mesh.bounds.center;

		borderVerticesPost.Sort(delegate(Vector3 x, Vector3 y) {
			return less(x, y, centerPost);
		});

		// Calcul de la différence entre les centre des deux surfaces pour la déformation 1
		this.centerDifference = centerAnt.z - centerPost.z;

		this.postBorderLine = GameObject.FindGameObjectWithTag ("postBorderLine");

		this.trianglesUnion = constructBorderVolume (borderVerticesAnt, borderVerticesPost);

		this.vertices = new Vector3[borderVerticesAnt.Count + borderVerticesPost.Count];
		for(int i = 0; i < borderVerticesAnt.Count; i++)
		{
			this.vertices[i] = borderVerticesAnt[i];
		}
		for(int i = 0; i < borderVerticesPost.Count; i++)
		{
			this.vertices[borderVerticesAnt.Count + i] = borderVerticesPost[i];
		}


		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.vertices = vertices;
		mesh.triangles = this.trianglesUnion.ToArray ();

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


		this.antBorderLine = GameObject.FindGameObjectWithTag ("antBorderLine");
		this.antBorderLine.transform.position = new Vector3 (0f, 0f, this.corneaCenter.z);

		LineRenderer antLine = this.antBorderLine.GetComponent<LineRenderer> ();
		antLine.SetVertexCount (borderVerticesAnt.Count + 1);
		for(int i = 0; i < borderVerticesAnt.Count; i++)
			antLine.SetPosition(i, borderVerticesAnt[i]);
		antLine.SetPosition (borderVerticesAnt.Count, borderVerticesAnt [0]);
		antLine.SetColors (Color.black, Color.black);
		antLine.SetWidth (0.01f, 0.01f);
		antLine.renderer.enabled = false;


		this.postBorderLine = GameObject.FindGameObjectWithTag ("postBorderLine");
		this.postBorderLine.transform.position = new Vector3 (0f, 0f, this.corneaCenter.z);
		
		LineRenderer postLine = this.postBorderLine.GetComponent<LineRenderer> ();
		postLine.SetVertexCount (borderVerticesPost.Count + 1);
		for(int i = 0; i < borderVerticesPost.Count; i++)
			postLine.SetPosition(i, borderVerticesPost[i]);
		postLine.SetPosition (borderVerticesPost.Count, borderVerticesPost [0]);
		
		postLine.SetColors (Color.black, Color.black);
		postLine.SetWidth (0.01f, 0.01f);
		postLine.renderer.enabled = false;

		this.antBorderLineIntersect = new List<Vector3>();

		// Création de la carte d'élévation
		createElevationMap (ImportData.anteriorDatas, ImportData.posteriorDatas);

		// Tri des sommets de la bordure intersect antérieure
		this.antBorderLineIntersect.Sort(delegate(Vector3 x, Vector3 y) {
			return less(x, y, centerAnt);
		});

		this.trianglesIntersect = constructBorderVolume(this.antBorderLineIntersect, borderVerticesPost);

		Vector3[] borderIntersectVertices = new Vector3[this.antBorderLineIntersect.Count + borderVerticesPost.Count];
		//borderAntIntersectVertices = new Vector3[antBorderLineIntersect.Count];
		//borderPostIntersectVertices = new Vector3[borderVerticesPost.Count];

		GameObject borderIntersect = GameObject.FindGameObjectWithTag("CorneaBorderIntersect");
		Mesh borderIntersectMesh = borderIntersect.GetComponent<MeshFilter> ().mesh;

		
		for(int i = 0; i < this.antBorderLineIntersect.Count; i++)
		{
			borderIntersectVertices[i] = this.antBorderLineIntersect[i];
			//borderAntIntersectVertices[i] = this.antBorderLineIntersect[i];
		}
		for(int i = 0; i < borderVerticesPost.Count; i++)
		{
			borderIntersectVertices[this.antBorderLineIntersect.Count + i] = borderVerticesPost[i];
			//borderPostIntersectVertices[i] = borderVerticesPost[i];
		}
		

		borderIntersectMesh.vertices = borderIntersectVertices;
		borderIntersectMesh.triangles = this.trianglesIntersect.ToArray ();

		// Bounds allows me to get the right center point
		// Normals give the light to the mesh
		// Optimization of the mesh 
		borderIntersectMesh.RecalculateBounds ();
		borderIntersectMesh.RecalculateNormals ();
		borderIntersectMesh.Optimize ();

		calculateBorderVolumeDeformationFactor(borderIntersectVertices, borderIntersectMesh.bounds.center.z);
		
		borderIntersect.renderer.enabled = false;
		// Set the cornea object at this point
		borderIntersect.transform.position = new Vector3 (0f, 0f, this.corneaCenter.z);


		// Creation du mesh de la surface antérieure intersectée avec la surface postérieure et assignation des couleures
		this.corneaAntIntersect = GameObject.FindGameObjectWithTag("CorneaAntIntersect");
		Mesh corneaAntIntersectMesh = new Mesh();
		corneaAntIntersectMesh.vertices = this.volumeAntDeformationVertices;
		corneaAntIntersectMesh.triangles = antVolumeTriangles;
		this.corneaAntIntersect.GetComponent<MeshFilter>().mesh = corneaAntIntersectMesh;
		this.corneaAntIntersect.GetComponent<MeshRenderer>().enabled = false;
		corneaAntIntersectMesh.RecalculateBounds ();
		corneaAntIntersectMesh.RecalculateNormals ();
		corneaAntIntersectMesh.Optimize ();

		this.corneaAntIntersect.transform.position = new Vector3(0f, 0f, this.corneaCenter.z);

		// Create the pivot of the cornea (game object parent of the cornea): Now, I only have to rotate thos object to make the right rotation for the cornea
		
		this.corneaPivot = GameObject.Find ("corneaPivot");
		
		this.corneaPivot.transform.position = this.corneaCenter;
		this.transform.parent = this.corneaPivot.transform;
		this.antBorderLine.transform.parent = this.corneaPivot.transform;
		this.postBorderLine.transform.parent = this.corneaPivot.transform;
		this.corneaAntIntersect.transform.parent = this.corneaPivot.transform;
		borderIntersect.transform.parent = this.corneaPivot.transform;



		ElevationMapColors elevationMapColors = new ElevationMapColors ();

		elevationColorMapAnt = elevationMapColors.calculateColor (this.antElevationMap.ToArray ());
		elevationColorMapPost = elevationMapColors.calculateColor (this.postElevationMap.ToArray ());
		//elevationColorMapAntIntersect = elevationMapColors.calculateColor(this.antElevationMap.ToArray());
		elevationSmoothColorMapAnt = elevationMapColors.calculateColorSmooth (this.antElevationMap.ToArray ());
		elevationSmoothColorMapPost = elevationMapColors.calculateColorSmooth (this.postElevationMap.ToArray ());
	}

	private void calculateBorderVolumeDeformationFactor(Vector3[] borderVerticesIntersect, float centerElevation)
	{
		borderVolumeFactorDeformation = new List<float>();

		for(int i = 0; i < borderVerticesIntersect.Length; i++)
			borderVolumeFactorDeformation.Add(borderVerticesIntersect[i].z - centerElevation);
	}

	private void initSavedIndexes(int[,] indexes) {
		for (int i = 0; i < indexes.GetLength(0); i++)
			for (int j = 0; j < indexes.GetLength(1); j++)
				indexes [i, j] = -1;
	}

	public void createElevationMap(float[] sommetsAnt, float[] sommetsPost) {
		//Read the file as one string considering every 101 numbers as a sequence. Create the final array.

		this.indexesAnt = new int[NBSommets, NBSommets];
		this.indexesPost = new int[NBSommets, NBSommets];
		this.initSavedIndexes (this.indexesAnt);
		this.initSavedIndexes (this.indexesPost);
		this.antElevationMap = new List<float> ();
		this.postElevationMap = new List<float>();


		int compteur = NBSommets + 1;

		int i = 1;
		int j = 1;
		
		while (compteur <= (NBSommets - 2) * (NBSommets - 2))
		{
			if(i == NBSommets) // 
			{
				// Fin d'une séquence, retour à la ligne
				i = 1;
				compteur += NBSommets + 1; /* Ici le compteur doit être incrémenté de 102 
				pour ne pas lier le maillage gauche de la nouvelle ligne à celui de droite de la ligne précédente*/
			}

			if (sommetsAnt[compteur] != -1000 && sommetsPost[compteur] != -1000) 
			{

				// sommet central de la zone étudiée
				float sommetCentreAnt = sommetsAnt[compteur];
				float sommetCentrePost = sommetsPost[compteur];

				antElevationMap.Add(sommetCentreAnt - sommetCentrePost);

				this.indexesAnt[i, j] = 1;
				if(sommetsAnt[compteur - (NBSommets + 1)] != -1000 && sommetsPost[compteur - (NBSommets + 1)] != -1000 && 
				   sommetsAnt[compteur - 1] != -1000 && sommetsPost[compteur - 1] != -1000 &&
				   sommetsAnt[compteur - NBSommets] != -1000 && sommetsPost[compteur - NBSommets] != -1000)
				{
					// Carré bas-gauche connu: calcul du centre et création des triangles
					float sg2Ant, sg2Post, sbg3Ant, sbg3Post, sb2Ant, sb2Post;
					
					sg2Ant = sommetsAnt[compteur - 1];
					sg2Post = sommetsPost[compteur - 1];
					sbg3Ant = sommetsAnt[compteur - (NBSommets + 1)];
					sbg3Post = sommetsPost[compteur - (NBSommets + 1)];
					sb2Ant = sommetsAnt[compteur - NBSommets];
					sb2Post = sommetsPost[compteur - NBSommets];
					float centerAnt = (sbg3Ant + sg2Ant + sommetCentreAnt + sb2Ant)/4;
					float centerPost = (sbg3Post + sg2Post + sommetCentrePost + sb2Post)/4;

					antElevationMap.Add(centerAnt - centerPost);

					if(this.indexesAnt[i - 1, j] == -1)
					{
						antElevationMap.Add(sg2Ant - sg2Post);
						this.indexesAnt[i - 1, j] = 1;
					}
					if(this.indexesAnt[i, j - 1] == -1)
					{
						antElevationMap.Add(sb2Ant - sb2Post);
						this.indexesAnt[i, j - 1] = 1;
					}
					if(this.indexesAnt[i - 1, j - 1] == -1)
					{
						antElevationMap.Add(sbg3Ant - sbg3Post);
						this.indexesAnt[i - 1, j - 1] = 1;
					}
				}
				else
				{
					if(sommetsAnt[compteur - (NBSommets + 1)] != -1000 &&
				   	sommetsAnt[compteur - 1] != -1000 &&
				   	sommetsAnt[compteur - NBSommets] != -1000)
					{
						antElevationMap.Add(-1000f);

						if(this.indexesAnt[i - 1, j] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i - 1, j] = 1;
						}
						if(this.indexesAnt[i, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i, j - 1] = 1;
						}
						if(this.indexesAnt[i - 1, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i - 1, j - 1] = 1;
						}
					}
				}

				if(sommetsAnt[compteur - NBSommets] != -1000 && sommetsPost[compteur - NBSommets] != -1000 && 
				   sommetsAnt[compteur + 1] != -1000 && sommetsPost[compteur + 1] != -1000 &&
				   sommetsAnt[compteur + (1- NBSommets)] != -1000 && sommetsPost[compteur + (1 - NBSommets)] != -1000)
				{
					// Carré bas-droit connu: calcul du centre et création des triangles
					float sbd3Ant, sbd3Post, sd2Ant, sd2Post, sb2Ant, sb2Post;
					
					sbd3Ant = sommetsAnt[compteur + (1 - NBSommets)];
					sbd3Post = sommetsPost[compteur + (1 - NBSommets)];
					sd2Ant = sommetsAnt[compteur + 1];
					sd2Post = sommetsPost[compteur + 1];
					sb2Ant = sommetsAnt[compteur - NBSommets];
					sb2Post = sommetsPost[compteur - NBSommets];
					float centerAnt = (sb2Ant + sommetCentreAnt + sd2Ant + sbd3Ant)/4;
					float centerPost = (sb2Post + sommetCentrePost + sd2Post + sbd3Post)/4;

					antElevationMap.Add(centerAnt - centerPost);

					antElevationMap.Add(sd2Ant - sd2Post);
					this.indexesAnt[i + 1, j] = 1;

					if(this.indexesAnt[i + 1, j - 1] == -1)
					{
						antElevationMap.Add(sbd3Ant - sbd3Post);
						this.indexesAnt[i + 1, j - 1] = 1;
					}
					if(this.indexesAnt[i, j - 1] == -1)
					{
						antElevationMap.Add(sb2Ant - sb2Post);
						this.indexesAnt[i, j - 1] = 1;
					}
				}

				else 
				{
					if(sommetsAnt[compteur - NBSommets] != -1000 &&
				   	sommetsAnt[compteur + 1] != -1000 &&
				   	sommetsAnt[compteur + (1- NBSommets)] != -1000)
					{
						antElevationMap.Add(-1000f);
						
						antElevationMap.Add(-1000f);
						this.indexesAnt[i + 1, j] = 1;
						
						if(this.indexesAnt[i + 1, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i + 1, j - 1] = 1;
						}
						if(this.indexesAnt[i, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i, j - 1] = 1;
						}
					}
				}
				
				if(sommetsAnt[compteur - 1] != -1000 && sommetsPost[compteur - 1] != -1000 && 
				   sommetsAnt[compteur + (NBSommets - 1)] != -1000 && sommetsPost[compteur + (NBSommets - 1)] != -1000 &&
				   sommetsAnt[compteur + NBSommets] != -1000 && sommetsPost[compteur + NBSommets] != -1000)
				{
					// Carré haut-gauche connu: calcul du centre et création des triangles
					float shg1Ant, shg1Post, sh2Ant, sh2Post, sg2Ant, sg2Post;
					shg1Ant = sommetsAnt[compteur + (NBSommets - 1)];
					shg1Post = sommetsPost[compteur + (NBSommets - 1)];
					sh2Ant = sommetsAnt[compteur + NBSommets];
					sh2Post = sommetsPost[compteur + NBSommets];
					sg2Ant = sommetsAnt[compteur - 1];
					sg2Post = sommetsPost[compteur - 1];
					float centerAnt = (sommetCentreAnt + sg2Ant + shg1Ant + sh2Ant)/4;
					float centerPost = (sommetCentrePost + sg2Post + shg1Post + sh2Post)/4;

					antElevationMap.Add(centerAnt - centerPost);

					antElevationMap.Add(sh2Ant - sh2Post);
					indexesAnt[i, j + 1] = 1;

					if(this.indexesAnt[i - 1, j] == -1)
					{
						antElevationMap.Add(sg2Ant - sg2Post);
						this.indexesAnt[i - 1, j] = 1;
					}
					if(indexesAnt[i - 1, j + 1] == -1)
					{
						antElevationMap.Add(shg1Ant - shg1Post);
						indexesAnt[i - 1, j + 1] = 1;
					}
				}

				else 
				{
					if(sommetsAnt[compteur - 1] != -1000 &&
				   	sommetsAnt[compteur + (NBSommets - 1)] != -1000 &&
				   	sommetsAnt[compteur + NBSommets] != -1000)
					{
						antElevationMap.Add(-1000f);
	
						antElevationMap.Add(-1000f);
						indexesAnt[i, j + 1] = 1;
						
						if(this.indexesAnt[i - 1, j] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i - 1, j] = 1;
						}
						if(indexesAnt[i - 1, j + 1] == -1)
						{
							antElevationMap.Add(-1000f);
							indexesAnt[i - 1, j + 1] = 1;
						}
					}
				}

				if(sommetsAnt[compteur + NBSommets] != -1000 && sommetsPost[compteur + NBSommets] != -1000 && 
				   sommetsAnt[compteur + (1 + NBSommets)] != -1000 && sommetsPost[compteur + (1 + NBSommets)] != -1000 &&
				   sommetsAnt[compteur + 1] != -1000 && sommetsPost[compteur + 1] != -1000)
				{
					// Carré haut-droit connu: calcul du centre et création des triangles
					float shd1Ant, shd1Post, sh2Ant, sh2Post, sd2Ant, sd2Post;
					shd1Ant = sommetsAnt[compteur + (1 + NBSommets)];
					shd1Post = sommetsPost[compteur + (1 + NBSommets)];
					sh2Ant = sommetsAnt[compteur + NBSommets];
					sh2Post = sommetsPost[compteur + NBSommets];
					sd2Ant = sommetsAnt[compteur + 1];
					sd2Post = sommetsPost[compteur + 1];
					float centerAnt = (sommetCentreAnt + sh2Ant + shd1Ant + sd2Ant)/4;
					float centerPost = (sommetCentrePost + sh2Post + shd1Post + sd2Post)/4;

					antElevationMap.Add(centerAnt - centerPost);

					antElevationMap.Add(shd1Ant - shd1Post);
					indexesAnt[i + 1, j+ 1] = 1;

					if(indexesAnt[i, j + 1] == -1)
					{
						antElevationMap.Add(sh2Ant - sh2Post);
						indexesAnt[i, j + 1] = 1;
					}
					if(this.indexesAnt[i + 1, j] == -1)
					{
						antElevationMap.Add(sd2Ant- sd2Post);
						this.indexesAnt[i + 1, j] = 1;
					}
				}

				else 
				{
					 if(sommetsAnt[compteur + NBSommets] != -1000 &&
				   	sommetsAnt[compteur + (1 + NBSommets)] != -1000 &&
				   	sommetsAnt[compteur + 1] != -1000)
					{
						antElevationMap.Add(-1000f);
	
						antElevationMap.Add(-1000f);
						indexesAnt[i + 1, j+ 1] = 1;
						
						if(indexesAnt[i, j + 1] == -1)
						{
							antElevationMap.Add(-1000f);
							indexesAnt[i, j + 1] = 1;
						}
						if(this.indexesAnt[i + 1, j] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i + 1, j] = 1;
						}
					}
				}
			}
			else
			{
				if(sommetsAnt[compteur] != -1000)
				{
					// sommet central de la zone étudiée
				
					antElevationMap.Add(-1000f);
					this.indexesAnt[i, j] = 1;
					if(sommetsAnt[compteur - (NBSommets + 1)] != -1000 && 
				   		sommetsAnt[compteur - 1] != -1000 &&
				   		sommetsAnt[compteur - NBSommets] != -1000)
					{
						// Carré bas-gauche connu: calcul du centre et création des triangles
	
						antElevationMap.Add(-1000f);
					
						if(this.indexesAnt[i - 1, j] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i - 1, j] = 1;
						}
						if(this.indexesAnt[i, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i, j - 1] = 1;
						}
						if(this.indexesAnt[i - 1, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i - 1, j - 1] = 1;
						}
					}
				
					if(sommetsAnt[compteur - NBSommets] != -1000 &&
				   		sommetsAnt[compteur + 1] != -1000 &&
				   		sommetsAnt[compteur + (1- NBSommets)] != -1000)
					{
						// Carré bas-droit connu: calcul du centre et création des triangles
					
						antElevationMap.Add(-1000f);
						
						antElevationMap.Add(-1000f);
						this.indexesAnt[i + 1, j] = 1;
					
						if(this.indexesAnt[i + 1, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i + 1, j - 1] = 1;
						}
						if(this.indexesAnt[i, j - 1] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i, j - 1] = 1;
						}
					}
				
					if(sommetsAnt[compteur - 1] != -1000 &&
				   		sommetsAnt[compteur + (NBSommets - 1)] != -1000 &&
				   		sommetsAnt[compteur + NBSommets] != -1000)
					{	
						// Carré haut-gauche connu: calcul du centre et création des triangles
					
						antElevationMap.Add(-1000f);
					
						antElevationMap.Add(-1000f);
						indexesAnt[i, j + 1] = 1;
					
						if(this.indexesAnt[i - 1, j] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i - 1, j] = 1;
						}
						if(indexesAnt[i - 1, j + 1] == -1)
						{
							antElevationMap.Add(-1000f);
							indexesAnt[i - 1, j + 1] = 1;
						}
					}
				
					if(sommetsAnt[compteur + NBSommets] != -1000 &&
				   		sommetsAnt[compteur + (1 + NBSommets)] != -1000 &&
					   	sommetsAnt[compteur + 1] != -1000)
					{
						// Carré haut-droit connu: calcul du centre et création des triangles
					
						antElevationMap.Add(-1000f);
						
						antElevationMap.Add(-1000f);
						indexesAnt[i + 1, j+ 1] = 1;
					
						if(indexesAnt[i, j + 1] == -1)
						{
							antElevationMap.Add(-1000f);
							indexesAnt[i, j + 1] = 1;
						}
						if(this.indexesAnt[i + 1, j] == -1)
						{
							antElevationMap.Add(-1000f);
							this.indexesAnt[i + 1, j] = 1;
						}
					}
				}
			}

			i += 2;
			if(i == NBSommets)
				j += 2;
			
			compteur += 2; 
		}

		// Création également du mesh de la surface antérieure qui est l'intersection avec la surface postérieure

		compteur = NBSommets + 1;
		
		i = 1;
		j = 1;
		
		while (compteur <= (NBSommets - 2) * (NBSommets - 2))
		{
			if(i == NBSommets) // 
			{
				// Fin d'une séquence, retour à la ligne
				i = 1;
				compteur += NBSommets + 1; /* Ici le compteur doit être incrémenté de 102 
				pour ne pas lier le maillage gauche de la nouvelle ligne à celui de droite de la ligne précédente*/
			}
			
			if (sommetsAnt[compteur] != -1000 && sommetsPost[compteur] != -1000) 
			{
				
				// sommet central de la zone étudiée
				float sommetCentreAnt = sommetsAnt[compteur];
				float sommetCentrePost = sommetsPost[compteur];
				
				postElevationMap.Add(sommetCentreAnt - sommetCentrePost);
				int indexVertex = postElevationMap.Count - 1;
				Vector3 vertexPost = this.postVertices[indexVertex];
				this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sommetCentreAnt);

				if((sommetsPost[compteur - (NBSommets + 1)] != -1000 && 
				    sommetsPost[compteur - 1] != -1000 &&
				    sommetsPost[compteur - NBSommets] != -1000) ||
				   (sommetsPost[compteur - NBSommets] != -1000 && 
				 sommetsPost[compteur + 1] != -1000 &&
				 sommetsPost[compteur + (1- NBSommets)] != -1000) ||
				   (sommetsPost[compteur - 1] != -1000 && 
				 sommetsPost[compteur + (NBSommets - 1)] != -1000 &&
				 sommetsPost[compteur + NBSommets] != -1000) ||
				   (sommetsPost[compteur + NBSommets] != -1000 && 
				 sommetsPost[compteur + (1 + NBSommets)] != -1000 &&
				 sommetsPost[compteur + 1] != -1000))
				{
					if(sommetsPost[compteur - (NBSommets + 1)] == -1000 || i - 1 == 0 ||
					   sommetsPost[compteur + (1- NBSommets)] == -1000 || j - 1 == 0 ||
					   sommetsPost[compteur + (NBSommets - 1)] == -1000 || i + 1 == NBSommets - 1 ||
					   sommetsPost[compteur + (1 + NBSommets)] == -1000 || j + 1 == NBSommets - 1 ||
					   sommetsPost[compteur - NBSommets] == -1000 || sommetsPost[compteur - 1] == -1000 ||
					   sommetsPost[compteur + NBSommets] == -1000 || sommetsPost[compteur + 1] == -1000)
						this.antBorderLineIntersect.Add(new Vector3(vertexPost.x, vertexPost.y, sommetCentreAnt));
				}

				this.indexesPost[i, j] = 1;
				if(sommetsAnt[compteur - (NBSommets + 1)] != -1000 && sommetsPost[compteur - (NBSommets + 1)] != -1000 && 
				   sommetsAnt[compteur - 1] != -1000 && sommetsPost[compteur - 1] != -1000 &&
				   sommetsAnt[compteur - NBSommets] != -1000 && sommetsPost[compteur - NBSommets] != -1000)
				{
					// Carré bas-gauche connu: calcul du centre et création des triangles
					float sg2Ant, sg2Post, sbg3Ant, sbg3Post, sb2Ant, sb2Post;
					
					sg2Ant = sommetsAnt[compteur - 1];
					sg2Post = sommetsPost[compteur - 1];
					sbg3Ant = sommetsAnt[compteur - (NBSommets + 1)];
					sbg3Post = sommetsPost[compteur - (NBSommets + 1)];
					sb2Ant = sommetsAnt[compteur - NBSommets];
					sb2Post = sommetsPost[compteur - NBSommets];
					float centerAnt = (sbg3Ant + sg2Ant + sommetCentreAnt + sb2Ant)/4;
					float centerPost = (sbg3Post + sg2Post + sommetCentrePost + sb2Post)/4;
					
					postElevationMap.Add(centerAnt - centerPost);
					indexVertex = postElevationMap.Count - 1;
					vertexPost = this.postVertices[indexVertex];
					this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, centerAnt);
					
					if(this.indexesPost[i - 1, j] == -1)
					{
						postElevationMap.Add(sg2Ant - sg2Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sg2Ant);
						this.indexesPost[i - 1, j] = 1;

						if(i - 1 == 0 || j - 1 == 0 || sommetsPost[compteur - 2 + NBSommets] == -1000 || sommetsPost[compteur - 2] == -1000 || sommetsPost[compteur - 2 - NBSommets] == -1000 || sommetsPost[compteur - 1 + NBSommets] == -1000 || sommetsPost[compteur + NBSommets] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x - 0.1f, vertexPost.y, sg2Ant));
					}
					if(this.indexesPost[i, j - 1] == -1)
					{
						postElevationMap.Add(sb2Ant - sb2Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sb2Ant);
						this.indexesPost[i, j - 1] = 1;

						if(j - 1 == 0 || i - 1 == 0 || sommetsPost[compteur - 1 - (2 * NBSommets)] == -1000 || sommetsPost[compteur - (2 * NBSommets)] == -1000 || sommetsPost[compteur + 1 - (2 * NBSommets)] == -1000 || sommetsPost[compteur + 1] == -1000 || sommetsPost[compteur + 1 - NBSommets] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x, vertexPost.y - 0.1f, sb2Ant));
					}
					if(this.indexesPost[i - 1, j - 1] == -1)
					{
						postElevationMap.Add(sbg3Ant - sbg3Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sbg3Ant);
						this.indexesPost[i - 1, j - 1] = 1;

						if(i - 1 == 0 || j - 1 == 0 || sommetsPost[compteur - 2] == -1000 || sommetsPost[compteur - 2 - (2 * NBSommets)] == -1000 || sommetsPost[compteur - 2 - NBSommets] == -1000 || sommetsPost[compteur - (2 * NBSommets)] == -1000 || sommetsPost[compteur - 1 - (2 * NBSommets)] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x - 0.1f, vertexPost.y - 0.1f, sbg3Ant));
					}
				}
				
				if(sommetsAnt[compteur - NBSommets] != -1000 && sommetsPost[compteur - NBSommets] != -1000 && 
				   sommetsAnt[compteur + 1] != -1000 && sommetsPost[compteur + 1] != -1000 &&
				   sommetsAnt[compteur + (1- NBSommets)] != -1000 && sommetsPost[compteur + (1 - NBSommets)] != -1000)
				{
					// Carré bas-droit connu: calcul du centre et création des triangles
					float sbd3Ant, sbd3Post, sd2Ant, sd2Post, sb2Ant, sb2Post;
					
					sbd3Ant = sommetsAnt[compteur + (1 - NBSommets)];
					sbd3Post = sommetsPost[compteur + (1 - NBSommets)];
					sd2Ant = sommetsAnt[compteur + 1];
					sd2Post = sommetsPost[compteur + 1];
					sb2Ant = sommetsAnt[compteur - NBSommets];
					sb2Post = sommetsPost[compteur - NBSommets];
					float centerAnt = (sb2Ant + sommetCentreAnt + sd2Ant + sbd3Ant)/4;
					float centerPost = (sb2Post + sommetCentrePost + sd2Post + sbd3Post)/4;
					
					postElevationMap.Add(centerAnt - centerPost);
					indexVertex = postElevationMap.Count - 1;
					vertexPost = this.postVertices[indexVertex];
					this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, centerAnt);
					
					postElevationMap.Add(sd2Ant - sd2Post);
					indexVertex = postElevationMap.Count - 1;
					vertexPost = this.postVertices[indexVertex];
					this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sd2Ant);
					this.indexesPost[i + 1, j] = 1;

					if(i + 1 == NBSommets - 1 || j - 1 == 0 || sommetsPost[compteur + 2 + NBSommets] == -1000 || sommetsPost[compteur + 2] == -1000 || sommetsPost[compteur + 2 - NBSommets] == -1000 || sommetsPost[compteur + NBSommets] == -1000 || sommetsPost[compteur + NBSommets + 1] == -1000)
						this.antBorderLineIntersect.Add(new Vector3(vertexPost.x + 0.1f, vertexPost.y, sd2Ant));
					
					if(this.indexesPost[i + 1, j - 1] == -1)
					{
						postElevationMap.Add(sbd3Ant - sbd3Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sbd3Ant);
						this.indexesPost[i + 1, j - 1] = 1;

						if(i + 1 == NBSommets - 1 || j - 1 == 0 || sommetsPost[compteur + 2] == -1000 || sommetsPost[compteur + 2 - (2 * NBSommets)] == -1000 || sommetsPost[compteur + 2 - NBSommets] == -1000 || sommetsPost[compteur - (2 * NBSommets)] == -1000 || sommetsPost[compteur + 1 - (2 * NBSommets)] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x + 0.1f, vertexPost.y - 0.1f, sbd3Ant));
					}
					if(this.indexesPost[i, j - 1] == -1)
					{
						postElevationMap.Add(sb2Ant - sb2Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sb2Ant);
						this.indexesPost[i, j - 1] = 1;

						if(j - 1 == 0 || i + 1 == NBSommets - 1 || sommetsPost[compteur - 1 - (2 * NBSommets)] == -1000 || sommetsPost[compteur - (2 * NBSommets)] == -1000 || sommetsPost[compteur + 1 - (2 * NBSommets)] == -1000 || sommetsPost[compteur - 1] == -1000 || sommetsPost[compteur - 1 - NBSommets] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x, vertexPost.y - 0.1f, sb2Ant));
					}
				}
				
				if(sommetsAnt[compteur - 1] != -1000 && sommetsPost[compteur - 1] != -1000 && 
				   sommetsAnt[compteur + (NBSommets - 1)] != -1000 && sommetsPost[compteur + (NBSommets - 1)] != -1000 &&
				   sommetsAnt[compteur + NBSommets] != -1000 && sommetsPost[compteur + NBSommets] != -1000)
				{
					// Carré haut-gauche connu: calcul du centre et création des triangles
					float shg1Ant, shg1Post, sh2Ant, sh2Post, sg2Ant, sg2Post;
					shg1Ant = sommetsAnt[compteur + (NBSommets - 1)];
					shg1Post = sommetsPost[compteur + (NBSommets - 1)];
					sh2Ant = sommetsAnt[compteur + NBSommets];
					sh2Post = sommetsPost[compteur + NBSommets];
					sg2Ant = sommetsAnt[compteur - 1];
					sg2Post = sommetsPost[compteur - 1];
					float centerAnt = (sommetCentreAnt + sg2Ant + shg1Ant + sh2Ant)/4;
					float centerPost = (sommetCentrePost + sg2Post + shg1Post + sh2Post)/4;
					
					postElevationMap.Add(centerAnt - centerPost);
					indexVertex = postElevationMap.Count - 1;
					vertexPost = this.postVertices[indexVertex];
					this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, centerAnt);
					
					postElevationMap.Add(sh2Ant - sh2Post);
					indexVertex = postElevationMap.Count - 1;
					vertexPost = this.postVertices[indexVertex];
					this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sh2Ant);
					indexesPost[i, j + 1] = 1;

					if(j + 1 == NBSommets - 1 || i - 1 == 0 || sommetsPost[compteur - 1 + (2 * NBSommets)] == -1000 || sommetsPost[compteur + (2 * NBSommets)] == -1000 || sommetsPost[compteur + 1 + (2 * NBSommets)] == -1000 || sommetsPost[compteur + 1] == -1000 || sommetsPost[compteur + 1 + NBSommets] == -1000)
						this.antBorderLineIntersect.Add(new Vector3(vertexPost.x, vertexPost.y + 0.1f, sh2Ant));
					
					if(this.indexesPost[i - 1, j] == -1)
					{
						postElevationMap.Add(sg2Ant - sg2Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sg2Ant);
						this.indexesPost[i - 1, j] = 1;

						if(i - 1 == 0 || j + 1 == NBSommets - 1 || sommetsPost[compteur - 2 + NBSommets] == -1000 || sommetsPost[compteur - 2] == -1000 || sommetsPost[compteur - 2 - NBSommets] == -1000 || sommetsPost[compteur - NBSommets] == -1000 || sommetsPost[compteur - 1 - NBSommets] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x - 0.1f, vertexPost.y, sg2Ant));
					}
					if(indexesPost[i - 1, j + 1] == -1)
					{
						postElevationMap.Add(shg1Ant - shg1Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, shg1Ant);
						indexesPost[i - 1, j + 1] = 1;

						if(i - 1 == 0 || j + 1 == NBSommets - 1 || sommetsPost[compteur + (2 * NBSommets)] == -1000 || sommetsPost[compteur - 2 + (2 * NBSommets)] == -1000 || sommetsPost[compteur - 1 + (2 * NBSommets)] == -1000 || sommetsPost[compteur - 2] == -1000 || sommetsPost[compteur - 2 + NBSommets] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x - 0.1f, vertexPost.y + 0.1f, shg1Ant));
					}
				}
				
				if(sommetsAnt[compteur + NBSommets] != -1000 && sommetsPost[compteur + NBSommets] != -1000 && 
				   sommetsAnt[compteur + (1 + NBSommets)] != -1000 && sommetsPost[compteur + (1 + NBSommets)] != -1000 &&
				   sommetsAnt[compteur + 1] != -1000 && sommetsPost[compteur + 1] != -1000)
				{
					// Carré haut-droit connu: calcul du centre et création des triangles
					float shd1Ant, shd1Post, sh2Ant, sh2Post, sd2Ant, sd2Post;
					shd1Ant = sommetsAnt[compteur + (1 + NBSommets)];
					shd1Post = sommetsPost[compteur + (1 + NBSommets)];
					sh2Ant = sommetsAnt[compteur + NBSommets];
					sh2Post = sommetsPost[compteur + NBSommets];
					sd2Ant = sommetsAnt[compteur + 1];
					sd2Post = sommetsPost[compteur + 1];
					float centerAnt = (sommetCentreAnt + sh2Ant + shd1Ant + sd2Ant)/4;
					float centerPost = (sommetCentrePost + sh2Post + shd1Post + sd2Post)/4;
					
					postElevationMap.Add(centerAnt - centerPost);
					indexVertex = postElevationMap.Count - 1;
					vertexPost = this.postVertices[indexVertex];
					this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, centerAnt);
					
					postElevationMap.Add(shd1Ant - shd1Post);
					indexVertex = postElevationMap.Count - 1;
					vertexPost = this.postVertices[indexVertex];
					this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, shd1Ant);
					indexesPost[i + 1, j+ 1] = 1;

					if(i + 1 == NBSommets - 1 || j + 1 == NBSommets - 1 || sommetsPost[compteur + (2 * NBSommets)] == -1000 || sommetsPost[compteur + 2 + (2 * NBSommets)] == -1000 || sommetsPost[compteur + 1 + (2 * NBSommets)] == -1000 || sommetsPost[compteur + 2] == -1000 || sommetsPost[compteur + 2 + NBSommets] == -1000)
						this.antBorderLineIntersect.Add(new Vector3(vertexPost.x + 0.1f, vertexPost.y + 0.1f, shd1Ant));
					
					if(indexesPost[i, j + 1] == -1)
					{
						postElevationMap.Add(sh2Ant - sh2Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sh2Ant);
						indexesPost[i, j + 1] = 1;

						if(i + 1 == NBSommets - 1 || j + 1 == NBSommets - 1 || sommetsPost[compteur - 1 + (2 * NBSommets)] == -1000 || sommetsPost[compteur + (2 * NBSommets)] == -1000 || sommetsPost[compteur +1 + (2 * NBSommets)] == -1000 || sommetsPost[compteur - 1] == -1000 || sommetsPost[compteur - 1 + NBSommets] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x, vertexPost.y + 0.1f, sh2Ant));
					}
					if(this.indexesPost[i + 1, j] == -1)
					{
						postElevationMap.Add(sd2Ant - sd2Post);
						indexVertex = postElevationMap.Count - 1;
						vertexPost = this.postVertices[indexVertex];
						this.volumeAntDeformationVertices[indexVertex] = new Vector3(vertexPost.x, vertexPost.y, sd2Ant);
						this.indexesPost[i + 1, j] = 1;

						if(i + 1 == NBSommets - 1 || j + 1 == NBSommets - 1 || sommetsPost[compteur + 2 + NBSommets] == -1000 || sommetsPost[compteur + 2] == -1000 || sommetsPost[compteur + 2 - NBSommets] == -1000 || sommetsPost[compteur - NBSommets] == -1000 || sommetsPost[compteur - NBSommets + 1] == -1000)
							this.antBorderLineIntersect.Add(new Vector3(vertexPost.x + 0.1f, vertexPost.y, sd2Ant));
					}
				}
			}
			
			i += 2;
			if(i == NBSommets)
				j += 2;
			
			compteur += 2; 
		}

		// Determination of the middle distance between the two surfaces for the volume deformation calculation

		volumeFactorDeformation1 = new List<float>();
		volumeFactorDeformation2 = new List<float>();

		for(i = 0; i < this.postElevationMap.Count; i++)
		{
			if(this.postElevationMap[i] == -1000)
			{
				volumeFactorDeformation1.Add(0f);
				volumeFactorDeformation2.Add(0f);
			}
			else
			{
				volumeFactorDeformation1.Add((this.postElevationMap[i] - this.centerDifference)/2);
				volumeFactorDeformation2.Add(this.postElevationMap[i]/2);
			}
		}
	}

	public int less(Vector3 a, Vector3 b, Vector3 center)
	{
		if (a.x - center.x >= 0 && b.x - center.x < 0)
			return -1;
		if (a.x - center.x < 0 && b.x - center.x >= 0)
			return 1;
		if (a.x - center.x == 0 && b.x - center.x == 0) {
			if (a.y - center.y >= 0 || b.y - center.y >= 0)
				if(a.y > b.y)
					return -1;
				else
					return 1;
		}
		
		// compute the cross product of vectors (center -> a) x (center -> b)
		float det = (a.x - center.x) * (b.y - center.y) - (b.x - center.x) * (a.y - center.y);
		if (det < 0)
			return -1;
		if (det > 0)
			return 1;
		
		// points a and b are on the same line from the center
		// check which point is closer to the center
		float d1 = (a.x - center.x) * (a.x - center.x) + (a.y - center.y) * (a.y - center.y);
		float d2 = (b.x - center.x) * (b.x - center.x) + (b.y - center.y) * (b.y - center.y);
		if(d1 > d2)
			return -1;
		else
			return 1;
	}


	public List<int> constructBorderVolume(List<Vector3> borderVerticesAnt, List<Vector3> borderVerticesPost)
	{	
		List<int> triangles = new List<int> ();
		float shortestArete = 1000;
		int postStartCompteur = 0;
		int antStartCompteur = 0;
		float ratio = Convert.ToSingle(borderVerticesAnt.Count) / Convert.ToSingle(borderVerticesPost.Count);

		// Detection de l'arête la plus petite entre les deux surfaces
		for(int i = 0; i < borderVerticesAnt.Count; i++)
			for(int j = 0; j < borderVerticesPost.Count; j++)
				if(Vector3.Distance(borderVerticesAnt[i], borderVerticesPost[j]) < shortestArete)
				{
					shortestArete = Vector3.Distance(borderVerticesAnt[i], borderVerticesPost[j]);
					postStartCompteur = j;
					antStartCompteur = i;
				}

		// Création des triangles
		int postCompteur;
		float antCompteur = antStartCompteur;
		int antCompteurIndicePrec;
		int antCompteurIndice = antStartCompteur;

		for(int i = postStartCompteur; i < postStartCompteur + borderVerticesPost.Count; i++) 
		{
			postCompteur = i % borderVerticesPost.Count;
			antCompteur += ratio;
			antCompteurIndicePrec = antCompteurIndice;
			antCompteurIndice = Mathf.RoundToInt(antCompteur);
			int pas = (antCompteurIndice - antCompteurIndicePrec) % borderVerticesAnt.Count;
			antCompteurIndice = antCompteurIndice % borderVerticesAnt.Count;
			antCompteurIndicePrec = antCompteurIndicePrec % borderVerticesAnt.Count;

			if(pas == 1)
				// Création de deux triangles à partir du quadrilatère local
			{
				triangles.Add(borderVerticesAnt.Count + postCompteur);
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add(antCompteurIndicePrec);
				triangles.Add(antCompteurIndicePrec);
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add(borderVerticesAnt.Count + postCompteur);

				triangles.Add(antCompteurIndicePrec);
				triangles.Add(antCompteurIndice);
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add(antCompteurIndice);
				triangles.Add(antCompteurIndicePrec);
			}

			else if(pas == 2)
				// Création de 3 triangles à partir des 5 sommets
			{
				triangles.Add(borderVerticesAnt.Count + postCompteur);
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add(antCompteurIndicePrec);
				triangles.Add(antCompteurIndicePrec);
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add(borderVerticesAnt.Count + postCompteur);
				
				triangles.Add(antCompteurIndicePrec);
				triangles.Add((antCompteurIndicePrec + 1) % borderVerticesAnt.Count);
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add((antCompteurIndicePrec + 1) % borderVerticesAnt.Count);
				triangles.Add(antCompteurIndicePrec);

				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
				triangles.Add((antCompteurIndicePrec + 1) % borderVerticesAnt.Count);
				triangles.Add(antCompteurIndice);
				triangles.Add(antCompteurIndice);
				triangles.Add((antCompteurIndicePrec + 1) % borderVerticesAnt.Count);
				triangles.Add(borderVerticesAnt.Count + ((postCompteur + 1) % borderVerticesPost.Count));
			}
		}
		return triangles;
	}
}
