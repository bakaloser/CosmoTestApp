using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    [SerializeField]
    protected UIController uIController;
    [SerializeField]
    protected MovableObject movableObject;

    private int _score = 0;
    private float _distance = 0;
    private List<Vector3> _path;

    private void Start()
    {
        //load
        _score = PlayerPrefs.GetInt("score", 0);
        _distance = PlayerPrefs.GetFloat("distance", 0);
        uIController.UpdateUI(_score, _distance);

        movableObject.DistanceUpdate += UpdateDistance;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == movableObject.gameObject)
            {
                _path = new List<Vector3>();
                movableObject.Stop();
            }
            else
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _path = new List<Vector3>();
                        touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        _path.Add(touchPos);
                        break;
                    case TouchPhase.Moved:
                        touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                        _path.Add(touchPos);
                        break;
                    case TouchPhase.Ended:
                        _path.ForEach(x => movableObject.AddMove(x));
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == movableObject.gameObject)
                movableObject.Stop();
            else
                movableObject.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void UpdateScore(int points = 1)
    {
        _score += points;
        uIController.UpdateUI(_score, _distance);
    }

    public void UpdateDistance(float points)
    {
        _distance += points;
        uIController.UpdateUI(_score, _distance);
    }

    private void OnDestroy()
    {
        //save
        PlayerPrefs.SetInt("score", _score);
        PlayerPrefs.SetFloat("distance", _distance); 
        movableObject.DistanceUpdate -= UpdateDistance;
    }
}
