namespace VPI
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpInputs = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.RadiusCmb = new System.Windows.Forms.ComboBox();
            this.GeometryCmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InstrumentGB = new System.Windows.Forms.GroupBox();
            this.InstrumentListBox = new System.Windows.Forms.ListBox();
            this.InstrumentCmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ReliefCmb = new System.Windows.Forms.ComboBox();
            this.tpSelection = new System.Windows.Forms.TabPage();
            this.tpGeometric = new System.Windows.Forms.TabPage();
            this.tpChrome = new System.Windows.Forms.TabPage();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.EquipmentCmb = new System.Windows.Forms.ComboBox();
            this.ForceCmb = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.PodachaTrackBar = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.PodachaLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.EksCetrisitetCmb = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.FrequencyCmb = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ShpyndelCmb = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tpInputs.SuspendLayout();
            this.InstrumentGB.SuspendLayout();
            this.tpSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PodachaTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpInputs);
            this.tabControl1.Controls.Add(this.tpSelection);
            this.tabControl1.Controls.Add(this.tpGeometric);
            this.tabControl1.Controls.Add(this.tpChrome);
            this.tabControl1.Controls.Add(this.tpResults);
            this.tabControl1.Location = new System.Drawing.Point(-1, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(597, 308);
            this.tabControl1.TabIndex = 0;
            // 
            // tpInputs
            // 
            this.tpInputs.Controls.Add(this.EquipmentCmb);
            this.tpInputs.Controls.Add(this.label5);
            this.tpInputs.Controls.Add(this.label4);
            this.tpInputs.Controls.Add(this.RadiusCmb);
            this.tpInputs.Controls.Add(this.GeometryCmb);
            this.tpInputs.Controls.Add(this.label3);
            this.tpInputs.Controls.Add(this.InstrumentGB);
            this.tpInputs.Controls.Add(this.label2);
            this.tpInputs.Controls.Add(this.label1);
            this.tpInputs.Controls.Add(this.ReliefCmb);
            this.tpInputs.Location = new System.Drawing.Point(4, 22);
            this.tpInputs.Name = "tpInputs";
            this.tpInputs.Padding = new System.Windows.Forms.Padding(3);
            this.tpInputs.Size = new System.Drawing.Size(589, 282);
            this.tpInputs.TabIndex = 0;
            this.tpInputs.Tag = "INPUTS";
            this.tpInputs.Text = "Вхідні дані";
            this.tpInputs.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(354, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Радіус деформуючого інструменту";
            // 
            // RadiusCmb
            // 
            this.RadiusCmb.FormattingEnabled = true;
            this.RadiusCmb.Location = new System.Drawing.Point(357, 105);
            this.RadiusCmb.Name = "RadiusCmb";
            this.RadiusCmb.Size = new System.Drawing.Size(121, 21);
            this.RadiusCmb.TabIndex = 7;
            // 
            // GeometryCmb
            // 
            this.GeometryCmb.FormattingEnabled = true;
            this.GeometryCmb.Location = new System.Drawing.Point(357, 42);
            this.GeometryCmb.Name = "GeometryCmb";
            this.GeometryCmb.Size = new System.Drawing.Size(118, 21);
            this.GeometryCmb.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(354, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Геометрія деформуючого інструменту";
            // 
            // InstrumentGB
            // 
            this.InstrumentGB.Controls.Add(this.InstrumentListBox);
            this.InstrumentGB.Controls.Add(this.InstrumentCmb);
            this.InstrumentGB.Location = new System.Drawing.Point(26, 83);
            this.InstrumentGB.Name = "InstrumentGB";
            this.InstrumentGB.Size = new System.Drawing.Size(254, 170);
            this.InstrumentGB.TabIndex = 4;
            this.InstrumentGB.TabStop = false;
            this.InstrumentGB.Text = "Матеріал деформуючого інструменту";
            // 
            // InstrumentListBox
            // 
            this.InstrumentListBox.FormattingEnabled = true;
            this.InstrumentListBox.HorizontalScrollbar = true;
            this.InstrumentListBox.Location = new System.Drawing.Point(7, 60);
            this.InstrumentListBox.Name = "InstrumentListBox";
            this.InstrumentListBox.Size = new System.Drawing.Size(226, 95);
            this.InstrumentListBox.TabIndex = 3;
            // 
            // InstrumentCmb
            // 
            this.InstrumentCmb.FormattingEnabled = true;
            this.InstrumentCmb.Location = new System.Drawing.Point(6, 22);
            this.InstrumentCmb.Name = "InstrumentCmb";
            this.InstrumentCmb.Size = new System.Drawing.Size(227, 21);
            this.InstrumentCmb.TabIndex = 2;
            this.InstrumentCmb.SelectedValueChanged += new System.EventHandler(this.InstrumentCmb_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Форма мікрорельєфу";
            // 
            // ReliefCmb
            // 
            this.ReliefCmb.FormattingEnabled = true;
            this.ReliefCmb.Location = new System.Drawing.Point(33, 42);
            this.ReliefCmb.Name = "ReliefCmb";
            this.ReliefCmb.Size = new System.Drawing.Size(121, 21);
            this.ReliefCmb.TabIndex = 0;
            // 
            // tpSelection
            // 
            this.tpSelection.Controls.Add(this.ShpyndelCmb);
            this.tpSelection.Controls.Add(this.label10);
            this.tpSelection.Controls.Add(this.FrequencyCmb);
            this.tpSelection.Controls.Add(this.label9);
            this.tpSelection.Controls.Add(this.EksCetrisitetCmb);
            this.tpSelection.Controls.Add(this.label8);
            this.tpSelection.Controls.Add(this.PodachaLabel);
            this.tpSelection.Controls.Add(this.label7);
            this.tpSelection.Controls.Add(this.PodachaTrackBar);
            this.tpSelection.Controls.Add(this.label6);
            this.tpSelection.Controls.Add(this.ForceCmb);
            this.tpSelection.Location = new System.Drawing.Point(4, 22);
            this.tpSelection.Name = "tpSelection";
            this.tpSelection.Padding = new System.Windows.Forms.Padding(3);
            this.tpSelection.Size = new System.Drawing.Size(589, 282);
            this.tpSelection.TabIndex = 1;
            this.tpSelection.Tag = "SELECTION";
            this.tpSelection.Text = "Вибір";
            this.tpSelection.UseVisualStyleBackColor = true;
            // 
            // tpGeometric
            // 
            this.tpGeometric.Location = new System.Drawing.Point(4, 22);
            this.tpGeometric.Name = "tpGeometric";
            this.tpGeometric.Size = new System.Drawing.Size(589, 282);
            this.tpGeometric.TabIndex = 2;
            this.tpGeometric.Tag = "GEOMETRIC";
            this.tpGeometric.Text = "Геометричні параметри";
            this.tpGeometric.UseVisualStyleBackColor = true;
            // 
            // tpChrome
            // 
            this.tpChrome.Location = new System.Drawing.Point(4, 22);
            this.tpChrome.Name = "tpChrome";
            this.tpChrome.Size = new System.Drawing.Size(589, 282);
            this.tpChrome.TabIndex = 3;
            this.tpChrome.Tag = "CHROME";
            this.tpChrome.Text = "Хромування";
            this.tpChrome.UseVisualStyleBackColor = true;
            // 
            // tpResults
            // 
            this.tpResults.Location = new System.Drawing.Point(4, 22);
            this.tpResults.Name = "tpResults";
            this.tpResults.Size = new System.Drawing.Size(589, 282);
            this.tpResults.TabIndex = 4;
            this.tpResults.Tag = "RESULTS";
            this.tpResults.Text = "Результати";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(357, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Обладнання";
            // 
            // EquipmentCmb
            // 
            this.EquipmentCmb.FormattingEnabled = true;
            this.EquipmentCmb.Location = new System.Drawing.Point(360, 162);
            this.EquipmentCmb.Name = "EquipmentCmb";
            this.EquipmentCmb.Size = new System.Drawing.Size(121, 21);
            this.EquipmentCmb.TabIndex = 10;
            // 
            // ForceCmb
            // 
            this.ForceCmb.FormattingEnabled = true;
            this.ForceCmb.Location = new System.Drawing.Point(21, 39);
            this.ForceCmb.Name = "ForceCmb";
            this.ForceCmb.Size = new System.Drawing.Size(121, 21);
            this.ForceCmb.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(289, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Зусилля вдавлювання деформуючого інструмента, Р, Н";
            // 
            // PodachaTrackBar
            // 
            this.PodachaTrackBar.Location = new System.Drawing.Point(24, 93);
            this.PodachaTrackBar.Maximum = 100;
            this.PodachaTrackBar.Name = "PodachaTrackBar";
            this.PodachaTrackBar.Size = new System.Drawing.Size(262, 45);
            this.PodachaTrackBar.TabIndex = 2;
            this.PodachaTrackBar.Scroll += new System.EventHandler(this.PodachaTrackBar_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(262, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Вибір подачі деформуючого інструмента, S, мм/об";
            // 
            // PodachaLabel
            // 
            this.PodachaLabel.AutoSize = true;
            this.PodachaLabel.Location = new System.Drawing.Point(261, 125);
            this.PodachaLabel.Name = "PodachaLabel";
            this.PodachaLabel.Size = new System.Drawing.Size(0, 13);
            this.PodachaLabel.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(181, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Ексцентриситет інтрумента, е, мм";
            // 
            // EksCetrisitetCmb
            // 
            this.EksCetrisitetCmb.FormattingEnabled = true;
            this.EksCetrisitetCmb.Location = new System.Drawing.Point(24, 177);
            this.EksCetrisitetCmb.Name = "EksCetrisitetCmb";
            this.EksCetrisitetCmb.Size = new System.Drawing.Size(121, 21);
            this.EksCetrisitetCmb.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(349, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(208, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Частота осциляцій, n подх.х., подв.х./хв";
            // 
            // FrequencyCmb
            // 
            this.FrequencyCmb.FormattingEnabled = true;
            this.FrequencyCmb.Location = new System.Drawing.Point(352, 38);
            this.FrequencyCmb.Name = "FrequencyCmb";
            this.FrequencyCmb.Size = new System.Drawing.Size(121, 21);
            this.FrequencyCmb.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(352, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(221, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Частота обертання шпинделя, n шп, об/хв";
            // 
            // ShpyndelCmb
            // 
            this.ShpyndelCmb.FormattingEnabled = true;
            this.ShpyndelCmb.Location = new System.Drawing.Point(352, 93);
            this.ShpyndelCmb.Name = "ShpyndelCmb";
            this.ShpyndelCmb.Size = new System.Drawing.Size(121, 21);
            this.ShpyndelCmb.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 303);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Назва програми";
            this.tabControl1.ResumeLayout(false);
            this.tpInputs.ResumeLayout(false);
            this.tpInputs.PerformLayout();
            this.InstrumentGB.ResumeLayout(false);
            this.tpSelection.ResumeLayout(false);
            this.tpSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PodachaTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpInputs;
        private System.Windows.Forms.TabPage tpSelection;
        private System.Windows.Forms.TabPage tpGeometric;
        private System.Windows.Forms.TabPage tpChrome;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.GroupBox InstrumentGB;
        private System.Windows.Forms.ListBox InstrumentListBox;
        private System.Windows.Forms.ComboBox InstrumentCmb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ReliefCmb;
        private System.Windows.Forms.ComboBox GeometryCmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox RadiusCmb;
        private System.Windows.Forms.ComboBox EquipmentCmb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ForceCmb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar PodachaTrackBar;
        private System.Windows.Forms.Label PodachaLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox EksCetrisitetCmb;
        private System.Windows.Forms.ComboBox FrequencyCmb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ShpyndelCmb;
        private System.Windows.Forms.Label label10;
    }
}

