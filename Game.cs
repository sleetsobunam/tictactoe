using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Game : MonoBehaviour
{
    public int[] gamePuzzle = new int[9];

    public Image[] puzzleImages =  new Image[9];
    public Button[] puzzleButtons = new Button[9];
    public Sprite OImage;
    public Sprite XImage;
    public Sprite empty;

    public Sprite draw, xWins, oWins;
    public Image message;
    private int player = -1;
    private bool playerPlayed = false;
    private int turnCount = 0;
   

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            gamePuzzle[i] = 0;
            puzzleImages[i].sprite = empty;
            puzzleButtons[i].enabled = true;
        }
    }
    void Update()
    {
        if (moves(gamePuzzle).Count == 0 && eval(gamePuzzle) == 0)
            message.sprite = draw;
        else if (player == 1 && eval(gamePuzzle) == 0)
        {
            int move = getBestMove(player);
            gamePuzzle[move] = player;
            Debug.Log("Computer played");
            UpdateImages();
            player = -player;
            turnCount++;
        }
        else if (player == -1 && eval(gamePuzzle) == 0)
        {
            Debug.Log("Player?");
            if (playerPlayed)
            {
                player = -player;
                playerPlayed = false;
                turnCount++;
            }
            UpdateArray(); 
        }
        else if (eval(gamePuzzle) == 1)
        {
            message.sprite = xWins;
        }
        else if (eval(gamePuzzle) == -1)
        {
            message.sprite = oWins;
        }
    }
    

    public void OnClick()
    {
        playerPlayed = true;
    }

    public void restart()
    {
        gamePuzzle = new int[9];
        UpdateImages();
        message.sprite = empty;
        player = UnityEngine.Random.Range(0, 2) * 2 - 1;
    }
    private void UpdateArray()
    {
        for (int i = 0; i < 9; i++)
        {
            if (puzzleImages[i].sprite.Equals(XImage))
            {
                gamePuzzle[i] = 1;
            }
            if (puzzleImages[i].sprite.Equals(OImage))
            {
                gamePuzzle[i] = -1;
            }

            if (puzzleImages[i].sprite.Equals(empty))
            {
                gamePuzzle[i] = 0;
            }
        }
    }
    private void UpdateImages()
    {
        
        for(int i = 0; i < 9; i++)
        {
            if (gamePuzzle[i] == 1)
            {
                puzzleImages[i].sprite = XImage;
                puzzleButtons[i].enabled = false;
            }

            if (gamePuzzle[i] == -1)
            {
                puzzleImages[i].sprite = OImage;
                puzzleButtons[i].enabled = false;
            }

            if (gamePuzzle[i] == 0)
            {
                puzzleImages[i].sprite = empty;
                puzzleButtons[i].enabled = true;
            }
        }
    }
    private int eval(int[] puzzle)
    {
        for (int i = 0; i < 3; i++)
        {
            if (puzzle[i * 3] == puzzle[i*3+1] && puzzle[i*3] == puzzle[i*3+2])
            {
                if (puzzle[i * 3] == -1)
                    return -1;
                else if (puzzle[i * 3] == 1)
                    return 1;
            }
            if (puzzle[i] == puzzle[i+3] && puzzle[i] == puzzle[i+6])
            {
                if (puzzle[i] == -1)
                    return -1;
                else if (puzzle[i] == 1)
                    return 1;
            }
        }
        if (puzzle[0] == puzzle[4] && puzzle[0] == puzzle[8])
        {
            if (puzzle[0] == -1)
                return -1;
            else if (puzzle[0] == 1)
                return 1;
        }
        if (puzzle[2] == puzzle[4] && puzzle[2] == puzzle[6])
        {
            if (puzzle[2] == -1)
                return -1;
            else if (puzzle[2] == 1)
                return 1;
        }
        return 0;
    }
    
    private List<int> moves(int[] puzzle)
    {
        List<int> mv = new List<int>(9);
        for (int i = 0; i < 9; i++)
        {
            if (puzzle[i] == 0)
            {
                mv.Add(i);
            }
    
        }
        return mv;
    }
    
    private int alphabeta(int[] puzzle, int depth, int a, int b, int player)
    {
        if (moves(puzzle).Count == 0 || eval(puzzle) != 0 || depth == 9)
        {
            return eval(puzzle);
        }
    
        if (player == 1)
        {
            int value = -3;
            foreach(int move in moves(puzzle))
            {
                int[] newPuzzle = copy(puzzle);
                newPuzzle[move] = 1;
                value = Math.Max(value, alphabeta(newPuzzle, depth + 1, a, b, -1));
                if (value > b)
                    break;
                a = Math.Max(a, value);
            }
            return value;
        }
        else
        {
            int value = 3;
            foreach (int move in moves(puzzle))
            {
                int[] newPuzzle = copy(puzzle);
                newPuzzle[move] = -1;
                value = Math.Min(value, alphabeta(newPuzzle, depth + 1, a, b, 1));
                if (value < a)
                    break;
                b = Math.Min(b, value);
            }
            return value;
        }
    }
    
    private int getBestMove(int player)
    {
        int tempScore = -99;
        int bestMoveIndex = moves(gamePuzzle)[0];
        foreach (int i in moves(gamePuzzle))
        {
            int[] tboard = copy(gamePuzzle);
            tboard[i] = player;
            int score = alphabeta(tboard, 0, -2, 2, -player) * player;
            if (score > tempScore)
            {
                bestMoveIndex = i;
                tempScore = score;
            }
        }
        return bestMoveIndex;
    }

    private int[] copy(int[] array)
    {
        int[] copy = new int[9];
        for (int i = 0; i < array.Length; i++)
        {
            copy[i] = array[i];
        }

        return copy;
    }
}
