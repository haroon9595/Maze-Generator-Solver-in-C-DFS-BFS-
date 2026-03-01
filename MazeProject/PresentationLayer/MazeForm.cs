using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MazeProject.BusinessLogicLayer;
using MazeProject.DataAccessLayer;

namespace MazeProject.PresentationLayer
{
    public partial class MazeForm : Form
    {
        private Maze maze;
        private MazeGenerator mazeGenerator;
        private MazeSolver mazeSolver;

        private int cellSize;
        private int mazeRows;
        private int mazeCols;

        // Color scheme
        private Color currentCellColor = Color.FromArgb(255, 255, 100);
        private Color visitedCellColor = Color.FromArgb(180, 220, 255);
        private Color pathColor = Color.FromArgb(100, 220, 100);
        private Color wallColor = Color.FromArgb(40, 40, 40);
        private Color startColor = Color.FromArgb(0, 180, 0);
        private Color endColor = Color.FromArgb(220, 0, 0);
        private Color backgroundColor = Color.FromArgb(240, 245, 250);

        private int animationSpeed = 100;
        private bool isGenerating = false;
        private bool isSolving = false;
        private bool mazeGenerated = false;
        private CancellationTokenSource solvingCancellationTokenSource;

        private Panel mazePanel;
        private Button btnGenerate;
        private Button btnSolve;
        private Button btnStopSolving;
        private Button btnExit;
        private TrackBar trackSpeed;
        private Label lblPathLength;
        private Label lblStatus;
        private Label lblTitle;
        private Panel titleBar;
        private Panel controlPanel;

        public MazeForm(int rows, int cols)
        {
            mazeRows = rows;
            mazeCols = cols;

            InitializeComponent();
            InitializeMazeComponents();
            SetupUI();
            UpdateStatus($"Ready - {rows}x{cols} Maze - Click Generate Maze");
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = $"Maze Solver - {mazeRows}x{mazeCols}";
            this.Size = new Size(1000, 700);
            this.BackColor = backgroundColor;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(800, 600);
            this.ResumeLayout(false);
        }

        private void InitializeMazeComponents()
        {
            maze = new Maze(mazeRows, mazeCols);
            mazeGenerator = new MazeGenerator();
            mazeSolver = new MazeSolver();
        }

        private void SetupUI()
        {
            // Title bar
            titleBar = new Panel();
            titleBar.BackColor = Color.FromArgb(50, 70, 120);
            titleBar.Size = new Size(980, 50);
            titleBar.Location = new Point(10, 10);
            this.Controls.Add(titleBar);

            // Title
            lblTitle = new Label();
            lblTitle.Text = $"🎯 MAZE SOLVER - {mazeRows}x{mazeCols}";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Size = new Size(500, 40);
            lblTitle.Location = new Point(20, 15);
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            titleBar.Controls.Add(lblTitle);

            // Exit button in title bar
            btnExit = new Button();
            btnExit.Text = "← Exit to Menu";
            btnExit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnExit.ForeColor = Color.White;
            btnExit.BackColor = Color.FromArgb(200, 80, 80);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 100, 100);
            btnExit.Size = new Size(120, 35);
            btnExit.Location = new Point(840, 8);
            btnExit.Click += (s, e) => this.Close();
            titleBar.Controls.Add(btnExit);

            // Maze panel
            mazePanel = new Panel();
            mazePanel.BackColor = Color.White;
            mazePanel.BorderStyle = BorderStyle.FixedSingle;
            mazePanel.Paint += MazePanel_Paint;
            mazePanel.Size = new Size(600, 600);
            mazePanel.Location = new Point(20, 70);
            this.Controls.Add(mazePanel);

            // Control panel
            controlPanel = new Panel();
            controlPanel.BackColor = Color.White;
            controlPanel.BorderStyle = BorderStyle.FixedSingle;
            controlPanel.Size = new Size(330, 600);
            controlPanel.Location = new Point(640, 70);
            this.Controls.Add(controlPanel);

            // Controls title
            Label lblControlsTitle = new Label();
            lblControlsTitle.Text = "⚙️ CONTROLS";
            lblControlsTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblControlsTitle.ForeColor = Color.FromArgb(60, 60, 120);
            lblControlsTitle.Size = new Size(300, 30);
            lblControlsTitle.Location = new Point(15, 15);
            controlPanel.Controls.Add(lblControlsTitle);

            // Generate Button
            btnGenerate = new Button();
            btnGenerate.Text = "🚀 GENERATE MAZE (DFS)";
            btnGenerate.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.BackColor = Color.FromArgb(70, 130, 180);
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 100, 150);
            btnGenerate.Size = new Size(300, 45);
            btnGenerate.Location = new Point(15, 60);
            btnGenerate.Click += BtnGenerate_Click;
            controlPanel.Controls.Add(btnGenerate);

            // Solve Button
            btnSolve = new Button();
            btnSolve.Text = "🎯 SOLVE MAZE (BFS)";
            btnSolve.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnSolve.ForeColor = Color.White;
            btnSolve.BackColor = Color.FromArgb(60, 140, 60);
            btnSolve.FlatStyle = FlatStyle.Flat;
            btnSolve.FlatAppearance.BorderSize = 0;
            btnSolve.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 120, 40);
            btnSolve.Size = new Size(300, 45);
            btnSolve.Location = new Point(15, 115);
            btnSolve.Click += BtnSolve_Click;
            btnSolve.Enabled = false;
            controlPanel.Controls.Add(btnSolve);

            // Stop Solving Button
            btnStopSolving = new Button();
            btnStopSolving.Text = "⏹️ STOP SOLVING";
            btnStopSolving.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnStopSolving.ForeColor = Color.White;
            btnStopSolving.BackColor = Color.FromArgb(200, 80, 80);
            btnStopSolving.FlatStyle = FlatStyle.Flat;
            btnStopSolving.FlatAppearance.BorderSize = 0;
            btnStopSolving.FlatAppearance.MouseOverBackColor = Color.FromArgb(180, 60, 60);
            btnStopSolving.Size = new Size(300, 45);
            btnStopSolving.Location = new Point(15, 170);
            btnStopSolving.Click += BtnStopSolving_Click;
            btnStopSolving.Visible = false;
            controlPanel.Controls.Add(btnStopSolving);

            // Animation Speed
            Label lblSpeed = new Label();
            lblSpeed.Text = "📊 ANIMATION SPEED:";
            lblSpeed.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblSpeed.ForeColor = Color.FromArgb(60, 60, 100);
            lblSpeed.Size = new Size(200, 25);
            lblSpeed.Location = new Point(15, 230);
            controlPanel.Controls.Add(lblSpeed);

            // Speed value label
            Label lblSpeedValue = new Label();
            lblSpeedValue.Text = "MEDIUM";
            lblSpeedValue.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSpeedValue.ForeColor = Color.FromArgb(60, 140, 200);
            lblSpeedValue.Size = new Size(100, 25);
            lblSpeedValue.Location = new Point(215, 230);
            lblSpeedValue.Name = "lblSpeedValue";
            controlPanel.Controls.Add(lblSpeedValue);

            // Speed TrackBar
            trackSpeed = new TrackBar();
            trackSpeed.Minimum = 10;   // Very Fast
            trackSpeed.Maximum = 300;  // Very Slow
            trackSpeed.Value = 100;    // Medium
            trackSpeed.TickFrequency = 20;
            trackSpeed.Size = new Size(300, 45);
            trackSpeed.Location = new Point(15, 260);
            trackSpeed.ValueChanged += TrackSpeed_ValueChanged;
            controlPanel.Controls.Add(trackSpeed);

            // Status Panel
            Panel statusPanel = new Panel();
            statusPanel.BackColor = Color.FromArgb(245, 245, 250);
            statusPanel.BorderStyle = BorderStyle.FixedSingle;
            statusPanel.Size = new Size(300, 80);
            statusPanel.Location = new Point(15, 320);
            controlPanel.Controls.Add(statusPanel);

            Label lblStatusTitle = new Label();
            lblStatusTitle.Text = "📈 STATUS:";
            lblStatusTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblStatusTitle.ForeColor = Color.FromArgb(60, 60, 100);
            lblStatusTitle.Size = new Size(280, 25);
            lblStatusTitle.Location = new Point(10, 10);
            statusPanel.Controls.Add(lblStatusTitle);

            lblStatus = new Label();
            lblStatus.Text = "Ready";
            lblStatus.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblStatus.ForeColor = Color.FromArgb(80, 80, 120);
            lblStatus.Size = new Size(280, 40);
            lblStatus.Location = new Point(10, 35);
            statusPanel.Controls.Add(lblStatus);

            // Path Length Panel
            Panel pathPanel = new Panel();
            pathPanel.BackColor = Color.FromArgb(245, 245, 250);
            pathPanel.BorderStyle = BorderStyle.FixedSingle;
            pathPanel.Size = new Size(300, 60);
            pathPanel.Location = new Point(15, 410);
            controlPanel.Controls.Add(pathPanel);

            Label lblPathTitle = new Label();
            lblPathTitle.Text = "📏 PATH LENGTH:";
            lblPathTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPathTitle.ForeColor = Color.FromArgb(60, 60, 100);
            lblPathTitle.Size = new Size(280, 25);
            lblPathTitle.Location = new Point(10, 10);
            pathPanel.Controls.Add(lblPathTitle);

            lblPathLength = new Label();
            lblPathLength.Text = "0 cells";
            lblPathLength.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblPathLength.ForeColor = Color.FromArgb(60, 140, 200);
            lblPathLength.Size = new Size(280, 30);
            lblPathLength.Location = new Point(10, 25);
            lblPathLength.TextAlign = ContentAlignment.MiddleCenter;
            pathPanel.Controls.Add(lblPathLength);

            // Legend Panel
            Panel legendPanel = new Panel();
            legendPanel.BackColor = Color.FromArgb(245, 245, 250);
            legendPanel.BorderStyle = BorderStyle.FixedSingle;
            legendPanel.Size = new Size(300, 150);
            legendPanel.Location = new Point(15, 480);
            controlPanel.Controls.Add(legendPanel);

            Label lblLegendTitle = new Label();
            lblLegendTitle.Text = "🎨 COLOR LEGEND:";
            lblLegendTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblLegendTitle.ForeColor = Color.FromArgb(60, 60, 100);
            lblLegendTitle.Size = new Size(280, 25);
            lblLegendTitle.Location = new Point(10, 10);
            legendPanel.Controls.Add(lblLegendTitle);

            int legendY = 40;
            AddLegendItem(legendPanel, startColor, "Start Cell", ref legendY);
            AddLegendItem(legendPanel, endColor, "End Cell", ref legendY);
            AddLegendItem(legendPanel, visitedCellColor, "Explored Cells", ref legendY);
            AddLegendItem(legendPanel, pathColor, "Shortest Path", ref legendY);
            AddLegendItem(legendPanel, wallColor, "Walls", ref legendY);

            UpdateMazePanelSize();
        }

        private void AddLegendItem(Panel parent, Color color, string text, ref int y)
        {
            Panel colorBox = new Panel();
            colorBox.BackColor = color;
            colorBox.Size = new Size(20, 20);
            colorBox.Location = new Point(15, y);
            colorBox.BorderStyle = BorderStyle.FixedSingle;
            parent.Controls.Add(colorBox);

            Label label = new Label();
            label.Text = text;
            label.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            label.ForeColor = Color.FromArgb(60, 60, 100);
            label.Size = new Size(250, 20);
            label.Location = new Point(45, y);
            parent.Controls.Add(label);

            y += 25;
        }

        private void UpdateMazePanelSize()
        {
            if (mazePanel == null || maze == null) return;

            int maxCellSize = 30;
            cellSize = Math.Min(maxCellSize, mazePanel.Width / mazeCols);
            cellSize = Math.Min(cellSize, mazePanel.Height / mazeRows);

            if (cellSize < 8) cellSize = 8;

            mazePanel.Size = new Size(mazeCols * cellSize + 2, mazeRows * cellSize + 2);
        }

        private void MazePanel_Paint(object sender, PaintEventArgs e)
        {
            if (maze == null) return;

            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            for (int row = 0; row < maze.Rows; row++)
            {
                for (int col = 0; col < maze.Cols; col++)
                {
                    DrawCell(g, maze.Grid[row, col]);
                }
            }
        }

        private void DrawCell(Graphics g, Cell cell)
        {
            int x = cell.Col * cellSize;
            int y = cell.Row * cellSize;

            Brush fillBrush = Brushes.White;

            if (cell == maze.StartCell)
                fillBrush = new SolidBrush(startColor);
            else if (cell == maze.EndCell)
                fillBrush = new SolidBrush(endColor);
            else if (cell.IsInPath)
                fillBrush = new SolidBrush(pathColor);
            else if (cell.IsExplored)
                fillBrush = new SolidBrush(visitedCellColor);

            g.FillRectangle(fillBrush, x + 1, y + 1, cellSize - 1, cellSize - 1);

            using (Pen wallPen = new Pen(wallColor, 2))
            {
                if (cell.Walls[0])
                    g.DrawLine(wallPen, x, y, x + cellSize, y);
                if (cell.Walls[1])
                    g.DrawLine(wallPen, x + cellSize, y, x + cellSize, y + cellSize);
                if (cell.Walls[2])
                    g.DrawLine(wallPen, x, y + cellSize, x + cellSize, y + cellSize);
                if (cell.Walls[3])
                    g.DrawLine(wallPen, x, y, x, y + cellSize);
            }

            // Add S/E markers for start and end
            if (cell == maze.StartCell || cell == maze.EndCell)
            {
                string marker = cell == maze.StartCell ? "S" : "E";
                using (Font markerFont = new Font("Arial", cellSize / 2, FontStyle.Bold))
                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(marker, markerFont, textBrush,
                        x + cellSize / 2, y + cellSize / 2, sf);
                }
            }
        }

        private async void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (isGenerating) return;

            isGenerating = true;
            mazeGenerated = false;
            btnGenerate.Enabled = false;
            btnSolve.Enabled = false;
            trackSpeed.Enabled = false;

            UpdateStatus("🔧 Generating maze with DFS algorithm...");

            // Reset maze
            maze.ResetMaze();
            foreach (var cell in maze.Grid)
            {
                cell.IsCurrent = false;
                cell.IsExplored = false;
                cell.IsInPath = false;
                cell.Visited = false;
            }

            // Clear display
            mazePanel.Invalidate();
            await Task.Delay(50);

            // Generate maze ONLY ONCE
            mazeGenerator.GenerateMazeDFS(maze);
            mazeGenerated = true;

            // Show maze immediately
            mazePanel.Invalidate();
            await Task.Delay(200);

            UpdateStatus("✅ Maze generated successfully!");
            UpdatePathLength(0);

            btnGenerate.Enabled = true;
            btnSolve.Enabled = true;
            trackSpeed.Enabled = true;
            isGenerating = false;
        }

        private async void BtnSolve_Click(object sender, EventArgs e)
        {
            if (isSolving || isGenerating || maze == null || !mazeGenerated) return;

            isSolving = true;
            solvingCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = solvingCancellationTokenSource.Token;

            btnGenerate.Enabled = false;
            btnSolve.Enabled = false;
            btnStopSolving.Visible = true;
            trackSpeed.Enabled = false;

            UpdateStatus("🔍 Solving maze with BFS algorithm...");

            try
            {
                // Reset only solving states (DO NOT regenerate maze)
                foreach (var cell in maze.Grid)
                {
                    cell.IsCurrent = false;
                    cell.IsExplored = false;
                    cell.IsInPath = false;
                    cell.Parent = null;
                }

                // Clear display
                mazePanel.Invalidate();
                await Task.Delay(100);

                // Solve maze
                List<Cell> path = mazeSolver.SolveMazeBFS(maze);

                if (cancellationToken.IsCancellationRequested)
                {
                    UpdateStatus("⏹️ Solving cancelled.");
                    return;
                }

                if (path.Count == 0)
                {
                    UpdateStatus("❌ No path found!");
                    ResetSolvingState();
                    return;
                }

                // Show ALL explored cells at once (blue cells)
                UpdateStatus("🌊 Showing explored cells...");

                // Mark all explored cells
                foreach (var cell in maze.Grid)
                {
                    if (cell.IsExplored)
                    {
                        // Already marked by BFS
                    }
                }

                mazePanel.Invalidate();
                await Task.Delay(300);

                if (cancellationToken.IsCancellationRequested)
                {
                    UpdateStatus("⏹️ Solving cancelled.");
                    return;
                }

                // Animate the shortest path (green line)
                UpdateStatus("🟢 Drawing shortest path...");

                // Reset path state
                foreach (var cell in path)
                {
                    cell.IsInPath = false;
                }

                // Animate path drawing - ONE CELL AT A TIME
                for (int i = 0; i < path.Count; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        UpdateStatus("⏹️ Solving cancelled.");
                        return;
                    }

                    path[i].IsInPath = true;

                    // Repaint every 2 cells for smooth animation
                    if (i % 2 == 0 || i == path.Count - 1)
                    {
                        mazePanel.Invalidate();
                        await Task.Delay(animationSpeed);
                    }

                    // Update status
                    if (i % 5 == 0)
                    {
                        UpdateStatus($"🟢 Path drawing... {i + 1}/{path.Count} cells");
                    }
                }

                mazePanel.Invalidate();
                UpdateStatus("🎉 Maze solved! Shortest path found.");
                UpdatePathLength(path.Count);
            }
            catch (OperationCanceledException)
            {
                UpdateStatus("⏹️ Solving cancelled.");
            }
            finally
            {
                ResetSolvingState();
            }
        }

        private void BtnStopSolving_Click(object sender, EventArgs e)
        {
            if (isSolving && solvingCancellationTokenSource != null)
            {
                solvingCancellationTokenSource.Cancel();
                UpdateStatus("🛑 Stopping solving process...");
            }
        }

        private void ResetSolvingState()
        {
            isSolving = false;
            btnGenerate.Enabled = true;
            btnSolve.Enabled = mazeGenerated;
            btnStopSolving.Visible = false;
            trackSpeed.Enabled = true;

            if (solvingCancellationTokenSource != null)
            {
                solvingCancellationTokenSource.Dispose();
                solvingCancellationTokenSource = null;
            }
        }

        private void TrackSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (trackSpeed == null) return;

            // Update animation speed
            animationSpeed = 310 - trackSpeed.Value;

            // Update speed label
            string speedText = "";
            if (trackSpeed.Value <= 50) speedText = "VERY FAST";
            else if (trackSpeed.Value <= 100) speedText = "FAST";
            else if (trackSpeed.Value <= 150) speedText = "MEDIUM";
            else if (trackSpeed.Value <= 200) speedText = "SLOW";
            else speedText = "VERY SLOW";

            // Update the speed value label
            foreach (Control ctrl in controlPanel.Controls)
            {
                if (ctrl.Name == "lblSpeedValue")
                {
                    ctrl.Text = speedText;
                    break;
                }
            }
        }

        private void UpdatePathLength(int length)
        {
            if (lblPathLength != null)
            {
                lblPathLength.Text = $"{length} cells";
                lblPathLength.ForeColor = length > 0 ? Color.FromArgb(60, 180, 80) : Color.FromArgb(60, 140, 200);
            }
        }

        private void UpdateStatus(string message)
        {
            if (lblStatus != null)
            {
                lblStatus.Text = message;

                // Color coding for status
                if (message.Contains("✅") || message.Contains("🎉"))
                    lblStatus.ForeColor = Color.FromArgb(60, 180, 80);
                else if (message.Contains("❌"))
                    lblStatus.ForeColor = Color.FromArgb(220, 80, 80);
                else if (message.Contains("🔍") || message.Contains("🔧"))
                    lblStatus.ForeColor = Color.FromArgb(60, 140, 200);
                else
                    lblStatus.ForeColor = Color.FromArgb(100, 100, 120);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (mazePanel != null && controlPanel != null && titleBar != null)
            {
                // Update title bar width
                titleBar.Width = this.ClientSize.Width - 20;
                if (titleBar.Controls.Count > 1)
                {
                    // Reposition exit button
                    Control exitBtn = titleBar.Controls[1];
                    exitBtn.Left = titleBar.Width - exitBtn.Width - 10;
                }

                // Calculate new positions
                int controlPanelWidth = 330;
                int mazePanelWidth = Math.Min(600, this.ClientSize.Width - controlPanelWidth - 60);
                int mazePanelHeight = Math.Min(600, this.ClientSize.Height - 90);

                // Update maze panel size
                mazePanel.Size = new Size(mazePanelWidth, mazePanelHeight);

                // Recalculate cell size
                int maxCellSize = 30;
                cellSize = Math.Min(maxCellSize, mazePanel.Width / mazeCols);
                cellSize = Math.Min(cellSize, mazePanel.Height / mazeRows);
                if (cellSize < 8) cellSize = 8;

                mazePanel.Size = new Size(mazeCols * cellSize + 2, mazeRows * cellSize + 2);

                // Reposition panels
                mazePanel.Location = new Point(
                    20,
                    Math.Max(70, (this.ClientSize.Height - mazePanel.Height) / 2)
                );

                controlPanel.Location = new Point(
                    mazePanel.Right + 20,
                    mazePanel.Top
                );

                mazePanel.Invalidate();
            }
        }
    }
}