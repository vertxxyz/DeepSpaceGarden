
namespace Bowk
{
		
	using UnityEngine;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	using System.Collections;
	using System.Xml; 
	using System.Xml.Serialization;
	using System.IO;
	using System.Text;

	public class SerializeHelper
	{ 
		public static bool LoadData<T>(string a_FileName, ref T a_Data)
		{
			a_FileName = a_FileName.Replace(".", "_");
			Debug.Log("LOAD: " + a_FileName);
			if (PlayerPrefs.HasKey(a_FileName))
	        {
				a_Data = (T)DeserializeObject<T>(PlayerPrefs.GetString(a_FileName));
				if (a_Data != null)
				{
	            	return true;
				}
	        }
			else
			{
				Debug.Log("Did not find key: " + a_FileName);
			}

			return false;
		}
		
		public static bool LoadData<T>(TextAsset a_XMLData, ref T a_Data)
		{
			Profiler.BeginSample("Serialize: Load. " + typeof(T).ToString());
			
			bool bLoaded = false;
			if (a_XMLData != null)
			{
				a_Data = (T)DeserializeObject<T>(a_XMLData.text);
				bLoaded = true;
			}

			Profiler.EndSample();
			return bLoaded;
		}

		public static void SaveData<T>(T a_Data, string a_FileName)
		{
	        string sData = SerializeObject<T>(a_Data);

			a_FileName = a_FileName.Replace(".", "_");
			Debug.Log("SAVE: " + a_FileName);

			PlayerPrefs.SetString(a_FileName, sData);
			return;
		}

		/// <summary>
		/// Clones using serialization.
		/// </summary>
		public static object Clone<T>(T a_Original)
		{
			Profiler.BeginSample("Serialize: Clone. " + typeof(T).ToString());
			string xmlData = SerializeObject<T>(a_Original);
			object oReturn = DeserializeObject<T>(xmlData);
			Profiler.EndSample();
			return oReturn;
		}

		public static string SerializeObject<T>(object pObject)
	    {
			string xmlizedString = "";
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings 
			{ 
				Indent = true, 
				OmitXmlDeclaration = false, 
				Encoding = Encoding.UTF8,
				CheckCharacters = false
			};

			MemoryStream memoryStream = new MemoryStream();
			
			XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
			
			XmlSerializer xs = new XmlSerializer(typeof(T));
			xs.Serialize(xmlWriter, pObject);
			memoryStream.Position = 0;
			using (StreamReader sr = new StreamReader(memoryStream))
			{
				xmlizedString = sr.ReadToEnd();
			}

			//Debug.Log("XMl80:" + xmlizedString);

			return xmlizedString;
		} 
		
		public static object DeserializeObject<T>(string pXmlizedString) 
		{ 
			if (string.IsNullOrEmpty(pXmlizedString))
			{
				return null;
			}

			XmlSerializer xs = new XmlSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
			return xs.Deserialize(memoryStream); 
		}

		private static string ValidateString(string str)
		{
			return UTF8ByteArrayToString(StringToUTF8ByteArray(str));
		}

		private static string UTF8ByteArrayToString(byte[] characters) 
		{
			char[] chars = new char[characters.Length / sizeof(char)];
			System.Buffer.BlockCopy(characters, 0, chars, 0, characters.Length);
			return new string(chars);
		}
		
		private static byte[] StringToUTF8ByteArray(string pXmlString) 
		{ 
			UTF8Encoding encoding = new UTF8Encoding(); 
			byte[] byteArray = encoding.GetBytes(pXmlString); 
			return byteArray; 
		}
	}

}


