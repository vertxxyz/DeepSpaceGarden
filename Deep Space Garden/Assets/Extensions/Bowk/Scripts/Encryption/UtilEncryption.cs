
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;

	public class UtilEncryption
	{
		static public  string Md5Sum(string strToEncrypt)
		{
	#if UNITY_WP8 || UNITY_METRO
			MD5.MD5Custom md = new MD5.MD5Custom();
			md.Value = strToEncrypt;
			string returnSring = md.FingerPrint;
			Debug.Log("MD5:" + returnSring);
			return returnSring;
	#else		
			System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
			byte[] bytes = ue.GetBytes(strToEncrypt);

			// encrypt bytes
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

			// encrypt bytes
			byte[] hashBytes = md5.ComputeHash(bytes);
		 
			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";
		 
			for (int i = 0; i < hashBytes.Length; i++)
			{
				hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}
		 
			return hashString.PadLeft(32, '0');
	#endif
		}

	}

}