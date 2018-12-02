using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance { get; private set; }
    private const string BIRD_IDENTIFIER = "Birds";

    [SerializeField] private GameObject m_BirdPrefab;
    private Pool m_BirdPool;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        m_BirdPool = ObjectPool.Instance.CreatePool(m_BirdPrefab, 2, BIRD_IDENTIFIER);
    }

    public void SpawnBird(float posZ)
    {
        bool startFromLeft = Random.value > 0.5f;
        float randomPosX = startFromLeft ? -50 : 50;
        float rotation = startFromLeft ? 0 : 180;
        float randomHeight = Random.Range(10f, 20f);
        GameObject bird = m_BirdPool.Get();
        bird.transform.SetParent(transform);
        bird.transform.localPosition = new Vector3(randomPosX, randomHeight, Random.Range(posZ, posZ + 30));
        bird.transform.localRotation = Quaternion.Euler(0, rotation, 0);

        float randomScale = Random.Range(0.8f, 1.2f);
        bird.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        bird.transform.DOLocalMoveX(-randomPosX, Random.Range(3f, 5f)).SetEase(Ease.Linear).OnComplete(() =>
        {
            m_BirdPool.Despawn(bird);
        });
    }
}
