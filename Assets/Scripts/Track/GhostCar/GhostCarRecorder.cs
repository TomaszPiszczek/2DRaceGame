using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostCarRecorder : MonoBehaviour
{
    public Transform carSpriteObject;
    GhostCarData ghostCarData = new GhostCarData();
    bool isRecording = true;
    public GameObject ghostCarPlaybackPrefab;
    CarInputHandler carInputHandler;

    Rigidbody2D carRigidBody2D;

    private void Awake()
    {
        carRigidBody2D =  GetComponent<Rigidbody2D>();
        carInputHandler = GetComponent<CarInputHandler>();
    }
    private void Start() {
        GameObject ghostCar = Instantiate(ghostCarPlaybackPrefab);
        ghostCar.GetComponent<GhostCarPlayback>().LoadData(carInputHandler.playerNumber);



        StartCoroutine(RecordCarPostionCo());
        StartCoroutine(SaveCarPostionCO());
    }

    IEnumerator SaveCarPostionCO()
    {
        yield return new WaitForSeconds(20);
        SaveData();
    }
   IEnumerator RecordCarPostionCo()
   {
    while(isRecording)
    {
    if(carSpriteObject !=null)

        {
        ghostCarData.AddDataItem(new GhostCarDataListItem(carRigidBody2D.position,carRigidBody2D.rotation,carSpriteObject.localScale,Time.timeSinceLevelLoad));

        }
    
    yield return new WaitForSeconds(0.01f);


    }
   }

   void SaveData()
   {
    string jsonEncodedData = JsonUtility.ToJson(ghostCarData);
    Debug.Log($"Saved data {jsonEncodedData}");

    if(carInputHandler != null)
    {
        PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name}_{carInputHandler.playerNumber}_ghost",jsonEncodedData);

        PlayerPrefs.Save();
    }

    isRecording = false;
   }
}
