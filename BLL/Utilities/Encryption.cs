using System;
using System.Security;
using System.Security.Permissions;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;


/// <summary>
/// Summary description for Encryption
/// </summary>
public class EncryptPassword
{
    //private DataStore dStoreParameterInfo = new DataStore(ConfigurationSettings.AppSettings.Get("GeneralPBLCommon"), "d_parameter_info");
    private DataTable dtparameterinfo = new DataTable();
    //public EncryptPassword()
    //{
    //    //String help_path = System.Configuration.ConfigurationManager.AppSettings.Get("HelpPath") + "temp";
    //    //String temp = help_path + @"\login_parameter_info.txt";
    //    //if (File.Exists(temp))
    //    //{
    //    //    dtparameterinfo = SBSFunctionLibrary.TextFiletoDataTable(temp);
    //    //    //dStoreParameterInfo.ImportFile(ConfigurationSettings.AppSettings.Get("HelpPath") + "login_parameter_info.txt", FileSaveAsType.Text);
    //    //}
    //}

    public string sbs_encrypt(string password)
    {
        int p = 11, q = 13;
        double n, z, d, e;
        d = e = 0;
        string encryptpassword = "";

        n = p * q;
        z = (p - 1) * (q - 1);

        //find value of d
        for (int i = 2; i < z; i++)
        {
            if ((z % i) != 0)
            {
                d = i;
                break;
            }
        }

        //find value of e
        for (int i = 1; i < z; i++)
        {
            if (((d * i) - 1) % z == 0)
            {
                e = i;
                break;
            }
        }

        //encrypt given string
        char[] chararray = password.ToCharArray();
        for (int i = 0; i < chararray.Length; i++)
        {
            double ascii = (byte)chararray[i];
            int ciphertext = Convert.ToInt16(Math.Pow(ascii, e) % n);
            //encryptpassword += Convert.ToChar(ciphertext).ToString();
            encryptpassword += ciphertext.ToString();
        }
        return (encryptpassword);
    }

    public bool ValidatePassword(String stPassword)
    {
        String passlength = "";
        String passalpha = "";
        String passnumber = "";
        if (dtparameterinfo.Rows.Count > 0)
        {
            passalpha = dtparameterinfo.Rows[0]["param_value"].ToString();
            passlength = dtparameterinfo.Rows[1]["param_value"].ToString();
            passnumber = dtparameterinfo.Rows[2]["param_value"].ToString();
        }

        int DigitCount = 0, CharCount = 0, SpecialCount = 0;
        char[] charPassword = stPassword.ToCharArray();

        if (charPassword.Length > Convert.ToInt16(passlength))
        {
            //SBSMessageBox.Show("Maximum Length of Your Password can not Exceed " + passlength + "Character");
            return false;
        }
        for (int i = 0; i < charPassword.Length; i++)
        {
            int ascii = (byte)charPassword[i];
            if (ascii >= 48 && ascii <= 57)
                DigitCount++;
            else if ((ascii >= 65 && ascii <= 90) || (ascii >= 97 && ascii <= 122))
                CharCount++;
            else
                SpecialCount++;
        }
        if ((CharCount > Convert.ToInt16(passalpha)) || (DigitCount > Convert.ToInt16(passnumber)))
        {
            return false;
        }
        else
            return true;

    }
    //public static string Encrypt(string clearText)
    //{
    //    string EncryptionKey = "trkr2016";
    //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(clearBytes, 0, clearBytes.Length);
    //                cs.Close();
    //            }
    //            clearText = Convert.ToBase64String(ms.ToArray());
    //        }
    //    }
    //    return clearText;
    //}
    //public static string Decrypt(string cipherText)
    //{
    //    string EncryptionKey = "trkr2016";
    //    cipherText = cipherText.Replace(" ", "+");
    //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(cipherBytes, 0, cipherBytes.Length);
    //                cs.Close();
    //            }
    //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
    //        }
    //    }
    //    return cipherText;
    //}
    public string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
}