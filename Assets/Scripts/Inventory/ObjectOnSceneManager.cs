using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class ObjectOnSceneManager : Saver
{
    [SerializeField] private List<InteractibleObjects> interactibleOnScene = new List<InteractibleObjects>();
    [SerializeField] List<InteractibleObjects> interactibleObjectsList = new List<InteractibleObjects>();
    [SerializeField] List<InteractibleObjectsData> objectsDataList = new List<InteractibleObjectsData>();

    private void Awake()
    {
        ObjectManager.onDrop += Drop;
        ObjectManager.onTake += Take;
        
    }

    private void Start()
    {
        interactibleObjectsList = ObjectManager.Instance.interactibleObjects;
        if (!SaveData.Has("progress"))
        {
            FindAllinScene();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ObjectManager.onDrop -= Drop;
        ObjectManager.onTake -= Take;
    }


    private void Drop(InteractibleObjects interactibleObjects)
    {
        if (!interactibleOnScene.Contains(interactibleObjects))
        {
            interactibleOnScene.Add(interactibleObjects);
        }
    }
    private void Take(InteractibleObjects interactibleObjects)
    {
        if (interactibleOnScene.Contains(interactibleObjects))
        {
            interactibleOnScene.Remove(interactibleObjects);
        }
    }


    private void FindAllinScene()
    {
        InteractibleObjects[] curObjects = FindObjectsOfType<InteractibleObjects>();
        foreach (var v in curObjects)
        {
            if (v != null)
            {
                interactibleOnScene.Add(v);
            }
        }
    }
    private void DeleteAllinScene()
    {
       for(int i = 0; i < interactibleOnScene.Count; i++)
       {         
            Destroy(interactibleOnScene[i].gameObject);
       }
        interactibleOnScene.Clear();
    }
   
    public override void Save()
    {
        objectsDataList.Clear();

        foreach (var i in interactibleOnScene)
        {
            Debug.Log(i);
            InteractibleObjectsData objectData = new InteractibleObjectsData(i.gameObject.transform, i.typeInteractible);         
            objectsDataList.Add(objectData);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(List<InteractibleObjectsData>));
        using (StringWriter writer = new StringWriter())
        {
            serializer.Serialize(writer, objectsDataList);
            string xml = writer.ToString();
            Debug.Log(xml);
            SaveData.SetString("interactibleObjectsDataList", xml);
        }

    }

   
    public override void Load()
    {
        FindAllinScene();
        if (SaveData.Has("interactibleObjectsDataList"))
        {
            string xml = SaveData.GetString("interactibleObjectsDataList");
            if (xml.Length > 0)
            {
                DeleteAllinScene();

                XmlSerializer serializer = new XmlSerializer(typeof(List<InteractibleObjectsData>));
                using (StringReader reader = new StringReader(xml))
                {
                    List<InteractibleObjectsData> objectsDataList = (List<InteractibleObjectsData>)serializer.Deserialize(reader);

                    foreach (var objectData in objectsDataList)
                    {
                        InteractibleObjects interactibleObject = GetInteractibleObjectByType(objectData.typeInteractible);

                        if (interactibleObject != null)
                        {
                            InteractibleObjects curInt = Instantiate(interactibleObject, objectData.GetPosition(), objectData.GetRotation());
                            interactibleOnScene.Add(curInt);
                        }
                    }
                }
            }
        }
    }

    public override void DeleteSave()
    {
        SaveData.Delete("interactibleObjectsDataList");
    }

    private InteractibleObjects GetInteractibleObjectByType(string type)
    {
        InteractibleObjects intObj = null;
        foreach(var o in interactibleObjectsList)
        {
            if (o.typeInteractible.ToString().ToLower() == type.ToLower())
            {
                intObj = o;
            }
        }
        return intObj;

    }
}
[System.Serializable]
public class InteractibleObjectsData
{
    public string typeInteractible;
    public float posX;
    public float posY;
    public float posZ;
    public float rotX, rotY, rotZ, rotW;
    public InteractibleObjectsData()
    {
    }
    public InteractibleObjectsData(Transform trans, TypeInteractible typeInteractible)
    {
        this.typeInteractible = typeInteractible.ToString();
        posX = (float)Math.Round(trans.position.x, 3);
        posY = (float)Math.Round(trans.position.y, 3);
        posZ = (float)Math.Round(trans.position.z, 3);
        rotX = (float)Math.Round(trans.rotation.x, 3);
        rotY = (float)Math.Round(trans.rotation.y, 3);
        rotZ = (float)Math.Round(trans.rotation.z, 3);
        rotW = (float)Math.Round(trans.rotation.w, 3);
    }      

    public Vector3 GetPosition()
    {
        return new Vector3(posX, posY, posZ); 
    }
    public Quaternion GetRotation()
    {
        return new Quaternion(rotX, rotY, rotZ, rotW);
    }
}
