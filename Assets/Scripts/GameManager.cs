using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public Car currentCar;

    [SerializeField] private Car[] _carsInScene;
    [SerializeField] private Level[] _levels;
    [SerializeField] private Sprite[] _carSprites;
    private Level _currentLevel;
    private int indexLevel = -1;

    [Header("UI References")]
    [SerializeField] private Text _roundText;
    [SerializeField] private Text _infoText;
    [SerializeField] private GameObject _timerObject;
    [SerializeField] private Text _timerText;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private GameObject _homeButton;

    private float _timer = 10.0f;
    private bool _isRemember = false;
    public bool isRemember => _isRemember;

    private bool _isPlace = false;
    private bool _isOver = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        InitializeLevel();
    }

    public void InitializeLevel()
    {
        indexLevel += 1;
        _nextButton.SetActive(false);
        _isRemember = false;
        _isOver = false;
        _isPlace = false;
        _timer = 10.0f;

        foreach (var car in _carsInScene)
        {
            car.ResetCar();
            car.gameObject.SetActive(false);
        }

        _currentLevel = _levels[indexLevel];
        foreach (var car in _currentLevel.cars)
        {
            car.gameObject.SetActive(true);
            car.InitializeCar(_carSprites[Random.Range(0, _carSprites.Length)]);
        }

        _roundText.text = $"Round {indexLevel + 1}";
        _timerObject.SetActive(true);
        _isRemember = true;
    }

    private void Update()
    {
        if (_isRemember)
        {
            _infoText.text = "You have 10 seconds to remember cars";
            _timer -= Time.deltaTime;
            _timerText.text = Mathf.CeilToInt(_timer).ToString();
            if(_timer <= 0)
            {
                _isPlace = true;
                _isRemember = false;
                _timer = 10.0f;
            }
        }

        if(_isPlace && !_isOver)
        {
            _infoText.text = "Now place the cars in right order";
            _timer -= Time.deltaTime;
            _timerText.text = Mathf.CeilToInt(_timer).ToString();
            if (_timer <= 0)
                GameOver();
        }

        if (!_isRemember && _isPlace)
            _isOver = CheckIsOver();

        if(_isOver)
        {
            _infoText.text = $"Done!";
            _nextButton.SetActive(true);
            _timerObject.SetActive(false);
        }

    }

    public bool CheckIsOver()
    {
        foreach(Car car in _currentLevel.cars)
        {
            if (!car.isOver)
                return false;
        }

        return true;
    }

    private void GameOver()
    {
        _infoText.text = "Time is up! You lost.";
        _isPlace = false;
        _isRemember = false;
        _timerObject.SetActive(false);
        _homeButton.SetActive(true);
        Database.instance.AddNewScoreDatabase(indexLevel + 1);
        if (AudioManager.instance.isVibration)
            Vibration.Vibrate();
    }

    public void Home() => UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
}

[System.Serializable]
public struct Level
{
    public Car[] cars;
}
