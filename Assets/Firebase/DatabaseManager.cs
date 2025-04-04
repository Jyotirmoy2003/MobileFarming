using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using System.IO;

public class DatabaseManager : MonoSingleton<DatabaseManager>
{
    private DatabaseReference databaseReference;
    private FirebaseDatabase database;
    private bool isFirebaseReady = false;
    private string databaseUrl = "";
    private string lastGeneratedCode;

    private async void Start()
    {
        LoadDatabaseURL();
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
    }


    [NaughtyAttributes.Button]
    public void CreateUser()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet!");
            return;
        }

        CreateSharedInfo(lastGeneratedCode = CodeGenerator.GenerateRandomCode(), 100, true);
    }

    [NaughtyAttributes.Button ]
    void ReadValue()
    {
       Debug.Log( ValidateCode(lastGeneratedCode));
       DeleteCode(lastGeneratedCode);
    }

    public void CreateSharedInfo(string code, int goldAmount, bool isValid)
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet!");
            return;
        }

        SharedInfo sharedInfo = new SharedInfo(code, goldAmount, isValid);
        string jsonData = JsonUtility.ToJson(sharedInfo);

        // Write data in database
        databaseReference.Child("Codes").Child(code).SetRawJsonValueAsync(jsonData)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("Data successfully written to Firebase!");
                }
                else
                {
                    Debug.LogError("Error writing data: " + task.Exception);
                }
            });
    }


    


    //Read data from the database
    public async Task<int> ValidateCode(string code)
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not initialized yet!");
            return -1;
        }

        DatabaseReference codeReference = databaseReference.Child("Codes").Child(code);
        
        var dataSnapshot = await codeReference.GetValueAsync();

        if (dataSnapshot.Exists)
        {
            string jsonData = dataSnapshot.GetRawJsonValue();
            SharedInfo sharedInfo = JsonUtility.FromJson<SharedInfo>(jsonData);

            if (sharedInfo.isValid)
            {
                Debug.Log($"Code {code} is valid! Gold Amount: {sharedInfo.goldAmount}");
                return sharedInfo.goldAmount;
            }
        }

        Debug.Log($"Code {code} is invalid or not found in database.");
        return -1;
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
    public int goldAmount;
    public bool isValid;

    public SharedInfo(string code,int goldAmount,bool isValid)
    {
        this.code = code;
        this.goldAmount = goldAmount;
        this.isValid = isValid;
    }

    public SharedInfo()
    {
        code ="";
        goldAmount =0;
        isValid = false;
    }
}