using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    protected AppController appController;
    [SerializeField]
    protected EnemyObject foodPrefab;
    [SerializeField]
    protected int maxFoodCount = 5;

    private Vector2 _prefabSize;
    private int _foodCount = 0;
    private float _spawnTime = 2f;
    private float _spawnDelay = 3f;

    private bool IsSpawning => _foodCount < maxFoodCount;

    private void Start()
    {
        _prefabSize = foodPrefab.gameObject.GetComponent<SpriteRenderer>().size;
        _foodCount = 0;

        GenerateObj();
        InvokeRepeating(nameof(Generator), _spawnTime, _spawnDelay);
    }

    private void Generator()
    {
        if (IsSpawning)
            GenerateObj();
    }

    private void GenerateObj()
    {
        float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y + _prefabSize.y / 2, 
                                    Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y - _prefabSize.y / 2);
        float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x + _prefabSize.x / 2, 
                                    Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - _prefabSize.x / 2);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        EnemyObject go = Instantiate(foodPrefab, this.transform);
        go.transform.position = spawnPosition;
        go.OnDestroying += DestroyCurEnemy;
        _foodCount++;
    }

    private void DestroyCurEnemy(EnemyObject obj)
    {
        _foodCount--;
        appController.UpdateScore();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        _foodCount = 0;
    }
}
