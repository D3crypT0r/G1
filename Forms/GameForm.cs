using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class GameForm : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        private int maxWidth;
        private int maxHeight;
        private int score;
        private bool gameOver;
        private Random rand = new Random();
        
        enum Direction { Left, Right, Up, Down }
        Direction currentDirection;

        public GameForm()
        {
            InitializeComponent();
            SetupGame();
            SetupUI();
        }

        private void SetupUI()
        {
            this.BackColor = Color.FromArgb(25, 25, 35);
            this.Text = "Snake Game";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.KeyPreview = true;
            
            // Score Label
            lblScore.ForeColor = Color.FromArgb(0, 174, 219);
            lblScore.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblScore.BackColor = Color.Transparent;
            
            // Game Over Panel
            panelGameOver.Visible = false;
            panelGameOver.BackColor = Color.FromArgb(50, 50, 60, 150);
            panelGameOver.Size = new Size(pbCanvas.Width, pbCanvas.Height);
            panelGameOver.Location = new Point(pbCanvas.Location.X, pbCanvas.Location.Y);
            
            lblGameOver.ForeColor = Color.White;
            lblGameOver.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblFinalScore.ForeColor = Color.FromArgb(0, 174, 219);
            lblFinalScore.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            
            btnRestart.BackColor = Color.FromArgb(0, 120, 215);
            btnRestart.ForeColor = Color.White;
            btnRestart.FlatStyle = FlatStyle.Flat;
            btnRestart.FlatAppearance.BorderSize = 0;
            btnRestart.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void SetupGame()
        {
            maxWidth = pbCanvas.Width / Settings.Width;
            maxHeight = pbCanvas.Height / Settings.Height;
            gameTimer.Interval = 1000 / Settings.Speed;
            pbCanvas.BackColor = Color.FromArgb(40, 40, 50);
            
            NewGame();
        }

        private void NewGame()
        {
            score = 0;
            lblScore.Text = "SCORE: 0";
            gameOver = false;
            panelGameOver.Visible = false;
            Snake.Clear();
            
            // Initial snake head
            Circle head = new Circle { X = 10, Y = 10 };
            Snake.Add(head);
            
            GenerateFood();
            currentDirection = Direction.Right;
            gameTimer.Start();
        }

        private void GenerateFood()
        {
            // Ensure food doesn't spawn on snake
            bool validPosition;
            do
            {
                validPosition = true;
                food = new Circle
                {
                    X = rand.Next(0, maxWidth),
                    Y = rand.Next(0, maxHeight)
                };
                
                foreach (Circle segment in Snake)
                {
                    if (segment.X == food.X && segment.Y == food.Y)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (gameOver) return;

            MoveSnake();
            CheckCollision();
            pbCanvas.Invalidate();
        }

        private void MoveSnake()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (currentDirection)
                    {
                        case Direction.Left: Snake[i].X--; break;
                        case Direction.Right: Snake[i].X++; break;
                        case Direction.Up: Snake[i].Y--; break;
                        case Direction.Down: Snake[i].Y++; break;
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void CheckCollision()
        {
            Circle head = Snake[0];
            
            // Wall collision
            if (head.X < 0 || head.Y < 0 || head.X >= maxWidth || head.Y >= maxHeight)
            {
                GameOver();
            }
            
            // Self collision
            for (int i = 1; i < Snake.Count; i++)
            {
                if (head.X == Snake[i].X && head.Y == Snake[i].Y)
                {
                    GameOver();
                }
            }
            
            // Food collision
            if (head.X == food.X && head.Y == food.Y)
            {
                EatFood();
            }
        }

        private void EatFood()
        {
            score += Settings.Points;
            lblScore.Text = $"SCORE: {score}";
            
            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);
            
            GenerateFood();
        }

        private void GameOver()
        {
            gameOver = true;
            gameTimer.Stop();
            
            lblFinalScore.Text = $"FINAL SCORE: {score}";
            panelGameOver.Visible = true;
        }

        private void PbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            if (!gameOver)
            {
                // Draw grid
                for (int x = 0; x < maxWidth; x++)
                {
                    for (int y = 0; y < maxHeight; y++)
                    {
                        canvas.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 55)), 
                            x * Settings.Width, y * Settings.Height, 
                            Settings.Width - 1, Settings.Height - 1);
                    }
                }
                
                // Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush brush = (i == 0) ? 
                        new SolidBrush(Settings.HeadColor) : 
                        new SolidBrush(Settings.BodyColor);
                    
                    canvas.FillEllipse(brush,
                        Snake[i].X * Settings.Width,
                        Snake[i].Y * Settings.Height,
                        Settings.Width, Settings.Height);
                }
                
                // Draw food with shine effect
                canvas.FillEllipse(new SolidBrush(Settings.FoodColor),
                    food.X * Settings.Width,
                    food.Y * Settings.Height,
                    Settings.Width, Settings.Height);
                
                canvas.FillEllipse(Brushes.White,
                    food.X * Settings.Width + 5,
                    food.Y * Settings.Height + 3,
                    4, 4);
            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameOver) return;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (currentDirection != Direction.Right) 
                        currentDirection = Direction.Left;
                    break;
                case Keys.Right:
                    if (currentDirection != Direction.Left) 
                        currentDirection = Direction.Right;
                    break;
                case Keys.Up:
                    if (currentDirection != Direction.Down) 
                        currentDirection = Direction.Up;
                    break;
                case Keys.Down:
                    if (currentDirection != Direction.Up) 
                        currentDirection = Direction.Down;
                    break;
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            NewGame();
        }
    }
}
