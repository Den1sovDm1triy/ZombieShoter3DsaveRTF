using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    [SerializeField] private Transform pointerHook;
    [SerializeField] PointerIcon _pointerEnemyIconPrefab;
    [SerializeField] PointerIcon _pointerWeaponItemIconPrefab;
    private List<PointerData> _enemyPointers = new List<PointerData>();
    private List<PointerData> _weaponItemPointers = new List<PointerData>();
    Transform _playerTransform;
    Camera _camera;

    public static PointerManager Instance;

    private void Awake()
    {
        _camera = Camera.main;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _playerTransform = PlayerInstance.Instance.pointOfSearch.transform;
    }

    public void AddToEnemyList(EnemyPointer enemyPointer)
    {
        PointerIcon newPointer = Instantiate(_pointerEnemyIconPrefab, pointerHook);
        PointerData data = new PointerData { pointer = enemyPointer, pointerIcon = newPointer };
        _enemyPointers.Add(data);
    }


    public void AddToWeaponItemList(Pointer itemPointer)
    {

        PointerIcon newPointer = Instantiate(_pointerWeaponItemIconPrefab, pointerHook);
        PointerData data = new PointerData { pointer = itemPointer, pointerIcon = newPointer };
        _weaponItemPointers.Add(data);
    }

    public void RemoveFromWeaponItemList(Pointer itemPointer)
    {
        // Создаем временный список для хранения элементов, которые нужно удалить
        List<PointerData> pointersToRemove = new List<PointerData>();

        foreach (var data in _weaponItemPointers)
        {
            if (data.pointer == itemPointer)
            {
                pointersToRemove.Add(data);
            }
        }

        // Удаляем элементы из основного списка и уничтожаем связанные с ними объекты
        foreach (var pointerToRemove in pointersToRemove)
        {
            _weaponItemPointers.Remove(pointerToRemove);
            Destroy(pointerToRemove.pointerIcon.gameObject);
        }
    }






    public void RemoveFromEnemyList(Pointer enemyPointer)
    {
        // Создаем временный список для хранения элементов, которые нужно удалить
        List<PointerData> pointersToRemove = new List<PointerData>();

        foreach (var data in _enemyPointers)
        {
            if (data.pointer == enemyPointer)
            {
                pointersToRemove.Add(data);
            }
        }

        // Удаляем элементы из основного списка и уничтожаем связанные с ними объекты
        foreach (var pointerToRemove in pointersToRemove)
        {
            _enemyPointers.Remove(pointerToRemove);
            Destroy(pointerToRemove.pointerIcon.gameObject);
        }
    }

    private void LateUpdate()
    {        
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        // Сортируем список по расстоянию от игрока
        _enemyPointers.Sort((a, b) => {
            float distanceA = Vector3.Distance(a.pointer.transform.position, _playerTransform.position);
            float distanceB = Vector3.Distance(b.pointer.transform.position, _playerTransform.position);
            return distanceA.CompareTo(distanceB);
        });

        // Ограничиваем количество обрабатываемых указателей до 5
        int pointersEnemyToShow = Mathf.Min(_enemyPointers.Count, 5);

        // Переключаем видимость ближайших указателей
        for (int i = 0; i < pointersEnemyToShow; i++)
        {
            Pointer enemyPointer = _enemyPointers[i].pointer;
            PointerIcon pointerEnemyIcon = _enemyPointers[i].pointerIcon;

            Vector3 toEnemy = enemyPointer.transform.position - _playerTransform.position;
            Ray ray = new Ray(_playerTransform.position, toEnemy);
            float rayMinDistance = Mathf.Infinity;
            int index = 0;
            for (int p = 0; p < 4; p++)
            {
                if (planes[p].Raycast(ray, out float distance))
                {
                    if (distance < rayMinDistance)
                    {
                        rayMinDistance = distance;
                        index = p;
                    }
                }
            }
            rayMinDistance = Mathf.Clamp(rayMinDistance, 0, toEnemy.magnitude);
            Vector3 worldPosition = ray.GetPoint(rayMinDistance);
            Vector3 position = _camera.WorldToScreenPoint(worldPosition);
            Quaternion rotation = GetIconRotation(index);

            if (toEnemy.magnitude > rayMinDistance)
            {
                pointerEnemyIcon.Show();
            }
            else
            {
                pointerEnemyIcon.Hide();
            }
            pointerEnemyIcon.SetPosition(position, rotation);
        }

        // Скрываем остальные указатели
        for (int i = pointersEnemyToShow; i < _enemyPointers.Count; i++)
        {
            _enemyPointers[i].pointerIcon.Hide();
        }

        for(int i=0; i<_weaponItemPointers.Count; i++)
        {
            Pointer weaponPointer = _weaponItemPointers[i].pointer;
            PointerIcon pointerWeaponItemIcon = _weaponItemPointers[i].pointerIcon;

            Vector3 toWeapon = weaponPointer.transform.position - _playerTransform.position;
            Ray ray = new Ray(_playerTransform.position, toWeapon);
            float rayMinDistance = Mathf.Infinity;
            int index = 0;
            for (int p = 0; p < 4; p++)
            {
                if (planes[p].Raycast(ray, out float distance))
                {
                    if (distance < rayMinDistance)
                    {
                        rayMinDistance = distance;
                        index = p;
                    }
                }
            }
            rayMinDistance = Mathf.Clamp(rayMinDistance, 0, toWeapon.magnitude);
            Vector3 worldPosition = ray.GetPoint(rayMinDistance);
            Vector3 position = _camera.WorldToScreenPoint(worldPosition);
            Quaternion rotation = GetIconRotation(index);
            pointerWeaponItemIcon.Show();
            pointerWeaponItemIcon.SetPosition(position, rotation);
        }
        



    }

    private Quaternion GetIconRotation(int planeIndex)
    {
        if (planeIndex == 0)
        {
            return Quaternion.Euler(0, 0, 90);
        }
        else if (planeIndex == 1)
        {
            return Quaternion.Euler(0, 0, -90);
        }
        else if (planeIndex == 2)
        {
            return Quaternion.Euler(0, 0, 180);
        }
        else if (planeIndex == 3)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        return Quaternion.identity;
    }
}
[System.Serializable]
public class PointerData
{
    public Pointer pointer;
    public PointerIcon pointerIcon;
}
