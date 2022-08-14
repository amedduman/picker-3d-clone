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
        int _collectedCollectable;
        bool _hasCalledForLevelFailCheck;
        // Start is called before the first frame update
        void Start()
        {
            _collectedCollectableCountText.text = "0";
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out PickerEntity picker))
            {
                picker.Stop();
                picker.PushCollectables();
            }

            if(other.TryGetComponent(out CollectableEntity collectable))
            {
                if(collectable.HasCounted) return;

                if(!_hasCalledForLevelFailCheck)
                {
                    _hasCalledForLevelFailCheck = true;
                    DOVirtual.DelayedCall(GameValues.WaitForLevelEndResult, ()=> LevelFailCheck());
                }

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

        void UpdateCollectedCountText()
        {

        }
    }

}