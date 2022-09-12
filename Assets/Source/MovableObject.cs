using System;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Action<float> DistanceUpdate;

    private List<Vector3> _moves;
    private float _lerpTime = 1.5f;
    private float _lerpTimeDelta = 4f;
    private float _time = 0;
    private Vector3 _prevPosition;
    private bool _isSmooth = false;

    void Start()
    {
        _moves = new List<Vector3>();
        transform.position = Vector3.zero;
        _prevPosition = transform.position;
    }

    void Update()
    {
        if (_moves.Count > 0)
        {
            if (_isSmooth)
            {
                _lerpTime = _lerpTimeDelta / Vector3.Distance(_prevPosition, _moves[0]);
                transform.position = Vector3.Lerp(transform.position, _moves[0], _lerpTime * Time.deltaTime); 

                _time = Mathf.Lerp(_time, 1f, _lerpTime * Time.deltaTime);
                if (_time > 0.9f)
                    EndMove();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _moves[0], 4 * Time.deltaTime);
                if (Vector3.Distance(transform.position, _moves[0]) <= 0.0001f)
                    EndMove();
            }
        }
    }

    private void EndMove()
    {
        _time = 0;
        _moves.RemoveAt(0);
        float distance = Vector3.Distance(_prevPosition, transform.position);
        DistanceUpdate.Invoke(distance);
        _prevPosition = transform.position;
    }

    public void AddMove(Vector3 pos)
    {
        _isSmooth = false;
        _moves.Add(new Vector3(pos.x, pos.y, 0));
    }

    public void Move(Vector3 pos)
    {
        _isSmooth = true;
        Stop();
        _moves.Add(new Vector3(pos.x, pos.y, 0));
    }

    public void Stop()
    {
        _time = 0;
        _moves = new List<Vector3>();
        float distance = Vector3.Distance(_prevPosition, transform.position);
        _prevPosition = transform.position;
        DistanceUpdate.Invoke(distance);
    }
}
