using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuckenScripts : MonoBehaviour
{
    public Transform[] points;
    public GameObject eggPrefab;
    private float speed;
    public float moveSpeedNormal = 3f;
    public float angrySpeed = 6f;
    public float eatTime = 2f;

    private int currentPointIndex = 0;
    private bool isEating = false;
    private Animator animator;
    float randomDistance = 0.1f;
    bool isAngry = false;
    bool isReadyToNest = false;
    bool isNesting = false;


    void Start()
    {
        speed = moveSpeedNormal;
        animator = GetComponent<Animator>();       
        MoveToNextPoint();
    }



    void Update()
    {
        if (!isEating&&!isNesting)
        {
            MoveToPoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isAngry)
            {               
                StopAllCoroutines();
                StartCoroutine(AngryCor());
            }
        }       
    }

    private IEnumerator AngryCor()
    {
        isAngry = true;
        speed = angrySpeed;
        isEating = false;
        animator.SetBool("Eat", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true);
        eatTime = 0;
        currentPointIndex = Random.Range(0, points.Length);
        yield return new WaitForSeconds(15f);
        eatTime = 2;
        isAngry = false;
        speed = moveSpeedNormal;
        animator.SetBool("Eat", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Run", false);
        MoveToNextPoint();

    }

    void MoveToPoint()
    {        
        if (Vector3.Distance(transform.position, points[currentPointIndex].position) < randomDistance)
        {
            if(!isAngry&&!isReadyToNest)
            StartCoroutine(EatAndMove());
            if (!isAngry && isReadyToNest)
            {
                isReadyToNest = false;
                StartCoroutine(Nest());
            }
            else
            {
                MoveToNextPoint();
            }
        }
        else
        {          
            transform.LookAt(points[currentPointIndex]);
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, speed * Time.deltaTime);            
            animator.SetBool("Walk", true);
        }
    }

    void MoveToNextPoint()
    {
        var x = Random.Range(0, 1000);
        if (x < 990)
        {
            currentPointIndex = Random.Range(0, points.Length-3);
        }
        else
        {
            currentPointIndex = Random.Range(points.Length - 3, points.Length);
            isReadyToNest = true;
            randomDistance = 0.1f;
            eatTime = 10f;
        }
        animator.SetBool("Walk", false);
    }

    IEnumerator EatAndMove()
    {
      
            isEating = true;
            // Включаем анимацию поедания
            animator.SetBool("Eat", true);
            animator.SetBool("Walk", false);
            yield return new WaitForSeconds(eatTime);
            // Выключаем анимацию поедания
            animator.SetBool("Eat", false);
            animator.SetBool("Walk", true);
            isEating = false;
            speed = Random.Range(1.5f, speed);
            eatTime = Random.Range(1, eatTime);
            randomDistance = Random.Range(0.1f, 3f);
            MoveToNextPoint();
      
    }

    IEnumerator Nest()
    {
        isNesting = true;
        animator.SetBool("Eat", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("TurnHead", true);
        yield return new WaitForSeconds(eatTime);
        isNesting = false;
        Instantiate(eggPrefab, transform.position, Quaternion.identity);
        animator.SetBool("TurnHead", false);
        animator.SetBool("Eat", false);
        animator.SetBool("Walk", true);
        isEating = false;
        speed = Random.Range(1.5f, speed);
        eatTime = Random.Range(1, eatTime);
        randomDistance = Random.Range(0.1f, 3f);
        MoveToNextPoint();

    }
}