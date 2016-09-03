using UnityEngine.VR.WSA.Input;
using UnityEngine;
using Singleton;

//manages all of the gaze/gesture user inputs
public class GazeGestureManager : Singleton<GazeGestureManager> {
    
    //the array containing all active selectmenus
    public GameObject[] selectmenu;
    // the hologram that is currently being gazed at.
    public GameObject FocusedObject;
    // the previous focused object
    private GameObject oldFocusObject = null;
    // is there a drag action in progress
    public bool IsNavigating = false;
    // is the program in start mode (has the room been placed?)
    public bool StartMode = false;
    public Vector3 NavigationPosition { get; private set; }
    
    GestureRecognizer recognizer;

    // Use this for initialization
    void Awake() {
        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        
        recognizer.SetRecognizableGestures(GestureSettings.Tap | 
                                           GestureSettings.NavigationX);
        
        recognizer.TappedEvent += (source, tapCount, ray) => {
            //clear out select menu every time a click happens that didn't spawn it
            selectmenu = GameObject.FindGameObjectsWithTag("selectmenu");
            foreach (GameObject selectbutton in selectmenu) {
                Destroy(selectbutton);
            }
            // tell the selected object it is being clicked on
            if (FocusedObject != null) {
                FocusedObject.SendMessageUpwards("OnSelect");
            }
        };
        
        //start events to manage drag actions
        recognizer.NavigationStartedEvent += NavigationRecognizer_NavigationStartedEvent;
        recognizer.NavigationUpdatedEvent += NavigationRecognizer_NavigationUpdatedEvent;
        recognizer.NavigationCompletedEvent += NavigationRecognizer_NavigationCompletedEvent;
        recognizer.NavigationCanceledEvent += NavigationRecognizer_NavigationCanceledEvent;

        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update() {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject) {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
    
    private void NavigationRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray) {
        IsNavigating = true;
        NavigationPosition = relativePosition;
        FocusedObject.SendMessageUpwards("OnSelect");
    }

    private void NavigationRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray) {
        IsNavigating = true;
        NavigationPosition = relativePosition;
    }

    private void NavigationRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray) {
        if (HandManager.FocusedGameObject == null) {
            IsNavigating = false;
        }
    }

    private void NavigationRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray) {
        if (HandManager.FocusedGameObject == null) {
            IsNavigating = false;
        }
    }
    
    public void ResetGestureRecognizers() {
        recognizer.CancelGestures();
    }
}
