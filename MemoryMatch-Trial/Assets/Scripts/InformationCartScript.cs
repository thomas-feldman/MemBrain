using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store and carry information between loading scenes.
/// 
/// To store information to pass, use InformationCartScript.instance.StoreInformation().
/// 
/// To store information other than strings, use the following method;
/// EXAMPLE
/// List<string> informationToPass = new List<string>(); // Create an empty list of strings
/// 
/// informationToPass.Add( YOUR_VARIABLE_HERE.ToString() ); // This converts the information into a string, and adds it to the list.
/// 
/// To retrieve information, use InformationCartScript.instance.RetrieveInformation()
/// 
/// When you retrieve information, it will be returned as a List<string> which needs to be converted back, or interpreted on the script end.
/// EXAMPLE - storing (INT, FLOAT, STRING)
/// List<string> retrievedInformation = InformationCartScript.instance.RetrieveInformation(MY_STORAGE_KEY);
/// 
/// int retrievedInt = (int)retrievedInformation[0];
/// float retrievedFloat = (float)retrievedInformation[1];
/// string retrievedString = retrievedInformation[2];
/// 
/// </summary>
public class InformationCartScript : MonoBehaviour {

    #region InformationCart Instancing
    // The public instance to access the information cart
    public static InformationCartScript instance = null;

    private void Awake()
    {
        // Check if a gamemanager exists
        if (instance == null)
        {
            instance = this; // If not, set it to this
        }
        else
        {
            Destroy(gameObject); // Otherwise destroy this
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // Stores the list of scripts to pass to
    private List<string> StorageKeyList = new List<string>();

    // Stores a list of lists of strings.
    private List<List<string>> InformationList = new List<List<string>>();

    /// <summary>
    /// Stores a storageKey, and associated information, into the lists at the same index.
    /// </summary>
    /// <param name="storageKey">The key to store the information under.</param>
    /// <param name="informationStrings">Information to store. Try to use a unique key to avoid later retrieving the wrong information.</param>
    public void StoreInformation(string storageKey, List<string> informationStrings)
    {
        StorageKeyList.Add(storageKey);
        InformationList.Add(informationStrings);
    }

    /// <summary>
    /// Finds the information stored with a given storageKey, removes it from the lists and returns it.
    /// </summary>
    /// <param name="storageKey">The key used to store the information.</param>
    /// <returns></returns>
    public List<string> RetrieveInformation(string storageKey)
    {
        // Find the index that contains the storageKey
        int index = StorageKeyList.IndexOf(storageKey);

        // Retrieve and store the information at that index
        List<string> informationRetrieved = InformationList[index];

        // Remove those items from the list
        StorageKeyList.RemoveAt(index);
        InformationList.RemoveAt(index);

        // Return the retrieved information
        return informationRetrieved;
    }
}
