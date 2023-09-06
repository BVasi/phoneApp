using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace lab2_MTP
{
    public partial class Form1 : Form
    {
        private List<Person> listOfPersons_ = new List<Person>();
        private TreeNode[] parentNodes_ = new TreeNode[4];

        public Form1()
        {
            InitializeComponent();
            initializeTreeView();
            getDataIn();
            makeChangesInTreeView();
        }

        private void getDataIn()
        {
            string path = Application.StartupPath;
            StreamReader streamReader = new StreamReader(path + "\\agenda.txt");
            string name = "";
            DateTime birthDate = DateTime.MinValue;
            string phoneNumber = "";
            string address = "";
            Category_ category = 0;
            while (!streamReader.EndOfStream)
            {
                name = streamReader.ReadLine().Replace("Nume: ", "");
                birthDate = Convert.ToDateTime(streamReader.ReadLine().Replace("Data nasterii: ", ""));
                phoneNumber = streamReader.ReadLine().Replace("Numar de telefon: ", "");
                address = streamReader.ReadLine().Replace("Adresa: ", "");
                category = (Category_)Enum.Parse(typeof(Category_), streamReader.ReadLine().Replace("Categorie: ", ""));
                listOfPersons_.Add(new Person(name, birthDate, phoneNumber, address, category));
            }
            streamReader.Close();
        }

        private void initializeTreeView()
        {
            categoryComboBox.DataSource = Enum.GetValues(typeof(Category_));

            TreeNode friends = new TreeNode();
            friends.Text = Category_.Prieteni.ToString();
            friends.Name = Category_.Prieteni.ToString();

            parentNodes_[0] = friends;

            TreeNode colleagues = new TreeNode();
            colleagues.Text = Category_.Colegi.ToString();
            colleagues.Name = Category_.Colegi.ToString();

            parentNodes_[1] = colleagues;

            TreeNode relatives = new TreeNode();
            relatives.Text = Category_.Rude.ToString();
            relatives.Name = Category_.Rude.ToString();

            parentNodes_[2] = relatives;

            TreeNode others = new TreeNode();
            others.Text = Category_.Diversi.ToString();
            others.Name = Category_.Diversi.ToString();

            parentNodes_[3] = others;

            treeView1.Nodes.Add(parentNodes_[0]);
            treeView1.Nodes.Add(parentNodes_[1]);
            treeView1.Nodes.Add(parentNodes_[2]);
            treeView1.Nodes.Add(parentNodes_[3]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            DateTime birthDate = dateTimePicker1.Value;
            string phoneNumber = phoneTextBox.Text;
            string address = addressTextBox.Text;
            Category_ category = (Category_)categoryComboBox.SelectedItem;
            listOfPersons_.Add(new Person(name, birthDate, phoneNumber, address, category));
            TreeNode childNode = new TreeNode();
            childNode.Text = name;
            parentNodes_[(int)category].Nodes.Add(childNode);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (parentNodes_.Contains(e.Node))
            {
                return;
            }
            string name = e.Node.Text;
            Person person = listOfPersons_.Find(x => x.Nume.ToLower() == name.ToLower());
            if (person != null)
            {
                propertyGrid1.SelectedObject = person;
            }
            else
            {
                propertyGrid1.SelectedObject = null;
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string searchedPersonName = searchedNameTextBox.Text;
            if (string.IsNullOrEmpty(searchedPersonName))
            {
                return;
            }
            Person person = listOfPersons_.Find(x => x.Nume.ToLower() == searchedPersonName.ToLower());
            if (person != null)
            {
                propertyGrid1.SelectedObject = person;
            }
            else
            {
                propertyGrid1.SelectedObject = null;
                MessageBox.Show($"{searchedPersonName} nu exista in agenda!");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath;
            StreamWriter streamWriter = new StreamWriter(path + "\\agenda.txt", false);
            foreach (Person person in listOfPersons_)
            {
                streamWriter.WriteLine($"Nume: {person.Nume}");
                streamWriter.WriteLine($"Data nasterii: {person.DataNasterii}");
                streamWriter.WriteLine($"Numar de telefon: {person.NumarDeTelefon}");
                streamWriter.WriteLine($"Adresa: {person.Adresa}");
                streamWriter.WriteLine($"Categorie: {person.Categorie}" );
            }
            streamWriter.Close();
        }

        private void makeChangesInTreeView()
        {
            treeView1.Nodes.Clear();
            initializeTreeView();
            foreach (Person person in listOfPersons_)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = person.Nume;
                parentNodes_[(int)person.Categorie].Nodes.Add(childNode);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            Person selectedPerson = (Person)propertyGrid1.SelectedObject;
            if (selectedPerson == null)
            {
                return;
            }
            if (MessageBox.Show($"Doriti sa stergeti persoana {selectedPerson.Nume}?", "Intrebare",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                listOfPersons_.Remove(selectedPerson);
                propertyGrid1.SelectedObject = null;
                makeChangesInTreeView();
            }
        }
    }
}