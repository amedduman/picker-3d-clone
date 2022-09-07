namespace Picker3d
{
    using UnityEngine;
    using TMPro;
    using DG.Tweening;
    using UnityEngine.Serialization;

    public class LevelEndPool : MonoBehaviour
    {
        [FormerlySerializedAs("_necessaryCollectableCount")] public int NecessaryCollectableCount = 10;
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
            _targetCollectableCountText.text = NecessaryCollectableCount.ToString();
            _collectedCollectableCountText.text = "0";
        }

        void OnDestroy()
        {
            DOTween.Kill(_ground.transform); 
        }

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
           return _collectedCollectable >= NecessaryCollectableCount;
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