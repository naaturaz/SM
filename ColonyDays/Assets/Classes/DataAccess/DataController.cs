using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using UnityEngine;

/// <summary>
/// The saves and Loads etc 
/// </summary>
public class DataController
{
    private static string _path;
    static float _iconShown;


    public static void Start()
    {
        DefinePath();
        CheckIfSugarMillFolderExists();

    }



    private static void DefinePath()
    {
        var a = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\SugarMill";
        Debug.Log(a);
        _path = a;
    }

    private static void CheckIfSugarMillFolderExists()
    {
        var exist = Directory.Exists(_path);

        if (!exist)
        {
            Directory.CreateDirectory(_path);
        }
    }

    
    private static string savePath;
    /// <summary>
    /// Each save is a directory
    /// </summary>
    /// <param name="name"></param>
    public static void SaveGame(string name, bool quickSave = false)
    {
        savePath = _path + @"\" + name;
        var exist = Directory.Exists(savePath);
        var hasSpace = HasHDDSpace();
        
        if (!hasSpace)
        {
            //todo notify cant bz space
            Debug.Log("Not enough space on " + _path.ToCharArray()[0] + " Drive to save");
            Dialog.OKDialog(H.NotHDDSpace, _path.ToCharArray()[0].ToString());
            return;
        }

        //ask wants overWrite ... if is quicksave not need to ask
        if (exist && !quickSave)
        {
            Dialog.OKCancelDialog(H.OverWrite);
        }
        else
        {
            //if is not quickSave then needs to call EscapeKey
            SaveNow(!quickSave);
            PlayerPrefs.SetString("Last_Saved", name);
            PlayerPrefs.SetString(name, DateTime.Now.ToString());
           
            Debug.Log("DateTie now:" + DateTime.Now.ToString());
            //Debug.Log("Ticks now:" + DateTime.Now.Ticks.ToString());

        }
        PlayerPrefs.Save();

        _iconShown = Time.time;
        Program.MouseListener.CurrForm.ShowAutoSave();

    }

   

    public static bool ThereIsALastSavedFile()
    {
        var tileNameSelected = PlayerPrefs.GetString("Last_Saved");
        if (string.IsNullOrEmpty(tileNameSelected))
        {
            return false;
        }
        
        //making sure was not deleted that save 
        var saves = Directory.GetDirectories(SugarMillPath()).ToList();
        for (int i = 0; i < saves.Count; i++)
        {
            if (saves[i] == ReturnPathPlsThsName(tileNameSelected))
            {
                return true;
            }
        }
        return false;
    }

    static string ReturnPathPlsThsName(string name)
    {
        return _path + @"\" + name;
    }


    /// <summary>
    /// Saving all XMLS
    /// 
    /// if 'callEscapeKey' will call : Program.InputMain.EscapeKey();
    /// should be called always unless is a quicksave
    /// </summary>
    public static void SaveNow(bool callEscapeKey)
    {
        PlayerPrefs.Save();

        Directory.CreateDirectory(savePath);
        XMLSerie.SaveGame(savePath); 

        //print("Resave Spawner and Buildings");
        Program.gameScene.controllerMain.TerraSpawnController.ReSaveData();
        BuildingPot.SaveLoad.Save();
        PersonPot.SaveLoad.Save();
        CamControl.CAMRTS.InputRts.SaveLastCamPos();

        savePath = "";

        if (callEscapeKey)
        {
            //so goes back to show the game 
            Program.InputMain.EscapeKey();
        }
    }

    private static bool HasHDDSpace()
    {
        //bytes
        ulong space = 100 * 1024 * 1024; //*1024*1024 makes it MegaBytes
        return DriveSpace.DriveFreeBytes(_path, out space);
    }


    public static bool ThereIsAtLeastAGameToLoad()
    {
        var saves = Directory.GetDirectories(SugarMillPath()).ToList();
        return saves.Count > 0;
    }


    internal static string SugarMillPath()
    {
        return _path;
    }



    internal static void LoadGame(string p)
    {
        PlayerPrefs.SetString("Last_Saved", p);
        PlayerPrefs.Save();

        XMLSerie.LoadGame(_path + @"\" + p);
        Load2ndStep();
    }

    internal static void LoadGameTuto()
    {
        //PlayerPrefs.SetString("Last_Saved", "");
        //PlayerPrefs.Save();

        XMLSerie.LoadGame(Application.dataPath + @"\" + "Tutorial");
        Load2ndStep();
    }

    static void Load2ndStep()
    {
        SetLoadedTerrainInTerraRoot();

        Program.RedoGame();

        //now action the continue button event
        BuildingPot.LoadBuildingsNow();
        Program.MyScreen1.DestroyCurrLoadLoading();
    }

    /// <summary>
    /// It sets Program.MyScreen1.TerraRoot so it loads the terrain saved 
    /// </summary>
    /// <param name="p"></param>
    static void SetLoadedTerrainInTerraRoot()
    {
        PersonData pData = XMLSerie.ReadXMLPerson();

        //so it loads the saved terrain into the new game 
        Program.MyScreen1.TerraRoot = MyScreen.AddPrefabTerrainRoot(pData.PersonControllerSaveLoad.TerrainName);
    }




    internal static void ContinueGame()
    {
        var name = PlayerPrefs.GetString("Last_Saved");
        LoadGame(name);
    }

    public static void DeleteGame(string p)
    {
        savePath = _path + @"\" + p;

        if (Directory.Exists(savePath))
        {
            Dialog.OKCancelDialog(H.DeleteDialog);
        }
    }

    public static void DeleteNow()
    {
        if (Directory.Exists(savePath))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(savePath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            Directory.Delete(savePath);
            Program.MyScreen1.DeleteSavedGameCallBack();
        }

        savePath = "";
    }


#region AutoSave

    private static float lastAutoSavedFile;
    public static void Update()
    {
        if (Time.time > lastAutoSavedFile + Settings.AutoSaveFrec )
        {
            AutoSave();
            lastAutoSavedFile = Time.time;
        }
        if (Time.time > _iconShown + 3 && _iconShown > 0)
        {
            _iconShown = -1;
            Program.MouseListener.CurrForm.HideAutoSaveIcon();
        }
    }



    static void AutoSave()
    {
        if (!Program.gameScene.GameFullyLoaded())
        {
            return;
        }
        SaveGame("AutoSave", true);
    }
#endregion

    
    public static void LastModifiedTime()
    {
        if (Directory.Exists(savePath))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(savePath);
            foreach (FileInfo file in di.GetFiles())
            {

            }
            Directory.Delete(savePath);
            Program.MyScreen1.DeleteSavedGameCallBack();
        }

        savePath = "";
    }

    bool IsLoadFileValid(string name)
    {
        var thisPath = _path + @"\" + name;
        var getSavedStamp = PlayerPrefs.GetString(name);

        return false;
    }


}



class DriveSpace
{
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
    out ulong lpFreeBytesAvailable,
    out ulong lpTotalNumberOfBytes,
    out ulong lpTotalNumberOfFreeBytes);

    public static bool DriveFreeBytes(string folderName, out ulong freespace)
    {
        freespace = 0;
        if (string.IsNullOrEmpty(folderName))
        {
            throw new ArgumentNullException("folderName");
        }

        if (!folderName.EndsWith("\\"))
        {
            folderName += '\\';
        }

        ulong free = 0, dummy1 = 0, dummy2 = 0;

        if (GetDiskFreeSpaceEx(folderName, out free, out dummy1, out dummy2))
        {
            freespace = free;
            return true;
        }
        else
        {
            return false;
        }
    }
}




public class SimpleEncryption
{
    #region Constructor
    public SimpleEncryption(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltValueBytes = Encoding.UTF8.GetBytes(SaltValue);

        _DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltValueBytes, PasswordIterations);
        _InitVectorBytes = Encoding.UTF8.GetBytes(InitVector);
        _KeyBytes = _DeriveBytes.GetBytes(32);
    }
    #endregion

    #region Private Fields
    private readonly Rfc2898DeriveBytes _DeriveBytes;
    private readonly byte[] _InitVectorBytes;
    private readonly byte[] _KeyBytes;
    #endregion

    private const string InitVector = "T=A4rAzu94ez-dra";
    private const int PasswordIterations = 1000; //2;
    private const string SaltValue = "d=?ustAF=UstenAr3B@pRu8=ner5sW&h59_Xe9P2za-eFr2fa&ePHE@ras!a+uc@";

    public string Decrypt(string encryptedText)
    {
        byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);
        string plainText;

        RijndaelManaged rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };

        try
        {
            using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(_KeyBytes, _InitVectorBytes))
            {
                using (MemoryStream memoryStream = new MemoryStream(encryptedTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        //TODO: Need to look into this more. Assuming encrypted text is longer than plain but there is probably a better way
                        byte[] plainTextBytes = new byte[encryptedTextBytes.Length];

                        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                    }
                }
            }
        }
        catch (CryptographicException exception)
        {
            plainText = string.Empty; // Assume the error is caused by an invalid password
        }

        return plainText;
    }

    public string Encrypt(string plainText)
    {
        string encryptedText;
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        RijndaelManaged rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };

        using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(_KeyBytes, _InitVectorBytes))
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    byte[] cipherTextBytes = memoryStream.ToArray();
                    encryptedText = Convert.ToBase64String(cipherTextBytes);
                }
            }
        }

        return encryptedText;
    }
}