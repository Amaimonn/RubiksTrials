using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets._Game.Scripts.MVVM
{
    public abstract class DecisionPopUp : MonoBehaviour, IDisposable
    {
        [SerializeField] protected TMP_Text _questionText;
        [SerializeField] protected Button _submitButton;
        [SerializeField] protected Button _cancelButton;
        protected bool _isInputEnabled = false;

        public virtual void Bind(Action submitAction, Action cancelAction = null, string text = null)
        {
            _submitButton.onClick.AddListener(() => {
                if (_isInputEnabled)
                    submitAction?.Invoke();
            });

            _cancelButton.onClick.AddListener(() => {
                if (_isInputEnabled)
                    cancelAction?.Invoke();
            });
            
            if (text != null)
                _questionText.text = text;
        }

        public abstract void Show();
        public abstract void Close();

        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            _submitButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        }
    }
}