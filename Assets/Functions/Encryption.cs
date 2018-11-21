using AuthLib.Functions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Functions {
	// Token: 0x02000019 RID: 25
	public class Encryption {
		// Token: 0x0600009E RID: 158 RVA: 0x0000B2A8 File Offset: 0x000094A8
		public static string Encrypt(string clearText) {
			string EncryptionKey = "123";
			//string EncryptionKey = FingerPrint.Value();
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create()) {
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
				{
					73,
					118,
					97,
					110,
					32,
					77,
					101,
					100,
					118,
					101,
					100,
					101,
					118
				});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream()) {
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)) {
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000B3A0 File Offset: 0x000095A0
		public static string Decrypt(string cipherText) {
			string EncryptionKey = "123";
			//string EncryptionKey = FingerPrint.Value();
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create()) {
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[]
				{
					73,
					118,
					97,
					110,
					32,
					77,
					101,
					100,
					118,
					101,
					100,
					101,
					118
				});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream()) {
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)) {
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						try {
							cs.Close();
						} catch (CryptographicException CE) {
							File.Delete("autologin.data");
							return "";
						}
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}
	}
}
