namespace DroneRouteMap
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonRouteDot = new System.Windows.Forms.Button();
            this.buttonRoutePol = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonDot = new System.Windows.Forms.RadioButton();
            this.radioButtonPol = new System.Windows.Forms.RadioButton();
            this.labelLat = new System.Windows.Forms.Label();
            this.labelLng = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDronRadius = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(12, 12);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 2;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(667, 360);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 0D;
            this.gMapControl1.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.gMapControl1_OnMarkerClick);
            this.gMapControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gMapControl1_MouseClick);
            this.gMapControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gMapControl1_MouseDoubleClick);
            this.gMapControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gMapControl1_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(688, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // buttonRouteDot
            // 
            this.buttonRouteDot.Location = new System.Drawing.Point(693, 280);
            this.buttonRouteDot.Name = "buttonRouteDot";
            this.buttonRouteDot.Size = new System.Drawing.Size(75, 23);
            this.buttonRouteDot.TabIndex = 2;
            this.buttonRouteDot.Text = "buttonRouteDot";
            this.buttonRouteDot.UseVisualStyleBackColor = true;
            this.buttonRouteDot.Click += new System.EventHandler(this.buttonRouteDot_Click);
            // 
            // buttonRoutePol
            // 
            this.buttonRoutePol.Location = new System.Drawing.Point(691, 238);
            this.buttonRoutePol.Name = "buttonRoutePol";
            this.buttonRoutePol.Size = new System.Drawing.Size(75, 23);
            this.buttonRoutePol.TabIndex = 3;
            this.buttonRoutePol.Text = "buttonRoutePol";
            this.buttonRoutePol.UseVisualStyleBackColor = true;
            this.buttonRoutePol.Click += new System.EventHandler(this.buttonRoutePol_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(690, 264);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Путь в джсон";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(685, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Путь по полигону";
            // 
            // radioButtonDot
            // 
            this.radioButtonDot.AutoSize = true;
            this.radioButtonDot.Checked = true;
            this.radioButtonDot.Location = new System.Drawing.Point(691, 117);
            this.radioButtonDot.Name = "radioButtonDot";
            this.radioButtonDot.Size = new System.Drawing.Size(55, 17);
            this.radioButtonDot.TabIndex = 6;
            this.radioButtonDot.TabStop = true;
            this.radioButtonDot.Text = "Точки";
            this.radioButtonDot.UseVisualStyleBackColor = true;
            this.radioButtonDot.CheckedChanged += new System.EventHandler(this.radioButtonDot_CheckedChanged);
            // 
            // radioButtonPol
            // 
            this.radioButtonPol.AutoSize = true;
            this.radioButtonPol.Location = new System.Drawing.Point(691, 140);
            this.radioButtonPol.Name = "radioButtonPol";
            this.radioButtonPol.Size = new System.Drawing.Size(68, 17);
            this.radioButtonPol.TabIndex = 7;
            this.radioButtonPol.Text = "Полигон";
            this.radioButtonPol.UseVisualStyleBackColor = true;
            this.radioButtonPol.CheckedChanged += new System.EventHandler(this.radioButtonPol_CheckedChanged);
            // 
            // labelLat
            // 
            this.labelLat.AutoSize = true;
            this.labelLat.Location = new System.Drawing.Point(689, 52);
            this.labelLat.Name = "labelLat";
            this.labelLat.Size = new System.Drawing.Size(35, 13);
            this.labelLat.TabIndex = 8;
            this.labelLat.Text = "label4";
            // 
            // labelLng
            // 
            this.labelLng.AutoSize = true;
            this.labelLng.Location = new System.Drawing.Point(689, 84);
            this.labelLng.Name = "labelLng";
            this.labelLng.Size = new System.Drawing.Size(35, 13);
            this.labelLng.TabIndex = 9;
            this.labelLng.Text = "label5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(689, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Радиус дрона";
            // 
            // textBoxDronRadius
            // 
            this.textBoxDronRadius.Location = new System.Drawing.Point(686, 188);
            this.textBoxDronRadius.Name = "textBoxDronRadius";
            this.textBoxDronRadius.Size = new System.Drawing.Size(82, 20);
            this.textBoxDronRadius.TabIndex = 11;
            this.textBoxDronRadius.Text = "1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 384);
            this.Controls.Add(this.textBoxDronRadius);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelLng);
            this.Controls.Add(this.labelLat);
            this.Controls.Add(this.radioButtonPol);
            this.Controls.Add(this.radioButtonDot);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRoutePol);
            this.Controls.Add(this.buttonRouteDot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gMapControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonRouteDot;
        private System.Windows.Forms.Button buttonRoutePol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButtonDot;
        private System.Windows.Forms.RadioButton radioButtonPol;
        private System.Windows.Forms.Label labelLat;
        private System.Windows.Forms.Label labelLng;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDronRadius;
    }
}

