using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Saver
{
    [SerializeField]private BoxCollider cameraCollider;
    [SerializeField]private Rigidbody cameraRb;
    [SerializeField] private GameObject marker;

    private void Start()
    {        
        PlayerHealth.onDeath += Death;        
    }
    private void OnDestroy()
    {
        base.OnDestroy();
        PlayerHealth.onDeath -= Death;
    }
    private void Death()
    {
        marker.SetActive(false);
        cameraCollider.enabled = true;
        cameraRb.isKinematic = false;
        //cameraRb.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))*10f, ForceMode.Impulse);
        //cameraRb.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 10f, ForceMode.Impulse);
    }

    public override void Save()
    {
        SaveData.SetFloat("PlayerPosX", transform.position.x);
        SaveData.SetFloat("PlayerPosY", transform.position.y);
        SaveData.SetFloat("PlayerPosZ", transform.position.z);
      
        SaveData.SetFloat("PlayerRotX", transform.rotation.eulerAngles.x);
        SaveData.SetFloat("PlayerRotY", transform.rotation.eulerAngles.y);
        SaveData.SetFloat("PlayerRotZ", transform.rotation.eulerAngles.z);


        SaveData.SetFloat("PlayerScaleX", transform.localScale.x);
        SaveData.SetFloat("PlayerScaleY", transform.localScale.y);
        SaveData.SetFloat("PlayerScaleZ", transform.localScale.z);

    }

    public override void Load()
    {       
        float posX = SaveData.GetFloat("PlayerPosX", 0f);
        float posY = SaveData.GetFloat("PlayerPosY", 0f);
        float posZ = SaveData.GetFloat("PlayerPosZ", 0f);
        transform.position = new Vector3(posX, posY, posZ);
               
        float rotX = SaveData.GetFloat("PlayerRotX", 0f);
        float rotY = SaveData.GetFloat("PlayerRotY", 0f);
        float rotZ = SaveData.GetFloat("PlayerRotZ", 0f);
        transform.rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
               
        float scaleX = SaveData.GetFloat("PlayerScaleX", 1f);
        float scaleY = SaveData.GetFloat("PlayerScaleY", 1f);
        float scaleZ = SaveData.GetFloat("PlayerScaleZ", 1f);
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    public override void DeleteSave()
    {
        SaveData.Delete("PlayerPosX");
        SaveData.Delete("PlayerPosY");
        SaveData.Delete("PlayerPosZ");

        SaveData.Delete("PlayerRotX");
        SaveData.Delete("PlayerRotY");
        SaveData.Delete("PlayerRotZ");

        SaveData.Delete("PlayerScaleX");
        SaveData.Delete("PlayerScaleY");
        SaveData.Delete("PlayerScaleZ");
    }
}
