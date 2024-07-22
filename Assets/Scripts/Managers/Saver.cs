using UnityEngine;

public class Saver : MonoBehaviour
{
    

    public void OnEnable()
    {
        SaveManager.onSave += Save;
        SaveManager.onLoad += Load;
        SaveManager.onDeleteSave += DeleteSave;
    }
    public  virtual void  OnDestroy()
    {
        SaveManager.onSave -= Save;
        SaveManager.onLoad -= Load;
        SaveManager.onDeleteSave -= DeleteSave;
    }
    public virtual void Save() { }
    public virtual void Load() { }

    public virtual void DeleteSave() 
    { 
    }
}
