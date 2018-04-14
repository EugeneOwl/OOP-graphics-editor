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
using Newtonsoft.Json;

namespace Editor
{
    public partial class Form1 : Form
    {
        private int lastRadiobuttonY;
        private int serializingMode;
        private List<Figure> figures;
        private List<RadioButton> radioButtons;
        private List<Type> figureClasses;
        private List<Point> manualPoints;
        private Figure manualFigure;
        private const string xmlFilePath  = "C:\\Users\\npofa\\source\\repos\\OOP\\OOP-graphics-editor\\Editor\\Editor\\data.xml";
        private const string jsonFilePath = "C:\\Users\\npofa\\source\\repos\\OOP\\OOP-graphics-editor\\Editor\\Editor\\data.json";
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

        private void SerializeAllXML(string path = xmlFilePath)
        {
            ClearFile(path);
            XmlSerializer serializer = CreateSerializerXML();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, figures);
            }
        }

        private void DeserializeAllXML(string path = xmlFilePath)
        {
            if (!IsFileEmpty(path))
            {
                XmlSerializer serializer = CreateSerializerXML();
                using (StreamReader fs = new StreamReader(path))
                {
                    figures = (List<Figure>)serializer.Deserialize(fs);
                }
            }
        }

        private void SerializeAllJSON(string path = jsonFilePath)
        {
            var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            string json2 = JsonConvert.SerializeObject(figures, jset);
            File.WriteAllText(path, json2);
        }

        private void DeserializeAllJSON(string path = jsonFilePath)
        {
            string json = File.ReadAllText(path);
            var jset = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            figures = (List<Figure>)JsonConvert.DeserializeObject(json, jset);
        }

        private bool IsFileEmpty(string path)
        {
            return new FileInfo(path).Length == 0;
        }

        private XmlSerializer CreateSerializerXML()
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
                serializingMode = 1;
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
                OptionsForm optionsForm = new OptionsForm(this.figures.Count, serializingMode);
                optionsForm.ShowDialog();
                serializingMode = optionsForm.GetSerializingMode();
                if (optionsForm.isValidToDelete && optionsForm.GetDeletedNumber() != -1)
                {
                    figures.RemoveAt(optionsForm.GetDeletedNumber());
                }
                if (optionsForm.isValidToEdit && optionsForm.GetEditedNumber() != -1)
                {
                    EditFigureByNumber(optionsForm.GetEditedNumber());
                }
                if (optionsForm.isNeedToBeDeserialized)
                {
                    if (serializingMode == 1)
                        DeserializeAllXML();
                    else if (serializingMode == 2)
                        DeserializeAllJSON();
                    this.label1.Text = "Deserializing to " + serializingMode;
                }
                if (optionsForm.isNeedToBeSerialized)
                {
                    if (serializingMode == 1)
                        SerializeAllXML();
                    else if (serializingMode == 2)
                        SerializeAllJSON();
                    this.label1.Text = "Serializing to " + serializingMode;
                }
                this.Invalidate();
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
