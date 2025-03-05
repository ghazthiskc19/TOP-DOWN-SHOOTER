using System;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer {get; private set;}
    public Vector2 DirectionToPlayer {get; private set;}
    [SerializeField]
    private float _playerAwarenessDistance;
    private Transform _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyPosition = transform.position;
        Vector2 playerPosition = _player.position;

        Vector2 distanceToPlayer = playerPosition - enemyPosition;
        DirectionToPlayer = distanceToPlayer.normalized;

        float actualDistance = distanceToPlayer.magnitude;
        AwareOfPlayer = actualDistance <= _playerAwarenessDistance;
    }
}
