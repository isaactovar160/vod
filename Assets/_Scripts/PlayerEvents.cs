using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    #region Events
        public static UnityAction OnTouchpadUp = null;
        public static UnityAction OnTouchpadDown = null;
        public static UnityAction <OVRInput.Controller, GameObject> OnControllerSource = null;
    #endregion

    #region Anchors
        public GameObject m_LeftAnchor;
        public GameObject m_RightAnchor;
        public GameObject m_HeadAnchor;
    #endregion

    #region Input
        private Dictionary<OVRInput.Controller, GameObject> m_ControllerSets = null;
        private OVRInput.Controller m_Inputsource = OVRInput.Controller.None;
        private OVRInput.Controller m_Controller = OVRInput.Controller.None;
        private bool m_InputActive = true;
    #endregion  

    private void Awake()
    {
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;

        m_ControllerSets = CreateControllerSets();
    }

    private void OnDestroy()
    {
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    // Update is called once per frame
    void Update()
    {
        //check For Active Input
        if (!m_InputActive)
        {
            return;
        }
        //check if controller exist
        CheckForController();
        //check For Input Source
        CheckInputsource();

        //check for actual input
        Input();
    }

    private void CheckForController(){
        
        OVRInput.Controller controllerCheck = m_Controller;

        //Right Remote
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
        {
            controllerCheck = OVRInput.Controller.RTrackedRemote;
        }
        //Left Remote
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            controllerCheck = OVRInput.Controller.LTrackedRemote;
        }
        //Headset
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote) && !OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            controllerCheck = OVRInput.Controller.Touchpad;
        }
        //Update
        m_Controller = UpdateSource(controllerCheck, m_Controller);
    }

    private void CheckInputsource(){
        //Update
        m_Inputsource = UpdateSource(OVRInput.GetActiveController(), m_Inputsource);
    }

    private void Input(){
        //TouchPad down
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            if (OnTouchpadDown != null)
            {
                OnTouchpadDown();
            }
        }
        //TouchPad up
        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            if (OnTouchpadUp != null)
            {
                OnTouchpadUp();
            }
        }
    }

    private OVRInput.Controller UpdateSource(OVRInput.Controller check, OVRInput.Controller previous){
        //Values are the same, return
        if (check == previous)
        {
            return previous;
        }
        //Get controller Object
        GameObject controllerObject = null;
        m_ControllerSets.TryGetValue(check, out controllerObject);
        //If no controller, set to headset
        if (controllerObject == null)
        {
            controllerObject = m_HeadAnchor;
        }
        //Send out event
        if (OnControllerSource != null)
        {
            OnControllerSource(check, controllerObject);
        }

        //Return new value
        return check;
    }

    private void PlayerFound(){

    }
    
    private void PlayerLost(){

    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets(){
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>(){
            {OVRInput.Controller.LTrackedRemote, m_LeftAnchor},
            {OVRInput.Controller.RTrackedRemote, m_RightAnchor},
            {OVRInput.Controller.Touchpad, m_HeadAnchor},
        };
        return newSets;
    }
}
