using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VPI
{
    public partial class Form1 : Form
    {
        Constants Constants;

        public Form1()
        {   
            Constants = new Constants();
            InitializeComponent();
            ReliefCmb.Items.AddRange(Constants.Names[Topics.MicroRelief].ToArray());
            InstrumentCmb.Items.AddRange(Constants.Names[Topics.DestroyInstrument].ToArray());
            GeometryCmb.Items.AddRange(Constants.Names[Topics.InstrumentGeometry].ToArray());
            RadiusCmb.Items.AddRange(Constants.Names[Topics.Radius].ToArray());
            EquipmentCmb.Items.AddRange(Constants.Names[Topics.Equipment].ToArray());
            ForceCmb.Items.AddRange(Constants.Names[Topics.Force].ToArray());
            EksCetrisitetCmb.Items.AddRange(Constants.Names[Topics.EksCentr].ToArray());
            FrequencyCmb.Items.AddRange(Constants.Names[Topics.Frequency].ToArray());
            ShpyndelCmb.Items.AddRange(Constants.Names[Topics.Shpyndel].ToArray());
        }

        private void InstrumentCmb_SelectedValueChanged(object sender, EventArgs e)
        {
            InstrumentListBox.Text = "";
            string SelectedItem = (sender as System.Windows.Forms.ComboBox).SelectedItem.ToString();

            switch (SelectedItem) 
            {
                case Constants.pidshipnik:
                    InstrumentListBox.DataSource = Constants.PidshipnikStali;
                    break;
                case Constants.instr:
                    InstrumentListBox.DataSource = Constants.InstrumentStali;
                    break;
                case Constants.splavy:
                    InstrumentListBox.DataSource = Constants.TverdiSplavy;
                    break;
                case Constants.natureAlmazy:
                    InstrumentListBox.DataSource = Constants.NatureAlmaz;
                    break;
                case Constants.sinteticAlazy:
                    InstrumentListBox.DataSource = Constants.SynteticAlmaz;
                    break;
            }
        }

        private void PodachaTrackBar_Scroll(object sender, EventArgs e)
        {
            int value = (sender as System.Windows.Forms.TrackBar).Value;
            double output = ((12.5 - 0.08)/100.0) * (value) + 0.08;
            PodachaLabel.Text = output.ToString().Substring(0, 2 + output.ToString().IndexOf(","));
        }
    }
}
