using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

public class BFSCreate
{
	private float[][] J; // Matrice de Jacobi relative au centre approximatif de la BFS
	private float[][] JT; // J transposé
	private float[] d;  
	private Vector3[] vertices;

	public BFSCreate(Vector3[] vertices) {
		this.vertices = vertices;
	}
	
	public Sphere BFSCalculation() {
		
		/* Calcul matriciel: JP = -d */
		
		/* 1. Définition des matrices */
		
		/***************************************************************************************************************************/
		
		/* Définition de J et J(transposé) */
		
		this.J = MatricesLibrary.MatrixCreate(this.vertices.Length, 4);
		this.d = new float[this.vertices.Length];
		this.JT = MatricesLibrary.MatrixCreate(4, this.vertices.Length);
		
		for (int i = 0; i < this.vertices.Length; i++) 
		{
			for(int j = 0; j < 4; j++)
			{
				if(j == 0)
					this.J[i][j] = -2 * this.vertices[i].x;
				else if(j == 1)
					this.J[i][j] = -2 * this.vertices[i].y;
				else if(j == 2)
					this.J[i][j] = -2 * this.vertices[i].z;
				else
					this.J[i][j] = 1;
				this.JT[j][i] = J[i][j];
			}
		}
		
		/* Définition de -d */
		
		for (int i = 0; i < this.vertices.Length; i++) 
		{
			this.d[i] = -(Convert.ToSingle(Math.Pow(this.vertices[i].x, 2) + Math.Pow(this.vertices[i].y, 2) + Math.Pow(this.vertices[i].z, 2)));
		}
		
		/* JT * J */
		
		float[][] JTJ = MatricesLibrary.MatrixProduct (JT, J);
		
		/* JT * (-d) */
		
		float[] JTd = MatricesLibrary.MatrixVectorProduct (JT, d);
		
		/***************************************************************************************************************************/
		
		/* 2. Calcul */
		
		/* Linear Equation System solving: Gauss Method: JP = -d */
		
		float[] results = MatricesLibrary.SystemSolve (JTJ, JTd);
		
		float radius = Convert.ToSingle (Math.Sqrt (Math.Pow (results [0], 2) + Math.Pow (results [1], 2) + Math.Pow (results [2], 2) - results [3]));
		
		return new Sphere () {CX = results[0], CY = results[1], CZ = results[2], R = radius};
	}
}



