using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    public partial class OptionsForm : Form
    {
        public bool isValidToDelete, isValidToEdit, isNeedToBeSerialized, isNeedToBeDeserialized;
        private int amount, serializingMode;

        public OptionsForm(int amount, int serializingMode)
        {
            this.amount = amount;
            this.serializingMode = serializingMode;
            isValidToDelete = isValidToEdit = isNeedToBeDeserialized = isNeedToBeSerialized = false;
            InitializeComponent();
            this.label1.Text = amount + " objects drawn.";
            if (serializingMode == 2)
                this.radioButton2.Checked = true;
            else
                this.radioButton1.Checked = true;
        }

        public int GetSerializingMode()
        {
            return this.serializingMode;
        }

        public int GetDeletedNumber()
        {
            return GetParameterFromTextBox(this.textBox1.Text);
        }

        public int GetEditedNumber()
        {
            return GetParameterFromTextBox(this.textBox2.Text);
        }

        public int GetParameterFromTextBox(string text)
        {
            int result;
            try
            {
                result = Int32.Parse(text);
            }
            catch (FormatException)
            {
                result = -1;
            }
            return result;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            serializingMode = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            serializingMode = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.isNeedToBeSerialized = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.isNeedToBeDeserialized = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ValidteForm();
        }

        private void ValidteForm()
        {
            isValidToEdit = false;
            isValidToDelete = false;
            if (GetEditedNumber() > -1 && GetEditedNumber() < amount)
            {
                isValidToEdit = true;
            }
            if (GetDeletedNumber() > -1 && GetDeletedNumber() < amount)
            {
                isValidToDelete = true;
            }
            if (isValidToEdit || isValidToDelete)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid data.");
            }
        }
    }
}
