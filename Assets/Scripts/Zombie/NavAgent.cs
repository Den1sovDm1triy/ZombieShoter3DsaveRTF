using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Класс отвечает за перемещение и поворот объекта
/// </summary>
[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(NavMeshAgent))]
public class NavAgent : MonoBehaviour
{
    public NavMeshAgent agent;

    /// <summary>
    /// Скорость перемещения
    /// </summary>
    public float Speed { get => agent.speed; set => agent.speed = value; }

    /// <summary>
    /// Ускорение
    /// </summary>
    public float Acceleration { get => agent.acceleration; set => agent.acceleration = value; }

    /// <summary>
    /// Скорость поворота
    /// </summary>
    public float AngularSpeed { get => agent.angularSpeed; set => agent.angularSpeed = value; }

    /// <summary>
    /// Выполняется асинхронный метод движения или поворота
    /// </summary>
    public bool IsBusy { get; private set; }

    /// <summary>
    /// В этом кадре есть управление движением агента или выполняется асинхронный метод движения
    /// </summary>
    public bool IsMoving
    {
        get => isMoving;
        private set
        {
            isMoving = value;
            isMovingOnLastFrame = value;
        }
    }

    /// <summary>
    /// Текущая скорость агента
    /// </summary>
    public Vector3 Velocity => agent.velocity;

    /// <summary>
    /// Текущая скорость относительно максимальной
    /// Если отрицательная - агент пятится назад
    /// </summary>
    public float RelativeVelocity { get; private set; }

    /// <summary>
    /// Текущая угловая скорость относительно максимальной
    /// Отрицательная - влево, положительная - вправо
    /// </summary>
    public float RelativeAngularVelocity { get; private set; }

    private Vector3 prevDirection;
    private bool isMoving;
    private bool isMovingOnLastFrame;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void LateUpdate()
    {
        if (agent.enabled)
        {
            float sign = Mathf.Sign(Vector3.Dot(Velocity, transform.forward));
            RelativeVelocity = sign * Velocity.magnitude / Speed;
            RelativeAngularVelocity = Vector3.SignedAngle(prevDirection, transform.forward, transform.up) / (AngularSpeed * Time.deltaTime);
            prevDirection = transform.forward;
            isMoving = isMovingOnLastFrame || agent.hasPath;
            isMovingOnLastFrame = false;
        }
    }

    /// <summary>
    /// Вкл/Откл управление агентом
    /// </summary>
    /// <param name="state"></param>
    public void Enable(bool state)
    {
        if (agent.enabled)
        {
            Stop();
        }
        else
        {
            prevDirection = transform.forward;
        }

        agent.enabled = state;
    }

    /// <summary>
    /// Остановить движение и поворот
    /// </summary>
    public void Stop()
    {
        if (!agent.enabled) return;
        StopAllCoroutines();
        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
        IsBusy = false;
        IsMoving = false;
    }

    /// <summary>
    /// Можно ли добраться в точку
    /// </summary>
    /// <param name="position"></param>
    /// <param name="pathLenghtMaxRatio"> Допустимое отношение длины пути к расстоянию. Если путь слишком извилистый - его можно отсечь этим параметром </param>
    /// <returns></returns>
    public bool CanMoveTo(Vector3 position, float pathLenghtMaxRatio = 0f)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        position.y = transform.position.y;
        if (!agent.isOnNavMesh) return false;
        if (agent.CalculatePath(position, navMeshPath))
        {
            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                if (pathLenghtMaxRatio <= 0f) return true;

                // Высчитываем длину пути. Вдруг он окажется недопустимо длинным
                float pathLenght = 0f;
                for (int i = 1; i < navMeshPath.corners.Length; ++i)
                {
                    pathLenght += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
                }

                float distance = Vector3.Distance(navMeshPath.corners[0], navMeshPath.corners[navMeshPath.corners.Length - 1]);

                return pathLenght / distance <= pathLenghtMaxRatio;
            }
        }

        return false;
    }

    /// <summary>
    /// Запустить движение в заданную точку
    /// </summary>
    /// <param name="position"> Целевая позиция </param>
    /// <returns></returns>
    public void MoveToPositionAsync(Vector3 position)
    {
        position.y = transform.position.y;
        StartCoroutine(MoveCoroutine(position));
    }

    /// <summary>
    /// Двигаться к цели втечение одного кадра
    /// </summary>
    /// <param name="target"> Цель </param>
    public void MoveToTarget(GameObject target)
    {
        Vector3 position = Vector3.ProjectOnPlane(target.transform.position, Vector3.up);
        if (target != null && Vector3.Distance(position, transform.position) > agent.stoppingDistance)
        {
            IsMoving = true;
            NavMeshPath path = new NavMeshPath();
            // Строим путь
            if (!agent.isOnNavMesh) return;
            if (!agent.CalculatePath(position, path)) return;
            // Двигаемся по направлению к ближайшей точки пути
            Vector3 direction = path.corners[0] - transform.position;
            agent.velocity = direction.normalized * Mathf.Clamp(Velocity.magnitude + Acceleration * Time.fixedDeltaTime, 0f, Speed);
            // Поворачиваемся туда же
            TurnInDirection(direction);
        }
    }

    /// <summary>
    /// Двигаться в указанном направлении втечение одного кадра
    /// Длина вектора == 1 означает максимальную скорость
    /// Автоматически учитываются препятствия на пути
    /// </summary>
    /// <param name="direction"></param>
    public void MoveInDirection(Vector3 direction)
    {
        direction.y = 0f;
        direction *= Speed * Time.fixedDeltaTime;
        if (direction == Vector3.zero) return;
        NavMeshHit hit = new NavMeshHit();
        bool collision = false;

        for (int i = 0; i < 2; ++i)
        {
            if (agent.Raycast(transform.position + direction + direction.normalized * agent.stoppingDistance, out hit))
            {
                collision = true;
                // Если на пути есть препятствие - пробуем его обойти по нормали от поверхности препятствия
                direction -= Vector3.Project(direction, hit.normal);
            }
            else
            {
                collision = false;
                break;
            }
        }

        if (collision == true)
        {
            agent.velocity = Vector3.zero;
        }
        else
        {
            IsMoving = true;
            agent.velocity = direction / Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Запустить поворот к заданному направлению
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public void TurnToDirectionAsync(Vector3 direction)
    {
        StartCoroutine(TurnCoroutine(direction));
    }

    /// <summary>
    /// Запустить поворот по направлению к цели
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public void TurnToObjectAsync(GameObject target)
    {
        StartCoroutine(TurnCoroutine(target.transform.position - transform.position));
    }

    /// <summary>
    /// Повернуться втечение одного кадра в указанном направлении
    /// Скорость поворота масимальная
    /// </summary>
    /// <param name="direction"></param>
    public void TurnInDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), AngularSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Запустить движение в заданную точку
    /// </summary>
    /// <param name="position"> Целевая позиция </param>
    /// <returns></returns>
    private IEnumerator MoveCoroutine(Vector3 position)
    {
        IsBusy = true;
        if(agent.isOnNavMesh)
        agent.SetDestination(position);
        yield return new WaitWhile(() => agent.pathPending);
        yield return new WaitWhile(() => agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance > agent.stoppingDistance);
        Stop();
    }

    /// <summary>
    /// Запустить поворот к заданному направлению
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator TurnCoroutine(Vector3 direction)
    {
        IsBusy = true;
        Quaternion originRotation = transform.rotation;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);
        float duration = Vector3.Angle(direction, transform.forward) / AngularSpeed;
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(originRotation, Quaternion.LookRotation(direction), Mathf.SmoothStep(0f, 1f, counter / duration));
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.LookRotation(direction);
        Stop();
    }
}
