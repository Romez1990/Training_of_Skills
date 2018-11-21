using System;
using System.Diagnostics;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace AuthLib.Functions {
	// Token: 0x02000004 RID: 4
	public class FingerPrint {
		// Token: 0x0600000A RID: 10 RVA: 0x00002364 File Offset: 0x00000564
		public static string Value() {
			bool flag = string.IsNullOrEmpty(FingerPrint.HWID);
			if (flag) {
				Stopwatch timer = new Stopwatch();
				timer.Start();
				FingerPrint.CPU = FingerPrint.GetHash(FingerPrint.cpuId(), false);
				Console.WriteLine(string.Concat(new object[]
				{
					FingerPrint.CPU,
					" found in ",
					timer.Elapsed.TotalMilliseconds,
					"ms"
				}));
				FingerPrint.BIOS = FingerPrint.GetHash(FingerPrint.biosId(), false);
				Console.WriteLine(string.Concat(new object[]
				{
					FingerPrint.BIOS,
					" found in ",
					timer.Elapsed.TotalMilliseconds,
					"ms"
				}));
				FingerPrint.BASE = FingerPrint.GetHash(FingerPrint.baseId(), false);
				Console.WriteLine(string.Concat(new object[]
				{
					FingerPrint.BASE,
					" found in ",
					timer.Elapsed.TotalMilliseconds,
					"ms"
				}));
				FingerPrint.VIDEO = FingerPrint.GetHash(FingerPrint.videoId(), false);
				Console.WriteLine(string.Concat(new object[]
				{
					FingerPrint.VIDEO,
					" found in ",
					timer.Elapsed.TotalMilliseconds,
					"ms"
				}));
				FingerPrint.SYSTEM = FingerPrint.GetHash(FingerPrint.OSid(), false);
				Console.WriteLine(string.Concat(new object[]
				{
					FingerPrint.SYSTEM,
					" found in ",
					timer.Elapsed.TotalMilliseconds,
					"ms"
				}));
				FingerPrint.MACHINE = FingerPrint.GetHash(FingerPrint.machineName(), false);
				Console.WriteLine(string.Concat(new object[]
				{
					FingerPrint.MACHINE,
					" found in ",
					timer.Elapsed.TotalMilliseconds,
					"ms"
				}));
				FingerPrint.HWID = FingerPrint.GetHash(string.Concat(new string[]
				{
					"CPU >> ",
					FingerPrint.CPU,
					"\nBIOS >> ",
					FingerPrint.BIOS,
					"\nBASE >> ",
					FingerPrint.BASE,
					"\nVIDEO >> ",
					FingerPrint.VIDEO,
					"\nOS >> ",
					FingerPrint.SYSTEM,
					"\nMACHINE >> ",
					FingerPrint.MACHINE
				}), true);
				Console.WriteLine("Hashed all Hardware Serials in " + timer.Elapsed.TotalMilliseconds + "ms");
				Console.WriteLine("Hashed all Hardware Serials: " + FingerPrint.HWID);
			} else {
				Console.WriteLine("Fingerprint Exists");
			}
			return FingerPrint.HWID;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000263C File Offset: 0x0000083C
		public static string GetHash(string s, bool seperators = false) {
			MD5 sec = new MD5CryptoServiceProvider();
			ASCIIEncoding enc = new ASCIIEncoding();
			byte[] bt = enc.GetBytes(s);
			string result;
			if (seperators) {
				result = FingerPrint.GetHexString(sec.ComputeHash(bt));
			} else {
				result = FingerPrint.GetHexString(sec.ComputeHash(bt)).Replace("-", "");
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002694 File Offset: 0x00000894
		private static string GetHexString(byte[] bt) {
			string s = string.Empty;
			for (int i = 0; i < bt.Length; i++) {
				byte b = bt[i];
				int j = (int)b;
				int n = j & 15;
				int n2 = j >> 4 & 15;
				bool flag = n2 > 9;
				if (flag) {
					s += ((char)(n2 - 10 + 65)).ToString();
				} else {
					s += n2.ToString();
				}
				bool flag2 = n > 9;
				if (flag2) {
					s += ((char)(n - 10 + 65)).ToString();
				} else {
					s += n.ToString();
				}
				bool flag3 = i + 1 != bt.Length && (i + 1) % 2 == 0;
				if (flag3) {
					s += "-";
				}
			}
			return s;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002770 File Offset: 0x00000970
		private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue) {
			string result = "";
			ManagementClass mc = new ManagementClass(wmiClass);
			ManagementObjectCollection moc = mc.GetInstances();
			foreach (ManagementBaseObject managementBaseObject in moc) {
				ManagementObject mo = (ManagementObject)managementBaseObject;
				bool flag = mo[wmiMustBeTrue].ToString() == "True";
				if (flag) {
					bool flag2 = result == "";
					if (flag2) {
						try {
							result = mo[wmiProperty].ToString();
							break;
						} catch {
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000282C File Offset: 0x00000A2C
		private static string identifier(string wmiClass, string wmiProperty) {
			ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select " + wmiProperty + " From " + wmiClass);
			ManagementObjectCollection mbsList = mbs.Get();
			string id = "";
			foreach (ManagementBaseObject managementBaseObject in mbsList) {
				ManagementObject mo = (ManagementObject)managementBaseObject;
				bool flag = id == "";
				if (flag) {
					try {
						id = mo[wmiProperty].ToString();
						break;
					} catch {
					}
				}
			}
			return id;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028D8 File Offset: 0x00000AD8
		private static string cpuId() {
			string retVal = FingerPrint.identifier("Win32_Processor", "ProcessorId");
			bool flag = retVal == "";
			if (flag) {
				retVal = FingerPrint.identifier("Win32_Processor", "Name");
				bool flag2 = retVal == "";
				if (flag2) {
					retVal = FingerPrint.identifier("Win32_Processor", "Manufacturer");
				}
				retVal += FingerPrint.identifier("Win32_Processor", "MaxClockSpeed");
			}
			return retVal;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002954 File Offset: 0x00000B54
		private static string biosId() {
			return FingerPrint.identifier("Win32_BIOS", "Manufacturer") + FingerPrint.identifier("Win32_BIOS", "SMBIOSBIOSVersion") + FingerPrint.identifier("Win32_BIOS", "IdentificationCode") + FingerPrint.identifier("Win32_BIOS", "SerialNumber");
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000029A8 File Offset: 0x00000BA8
		private static string baseId() {
			return FingerPrint.identifier("Win32_BaseBoard", "Model") + FingerPrint.identifier("Win32_BaseBoard", "Manufacturer") + FingerPrint.identifier("Win32_BaseBoard", "Name") + FingerPrint.identifier("Win32_BaseBoard", "SerialNumber");
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000029FC File Offset: 0x00000BFC
		private static string videoId() {
			return FingerPrint.identifier("Win32_VideoController", "Name");
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002A20 File Offset: 0x00000C20
		private static string OSid() {
			return FingerPrint.identifier("Win32_OperatingSystem", "Name") + FingerPrint.identifier("Win32_OperatingSystem", "Manufacturer") + FingerPrint.identifier("Win32_OperatingSystem", "WindowsDirectory");
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002A64 File Offset: 0x00000C64
		private static string machineName() {
			return Environment.MachineName;
		}

		// Token: 0x04000003 RID: 3
		private static string CPU = "";

		// Token: 0x04000004 RID: 4
		private static string BIOS = "";

		// Token: 0x04000005 RID: 5
		private static string BASE = "";

		// Token: 0x04000006 RID: 6
		private static string VIDEO = "";

		// Token: 0x04000007 RID: 7
		private static string SYSTEM = "";

		// Token: 0x04000008 RID: 8
		private static string MACHINE = "";

		// Token: 0x04000009 RID: 9
		public static string HWID = string.Empty;
	}
}
