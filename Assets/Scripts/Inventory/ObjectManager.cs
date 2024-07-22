using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
public class ObjectManager : Saver
{


    public static Action <bool> onUseInteractibleWeapon;
    public static Action<InteractibleObjects> onDrop;
    public static Action<InteractibleObjects> onTake;
    public UnityEvent OnTake;
    public static Action onHasNoSlots;
    public static Action<bool> onReadyObjectToDrop;

    [SerializeField] private int currentCapasity;
    [SerializeField] private int maxCapasity;
    [SerializeField] private int startCapacity;
    public List<InteractibleObjects> interactibleObjects;
    [SerializeField] private List<ObjectButton> slots = new List<ObjectButton>();
    [SerializeField] private ObjectButton buttonPrefab;
    [SerializeField] private Transform buttonHook;
    [SerializeField] private ObjectButton lastPressedButton = null;
    [SerializeField] private InteractibleObjects currentobject;
    [SerializeField] private GameObject currentReadyObject;
    [SerializeField] private Transform objectPoint;
    private Ray ray;
    [SerializeField] private float raycastDistance = 5;
    [SerializeField] private LayerMask layerMask;
    private bool canRotate;

    [SerializeField] private Material buildMaterial;
    [SerializeField] private Material curMaterial;
    [SerializeField] private Material startMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Button rightButton, leftButton;

    public static ObjectManager Instance;

    [SerializeField] private Button dropButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Transform spawnShopPoint;
    [SerializeField] private Vector3 posToSpawn;
    [SerializeField] private GameObject noSlotsMessage;
    [SerializeField] private float fuelCountForFlameThrower;    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        ObjectButton.onClickObjectButton += ClickObjectButton;
        UIManager.onTakeInteractible += TakeInteractible;
        UIManager.onDrop += Drop;
        rightButton.onClick.AddListener(RotateRight);
        leftButton.onClick.AddListener(RotateLeft);
        dropButton.onClick.AddListener(Drop);
        useButton.onClick.AddListener(Use);
        FlameThrower.onUpdateFuel += UpdateFuelCountForThrower;
        currentCapasity = startCapacity;
    }

    private void Use()
    {
        if (currentobject != null)
        {
            if (currentobject.isUsable&&currentobject.isUseInHand)
            {
                currentobject.Interact();
            }
        }
    }
    
    public void UpdateFuelCountForThrower(float count)
    {
        fuelCountForFlameThrower = count;
    }

    public float GetFuelCountForThrower()
    {
        return fuelCountForFlameThrower;
             
    }

    public override void Load()
    {
        fuelCountForFlameThrower = SaveData.GetFloat("throwerFuel", 100);           
             
        CreateButtons(currentCapasity);

        for (int i = 0; i < currentCapasity; i++)
        {
            if (SaveData.Has("slot" + i.ToString()))
            {
                string savedSlotData = SaveData.GetString("slot" + i.ToString());
                
                string[] parts = savedSlotData.Split('_');

                if (parts.Length == 2)
                {
                    string curType = parts[0];
                    int count;                    
                    if (int.TryParse(parts[1], out count))
                    {
                        slots[i].Init(SetInteractible(curType), count);                        
                    }
                    else
                    {                       
                        Debug.LogError("Error parsing count for slot " + i.ToString());
                    }
                }
                else
                {
                    Debug.LogError("Invalid format for slot " + i.ToString());
                }
            }
        }
    }

    public override void Save()
    {
        SaveData.SetFloat("throwerFuel", fuelCountForFlameThrower);
        SaveData.SetInt("capasity", slots.Count);
        foreach (var s in slots)
        {           
            SaveData.SetString("slot" + slots.IndexOf(s), s.curObj.typeInteractible.ToString()+"_"+s.curObj.count);
        }
    }
    private InteractibleObjects SetInteractible(string typeInteractible)
    {
        InteractibleObjects interactible = null;
        foreach (var i in interactibleObjects)
        {
            if (i.typeInteractible.ToString() == typeInteractible)
            {
                interactible = i;
            }
        }
        return interactible;
    }

    public bool IsHasSlotWhenTryBut(InteractibleObjects interactibleObjects)
    {
        foreach(var s in slots)
        {
            if (s.curObj.typeInteractible == interactibleObjects.typeInteractible)
            {
                return true;
            }
        }
        if (GetFreeSlot() >= 0)
        {
            return true;
        }
        else return false;
    }

    public void SpawnGood(InteractibleObjects interactibleObjects, int count)
    {

        ObjectButton curobjectButton = null;
        foreach (var s in slots)
        {
            if (s.curObj.typeInteractible == interactibleObjects.typeInteractible)
            {
                curobjectButton = s;
                curobjectButton.AddCount(count);
            }
        }
        if (curobjectButton == null)
        {
            int index = GetFreeSlot();
            curobjectButton = slots[index];
            curobjectButton.Init(interactibleObjects, count);
        }
    }


  

    public override void DeleteSave()
    {
        SaveData.Delete("capasity");
        for (int i = 0; i < maxCapasity; i++)
        {
            SaveData.Delete("slot" + i.ToString());
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        ObjectButton.onClickObjectButton -= ClickObjectButton;
        UIManager.onTakeInteractible -= TakeInteractible;
        UIManager.onDrop -= Drop;
        FlameThrower.onUpdateFuel -= UpdateFuelCountForThrower;
    }




    private void Start()
    {
        if (!SaveData.Has("progress"))
        {          
            CreateButtons(startCapacity);           
            fuelCountForFlameThrower = SaveData.GetFloat("throwerFuel", 100);
        }
    }

    private void CreateButtons(int cap)
    {
        for (int i = 0; i < cap; i++)
        {
            ObjectButton objectButton = Instantiate(buttonPrefab, buttonHook);           
            objectButton.Deinit();
            slots.Add(objectButton);
        }
    }

    private ObjectButton AddButton()
    {
       ObjectButton objectButton = Instantiate(buttonPrefab, buttonHook);       
       objectButton.Deinit();
       slots.Add(objectButton);
       return objectButton;
    }

    public bool IsHasSlot()
    {
        int countOfEmpty = 0;
        foreach (var s in slots)
        {
            if (s.curObj == null)
            {
                countOfEmpty++;
            }
        }
        if (countOfEmpty > 0) return true;
        else return false;
    }


    private void ClickObjectButton(ObjectButton objectButton)
    {
        if (objectButton != lastPressedButton)
        {
            if (lastPressedButton != null)
                lastPressedButton.UnpressButton();
            if (objectButton.curObj.typeInteractible!=TypeInteractible.none)
            {
                if (currentobject == null)
                {
                    ShowObject(objectButton);
                    objectButton.PressButton();
                }
                else
                {
                    HideObject();
                    ShowObject(objectButton);
                    objectButton.PressButton();
                }
            }
            else
            {
                if (currentobject != null)
                {
                    if(lastPressedButton!=null)
                    lastPressedButton.UnpressButton();
                    HideObject();
                }
            }
        }
        else
        {
            if (objectButton.curObj.typeInteractible != TypeInteractible.none)
            {
                if (currentobject == null)
                {
                    ShowObject(objectButton);
                    objectButton.PressButton();
                }
                else
                {
                    HideObject();                   
                    ShowRotateButtons(false);
                    objectButton.UnpressButton();

                }
            }
            else
            {
                ShowRotateButtons(false);
                return;
            }
            
        }
        lastPressedButton = objectButton;
    }



    private void Drop()
    {
        if(meshRenderer!=null)
        meshRenderer.sharedMaterial = startMaterial;
        lastPressedButton.AddCount(-1);
      
        currentobject.transform.DOScale(new Vector3(1, 1, 1), 0.1f);
        currentobject.gameObject.layer = 8;       
        currentobject.transform.SetParent(null);       
        currentobject.Drop();
        onDrop?.Invoke(currentobject);
        if (currentobject.isWeaponPoint) onUseInteractibleWeapon?.Invoke(false); 
        NavMeshObstacle navMeshObstacle = currentobject.GetComponent<NavMeshObstacle>();
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = true;
        }
        currentobject = null;
        ShowRotateButtons(false);
        if (lastPressedButton.curObj.count == 0)
        {
            lastPressedButton.Deinit();
            lastPressedButton.UnpressButton();
            lastPressedButton = null;
        }
        else
        {
            ShowObject(lastPressedButton);
        }        
    }
    private void PutObject(InteractibleObjects currentObject)
    {
        lastPressedButton.Deinit();
        lastPressedButton.UnpressButton();
    }


    private void TakeInteractible(InteractibleObjects interactibleObjects)
    {
        ObjectButton curobjectButton = null;
        foreach (var s in slots)
        {
            if (s.curObj != null)
            {
                if (s.curObj.typeInteractible == interactibleObjects.typeInteractible)
                {
                    curobjectButton = s;
                    curobjectButton.AddCount(1);
                    onTake?.Invoke(interactibleObjects);
                    OnTake?.Invoke();
                    Destroy(interactibleObjects.gameObject);
                }
            }
        }
        if (curobjectButton == null)
        {
            int index = GetFreeSlot();
            if (index >= 0)
            {
                slots[index].Init(interactibleObjects, 1);
                onTake?.Invoke(interactibleObjects);
                OnTake?.Invoke();
                Destroy(interactibleObjects.gameObject);
            }
            else
            {
                noSlotsMessage.SetActive(true);
            }
        }
        
    }

    private void IsHasSameObject(InteractibleObjects interactibleObjects)
    {
       
    }

    public bool isHasSlot(String typeInteractible)
    {
        return false;
    }
    public bool isHasSlot()
    {
        return false;
    }




    private int GetFreeSlot()
    {
        int index = -1;
        foreach (var s in slots)
        {
            Debug.Log(s.curObj.typeInteractible);
            if (s.curObj.typeInteractible == TypeInteractible.none)
            {
                index = slots.IndexOf(s);
               
                return index;
            }
        }
        return index;

    }


    private void HideObject()
    {
        if (currentobject != null)
        {
            if (currentobject.isWeaponPoint) onUseInteractibleWeapon?.Invoke(false);
            Destroy(currentobject.gameObject);
            currentobject = null;
        }
        onReadyObjectToDrop?.Invoke(false);

    }
    private void ShowObject(ObjectButton objectButton)
    {
        if (objectButton.curObj.typeInteractible == TypeInteractible.none) return;

        InteractibleObjects prefab = FindPrefab(objectButton);



        if (!prefab.isWeaponPoint)
        {
            currentobject = Instantiate(prefab, PointOfTake.Instance.point2);            
        }
        else
        {
            currentobject = Instantiate(prefab, PointOfTake.Instance.weaponPoint);
            currentobject.transform.position = PointOfTake.Instance.weaponPoint.position;
            currentobject.transform.localRotation = Quaternion.Euler(currentobject.takeRotation.x, currentobject.takeRotation.y, currentobject.takeRotation.z);
            onUseInteractibleWeapon?.Invoke(true);              
            currentobject.InHand();
        }

        if (currentobject.isUsable && currentobject.isUseInHand) useButton.gameObject.SetActive(true);
        meshRenderer = currentobject.gameObject.GetComponent<MeshRenderer>();
        curMaterial = meshRenderer.sharedMaterial;
        startMaterial = curMaterial;
        Collider col = currentobject.GetComponent<Collider>();
        if(col!=null) col.isTrigger = true;
        onReadyObjectToDrop?.Invoke(true);      
        currentobject.gameObject.layer = 2;
        currentobject.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0f);
        NavMeshObstacle navMeshObstacle = currentobject.GetComponent<NavMeshObstacle>();
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = false;
        }

    }

    private InteractibleObjects FindPrefab(ObjectButton objectButton)
    {
        InteractibleObjects cur = null;
        foreach (var s in interactibleObjects)
        {
            if (s.typeInteractible == objectButton.curObj.typeInteractible)
            {
                cur = s;
            }
        }
        return cur;
    }

    private void Update()
    {
        if (currentobject != null&&!currentobject.isWeaponPoint)
        {
            ray.origin = Camera.main.transform.position;
            ray.direction = Camera.main.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
            {
                canRotate = true;
                ShowRotateButtons(canRotate);
                currentobject.transform.position = transform.InverseTransformPoint(hit.point);
                meshRenderer.sharedMaterial = buildMaterial;
                // Set the rotation to make the object vertically oriented
                currentobject.transform.rotation = Quaternion.Euler(0f, currentobject.transform.eulerAngles.y, 0f);
                currentobject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                canRotate = false;
                ShowRotateButtons(canRotate);
                meshRenderer.sharedMaterial = startMaterial;
                currentobject.transform.position = PointOfTake.Instance.point2.position;
                currentobject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
        else if(currentobject != null && currentobject.isWeaponPoint)
        {
            dropButton.gameObject.SetActive(true);
        }
        else if (currentobject == null)
        {
            dropButton.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F)&&currentobject!=null)
        {
            Drop();
        }
    }
  

    public void ShowRotateButtons(bool isNeedShow)
    {
        if(isNeedShow)
        {
            rightButton.gameObject.SetActive(true);
            leftButton.gameObject.SetActive(true);
            dropButton.gameObject.SetActive(true);
            if (currentobject.isUsable)
                useButton.gameObject.SetActive(true);
        }
        else
        {
            rightButton.gameObject.SetActive(false);
            leftButton.gameObject.SetActive(false);
            dropButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
    }

    public void  RotateLeft()
    {
        if (canRotate)
        {
            if (currentobject != null)
            {
                currentobject.transform.Rotate(0f, -45f, 0f, Space.World);
            }
        }
    }

    public void RotateRight()
    {
        if(canRotate)
        {
            if (currentobject != null)
            {
                currentobject.transform.Rotate(0f, 45f, 0f, Space.World);
            }
     
        }
    }
   

   
    





}
