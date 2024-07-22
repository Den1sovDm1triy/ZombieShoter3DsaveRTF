using UnityEngine;

public class LensFlareController : MonoBehaviour
{
    public Light sunLight; 
    public FlareLayer flareLayer;

    void Update()
    {
        
        if (sunLight != null && flareLayer != null)
        {
            Vector3 toSun = sunLight.transform.position - Camera.main.transform.position;
            RaycastHit hit;

            
            if (Physics.Raycast(Camera.main.transform.position, toSun, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Untagged")) // Проверяем, является ли объект стеной
                {
                    
                    flareLayer.enabled = false;
                }
                else
                {
                    
                    flareLayer.enabled = true;
                }
            }
            else
            {
                
                flareLayer.enabled = true;
            }
        }
    }
}
