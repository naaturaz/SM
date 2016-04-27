using UnityEngine;
using UnityEngine.UI;

/*
 * The script attached to the MainMenuWindow
 */
public class MainMenuWindow : GUIElement
{
    private UnityEngine.UI.Button _continueBtn;
    private UnityEngine.UI.Button _resumeBtn;
    private UnityEngine.UI.Button _loadBtn;
    private UnityEngine.UI.Button _saveBtn; 

    // Use this for initialization
    void Start()
    {
        _continueBtn = GetChildCalled("Continue_Btn").GetComponent<UnityEngine.UI.Button>();
        _resumeBtn = GetChildCalled("Resume_Btn").GetComponent<UnityEngine.UI.Button>();
        _loadBtn = GetChildCalled("Load_Game_Btn").GetComponent<UnityEngine.UI.Button>();
        _saveBtn = GetChildCalled("Save_Game_Btn").GetComponent<UnityEngine.UI.Button>();

        //inipos is used for Hide and show 
        iniPos = transform.position;

        MakeButtonsInactiveIfNeeded();
    }

    void MakeButtonsInactiveIfNeeded()
    {
        if (!Program.gameScene.GameFullyLoaded() || Program.gameScene.GameController1.IsGameOver)
        {
            _resumeBtn.interactable = false;
            _saveBtn.interactable = false;
        }
        if (!DataController.ThereIsAtLeastAGameToLoad())
        {
            _loadBtn.interactable = false;
        }
        if (!DataController.ThereIsALastSavedFile() || Program.gameScene.GameFullyLoaded())
        {
            //if there is not lastSaved File or game is fully loaded. then not posible to conitnue a game 
            _continueBtn.interactable = false;
        }
    }

	// Update is called once per frame
	void Update () 
    {

	}

    
}
