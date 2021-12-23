using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class TankPlayer : MonoBehaviourPunCallbacks
    {
        // Reference to tank's movement script, used to disable and enable control.
        private Complete.TankMovement m_Movement;
        // Reference to tank's shooting script, used to disable and enable control.
        private Complete.TankShooting m_Shooting;

       // private Complete.TankHealth m_TankHealth;
        private void Awake()
        {
            m_Movement = GetComponent<Complete.TankMovement>();
            m_Shooting = GetComponent<Complete.TankShooting>();
            //m_TankHealth = GetComponent<Complete.TankHealth>();

            if (!photonView.IsMine)
            {
                m_Movement.enabled = false;
                m_Shooting.enabled = false;
                //m_TankHealth.enabled = false;
                enabled = false;
            }
        }
    }
}

