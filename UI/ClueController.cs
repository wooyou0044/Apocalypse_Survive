using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueController : MonoBehaviour
{
    [SerializeField] GameObject _nextButton;
    [SerializeField] GameObject _prevButton;

    int _clueNum = 0;
    int _num;

    int _nextNum = 0;
    int _prevNum = 0;
    InventoryManager _inventManage;

    void Awake()
    {
        _inventManage = GameObject.Find("InventoryBack").GetComponent<InventoryManager>();
        _num = 1;
        OpenClue(1);
    }

    void Update()
    {
        _clueNum = _inventManage._acquireClueNum;

        if (_clueNum ==1)
        {
            _prevButton.SetActive(false);
            _nextButton.SetActive(false);
        }

        if (_num == 1)
        {
            _prevButton.SetActive(false);
            if(_num != _clueNum)
                _nextButton.SetActive(true);
        }
        else if (_num == _clueNum)
        {
            _prevButton.SetActive(true);
            _nextButton.SetActive(false);
        }
        else
        {
            _prevButton.SetActive(true);
            _nextButton.SetActive(true);
        }
    }

    public void NextButton()
    {
        _num = _num + 1;
        OpenClue(_num);
    }

    public void PrevButton()
    {
        _num = _num - 1;
        OpenClue(_num);
    }

    void OpenClue(int clueNum)
    {
        Text clueTitle = transform.GetChild(0).GetComponentInChildren<Text>();
        Text clue = transform.GetChild(1).GetComponentInChildren<Text>();
        clueTitle.text = "Hint";
        if (clueNum == 1)
        {
            clueTitle.font = Resources.Load("Fonts/UhBee matsuko Bold") as Font;
            clue.font = Resources.Load("Fonts/UhBee matsuko") as Font;
            clue.text = "첫 번째 수와 두번 째 수를 더하면 11";
        }

        if (clueNum == 2)
        {
            clueTitle.font = Resources.Load("Fonts/UhBee UZ Bold") as Font;
            clue.font = Resources.Load("Fonts/UhBee UZ") as Font;
            clue.text = "두번째 수와 세번째 수를 더하면 17";
        }

        if (clueNum == 3)
        {
            clueTitle.font = Resources.Load("Fonts/UhBee yehee Bold") as Font;
            clue.font = Resources.Load("Fonts/UhBee yehee") as Font;
            clue.text = "세번째 수와 네번째 수를 곱하면 54";
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
        _inventManage.CloseInventorySlot();
    }
}
