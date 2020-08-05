using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuClickSystem : MonoBehaviour
{
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private GameObject _settingCanvas;
    [SerializeField] private GameObject _imageSelectorCanvas;
    [SerializeField] private GameObject _levelSelectorCanvas;

    public static void OnPlanetClick(Sprite sprite)
    {
        var gameLoader = Game.Get<GameLoader>();
        DontDestroyOnLoad(gameLoader.gameObject);
        gameLoader.Init(new GameLoaderData(){SelectedSprite = sprite});
        SceneManager.LoadSceneAsync("game");

    }


    //TODO: edit it later
    public void GoToImageSelector()
    {
        _menuCanvas.SetActive(false);
        _settingCanvas.SetActive(false);
        _levelSelectorCanvas.SetActive(false);
        _imageSelectorCanvas.SetActive(true);
    }
    
    public void GoToMainMenu()
    {
        _menuCanvas.SetActive(true);
        _settingCanvas.SetActive(false);
        _levelSelectorCanvas.SetActive(false);
        _imageSelectorCanvas.SetActive(false);
    }
    
    public void GoToSettingMenu()
    {
        _menuCanvas.SetActive(false);
        _settingCanvas.SetActive(true);
        _levelSelectorCanvas.SetActive(false);
        _imageSelectorCanvas.SetActive(false);
    }
    public void GoToLevelSelectorMenu()
    {
        _menuCanvas.SetActive(false);
        _settingCanvas.SetActive(false);
        _levelSelectorCanvas.SetActive(true);
        _imageSelectorCanvas.SetActive(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
