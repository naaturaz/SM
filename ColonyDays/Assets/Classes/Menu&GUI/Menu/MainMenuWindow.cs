using UnityEngine;

/*
 * The script attached to the MainMenuWindow
 */
public class MainMenuWindow : GUIElement
{
    private GameObject _continueBtn;
    private GameObject _resumeBtn; 

    // Use this for initialization
    void Start()
    {
        _continueBtn = GetChildCalled("Continue_Btn");
        _resumeBtn = GetChildCalled("Resume_Btn");

        //inipos is used for Hide and show 
        iniPos = transform.position;
    }

	// Update is called once per frame
	void Update () 
    {

	}

    /// <summary>
    /// 
    /// </summary>
    public void HideContinueAndResume()
    {
        _continueBtn.gameObject.SetActive(false);
        _resumeBtn.gameObject.SetActive(false);
    }

    public void MakeResumeActive()
    {
        HideContinueAndResume();
        _resumeBtn.gameObject.SetActive(true);
    }

    public void MakeContinueActive()
    {
        HideContinueAndResume();
        _continueBtn.gameObject.SetActive(true);
    }
}
