using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class ImportData 
{

	private static char[] delimiter = new char[]{' '};
	public static float[] anteriorDatas;
	public static float[] posteriorDatas;
	public static float[] pachymetryDatas;
	
	public void readTxtFile(String filename) {


		#if UNITY_IPHONE
		{
			String corneasPath = Application.persistentDataPath + "/Cornees/" + MenuLoadDatas.corneaId + "_cornee/" + MenuLoadDatas.corneaId + "_cornee" + filename + ".txt";
			try
			{
				using(StreamReader sr = new StreamReader(corneasPath))
				{
					// Read each line from the file: here only one
					if(filename.CompareTo("_ant") == 0)
						anteriorDatas = DataStringToDataFloat(sr.ReadLine());
					else if(filename.CompareTo("_pos") == 0)
						posteriorDatas = DataStringToDataFloat(sr.ReadLine());
					else if(filename.CompareTo("_pk") == 0)
						pachymetryDatas = DataStringToDataFloat(sr.ReadLine());
				}
			}
			catch(Exception e)
			{
				Debug.Log("the file could not be read:");
				Debug.Log(e.Message);
			}
		}
	
		#elif UNITY_ANDROID
		{
			String corneasPath = "Cornees/" + MenuLoadDatas.corneaId + "_cornea" + filename;
			try
			{
				using(StringReader sr = new StringReader(Resources.Load<TextAsset>(corneasPath).text))
				{
					if(filename.CompareTo("_ant") == 0)
						anteriorDatas = DataStringToDataFloat(sr.ReadLine());
					else if(filename.CompareTo("_pos") == 0)
						posteriorDatas = DataStringToDataFloat(sr.ReadLine());
					else if(filename.CompareTo("_pk") == 0)
						pachymetryDatas = DataStringToDataFloat(sr.ReadLine());
				}
			}
			catch(Exception e)
			{
				Debug.Log("the file could not be read:");
				Debug.Log(e.Message);
			}
		}
		#endif

	}
	

	private float[] DataStringToDataFloat(string dataString) {
		return Array.ConvertAll(dataString.Split (delimiter, StringSplitOptions.RemoveEmptyEntries), new Converter<string, float>(float.Parse));
	}
}
