using System;
using System.Security.Cryptography;
using System.Text;
using EncryptionClassLibrary.Encryption;

namespace MindHarbor.GenClassLib.MiscUtil {
	public static class EncryptionUtil {
		public static string EncodePasswordMD5(string originalPassword) {
			//Declarations
			Byte[] originalBytes;
			Byte[] encodedBytes;
			MD5 md5;

			//Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
			md5 = new MD5CryptoServiceProvider();
			originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
			encodedBytes = md5.ComputeHash(originalBytes);

			//Convert encoded bytes back to a 'readable' string
			return BitConverter.ToString(encodedBytes);
		}

		//private static readonly string embededKey = "MHA234@@$AV^&*^(ere5MHcc";

		public static string Encrypt(string strPlainText, string strKey24) {
			Symmetric sym = new Symmetric(Symmetric.Provider.TripleDES, true);
			EncryptionClassLibrary.Encryption.Data key = new EncryptionClassLibrary.Encryption.Data(strKey24);
			EncryptionClassLibrary.Encryption.Data encryptedData;
			encryptedData = sym.Encrypt(new EncryptionClassLibrary.Encryption.Data(strPlainText), key);
			return encryptedData.ToBase64();
		}

		public static string Decrypt(String strEncText, string strKey24) {
			Symmetric sym = new Symmetric(Symmetric.Provider.TripleDES, true);
			EncryptionClassLibrary.Encryption.Data key = new EncryptionClassLibrary.Encryption.Data(strKey24);
			EncryptionClassLibrary.Encryption.Data encryptedData = new EncryptionClassLibrary.Encryption.Data();
			encryptedData.Base64 = strEncText;
			EncryptionClassLibrary.Encryption.Data decryptedData = new EncryptionClassLibrary.Encryption.Data();
			decryptedData = sym.Decrypt(encryptedData, key);
			return decryptedData.ToString();
		}
	}
}