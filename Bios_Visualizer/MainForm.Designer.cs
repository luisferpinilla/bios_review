namespace Bios_Visualizer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            fileTextBox = new TextBox();
            OpenFile_Button = new Button();
            cargarProblemaButton = new Button();
            tabControl1 = new TabControl();
            tabPage3 = new TabPage();
            alarmasListView = new ListView();
            nivelColumnHeader = new ColumnHeader();
            descriptionColumnHeader = new ColumnHeader();
            notasColumnHeader = new ColumnHeader();
            imageList1 = new ImageList(components);
            tabPage2 = new TabPage();
            splitContainer2 = new SplitContainer();
            importacionesListView = new ListView();
            detalleImportacionListView = new ListView();
            tabPage1 = new TabPage();
            splitContainer1 = new SplitContainer();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            PlantaListView = new ListView();
            PlantaIngredientesListView = new ListView();
            fletesTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            directoTextBox = new TextBox();
            label3 = new Label();
            vencimientosTextBox = new TextBox();
            label4 = new Label();
            bodegajeTextBox = new TextBox();
            flowLayoutPanel3 = new FlowLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            tableLayoutPanel5 = new TableLayoutPanel();
            TotalTextBox = new TextBox();
            label6 = new Label();
            exportarReportesButton = new Button();
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            tabControl1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            flowLayoutPanel3.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // fileTextBox
            // 
            fileTextBox.Enabled = false;
            fileTextBox.Location = new Point(97, 3);
            fileTextBox.Margin = new Padding(4, 3, 4, 3);
            fileTextBox.Name = "fileTextBox";
            fileTextBox.Size = new Size(344, 23);
            fileTextBox.TabIndex = 0;
            // 
            // OpenFile_Button
            // 
            OpenFile_Button.Location = new Point(4, 3);
            OpenFile_Button.Margin = new Padding(4, 3, 4, 3);
            OpenFile_Button.Name = "OpenFile_Button";
            OpenFile_Button.Size = new Size(85, 23);
            OpenFile_Button.TabIndex = 1;
            OpenFile_Button.Text = "Seleccionar Archivo";
            OpenFile_Button.UseVisualStyleBackColor = true;
            OpenFile_Button.Click += OpenFile_Button_Click_1;
            // 
            // cargarProblemaButton
            // 
            cargarProblemaButton.Enabled = false;
            cargarProblemaButton.Location = new Point(449, 3);
            cargarProblemaButton.Margin = new Padding(4, 3, 4, 3);
            cargarProblemaButton.Name = "cargarProblemaButton";
            cargarProblemaButton.Size = new Size(50, 23);
            cargarProblemaButton.TabIndex = 2;
            cargarProblemaButton.Text = "Cargar";
            cargarProblemaButton.UseVisualStyleBackColor = true;
            cargarProblemaButton.Click += cargarProblemaButton_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.ItemSize = new Size(107, 25);
            tabControl1.Location = new Point(12, 74);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1258, 624);
            tabControl1.TabIndex = 3;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(alarmasListView);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Margin = new Padding(4, 3, 4, 3);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(4, 3, 4, 3);
            tabPage3.Size = new Size(1250, 591);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "General";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // alarmasListView
            // 
            alarmasListView.Columns.AddRange(new ColumnHeader[] { nivelColumnHeader, descriptionColumnHeader, notasColumnHeader });
            alarmasListView.Dock = DockStyle.Fill;
            alarmasListView.LargeImageList = imageList1;
            alarmasListView.Location = new Point(4, 3);
            alarmasListView.Margin = new Padding(4, 3, 4, 3);
            alarmasListView.Name = "alarmasListView";
            alarmasListView.Size = new Size(1242, 585);
            alarmasListView.SmallImageList = imageList1;
            alarmasListView.TabIndex = 0;
            alarmasListView.UseCompatibleStateImageBehavior = false;
            alarmasListView.View = View.Details;
            alarmasListView.SelectedIndexChanged += alarmasListView_SelectedIndexChanged;
            // 
            // nivelColumnHeader
            // 
            nivelColumnHeader.Text = "Nivel";
            nivelColumnHeader.Width = 120;
            // 
            // descriptionColumnHeader
            // 
            descriptionColumnHeader.Text = "Description";
            descriptionColumnHeader.Width = 200;
            // 
            // notasColumnHeader
            // 
            notasColumnHeader.Text = "Notas";
            notasColumnHeader.Width = 1100;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "HotItem.png");
            imageList1.Images.SetKeyName(1, "information_blue.png");
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(splitContainer2);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4, 3, 4, 3);
            tabPage2.Size = new Size(1250, 591);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Importaciones";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(4, 3);
            splitContainer2.Margin = new Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(importacionesListView);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(detalleImportacionListView);
            splitContainer2.Size = new Size(1242, 585);
            splitContainer2.SplitterDistance = 256;
            splitContainer2.TabIndex = 1;
            // 
            // importacionesListView
            // 
            importacionesListView.Dock = DockStyle.Fill;
            importacionesListView.FullRowSelect = true;
            importacionesListView.GridLines = true;
            importacionesListView.Location = new Point(0, 0);
            importacionesListView.Margin = new Padding(4, 3, 4, 3);
            importacionesListView.Name = "importacionesListView";
            importacionesListView.Size = new Size(1242, 256);
            importacionesListView.TabIndex = 0;
            importacionesListView.UseCompatibleStateImageBehavior = false;
            importacionesListView.View = View.Details;
            importacionesListView.ItemSelectionChanged += importacionesListView_ItemSelectionChanged;
            // 
            // detalleImportacionListView
            // 
            detalleImportacionListView.Dock = DockStyle.Fill;
            detalleImportacionListView.FullRowSelect = true;
            detalleImportacionListView.GridLines = true;
            detalleImportacionListView.Location = new Point(0, 0);
            detalleImportacionListView.Margin = new Padding(4, 3, 4, 3);
            detalleImportacionListView.MultiSelect = false;
            detalleImportacionListView.Name = "detalleImportacionListView";
            detalleImportacionListView.Size = new Size(1242, 325);
            detalleImportacionListView.TabIndex = 0;
            detalleImportacionListView.UseCompatibleStateImageBehavior = false;
            detalleImportacionListView.View = View.Details;
            detalleImportacionListView.MouseDoubleClick += detalleImportacionListView_MouseDoubleClick;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(splitContainer1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(1250, 591);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Plantas";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(4, 3);
            splitContainer1.Margin = new Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(chart1);
            splitContainer1.Panel1.Controls.Add(PlantaListView);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(PlantaIngredientesListView);
            splitContainer1.Size = new Size(1242, 585);
            splitContainer1.SplitterDistance = 204;
            splitContainer1.TabIndex = 2;
            // 
            // chart1
            // 
            chart1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Alignment = StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(566, 3);
            chart1.Margin = new Padding(4, 3, 4, 3);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            chart1.Series.Add(series1);
            chart1.Size = new Size(674, 201);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            // 
            // PlantaListView
            // 
            PlantaListView.Dock = DockStyle.Left;
            PlantaListView.FullRowSelect = true;
            PlantaListView.GridLines = true;
            PlantaListView.Location = new Point(0, 0);
            PlantaListView.Margin = new Padding(4, 3, 4, 3);
            PlantaListView.Name = "PlantaListView";
            PlantaListView.Size = new Size(560, 204);
            PlantaListView.TabIndex = 0;
            PlantaListView.UseCompatibleStateImageBehavior = false;
            PlantaListView.View = View.Details;
            PlantaListView.ItemSelectionChanged += PlantaListView_ItemSelectionChanged;
            // 
            // PlantaIngredientesListView
            // 
            PlantaIngredientesListView.Dock = DockStyle.Fill;
            PlantaIngredientesListView.FullRowSelect = true;
            PlantaIngredientesListView.GridLines = true;
            PlantaIngredientesListView.Location = new Point(0, 0);
            PlantaIngredientesListView.Margin = new Padding(4, 3, 4, 3);
            PlantaIngredientesListView.Name = "PlantaIngredientesListView";
            PlantaIngredientesListView.Size = new Size(1242, 377);
            PlantaIngredientesListView.TabIndex = 1;
            PlantaIngredientesListView.UseCompatibleStateImageBehavior = false;
            PlantaIngredientesListView.View = View.Details;
            // 
            // fletesTextBox
            // 
            fletesTextBox.Dock = DockStyle.Fill;
            fletesTextBox.Location = new Point(4, 30);
            fletesTextBox.Margin = new Padding(4, 3, 4, 3);
            fletesTextBox.Name = "fletesTextBox";
            fletesTextBox.ReadOnly = true;
            fletesTextBox.Size = new Size(119, 23);
            fletesTextBox.TabIndex = 4;
            fletesTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(4, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(119, 27);
            label1.TabIndex = 5;
            label1.Text = "Total Fletes";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(4, 0);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(119, 27);
            label2.TabIndex = 7;
            label2.Text = "Total Directo";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // directoTextBox
            // 
            directoTextBox.Dock = DockStyle.Fill;
            directoTextBox.Location = new Point(4, 30);
            directoTextBox.Margin = new Padding(4, 3, 4, 3);
            directoTextBox.Name = "directoTextBox";
            directoTextBox.ReadOnly = true;
            directoTextBox.Size = new Size(119, 23);
            directoTextBox.TabIndex = 6;
            directoTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(4, 0);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(119, 27);
            label3.TabIndex = 11;
            label3.Text = "Total Vencimientos";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // vencimientosTextBox
            // 
            vencimientosTextBox.Dock = DockStyle.Fill;
            vencimientosTextBox.Location = new Point(4, 30);
            vencimientosTextBox.Margin = new Padding(4, 3, 4, 3);
            vencimientosTextBox.Name = "vencimientosTextBox";
            vencimientosTextBox.ReadOnly = true;
            vencimientosTextBox.Size = new Size(119, 23);
            vencimientosTextBox.TabIndex = 10;
            vencimientosTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(4, 0);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(119, 27);
            label4.TabIndex = 9;
            label4.Text = "Total Bodegaje";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // bodegajeTextBox
            // 
            bodegajeTextBox.Dock = DockStyle.Fill;
            bodegajeTextBox.Location = new Point(4, 30);
            bodegajeTextBox.Margin = new Padding(4, 3, 4, 3);
            bodegajeTextBox.Name = "bodegajeTextBox";
            bodegajeTextBox.ReadOnly = true;
            bodegajeTextBox.Size = new Size(119, 23);
            bodegajeTextBox.TabIndex = 8;
            bodegajeTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Controls.Add(OpenFile_Button);
            flowLayoutPanel3.Controls.Add(fileTextBox);
            flowLayoutPanel3.Controls.Add(cargarProblemaButton);
            flowLayoutPanel3.Controls.Add(tableLayoutPanel1);
            flowLayoutPanel3.Controls.Add(tableLayoutPanel2);
            flowLayoutPanel3.Controls.Add(tableLayoutPanel3);
            flowLayoutPanel3.Controls.Add(tableLayoutPanel4);
            flowLayoutPanel3.Controls.Add(tableLayoutPanel5);
            flowLayoutPanel3.Controls.Add(exportarReportesButton);
            flowLayoutPanel3.Location = new Point(12, 12);
            flowLayoutPanel3.Margin = new Padding(4, 3, 4, 3);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(1457, 63);
            flowLayoutPanel3.TabIndex = 14;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(fletesTextBox, 0, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Location = new Point(507, 3);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(127, 55);
            tableLayoutPanel1.TabIndex = 15;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(directoTextBox, 0, 1);
            tableLayoutPanel2.Controls.Add(label2, 0, 0);
            tableLayoutPanel2.Location = new Point(642, 3);
            tableLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(127, 55);
            tableLayoutPanel2.TabIndex = 16;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(bodegajeTextBox, 0, 1);
            tableLayoutPanel3.Controls.Add(label4, 0, 0);
            tableLayoutPanel3.Location = new Point(777, 3);
            tableLayoutPanel3.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(127, 55);
            tableLayoutPanel3.TabIndex = 17;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(vencimientosTextBox, 0, 1);
            tableLayoutPanel4.Controls.Add(label3, 0, 0);
            tableLayoutPanel4.Location = new Point(912, 3);
            tableLayoutPanel4.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(127, 55);
            tableLayoutPanel4.TabIndex = 18;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(TotalTextBox, 0, 1);
            tableLayoutPanel5.Controls.Add(label6, 0, 0);
            tableLayoutPanel5.Location = new Point(1047, 3);
            tableLayoutPanel5.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(127, 55);
            tableLayoutPanel5.TabIndex = 19;
            // 
            // TotalTextBox
            // 
            TotalTextBox.Dock = DockStyle.Fill;
            TotalTextBox.Location = new Point(4, 30);
            TotalTextBox.Margin = new Padding(4, 3, 4, 3);
            TotalTextBox.Name = "TotalTextBox";
            TotalTextBox.ReadOnly = true;
            TotalTextBox.Size = new Size(119, 23);
            TotalTextBox.TabIndex = 10;
            TotalTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(4, 0);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(119, 27);
            label6.TabIndex = 11;
            label6.Text = "Total";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // exportarReportesButton
            // 
            exportarReportesButton.Enabled = false;
            exportarReportesButton.Location = new Point(1182, 3);
            exportarReportesButton.Margin = new Padding(4, 3, 4, 3);
            exportarReportesButton.Name = "exportarReportesButton";
            exportarReportesButton.Size = new Size(65, 53);
            exportarReportesButton.TabIndex = 15;
            exportarReportesButton.Text = "Exportar CSV";
            exportarReportesButton.UseVisualStyleBackColor = true;
            exportarReportesButton.Click += exportarReportesButton_Click;
            // 
            // backgroundWorker
            // 
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1283, 710);
            Controls.Add(flowLayoutPanel3);
            Controls.Add(tabControl1);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(1296, 573);
            Name = "MainForm";
            Text = "Bios Solver";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox fileTextBox;
        private Button OpenFile_Button;
        private Button cargarProblemaButton;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ListView PlantaListView;
        private ListView PlantaIngredientesListView;
        private SplitContainer splitContainer1;
        private ListView importacionesListView;
        private SplitContainer splitContainer2;
        private ListView detalleImportacionListView;
        private TextBox fletesTextBox;
        private Label label1;
        private Label label2;
        private TextBox directoTextBox;
        private Label label3;
        private TextBox vencimientosTextBox;
        private Label label4;
        private TextBox bodegajeTextBox;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label label6;
        private TextBox TotalTextBox;
        private Button exportarReportesButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private TabPage tabPage3;
        private ListView alarmasListView;
        private ColumnHeader nivelColumnHeader;
        private ColumnHeader descriptionColumnHeader;
        private ColumnHeader notasColumnHeader;
        private ImageList imageList1;
    }
}
