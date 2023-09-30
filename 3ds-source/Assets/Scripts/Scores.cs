using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Scores
{
	//variables in save
	public static readonly string rootDir = "data:/";
    public string name;
    public float score;

    //initializer
    public Scores(string name, float score)
    {
        this.name = name;
        this.score = score;
    }

	//load scores
	public static List<Scores> LoadAll(string rootDir)
	{
		List<Scores> result = new List<Scores>();

		string fileName = rootDir + "scores.bin";
		//if the save exists, load it
		if (File.Exists(fileName))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(fileName, FileMode.Open, FileAccess.Read);
			result = (List<Scores>)binaryFormatter.Deserialize(file);
			file.Close();
		}
		else
		{
			for (int i = 0; i < 5; i++)
			{
				result.Add(new Scores("MJS", 0f));
			}
			SaveAll(rootDir, result);
		}

		return result;
	}

	//save scores
	public static void SaveAll(string rootDir, List<Scores> data)
	{
#if UNITY_N3DS
		// It is not necessary to mount/unmount manually, but if you do then the unmount will happen immediately.
		// If you don't, then the file system is mounted as required, then unmounted at the end of the first engine tick where it's no longer being used.
		UnityEngine.N3DS.FileSystemSave.Mount();
#endif

		BinaryFormatter binaryFormatter = new BinaryFormatter();
		string fileName = rootDir + "scores.bin";
		FileStream file = File.Create(fileName);
		binaryFormatter.Serialize(file, data);
		file.Close();

#if UNITY_N3DS
		// This calls "nn::fs::CommitSaveData()", so the save operation is guaranteed safe and atomic.
		// If power is lost during the save operation, or some similar error occurs, then the save will not be corrupted.
		UnityEngine.N3DS.FileSystemSave.Unmount();
#endif
	}
}
