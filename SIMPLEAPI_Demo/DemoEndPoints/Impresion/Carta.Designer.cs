﻿
namespace DemoEndPoints.Impresion
{
    partial class Carta
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dp_fechaResolucion = new System.Windows.Forms.DateTimePicker();
            this.txt_numResolucion = new System.Windows.Forms.NumericUpDown();
            this.txt_unidadSii = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_archivo = new System.Windows.Forms.Button();
            this.txt_archivo = new System.Windows.Forms.TextBox();
            this.lbl_dte = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_logo = new System.Windows.Forms.Button();
            this.txt_logo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_enviar = new System.Windows.Forms.Button();
            this.txt_result = new System.Windows.Forms.TextBox();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_numResolucion)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dp_fechaResolucion);
            this.groupBox1.Controls.Add(this.txt_numResolucion);
            this.groupBox1.Controls.Add(this.txt_unidadSii);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(443, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(345, 182);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos";
            // 
            // dp_fechaResolucion
            // 
            this.dp_fechaResolucion.Location = new System.Drawing.Point(139, 80);
            this.dp_fechaResolucion.Name = "dp_fechaResolucion";
            this.dp_fechaResolucion.Size = new System.Drawing.Size(200, 20);
            this.dp_fechaResolucion.TabIndex = 6;
            // 
            // txt_numResolucion
            // 
            this.txt_numResolucion.Location = new System.Drawing.Point(139, 28);
            this.txt_numResolucion.Name = "txt_numResolucion";
            this.txt_numResolucion.Size = new System.Drawing.Size(120, 20);
            this.txt_numResolucion.TabIndex = 5;
            // 
            // txt_unidadSii
            // 
            this.txt_unidadSii.Location = new System.Drawing.Point(139, 54);
            this.txt_unidadSii.Name = "txt_unidadSii";
            this.txt_unidadSii.Size = new System.Drawing.Size(100, 20);
            this.txt_unidadSii.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Fecha Resolución :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Unidad SII :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Numero Resolución :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_archivo);
            this.groupBox2.Controls.Add(this.txt_archivo);
            this.groupBox2.Controls.Add(this.lbl_dte);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(404, 91);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Archivo";
            // 
            // btn_archivo
            // 
            this.btn_archivo.Location = new System.Drawing.Point(252, 23);
            this.btn_archivo.Name = "btn_archivo";
            this.btn_archivo.Size = new System.Drawing.Size(75, 23);
            this.btn_archivo.TabIndex = 2;
            this.btn_archivo.Text = "Cargar";
            this.btn_archivo.UseVisualStyleBackColor = true;
            this.btn_archivo.Click += new System.EventHandler(this.btn_archivo_Click);
            // 
            // txt_archivo
            // 
            this.txt_archivo.Location = new System.Drawing.Point(146, 25);
            this.txt_archivo.Name = "txt_archivo";
            this.txt_archivo.Size = new System.Drawing.Size(100, 20);
            this.txt_archivo.TabIndex = 1;
            // 
            // lbl_dte
            // 
            this.lbl_dte.AutoSize = true;
            this.lbl_dte.Location = new System.Drawing.Point(14, 28);
            this.lbl_dte.Name = "lbl_dte";
            this.lbl_dte.Size = new System.Drawing.Size(115, 13);
            this.lbl_dte.TabIndex = 0;
            this.lbl_dte.Text = "Selecciona el archivo :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_logo);
            this.groupBox3.Controls.Add(this.txt_logo);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(12, 109);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(404, 85);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Logo";
            // 
            // btn_logo
            // 
            this.btn_logo.Location = new System.Drawing.Point(252, 33);
            this.btn_logo.Name = "btn_logo";
            this.btn_logo.Size = new System.Drawing.Size(75, 23);
            this.btn_logo.TabIndex = 5;
            this.btn_logo.Text = "Cargar";
            this.btn_logo.UseVisualStyleBackColor = true;
            this.btn_logo.Click += new System.EventHandler(this.btn_logo_Click);
            // 
            // txt_logo
            // 
            this.txt_logo.Location = new System.Drawing.Point(146, 36);
            this.txt_logo.Name = "txt_logo";
            this.txt_logo.Size = new System.Drawing.Size(100, 20);
            this.txt_logo.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Selecciona el Logo :";
            // 
            // btn_enviar
            // 
            this.btn_enviar.Location = new System.Drawing.Point(391, 212);
            this.btn_enviar.Name = "btn_enviar";
            this.btn_enviar.Size = new System.Drawing.Size(75, 23);
            this.btn_enviar.TabIndex = 7;
            this.btn_enviar.Text = "Enviar";
            this.btn_enviar.UseVisualStyleBackColor = true;
            this.btn_enviar.Click += new System.EventHandler(this.btn_enviar_Click);
            // 
            // txt_result
            // 
            this.txt_result.Location = new System.Drawing.Point(29, 275);
            this.txt_result.Multiline = true;
            this.txt_result.Name = "txt_result";
            this.txt_result.Size = new System.Drawing.Size(759, 211);
            this.txt_result.TabIndex = 8;
            // 
            // Carta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 526);
            this.Controls.Add(this.txt_result);
            this.Controls.Add(this.btn_enviar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Carta";
            this.Load += new System.EventHandler(this.Carta_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_numResolucion)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dp_fechaResolucion;
        private System.Windows.Forms.NumericUpDown txt_numResolucion;
        private System.Windows.Forms.TextBox txt_unidadSii;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_archivo;
        private System.Windows.Forms.TextBox txt_archivo;
        private System.Windows.Forms.Label lbl_dte;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_logo;
        private System.Windows.Forms.TextBox txt_logo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_enviar;
        private System.Windows.Forms.TextBox txt_result;
        private System.Drawing.Printing.PrintDocument printDocument1;
    }
}