using System;


public class MatricesLibrary
{
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] MatrixCreate(int rows, int cols)
	{
		// allocates/creates a matrix initialized to all 0.0. assume rows and cols > 0
		// do error checking here
		float[][] result = new float[rows][];
		for (int i = 0; i < rows; ++i)
			result[i] = new float[cols];
		
		//for (int i = 0; i < rows; ++i)
		//  for (int j = 0; j < cols; ++j)
		//    result[i][j] = 0.0; // explicit initialization needed in some languages
		
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] MatrixRandom(int rows, int cols, float minVal, float maxVal, int seed)
	{
		// return a matrix with random values
		Random ran = new Random(seed);
		float[][] result = MatrixCreate(rows, cols);
		for (int i = 0; i < rows; ++i)
			for (int j = 0; j < cols; ++j)
				result[i][j] = (maxVal - minVal) * ran.Next() + minVal;
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] MatrixIdentity(int n)
	{
		// return an n x n Identity matrix
		float[][] result = MatrixCreate(n, n);
		for (int i = 0; i < n; ++i)
			result[i][i] = 1.0f;
		
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static string MatrixAsString(float[][] matrix)
	{
		string s = "";
		for (int i = 0; i < matrix.Length; ++i)
		{
			for (int j = 0; j < matrix[i].Length; ++j)
				s += matrix[i][j].ToString("F3").PadLeft(8) + " ";
			s += Environment.NewLine;
		}
		return s;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static bool MatrixAreEqual(float[][] matrixA, float[][] matrixB, float epsilon)
	{
		// true if all values in matrixA == corresponding values in matrixB
		int aRows = matrixA.Length; int aCols = matrixA[0].Length;
		int bRows = matrixB.Length; int bCols = matrixB[0].Length;
		if (aRows != bRows || aCols != bCols)
			throw new Exception("Non-conformable matrices in MatrixAreEqual");
		
		for (int i = 0; i < aRows; ++i) // each row of A and B
			for (int j = 0; j < aCols; ++j) // each col of A and B
				//if (matrixA[i][j] != matrixB[i][j])
				if (Math.Abs(matrixA[i][j] - matrixB[i][j]) > epsilon)
					return false;
		return true;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] MatrixProduct(float[][] matrixA, float[][] matrixB)
	{
		int aRows = matrixA.Length; int aCols = matrixA[0].Length;
		int bRows = matrixB.Length; int bCols = matrixB[0].Length;
		if (aCols != bRows)
			throw new Exception("Non-conformable matrices in MatrixProduct");
		
		float[][] result = MatrixCreate(aRows, bCols);
		
		for (int i = 0; i < aRows; ++i) // each row of A
			for (int j = 0; j < bCols; ++j) // each col of B
				for (int k = 0; k < aCols; ++k) // could use k < bRows
					result[i][j] += matrixA[i][k] * matrixB[k][j];
		
		//Parallel.For(0, aRows, i =>
		//  {
		//    for (int j = 0; j < bCols; ++j) // each col of B
		//      for (int k = 0; k < aCols; ++k) // could use k < bRows
		//        result[i][j] += matrixA[i][k] * matrixB[k][j];
		//  }
		//);
		
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[] MatrixVectorProduct(float[][] matrix, float[] vector)
	{
		// result of multiplying an n x m matrix by a m x 1 column vector (yielding an n x 1 column vector)
		int mRows = matrix.Length; int mCols = matrix[0].Length;
		int vRows = vector.Length;
		if (mCols != vRows)
			throw new Exception("Non-conformable matrix and vector in MatrixVectorProduct");
		float[] result = new float[mRows]; // an n x m matrix times a m x 1 column vector is a n x 1 column vector
		for (int i = 0; i < mRows; ++i)
			for (int j = 0; j < mCols; ++j)
				result[i] += matrix[i][j] * vector[j];
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] MatrixDecompose(float[][] matrix, out int[] perm, out int toggle)
	{
		// Doolittle LUP decomposition with partial pivoting.
		// rerturns: result is L (with 1s on diagonal) and U; perm holds row permutations; toggle is +1 or -1 (even or odd)
		int rows = matrix.Length;
		int cols = matrix[0].Length; // assume all rows have the same number of columns so just use row [0].
		if (rows != cols)
			throw new Exception("Attempt to MatrixDecompose a non-square mattrix");
		
		int n = rows; // convenience
		
		float[][] result = MatrixDuplicate(matrix); // make a copy of the input matrix
		
		perm = new int[n]; // set up row permutation result
		for (int i = 0; i < n; ++i) { perm[i] = i; }
		
		toggle = 1; // toggle tracks row swaps. +1 -> even, -1 -> odd. used by MatrixDeterminant
		
		for (int j = 0; j < n - 1; ++j) // each column
		{
			float colMax = Math.Abs(result[j][j]); // find largest value in col j
			int pRow = j;
			for (int i = j + 1; i < n; ++i)
			{
				if (result[i][j] > colMax)
				{
					colMax = result[i][j];
					pRow = i;
				}
			}
			
			if (pRow != j) // if largest value not on pivot, swap rows
			{
				float[] rowPtr = result[pRow];
				result[pRow] = result[j];
				result[j] = rowPtr;
				
				int tmp = perm[pRow]; // and swap perm info
				perm[pRow] = perm[j];
				perm[j] = tmp;
				
				toggle = -toggle; // adjust the row-swap toggle
			}
			
			if (Math.Abs(result[j][j]) < 1.0E-20) // if diagonal after swap is zero . . .
				return null; // consider a throw
			
			for (int i = j + 1; i < n; ++i)
			{
				result[i][j] /= result[j][j];
				for (int k = j + 1; k < n; ++k)
				{
					result[i][k] -= result[i][j] * result[j][k];
				}
			}
		} // main j column loop
		
		return result;
	} // MatrixDecompose
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] MatrixInverse(float[][] matrix)
	{
		int n = matrix.Length;
		float[][] result = MatrixDuplicate(matrix);
		
		int[] perm;
		int toggle;
		float[][] lum = MatrixDecompose(matrix, out perm, out toggle);
		if (lum == null)
			throw new Exception("Unable to compute inverse");
		
		float[] b = new float[n];
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
			{
				if (i == perm[j])
					b[j] = 1.0f;
				else
					b[j] = 0.0f;
			}
			
			float[] x = HelperSolve(lum, b); // 
			
			for (int j = 0; j < n; ++j)
				result[j][i] = x[j];
		}
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float MatrixDeterminant(float[][] matrix)
	{
		int[] perm;
		int toggle;
		float[][] lum = MatrixDecompose(matrix, out perm, out toggle);
		if (lum == null)
			throw new Exception("Unable to compute MatrixDeterminant");
		float result = toggle;
		for (int i = 0; i < lum.Length; ++i)
			result *= lum[i][i];
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[] HelperSolve(float[][] luMatrix, float[] b) // helper
	{
		// before calling this helper, permute b using the perm array from MatrixDecompose that generated luMatrix
		int n = luMatrix.Length;
		float[] x = new float[n];
		b.CopyTo(x, 0);
		
		for (int i = 1; i < n; ++i)
		{
			float sum = x[i];
			for (int j = 0; j < i; ++j)
				sum -= luMatrix[i][j] * x[j];
			x[i] = sum;
		}
		
		x[n - 1] /= luMatrix[n - 1][n - 1];
		for (int i = n - 2; i >= 0; --i)
		{
			float sum = x[i];
			for (int j = i + 1; j < n; ++j)
				sum -= luMatrix[i][j] * x[j];
			x[i] = sum / luMatrix[i][i];
		}
		
		return x;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[] SystemSolve(float[][] A, float[] b)
	{
		// Solve Ax = b
		int n = A.Length;
		
		// 1. decompose A
		int[] perm;
		int toggle;
		float[][] luMatrix = MatrixDecompose(A, out perm, out toggle);
		if (luMatrix == null)
			return null;
		
		// 2. permute b according to perm[] into bp
		float[] bp = new float[b.Length];
		for (int i = 0; i < n; ++i)
			bp[i] = b[perm[i]];
		
		// 3. call helper
		float[] x = HelperSolve(luMatrix, bp);
		return x;
	} // SystemSolve
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] MatrixDuplicate(float[][] matrix)
	{
		// allocates/creates a duplicate of a matrix. assumes matrix is not null.
		float[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
		for (int i = 0; i < matrix.Length; ++i) // copy the values
			for (int j = 0; j < matrix[i].Length; ++j)
				result[i][j] = matrix[i][j];
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] ExtractLower(float[][] matrix)
	{
		// lower part of a Doolittle decomposition (1.0s on diagonal, 0.0s in upper)
		int rows = matrix.Length; int cols = matrix[0].Length;
		float[][] result = MatrixCreate(rows, cols);
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
			{
				if (i == j)
					result[i][j] = 1.0f;
				else if (i > j)
					result[i][j] = matrix[i][j];
			}
		}
		return result;
	}
	
	public static float[][] ExtractUpper(float[][] matrix)
	{
		// upper part of a Doolittle decomposition (0.0s in the strictly lower part)
		int rows = matrix.Length; int cols = matrix[0].Length;
		float[][] result = MatrixCreate(rows, cols);
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < cols; ++j)
			{
				if (i <= j)
					result[i][j] = matrix[i][j];
			}
		}
		return result;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static float[][] PermArrayToMatrix(int[] perm)
	{
		// convert Doolittle perm array to corresponding perm matrix
		int n = perm.Length;
		float[][] result = MatrixCreate(n, n);
		for (int i = 0; i < n; ++i)
			result[i][perm[i]] = 1.0f;
		return result;
	}
	
	public static float[][] UnPermute(float[][] luProduct, int[] perm)
	{
		// unpermute product of Doolittle lower * upper matrix according to perm[]
		// no real use except to demo LU decomposition, or for consistency testing
		float[][] result = MatrixDuplicate(luProduct);
		
		int[] unperm = new int[perm.Length];
		for (int i = 0; i < perm.Length; ++i)
			unperm[perm[i]] = i;
		
		for (int r = 0; r < luProduct.Length; ++r)
			result[r] = luProduct[unperm[r]];
		
		return result;
	} // UnPermute
	
	// --------------------------------------------------------------------------------------------------------------
	
	public static string VectorAsString(float[] vector)
	{
		string s = "";
		for (int i = 0; i < vector.Length; ++i)
			s += vector[i].ToString("F3").PadLeft(8) + Environment.NewLine;
		s += Environment.NewLine;
		return s;
	}
	
	public static string VectorAsString(int[] vector)
	{
		string s = "";
		for (int i = 0; i < vector.Length; ++i)
			s += vector[i].ToString().PadLeft(2) + " ";
		s += Environment.NewLine;
		return s;
	}
	
	// --------------------------------------------------------------------------------------------------------------
	
}


