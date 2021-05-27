using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObject
{

    public float speed;

    float positionx;

    bool isEnd = false;

    bool isMove;

    Vector3 platform;

    Vector3 spawnerCenterPosition;

    [SerializeField]
    ParticleSystem particle;

    IEnumerator StartDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isMove = true;
    }

    public void OnSpawn()
    {
        GameLogic.Instance.saw = this;

        GameLogic.Instance.PlatformCount++;
        GameLogic.Instance.SetDifficulty();

        speed = GameLogic.Instance.SawMaxSpeed;

        positionx = transform.position.x;

        isMove = false;

        StartCoroutine(StartDelay(Random.Range(GameLogic.Instance.MinDelayTime, GameLogic.Instance.MaxDelayTime)));

        particle.Play();

        //spawnerCenterPosition = Spawner.Instance.ObstacleGoal.transform.position;
        spawnerCenterPosition = Spawner.Instance.ObsGoal;
    }

    private void Update()
    {
        if(isMove)
            transform.position = Vector3.MoveTowards(transform.position, spawnerCenterPosition, Time.deltaTime * speed);
        platform = transform.position;

    }

    private void OnBecameInvisible()
    {
        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            gameObject.SetActive(false);
    }
}
