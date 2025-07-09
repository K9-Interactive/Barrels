using UnityEngine;

namespace ITHappy {
    public class AnimationPipeline : MonoBehaviour {
        [SerializeField]
        private AnimationState[] m_States = new AnimationState[0];

        private int m_Iterator;
        private bool m_IsLocked;

        private void Awake() {
            m_Iterator = 0;
            for(int i = 1; i < m_States.Length; i++) {
                m_States[i].Disable();   
            }

            ActivateState();
        }

        public void SwitchNext() {
            if (m_IsLocked) {
                return;
            }

            if (m_States.Length == 0) {
                Debug.LogWarning("states array of 0 length");
                return;
            }
            m_IsLocked = true;
            m_States[m_Iterator].Disable(ActivateState);
            m_States[m_Iterator].DisableButton();

            m_Iterator++;
            if (m_Iterator >= m_States.Length) {
                m_Iterator = 0;
            }

            m_States[m_Iterator].EnableButton();
        }

        public void SwitchPrevious() {
            if(m_IsLocked) {
                return;
            }

            if (m_States.Length == 0) {
                Debug.LogWarning("states array of 0 length");
                return;
            }

            m_IsLocked = true;
            m_States[m_Iterator].Disable(ActivateState);
            m_States[m_Iterator].DisableButton();
            m_Iterator--;
            if (m_Iterator < 0) {
                m_Iterator = m_States.Length - 1;
            }

           m_States[m_Iterator].EnableButton();
        }

        private void ActivateState() {
            m_States[m_Iterator].Enable();
            m_IsLocked = false;
        }
    }
}