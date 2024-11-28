namespace Bios_Visualizer
{
    partial class ModificarDespachoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            cantCamionesNumericUpDown = new NumericUpDown();
            modificarButton = new Button();
            cancelarButton = new Button();
            label2 = new Label();
            inventarioPuertoTextBox = new TextBox();
            CapacidadRecepcionTextBox = new TextBox();
            label3 = new Label();
            inventarioPlantaTextBox = new TextBox();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)cantCamionesNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 114);
            label1.Name = "label1";
            label1.Size = new Size(127, 15);
            label1.TabIndex = 0;
            label1.Text = "Cantidad de Camiones";
            // 
            // cantCamionesNumericUpDown
            // 
            cantCamionesNumericUpDown.Location = new Point(164, 112);
            cantCamionesNumericUpDown.Name = "cantCamionesNumericUpDown";
            cantCamionesNumericUpDown.Size = new Size(100, 23);
            cantCamionesNumericUpDown.TabIndex = 1;
            // 
            // modificarButton
            // 
            modificarButton.Location = new Point(56, 157);
            modificarButton.Name = "modificarButton";
            modificarButton.Size = new Size(75, 23);
            modificarButton.TabIndex = 2;
            modificarButton.Text = "Modificar";
            modificarButton.UseVisualStyleBackColor = true;
            // 
            // cancelarButton
            // 
            cancelarButton.Location = new Point(137, 157);
            cancelarButton.Name = "cancelarButton";
            cancelarButton.Size = new Size(75, 23);
            cancelarButton.TabIndex = 3;
            cancelarButton.Text = "Cancelar";
            cancelarButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 25);
            label2.Name = "label2";
            label2.Size = new Size(119, 15);
            label2.TabIndex = 4;
            label2.Text = "Inventario Disponible";
            // 
            // inventarioPuertoTextBox
            // 
            inventarioPuertoTextBox.BackColor = SystemColors.ButtonHighlight;
            inventarioPuertoTextBox.Location = new Point(164, 22);
            inventarioPuertoTextBox.Name = "inventarioPuertoTextBox";
            inventarioPuertoTextBox.ReadOnly = true;
            inventarioPuertoTextBox.Size = new Size(100, 23);
            inventarioPuertoTextBox.TabIndex = 5;
            // 
            // CapacidadRecepcionTextBox
            // 
            CapacidadRecepcionTextBox.BackColor = SystemColors.ButtonHighlight;
            CapacidadRecepcionTextBox.Location = new Point(164, 51);
            CapacidadRecepcionTextBox.Name = "CapacidadRecepcionTextBox";
            CapacidadRecepcionTextBox.ReadOnly = true;
            CapacidadRecepcionTextBox.Size = new Size(100, 23);
            CapacidadRecepcionTextBox.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 54);
            label3.Name = "label3";
            label3.Size = new Size(134, 15);
            label3.TabIndex = 6;
            label3.Text = "Capacidad de recepción";
            // 
            // inventarioPlantaTextBox
            // 
            inventarioPlantaTextBox.BackColor = SystemColors.ButtonHighlight;
            inventarioPlantaTextBox.Location = new Point(164, 83);
            inventarioPlantaTextBox.Name = "inventarioPlantaTextBox";
            inventarioPlantaTextBox.ReadOnly = true;
            inventarioPlantaTextBox.Size = new Size(100, 23);
            inventarioPlantaTextBox.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 86);
            label4.Name = "label4";
            label4.Size = new Size(112, 15);
            label4.TabIndex = 8;
            label4.Text = "Inventario en Planta";
            // 
            // ModificarDespachoForm
            // 
            AcceptButton = modificarButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelarButton;
            ClientSize = new Size(295, 200);
            Controls.Add(inventarioPlantaTextBox);
            Controls.Add(label4);
            Controls.Add(CapacidadRecepcionTextBox);
            Controls.Add(label3);
            Controls.Add(inventarioPuertoTextBox);
            Controls.Add(label2);
            Controls.Add(cancelarButton);
            Controls.Add(modificarButton);
            Controls.Add(cantCamionesNumericUpDown);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ModificarDespachoForm";
            Text = "Modificar Despacho";
            ((System.ComponentModel.ISupportInitialize)cantCamionesNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NumericUpDown cantCamionesNumericUpDown;
        private Button modificarButton;
        private Button cancelarButton;
        private Label label2;
        private TextBox inventarioPuertoTextBox;
        private TextBox CapacidadRecepcionTextBox;
        private Label label3;
        private TextBox inventarioPlantaTextBox;
        private Label label4;
    }
}