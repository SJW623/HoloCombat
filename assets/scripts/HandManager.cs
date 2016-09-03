using UnityEngine.VR.WSA.Input;
using UnityEngine;
using Singleton;

//keeps track of the users hand movements
public class HandManager : Singleton<HandManager> {
    //is there a hand that is in field of view
    public bool HandDetected { get; private set; }
    //the object that is currently selected
    public static GameObject FocusedGameObject { get; private set; }

    void Awake() {
        //start tracking for hand movements
        InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        InteractionManager.SourceLost += InteractionManager_SourceLost;
        InteractionManager.SourcePressed += InteractionManager_SourcePressed;
        InteractionManager.SourceReleased += InteractionManager_SourceReleased;

        FocusedGameObject = null;
    }

    private void InteractionManager_SourceDetected(InteractionSourceState hand) {
        HandDetected = true;
    }

    private void InteractionManager_SourceLost(InteractionSourceState hand) {
        HandDetected = false;
        FocusedGameObject = null;

        GazeGestureManager.Instance.ResetGestureRecognizers();
    }

    private void InteractionManager_SourcePressed(InteractionSourceState hand) {
        if (GazeGestureManager.Instance.FocusedObject != null) {
            //Cache GazeGestureManager's FocusedObject in FocusedGameObject.
            FocusedGameObject = GazeGestureManager.Instance.FocusedObject;
        }
    }

    private void InteractionManager_SourceReleased(InteractionSourceState hand) {
        FocusedGameObject = null;

        GazeGestureManager.Instance.ResetGestureRecognizers();
    }
}