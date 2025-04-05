using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using System.IO;
using System;
using jy_util;

public class DatabaseManager : MonoSingleton<DatabaseManager>
{
    private DatabaseReference databaseReference;
    private FirebaseDatabase database;
    private bool isFirebaseReady = false;
    private string databaseUrl = "";
    private string lastGeneratedCode;

    private async void Start()
    {
        //LoadDatabaseURL();
        databaseUrl = "https://field-frenzy-default-rtdb.asia-southeast1.firebasedatabase.app/";
        await InitializeFirebase();
    }

    private void LoadDatabaseURL()
    {
        string path = Path.Combine(Application.dataPath, "credential.text");

        if (File.Exists(path))
        {
            databaseUrl = File.ReadAllText(path).Trim();
            Debug.Log($"Database URL loaded: {databaseUrl}");
        }
        else
        {
            Debug.LogError("credential.text file not found! Make sure it's placed in Assets folder.");
        }
    }

    private async Task InitializeFirebase()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == Firebase.DependencyStatus.Available && !string.IsNullOrEmpty(databaseUrl))
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            database = FirebaseDatabase.GetInstance(app, databaseUrl);
            databaseReference = database.RootReference;
            isFirebaseReady = true;
            Debug.Log("Firebase Initialized Successfully!");
        }
        else
        {
            Debug.LogError($"Firebase dependencies not met: {dependencyStatus} or Database URL is missing.");
        }
        Debug.Log("Database URL: " + databaseUrl);
    }


    [NaughtyAttributes.Button]
    public void CreateUser()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet!");
            return;
        }

        CreateSharedInfo(lastGeneratedCode = CodeGenerator.GenerateRandomCode(),E_Inventory_Item_Type.Corn, 100);
    }

    [NaughtyAttributes.Button ]
    void ReadValue()
    {
       Debug.Log( ValidateCode(lastGeneratedCode));
       DeleteCode(lastGeneratedCode);
    }

    public async Task<bool> CreateSharedInfo(string code,E_Inventory_Item_Type item_Type ,int itemAmount)
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet!");
            return false;
        }

        SharedInfo sharedInfo = new SharedInfo(code,(int)item_Type, itemAmount);
        string jsonData = JsonUtility.ToJson(sharedInfo);

        try
        {
            await databaseReference.Child("Codes").Child(code).SetRawJsonValueAsync(jsonData);
            Debug.Log("Data successfully written to Firebase!");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error writing data: " + ex.Message);
            return false;
        }
    }


    


    //Read data from the database
    public async Task<SharedInfo> ValidateCode(string code)
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not initialized yet!");
            return null;
        }

        DatabaseReference codeReference = databaseReference.Child("Codes").Child(code);
        
        var dataSnapshot = await codeReference.GetValueAsync();

        if (dataSnapshot.Exists)
        {
            string jsonData = dataSnapshot.GetRawJsonValue();
            SharedInfo sharedInfo = JsonUtility.FromJson<SharedInfo>(jsonData);

            
            Debug.Log($"Code {code} is valid! Gold Amount: {sharedInfo.itemAmount}");
            return sharedInfo;
        }
        

        Debug.Log($"Code {code} is invalid or not found in database.");
        return null;
    }




    public async Task DeleteCode(string code)
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not initialized yet!");
            return;
        }

        DatabaseReference codeReference = databaseReference.Child("Codes").Child(code);

        var dataSnapshot = await codeReference.GetValueAsync();

        if (dataSnapshot.Exists)
        {
            await codeReference.RemoveValueAsync();
            Debug.Log($"Code {code} deleted successfully from the database.");
        }
        else
        {
            Debug.Log($"Code {code} not found in the database.");
        }
    }
}




public class SharedInfo
{
    public string code;
    public int item_type;
    public int itemAmount;
    

    public SharedInfo(string code,int item_Type,int itemAmount)
    {
        this.code = code;
        this.item_type = item_Type;
        this.itemAmount = itemAmount;
    }

    public SharedInfo()
    {
        code ="";
        itemAmount =0;
       
    }
}