// Sergi
// CST-250

using System;
using System.Drawing;
using System.Windows.Forms;
using Minesweeper.Models;
using Minesweeper.Services;

namespace Minesweeper.GUI
{
    public class FrmNewGame : Form
    {
        private readonly TrackBar _trkSize;
        private readonly TrackBar _trkDifficulty;
        private readonly Label _lblSizeValue;
        private readonly Label _lblDifficultyValue;
        private readonly Button _btnPlay;

        public FrmNewGame()
        {
            Text = "Start a new Game";
            Width = 350;
            Height = 250;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            var lblTitle = new Label
            {
                Text = "Play Minesweeper",
                AutoSize = true,
                Font = new Font(Font, FontStyle.Bold),
                Location = new Point(20, 15)
            };
            Controls.Add(lblTitle);

            var lblSize = new Label
            {
                Text = "Size:",
                AutoSize = true,
                Location = new Point(20, 50)
            };
            Controls.Add(lblSize);

            _lblSizeValue = new Label
            {
                Text = "8",
                AutoSize = true,
                Location = new Point(70, 50)
            };
            Controls.Add(_lblSizeValue);

            _trkSize = new TrackBar
            {
                Minimum = 5,
                Maximum = 20,
                TickFrequency = 1,
                Value = 8,
                Width = 260,
                Location = new Point(20, 70)
            };
            _trkSize.ValueChanged += (s, e) => { _lblSizeValue.Text = _trkSize.Value.ToString(); };
            Controls.Add(_trkSize);

            var lblDiff = new Label
            {
                Text = "Difficulty (% bombs):",
                AutoSize = true,
                Location = new Point(20, 110)
            };
            Controls.Add(lblDiff);

            _lblDifficultyValue = new Label
            {
                Text = "10%",
                AutoSize = true,
                Location = new Point(170, 110)
            };
            Controls.Add(_lblDifficultyValue);

            _trkDifficulty = new TrackBar
            {
                Minimum = 5,
                Maximum = 30,
                TickFrequency = 1,
                Value = 10,
                Width = 260,
                Location = new Point(20, 130)
            };
            _trkDifficulty.ValueChanged += (s, e) => { _lblDifficultyValue.Text = $"{_trkDifficulty.Value}%"; };
            Controls.Add(_trkDifficulty);

            _btnPlay = new Button
            {
                Text = "Play",
                Width = 100,
                Height = 30,
                Location = new Point((ClientSize.Width - 100) / 2, 175),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            _btnPlay.Click += BtnPlay_Click;
            Controls.Add(_btnPlay);
        }

        private void BtnPlay_Click(object? sender, EventArgs e)
        {
            int size = _trkSize.Value;
            float difficultyPct = _trkDifficulty.Value / 100f;

            var board = new BoardModel(size);
            board.SetDifficultyPercentage(difficultyPct);

            IBoardService svc = new BoardService();
            svc.SetupBombs(board);
            svc.CountBombsNearby(board);

            using var gameForm = new FrmMinesweeper(board, svc);
            Hide();
            gameForm.ShowDialog();
            Show();
        }
    }
}
