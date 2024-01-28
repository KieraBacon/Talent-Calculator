using UnityEditor;
using UnityEngine;
using Utilities;

namespace SkillSystem.UI
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private ScriptableObjects.CharacterClassObject _serializedClass;
        
        [SerializeField] private Transform _layoutRoot;
        private CharacterInstance _characterInstance;
        public CharacterInstance CharacterInstance =>
            _characterInstance;
        
#if UNITY_EDITOR
        void OnValidate()
        {
            EditorApplication.delayCall += _OnValidate;
        }

        void _OnValidate()
        {
            if (this == null || !Application.isPlaying) return;
            UpdateInformation();
        }
#endif

        private void Start()
        {
            UpdateInformation();
        }

        private void UpdateInformation()
        {
            if (CharacterInstance != null && CharacterInstance.CharacterClass != null)
            {
                if (_serializedClass != null && CharacterInstance.CharacterClass.Equals(_serializedClass.CharacterClass)) return;
            }
            
            CharacterClass characterClass = null;
            if (_serializedClass != null)
            {
                characterClass = _serializedClass.CharacterClass;
            }
            
            if (characterClass == null) return;

            Pool<TalentTreeView> pool = PoolManager.Instance.Get<TalentTreeView>("Talent Tree View");
            pool.ReleaseAll();

            _characterInstance = new CharacterInstance(characterClass);
            
            foreach (TalentTreeInstance talentTreeInstance in CharacterInstance.TalentTreeInstances)
            {
                TalentTreeView talentTreeView = pool.Get(_layoutRoot);
                talentTreeView.TalentTreeInstance = talentTreeInstance;
            }
        }
    }
}