// Sergi
// CST-250

using System;
using System.Drawing;
using System.Windows.Forms;
using Minesweeper.Models;
using Minesweeper.Services;
using Timer = System.Windows.Forms.Timer;

namespace Minesweeper.GUI
{
    public class FrmMinesweeper : Form
    {
        private readonly BoardModel _board;
        private readonly IBoardService _boardService;

        private Button[,] _buttons = null!;
        private readonly Panel _boardPanel;
        private readonly Label _lblTime;
        private readonly Label _lblScore;
        private readonly Button _btnRestart;
        private readonly Timer _timer;

        private DateTime _startTime;
        private int _score;

        public FrmMinesweeper(BoardModel board, IBoardService boardService)
        {
            _board = board;
            _boardService = boardService;

            Text = "Minesweeper";
            Width = 900;
            Height = 700;
            StartPosition = FormStartPosition.CenterScreen;

            _boardPanel = new Panel
            {
                Left = 10,
                Top = 10,
                Width = 640,
                Height = 640,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(_boardPanel);

            var lblTimeCaption = new Label
            {
                Text = "Time:",
                AutoSize = true,
                Location = new Point(670, 40)
            };
            Controls.Add(lblTimeCaption);

            _lblTime = new Label
            {
                Text = "00:00",
                AutoSize = true,
                Location = new Point(670, 60)
            };
            Controls.Add(_lblTime);

            var lblScoreCaption = new Label
            {
                Text = "Score:",
                AutoSize = true,
                Location = new Point(670, 100)
            };
            Controls.Add(lblScoreCaption);

            _lblScore = new Label
            {
                Text = "0",
                AutoSize = true,
                Location = new Point(670, 120)
            };
            Controls.Add(_lblScore);

            _btnRestart = new Button
            {
                Text = "Restart",
                Width = 100,
                Height = 30,
                Location = new Point(670, 170)
            };
            _btnRestart.Click += BtnRestart_Click;
            Controls.Add(_btnRestart);

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += Timer_Tick;

            Load += FrmMinesweeper_Load;
        }

        private void FrmMinesweeper_Load(object? sender, EventArgs e)
        {
            BuildBoardButtons();
            _startTime = DateTime.Now;
            _timer.Start();
            UpdateStatusLabels();
        }

        private void BuildBoardButtons()
        {
            int size = _board.Size;
            _buttons = new Button[size, size];

            int cellSize = Math.Min(_boardPanel.Width, _boardPanel.Height) / size;

            _boardPanel.Controls.Clear();

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    var btn = new Button
                    {
                        Width = cellSize - 2,
                        Height = cellSize - 2,
                        Left = c * cellSize,
                        Top = r * cellSize,
                        BackColor = Color.LightGray,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Tag = (r, c)
                    };

                    btn.MouseUp += Cell_MouseUp;
                    _boardPanel.Controls.Add(btn);
                    _buttons[r, c] = btn;
                }
            }

            RefreshBoardUI();
        }

        private void Cell_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_board.GameState != GameState.InProgress) return;
            if (sender is not Button btn || btn.Tag is not ValueTuple<int, int> t) return;

            int r = t.Item1;
            int c = t.Item2;
            var cell = _board.Grid[r, c];

            if (e.Button == MouseButtons.Right)
            {
                if (!cell.IsRevealed)
                {
                    cell.IsFlagged = !cell.IsFlagged;
                    RefreshBoardUI();
                    CheckWinCondition();
                }
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                _boardService.Reveal(_board, r, c);
                RefreshBoardUI();

                if (cell.HasReward && cell.IsRevealed)
                {
                    _score += 100;
                }

                if (_board.GameState == GameState.Lost)
                {
                    _timer.Stop();
                    RevealAllForGameOver();
                    UpdateStatusLabels();
                    MessageBox.Show("Boom! You hit a bomb.", "Game Over");
                }
                else
                {
                    CheckWinCondition();
                }
            }
        }

        private void RefreshBoardUI()
        {
            int size = _board.Size;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    var cell = _board.Grid[r, c];
                    var btn = _buttons[r, c];

                    if (cell.IsRevealed)
                    {
                        if (cell.HasBomb)
                        {
                            btn.Text = "B";
                            btn.BackColor = Color.IndianRed;
                        }
                        else if (cell.HasReward)
                        {
                            btn.Text = "$";
                            btn.BackColor = Color.Goldenrod;
                        }
                        else if (cell.BombsNearby > 0)
                        {
                            btn.Text = cell.BombsNearby.ToString();
                            btn.BackColor = Color.White;
                        }
                        else
                        {
                            btn.Text = "";
                            btn.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        if (cell.IsFlagged)
                        {
                            btn.Text = "F";
                            btn.BackColor = Color.LightYellow;
                        }
                        else
                        {
                            btn.Text = "";
                            btn.BackColor = Color.LightGray;
                        }
                    }
                }
            }

            UpdateStatusLabels();
        }

        private void CheckWinCondition()
        {
            int bombs = 0;
            int flaggedBombs = 0;
            int unrevealedSafe = 0;

            for (int r = 0; r < _board.Size; r++)
            {
                for (int c = 0; c < _board.Size; c++)
                {
                    var cell = _board.Grid[r, c];

                    if (cell.HasBomb)
                    {
                        bombs++;
                        if (cell.IsFlagged) flaggedBombs++;
                    }
                    else
                    {
                        if (!cell.IsRevealed) unrevealedSafe++;
                    }
                }
            }

            if (bombs > 0 && flaggedBombs == bombs && unrevealedSafe == 0)
            {
                _board.GameState = GameState.Won;
                _timer.Stop();

                int seconds = (int)(DateTime.Now - _startTime).TotalSeconds;
                int finalScore = Math.Max(0, _score + Math.Max(0, 5000 - seconds * 10));

                UpdateStatusLabels();
                MessageBox.Show($"Congratulations! You won! Your score is {finalScore}!", "You Win");
            }
        }

        private void RevealAllForGameOver()
        {
            for (int r = 0; r < _board.Size; r++)
            {
                for (int c = 0; c < _board.Size; c++)
                {
                    _board.Grid[r, c].IsRevealed = true;
                }
            }
            RefreshBoardUI();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateStatusLabels();
        }

        private void UpdateStatusLabels()
        {
            var elapsed = DateTime.Now - _startTime;
            _lblTime.Text = elapsed.ToString(@"mm\:ss");
            _lblScore.Text = _score.ToString();
        }

        private void BtnRestart_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
