using System.Collections.Generic;
using PlayerSpace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagementSpace
{
    public class HeartsManagement : MonoBehaviour
    {
        [SerializeField] private GameObject _heartsContainer;
        [SerializeField] private GameObject _heartPrefab;
        [SerializeField] private float _heartsGap = 20f;

        private List<GameObject> _hearts = new List<GameObject>();

        private void OnEnable()
        {
            Player.OnPlayerInitLivesEvent += OnPlayerInitLives;
            Player.OnPlayerLostLifeEvent += OnPlayerLostLife;
        }

        private void OnPlayerInitLives()
        {
            foreach (var heart in _hearts)
            {
                Destroy(heart);
            }
            
            _hearts = new List<GameObject>();
            Debug.Log(Player.Instance.CountLives);
            
            for (int i = 0; i < Player.Instance.CountLives; i++)
            {
                GameObject heart = Instantiate(_heartPrefab, _heartsContainer.transform);
                RectTransform rectTransform = heart.GetComponent<RectTransform>();
                float heartWidth = rectTransform.sizeDelta.x * rectTransform.localScale.x;

                float positionX = (2*i + 1) * (heartWidth / 2) + _heartsGap;
                
                rectTransform.anchoredPosition = new Vector2(positionX, 0);
                
                _hearts.Add(heart);
            }
        }

        private void OnPlayerLostLife()
        {
            if (_hearts.Count > 0)
            {
                GameObject heart = _hearts[^1];
                
                _hearts.RemoveAt(_hearts.Count - 1);

                var heartImage = heart.GetComponent<Image>();

                DOTween.Sequence()
                    .Append(heartImage.DOFade(0.0f, 0.1f))
                    .Append(heartImage.DOFade(0.0f, 0.1f))
                    .Append(heartImage.DOFade(1f, 0.1f))
                    .Append(heartImage.DOFade(1f, 0.1f))
                    .Append(heartImage.DOFade(0.0f, 0.1f))
                    .OnComplete(() => Destroy(heart));
            }
        }
        
        private void OnDisable()
        {
            Player.OnPlayerInitLivesEvent -= OnPlayerInitLives;
            Player.OnPlayerLostLifeEvent -= OnPlayerLostLife;
        }
    }
}

