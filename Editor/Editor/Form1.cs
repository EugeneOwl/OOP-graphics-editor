using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace Editor
{
    public partial class Form1 : Form
    {
        private int lastRadiobuttonY;
        private List<Figure> figures;
        private List<RadioButton> radioButtons;
        private List<Type> figureClasses;
        private List<Point> manualPoints;
        private Figure manualFigure;
        private string xmlFilePath = "C:\\Users\\npofa\\source\\repos\\Editor\\Editor\\data.xml";
        private string assemblyName = "FigurePlugins";
        
        private bool CheckSignature()
        {
            Assembly loaded = Assembly.Load("FigurePlugins");

            byte[] evidenceKey = loaded.GetName().GetPublicKey();

            if (evidenceKey != null)
            {
                byte[] internalKey = Assembly.GetExecutingAssembly().GetName().GetPublicKey();
                if (evidenceKey.SequenceEqual(internalKey))
                    return true;
            }
            return false;
        }

        private void SerializeAll()
        {
            ClearFile(this.xmlFilePath);
            XmlSerializer serializer = CreateSerializer();
            using (FileStream fs = new FileStream(this.xmlFilePath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, figures);
            }
        }

        private void DeserializeAll()
        {
            if (!IsFileEmpty(xmlFilePath))
            {
                XmlSerializer serializer = CreateSerializer();
                using (StreamReader fs = new StreamReader(xmlFilePath))
                {
                    figures = (List<Figure>)serializer.Deserialize(fs);
                }
            }
            this.Invalidate();
        }

        private bool IsFileEmpty(string path)
        {
            return new FileInfo(path).Length == 0;
        }

        private XmlSerializer CreateSerializer()
        {
            Type[] types = new Type[figureClasses.Count];
            for (int typeNumber = 0; typeNumber < figureClasses.Count; typeNumber++)
            {
                types[typeNumber] = figureClasses[typeNumber];
            }
            XmlSerializer serializer = new XmlSerializer(
                typeof(List<Figure>),
                types
            );
            return serializer;
        }

        private void ClearFile(string path)
        {
            File.WriteAllText(path, string.Empty);
        }

        public Form1()
        {
            InitializeComponent();
            if (CheckSignature())
            {
                lastRadiobuttonY = 25;
                figures = new List<Figure>();
                radioButtons = new List<RadioButton>();
                figureClasses = new List<Type>();
                manualPoints = new List<Point>();
                GetListOfFigureClasses();
                FillRadioButtonList();
            }
        }

        private void GetListOfFigureClasses()
        {
            Assembly a = Assembly.Load(assemblyName);
            Type[] types = a.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(Figure)))
                {
                    figureClasses.Add(type);
                }
            }
        }

        private void FillRadioButtonList()
        {
            foreach (Type type in figureClasses)
            {
                AddRadioButtonToList(type);
            }
        }

        private void AddRadioButtonToList(Type type)
        {
            RadioButton radioButton = new RadioButton();
            radioButton.Location = new Point(25, lastRadiobuttonY);
            radioButton.Text = type.ToString();
            radioButton.MouseUp += (sender, e) =>
            {
                manualFigure  = (Figure)Activator.CreateInstance(type);
            };
            radioButtons.Add(radioButton);
            lastRadiobuttonY += 25;
        }

        private void DrawRadioButtons()
        {
            if (radioButtons != null)
                foreach (RadioButton radioButton in radioButtons)
                    this.Controls.Add(radioButton);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawRadioButtons();
            if (figures != null)
            {
                Drawer drawer = new Drawer();

                foreach (Figure figure in figures)
                {
                    drawer.DrawFigureByPath(e, figure.GetPath());
                }
            }
        }

        private int[] GetManualDrawingParametersArray()
        {
            int[] parameters = new int[6];
            parameters[0] = manualPoints[0].X;
            parameters[1] = manualPoints[0].Y;
            parameters[2] = manualPoints[1].X;
            parameters[3] = manualPoints[1].Y;
            parameters[4] = manualPoints[2].X;
            parameters[5] = manualPoints[2].Y;
            return parameters;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (manualFigure != null)
            {
                manualPoints.Add(new Point(e.X, e.Y));
                if (manualPoints.Count == 3)
                {
                    Object[] packedParameters = new Object[1];
                    packedParameters[0] = GetManualDrawingParametersArray();

                    MethodInfo SetManualParameters = manualFigure.GetType().GetMethod("SetManualParameters");
                    SetManualParameters.Invoke(manualFigure, packedParameters);
                    if (!figures.Contains(manualFigure))
                    {
                        figures.Add(manualFigure);
                    }
                    manualPoints.Clear();
                    this.Invalidate();
                }
            }
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (figures != null)
            {
                OptionsForm optionsForm = new OptionsForm(this.figures.Count);
                optionsForm.ShowDialog();
                if (optionsForm.isValidToDelete && optionsForm.GetDeletedNumber() != -1)
                {
                    figures.RemoveAt(optionsForm.GetDeletedNumber());
                    this.Invalidate();
                }
                if (optionsForm.isValidToEdit && optionsForm.GetEditedNumber() != -1)
                {
                    EditFigureByNumber(optionsForm.GetEditedNumber());
                }
                if (optionsForm.isNeedToBeDeserialized)
                {
                    DeserializeAll();
                }
                if (optionsForm.isNeedToBeSerialized)
                {
                    SerializeAll();
                }
            }
        }

        private void EditFigureByNumber(int number)
        {
            if (number > -1 && number < figures.Count)
            {
                manualPoints.Clear();
                manualFigure = figures[number];
                this.Invalidate();
            }
        }
    }
}
