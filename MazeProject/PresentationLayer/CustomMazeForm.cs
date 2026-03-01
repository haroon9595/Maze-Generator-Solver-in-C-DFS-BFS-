using System;
using System.Drawing;
using System.Windows.Forms;

namespace MazeProject.PresentationLayer
{
    public partial class CustomMazeForm : Form
    {
        public CustomMazeForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Custom Maze Size";
            this.Size = new Size(600, 400);
            this.BackColor = Color.FromArgb(240, 245, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(600, 400);
            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "🔧 CUSTOM MAZE SIZE";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(60, 60, 120);
            lblTitle.Size = new Size(550, 50);
            lblTitle.Location = new Point(25, 20);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            // Instructions
            Label lblInstructions = new Label();
            lblInstructions.Text = "Enter your desired maze dimensions (10-40)";
            lblInstructions.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblInstructions.ForeColor = Color.FromArgb(100, 100, 150);
            lblInstructions.Size = new Size(400, 30);
            lblInstructions.Location = new Point(100, 80);
            lblInstructions.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblInstructions);

            // Rows input
            Label lblRows = new Label();
            lblRows.Text = "Rows:";
            lblRows.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblRows.ForeColor = Color.FromArgb(60, 60, 100);
            lblRows.Size = new Size(80, 30);
            lblRows.Location = new Point(150, 140);
            lblRows.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblRows);

            NumericUpDown numRows = new NumericUpDown();
            numRows.Minimum = 10;
            numRows.Maximum = 40;
            numRows.Value = 20;
            numRows.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            numRows.Size = new Size(100, 30);
            numRows.Location = new Point(240, 140);
            this.Controls.Add(numRows);

            // Columns input
            Label lblCols = new Label();
            lblCols.Text = "Columns:";
            lblCols.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblCols.ForeColor = Color.FromArgb(60, 60, 100);
            lblCols.Size = new Size(80, 30);
            lblCols.Location = new Point(150, 190);
            lblCols.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblCols);

            NumericUpDown numCols = new NumericUpDown();
            numCols.Minimum = 10;
            numCols.Maximum = 40;
            numCols.Value = 20;
            numCols.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            numCols.Size = new Size(100, 30);
            numCols.Location = new Point(240, 190);
            this.Controls.Add(numCols);

            // Size preview
            Label lblPreview = new Label();
            lblPreview.Text = "Preview: 20x20 Maze";
            lblPreview.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblPreview.ForeColor = Color.FromArgb(80, 80, 120);
            lblPreview.Size = new Size(200, 30);
            lblPreview.Location = new Point(200, 230);
            lblPreview.TextAlign = ContentAlignment.MiddleCenter;
            lblPreview.Name = "lblPreview";
            this.Controls.Add(lblPreview);

            // Update preview when values change
            numRows.ValueChanged += (s, e) => UpdatePreview(numRows, numCols, lblPreview);
            numCols.ValueChanged += (s, e) => UpdatePreview(numRows, numCols, lblPreview);

            // Start button
            Button btnStart = new Button();
            btnStart.Text = "🚀 START CUSTOM MAZE";
            btnStart.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.BackColor = Color.FromArgb(70, 130, 180);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 100, 150);
            btnStart.Size = new Size(300, 50);
            btnStart.Location = new Point(150, 280);
            btnStart.Click += (s, e) =>
            {
                int rows = (int)numRows.Value;
                int cols = (int)numCols.Value;

                MazeForm mazeForm = new MazeForm(rows, cols);
                mazeForm.Show();
                this.Hide();

                mazeForm.FormClosed += (closedSender, closedArgs) =>
                {
                    this.Show();
                };
            };
            this.Controls.Add(btnStart);

            // Back button
            Button btnBack = new Button();
            btnBack.Text = "← BACK TO MENU";
            btnBack.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.BackColor = Color.FromArgb(120, 120, 150);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 130);
            btnBack.Size = new Size(200, 40);
            btnBack.Location = new Point(200, 340);
            btnBack.Click += (s, e) => this.Close();
            this.Controls.Add(btnBack);
        }

        private void UpdatePreview(NumericUpDown rowsControl, NumericUpDown colsControl, Label previewLabel)
        {
            int rows = (int)rowsControl.Value;
            int cols = (int)colsControl.Value;
            previewLabel.Text = $"Preview: {rows}x{cols} Maze";
        }
    }
}