using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TW.Scripts
{
    public class Game : MonoBehaviour
    {
        private static Game _instance;
        public static Game Instance => _instance;

        public Image fade;
        public Image visualProgress;
        public TextMeshProUGUI scoreProgress;
        public TextMeshProUGUI textMessage;

        public List<string> levels;

        private int _currentLevel;
        private int _currentScore;
        private int _totalToCollect;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            fade.gameObject.SetActive(true);
            fade.DOFade(0, 0);
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _currentLevel = SceneManager.GetActiveScene().buildIndex;
            if (_currentLevel == 0)
            {
                fade.DOFade(1, 0);
                SceneManager.LoadScene(levels[++_currentLevel]);
                ReinitialiseLevel();
                fade.DOFade(0, 1);
            }
            else
            {
                ReinitialiseLevel();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                NextLevel();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                PreviousLevel();
            }
        }

        private void ReinitialiseLevel()
        {
            _currentScore = 0;
            _totalToCollect = GetRemaining();
            visualProgress.fillAmount = 0;
            textMessage.text = "";
            scoreProgress.text = $"{_currentScore} / {_totalToCollect}";
        }

        public int GetRemaining()
        {
            return Item.Count;
        }

        public void RestartLevel()
        {
            var sequence = DOTween.Sequence();

            var fading = fade.DOFade(1, 1);
            sequence.Append(fading);
            fading.onComplete += () =>
            {
                SceneManager.LoadScene(levels[_currentLevel]);
                ReinitialiseLevel();
            };
            sequence.AppendInterval(.5f);
            sequence.Append(fade.DOFade(0, .1f));
        }

        public bool HasNextLevel()
        {
            return _currentLevel < levels.Count - 1;
        }

        public void NextLevel()
        {
            if (!HasNextLevel()) return;

            Item.ResetCount();
            var sequence = DOTween.Sequence();
            sequence.Append(fade.DOFade(1f, .5f));
            sequence.AppendCallback(() => SceneManager.LoadScene(levels[++_currentLevel]));
            sequence.AppendInterval(.5f);
            sequence.AppendCallback(ReinitialiseLevel);
            sequence.AppendInterval(.5f);
            sequence.Append(fade.DOFade(0, 1f));
        }

        public void PreviousLevel()
        {
            if (_currentLevel <= 1) return;

            Item.ResetCount();
            var sequence = DOTween.Sequence();
            sequence.Append(fade.DOFade(1f, .5f));
            sequence.AppendCallback(() => SceneManager.LoadScene(levels[--_currentLevel]));
            sequence.AppendInterval(.5f);
            sequence.AppendCallback(ReinitialiseLevel);
            sequence.AppendInterval(.5f);
            sequence.Append(fade.DOFade(0, 1f));
        }

        public void Score()
        {
            _currentScore++;
            visualProgress.fillAmount = _currentScore / (float) _totalToCollect;
            scoreProgress.text = $"{_currentScore} / {_totalToCollect}";

            if (_currentScore == _totalToCollect)
            {
                QueNextLevel("Well Done! You restored some control.");
            }
        }

        private void QueNextLevel(string message = "Loading next level")
        {
            var sequence = DOTween.Sequence();
            sequence.Append(fade.DOFade(.5f, .5f));
            sequence.AppendCallback(() => { textMessage.text = message; });
            sequence.AppendInterval(5);
            sequence.AppendCallback(NextLevel);
        }
    }
}