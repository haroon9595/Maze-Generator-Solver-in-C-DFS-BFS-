using System;
using System.Drawing;
using System.Windows.Forms;

namespace MazeProject.PresentationLayer
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Maze Solver - Main Menu";
            this.Size = new Size(900, 600);
            this.BackColor = Color.FromArgb(30, 30, 40);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 600);
            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "🎮 MAZE SOLVER PRO 🎮";
            lblTitle.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(220, 220, 255);
            lblTitle.Size = new Size(800, 80);
            lblTitle.Location = new Point(50, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            // Subtitle
            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Choose Your Maze Adventure!";
            lblSubtitle.Font = new Font("Segoe UI", 14, FontStyle.Italic);
            lblSubtitle.ForeColor = Color.FromArgb(180, 180, 220);
            lblSubtitle.Size = new Size(600, 40);
            lblSubtitle.Location = new Point(150, 100);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblSubtitle);

            // Decorative line
            Panel line = new Panel();
            line.BackColor = Color.FromArgb(70, 130, 180);
            line.Size = new Size(400, 3);
            line.Location = new Point(250, 150);
            this.Controls.Add(line);

            // Button 1: Small Maze (15x15)
            Button btnSmallMaze = CreateMenuButton("🏰 SMALL MAZE", "15x15 Grid", Color.FromArgb(70, 130, 180), 200);
            btnSmallMaze.Click += (s, e) => OpenMazeForm(15, 15);
            this.Controls.Add(btnSmallMaze);

            // Button 2: Medium Maze (25x25)
            Button btnMediumMaze = CreateMenuButton("🏯 MEDIUM MAZE", "25x25 Grid", Color.FromArgb(80, 180, 80), 280);
            btnMediumMaze.Click += (s, e) => OpenMazeForm(25, 25);
            this.Controls.Add(btnMediumMaze);

            // Button 3: Large Maze (35x35)
            Button btnLargeMaze = CreateMenuButton("🏛️ LARGE MAZE", "35x35 Grid", Color.FromArgb(220, 100, 60), 360);
            btnLargeMaze.Click += (s, e) => OpenMazeForm(35, 35);
            this.Controls.Add(btnLargeMaze);

            // Button 4: Custom Maze
            Button btnCustomMaze = CreateMenuButton("🔧 CUSTOM MAZE", "Choose Your Size", Color.FromArgb(160, 100, 220), 440);
            btnCustomMaze.Click += BtnCustomMaze_Click;
            this.Controls.Add(btnCustomMaze);

            // Footer
            Label lblFooter = new Label();
            lblFooter.Text = "© 2024 Maze Solver Pro | DFS + BFS Algorithms";
            lblFooter.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblFooter.ForeColor = Color.FromArgb(150, 150, 180);
            lblFooter.Size = new Size(400, 30);
            lblFooter.Location = new Point(250, 530);
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblFooter);

            // Decorative elements
            AddDecoration();
        }

        private Button CreateMenuButton(string title, string subtitle, Color color, int y)
        {
            Button button = new Button();
            button.Text = $"{title}\n{subtitle}";
            button.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.BackColor = color;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(color.R + 20, 255),
                Math.Min(color.G + 20, 255),
                Math.Min(color.B + 20, 255)
            );
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(
                Math.Max(color.R - 20, 0),
                Math.Max(color.G - 20, 0),
                Math.Max(color.B - 20, 0)
            );
            button.Size = new Size(400, 70);
            button.Location = new Point(250, y);
            button.TextAlign = ContentAlignment.MiddleCenter;
            return button;
        }

        private void AddDecoration()
        {
            // Left decoration
            Panel leftDeco = new Panel();
            leftDeco.Size = new Size(50, 400);
            leftDeco.Location = new Point(20, 150);
            leftDeco.BackColor = Color.Transparent;
            leftDeco.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(70, 130, 180), 3), 0, 0, 49, 399);
                e.Graphics.DrawLine(new Pen(Color.FromArgb(70, 130, 180), 2), 25, 50, 25, 350);
            };
            this.Controls.Add(leftDeco);

            // Right decoration
            Panel rightDeco = new Panel();
            rightDeco.Size = new Size(50, 400);
            rightDeco.Location = new Point(830, 150);
            rightDeco.BackColor = Color.Transparent;
            rightDeco.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(70, 130, 180), 3), 0, 0, 49, 399);
                e.Graphics.DrawLine(new Pen(Color.FromArgb(70, 130, 180), 2), 25, 50, 25, 350);
            };
            this.Controls.Add(rightDeco);
        }

        private void OpenMazeForm(int rows, int cols)
        {
            MazeForm mazeForm = new MazeForm(rows, cols);
            mazeForm.Show();
            this.Hide();

            mazeForm.FormClosed += (s, args) =>
            {
                this.Show();
            };
        }

        private void BtnCustomMaze_Click(object sender, EventArgs e)
        {
            CustomMazeForm customForm = new CustomMazeForm();
            customForm.Show();
            this.Hide();

            customForm.FormClosed += (s, args) =>
            {
                this.Show();
            };
        }
    }
}