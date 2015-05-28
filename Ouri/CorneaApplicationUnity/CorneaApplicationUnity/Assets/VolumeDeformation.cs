using UnityEngine;
using System.Collections;

public class VolumeDeformation {

	private float prevDeformationFactor;

	public VolumeDeformation()
	{
		this.prevDeformationFactor = 0f;
	}
	
	public Vector3[] zCorneaDeformateAnt1(float deformationFactor, Vector3[] vertices)
	{
		for(int i = 0; i < vertices.Length; i++) 
		{
			if(deformationFactor > this.prevDeformationFactor)
				vertices[i].z -= (BorderCornea.volumeFactorDeformation1[i] * (deformationFactor - this.prevDeformationFactor + 1f)) - BorderCornea.volumeFactorDeformation1[i];
			else if(deformationFactor < this.prevDeformationFactor)
				vertices[i].z += (BorderCornea.volumeFactorDeformation1[i] * (this.prevDeformationFactor - deformationFactor + 1f)) - BorderCornea.volumeFactorDeformation1[i];
		}
		return vertices;
	}
	
	public Vector3[] zCorneaDeformatePost1(float deformationFactor, Vector3[] vertices)
	{
		for(int i = 0; i < vertices.Length; i++) 
		{
			if(deformationFactor > this.prevDeformationFactor)
				vertices[i].z -= (BorderCornea.volumeFactorDeformation1[i] * (deformationFactor - this.prevDeformationFactor + 1f)) - BorderCornea.volumeFactorDeformation1[i];
			else if(deformationFactor < this.prevDeformationFactor)
				vertices[i].z += (BorderCornea.volumeFactorDeformation1[i] * (this.prevDeformationFactor - deformationFactor + 1f)) - BorderCornea.volumeFactorDeformation1[i];
		}
		this.prevDeformationFactor = deformationFactor;
		return vertices;
	}

	public Vector3[] zCorneaDeformateAnt2(float deformationFactor, Vector3[] vertices)
	{
		for(int i = 0; i < vertices.Length; i++) 
		{
			if(deformationFactor > this.prevDeformationFactor)
				vertices[i].z += (BorderCornea.volumeFactorDeformation2[i] * (deformationFactor - this.prevDeformationFactor + 1f)) - BorderCornea.volumeFactorDeformation2[i];
			else if(deformationFactor < this.prevDeformationFactor)
				vertices[i].z -= (BorderCornea.volumeFactorDeformation2[i] * (this.prevDeformationFactor - deformationFactor + 1f)) - BorderCornea.volumeFactorDeformation2[i];
		}
		return vertices;
	}
	
	public Vector3[] zCorneaDeformatePost2(float deformationFactor, Vector3[] vertices)
	{
		for(int i = 0; i < vertices.Length; i++) 
		{
			if(deformationFactor > this.prevDeformationFactor)
				vertices[i].z -= (BorderCornea.volumeFactorDeformation2[i] * (deformationFactor - this.prevDeformationFactor + 1f)) - BorderCornea.volumeFactorDeformation2[i];
			else if(deformationFactor < this.prevDeformationFactor)
				vertices[i].z += (BorderCornea.volumeFactorDeformation2[i] * (this.prevDeformationFactor - deformationFactor + 1f)) - BorderCornea.volumeFactorDeformation2[i];
		}
		this.prevDeformationFactor = deformationFactor;
		return vertices;
	}

	public Vector3[] zCorneaDeformateBorder(float deformationFactor, Vector3[] vertices)
	{
		for(int i = 0; i < vertices.Length; i++) 
		{
			if(deformationFactor > this.prevDeformationFactor)
				vertices[i].z += ((BorderCornea.borderVolumeFactorDeformation[i] * (deformationFactor - this.prevDeformationFactor + 1f)) - BorderCornea.borderVolumeFactorDeformation[i]) * 10f;
			else if(deformationFactor < this.prevDeformationFactor)
				vertices[i].z -= ((BorderCornea.borderVolumeFactorDeformation[i] * (this.prevDeformationFactor - deformationFactor + 1f)) - BorderCornea.borderVolumeFactorDeformation[i]) * 10f;
		}
		this.prevDeformationFactor = deformationFactor;
		return vertices;
	}
}
