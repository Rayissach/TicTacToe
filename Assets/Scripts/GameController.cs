using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    private string playerSide;
    public GameObject gameOverPanel;
    public Text gameOverText;
    private int moveCount;
    public GameObject restartButton;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;

    private string computerSide;
    public bool playerMove;
    public float delay;
    private int value;

    //Show board in it's initial stage for start
    void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReferenceOnButtons();
        moveCount = 0;
        restartButton.SetActive(false);
        playerMove = true;
    }

    //Computer algorithm to pick random grid space
    void Update()
    {
        if(playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 100)
            {
                value = Random.Range(0,8);
                if (buttonList [value].GetComponentInParent<Button>().interactable == true)
                {
                    buttonList [value].text = GetComputerSide();
                    buttonList [value].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
        }
    }
    

    //Iterates through button list to get button ref from GridSpace.cs
    void SetGameControllerReferenceOnButtons()
    {
        for(int i = 0; i < buttonList.Length; i++ ) 
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    //Player may choose X / O to start
    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if(playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColor(playerX, playerO);
        }
        else
        {
            computerSide = "X";
            SetPlayerColor(playerO, playerX);
        }

        StartGame();
    }

    //Starts game
    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        //Horizontal Rows
        if( buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide ||
            buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide ||
            buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide )
        {
            GameOver(playerSide);
        }

        //Diagonal Rows
        else if( buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide ||
                 buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide )
        {
            GameOver(playerSide);
        }        
      
        //Vertical Rows
        else if( buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide || 
                 buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide ||
                 buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide )
        {
            GameOver(playerSide);
        }

        //Computer side moves
        //Horizontal Rows
        else if( buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide ||
                 buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide ||
                 buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide )
        {
            GameOver(computerSide);
        }

        //Diagonal Rows
        else if( buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide ||
                 buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide )
        {
            GameOver(computerSide);
        }        
      
        //Vertical Rows
        else if( buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide || 
                 buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide ||
                 buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide )
        {
            GameOver(computerSide);
        }

        //If all boxes are selected game over
        else if( moveCount >= 9 )
        {
            GameOver("draw");
        }
        else
        {
        ChangeSides();
        delay = 10;
        }

    }

    //Toggles between active and inactive player colors
    void SetPlayerColor(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    //Alerts users when game is over, either in a win or a draw
    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);

        if(winningPlayer == "draw")
        {
            SetGameOverText("It's a Draw!");
            SetPlayerColorsInactive();
        } 
        else
        {
            SetGameOverText(winningPlayer + " Wins!");
        }

        restartButton.SetActive(true);
    }

    //Changes player side after each turn
    void ChangeSides()
    {
        // when wanting to play with local 2 player
        // playerSide = (playerSide == "X") ? "O" : "X";
        playerMove = (playerMove == true) ? false : true;
        // when wanting to play with local 2 player
        // if(playerSide == "X")
        //change computer sides
        if(playerMove == true)
        {
            SetPlayerColor(playerX, playerO);
        }
        else
        {
           SetPlayerColor(playerO, playerX);
        }
    }

    //GameOver text is shown when game over
    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    //Function to Restart Game after a player wins
    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
        playerMove = true;
        delay = 10;

        for(int i = 0; i < buttonList.Length; i++ ) 
        {
            buttonList[i].text = "";
        }
    }

    //Set interactive game board for game play
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    //Toggle to choose Player
    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    //When not players turn set color to inactive
    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
}
