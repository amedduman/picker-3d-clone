namespace Picker3d
{
    using UnityEngine;
    using TMPro;
    using DG.Tweening;

    public class LevelEndPool : MonoBehaviour
    {
        [SerializeField] int _necessaryCollectableCount = 10;
        [SerializeField] TextMeshPro _collectedCollectableCountText;
        [SerializeField] TextMeshPro _targetCollectableCountText;
        [SerializeField] GameObject _ground;
        [SerializeField] float _groundMoveYAmount = 1;
        [SerializeField] Collider _pickerTrigger;
        [SerializeField] Collider[] _colliders;
        int _collectedCollectable;
        bool _hasCalledForLevelFailCheck;
        bool _isActive;

        void OnEnable()
        {
            GameManager.Instance.OnNextLevelButtonPressed += HandleNextLevelButtonPressed;
        }

        void OnDisable()
        {
            GameManager.Instance.OnNextLevelButtonPressed -= HandleNextLevelButtonPressed;
        }

        void Start()
        {
            _targetCollectableCountText.text = _necessaryCollectableCount.ToString();
            _collectedCollectableCountText.text = "0";
        }

        void OnDestroy()
        {
            DOTween.Kill(_ground.transform); 
        }

        // BUG:  sifir collectable ile gelirse again butonu cikmiyor
        void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out PickerEntity picker))
            {
                _isActive = true;
                _pickerTrigger.enabled = false;
                picker.Stop();
                picker.PushCollectables();

                if(!_hasCalledForLevelFailCheck) 
                {
                    _hasCalledForLevelFailCheck = true;
                    DOVirtual.DelayedCall(GameValues.WaitForLevelEndResult, ()=> LevelFailCheck());
                }
            }

            if(other.TryGetComponent(out CollectableEntity collectable))
            {
                if(collectable.HasCounted) return;

                // if(!_hasCalledForLevelFailCheck)
                // {
                //     _hasCalledForLevelFailCheck = true;
                //     DOVirtual.DelayedCall(GameValues.WaitForLevelEndResult, ()=> LevelFailCheck());
                // }

                collectable.HasCounted = true;

                _collectedCollectable += 1;
                if(HasWon())
                {
                    GameManager.Instance.LevelPassed();
                }

                _collectedCollectableCountText.text = _collectedCollectable.ToString();
            }
        }

        void LevelFailCheck()
        {
            if(!HasWon())
            {
                GameManager.Instance.LevelFailed();
            }
        }

        bool HasWon()
        {
           return _collectedCollectable >= _necessaryCollectableCount;
        }

        void HandleNextLevelButtonPressed()
        {
            if(!_isActive) return;
            foreach (var coll in _colliders)
            {
                coll.enabled = false;
            }

            _ground.transform.DOLocalMoveY(_groundMoveYAmount, .7f).OnComplete(()=> GameManager.Instance.NextLevelReady());
        }
    }

}