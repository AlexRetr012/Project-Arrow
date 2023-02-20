using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;
using System.Reflection.Emit;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<int, string> dLabels = new Dictionary<int, string>();
        Dictionary<int, string> dFileSys = new Dictionary<int, string>();
        Dictionary<int, long> dCapacity = new Dictionary<int, long>();
        Dictionary<int, long> dFreeSpace = new Dictionary<int, long>();
        Dictionary<int, long> dLockedSpace = new Dictionary<int, long>();
        Dictionary<int, string> dCurrentStatus = new Dictionary<int, string>();

        private void Form1_Load(object sender, EventArgs e)
        {
            //Taking out info about  disk by preloading it
            long lockedSpace = 0;
            dLabels = new Dictionary<int, string>();
            dFileSys = new Dictionary<int, string>();
            dCapacity = new Dictionary<int, long>();
            dFreeSpace = new Dictionary<int, long>();
            dLockedSpace = new Dictionary<int, long>();
            dCurrentStatus = new Dictionary<int, string>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            int i = 0;
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Fixed)
                {
                    diskSelectionComboBox.Items.Add(d.Name);
                    dLabels.Add(i, d.Name);
                    dFileSys.Add(i, d.DriveFormat);
                    dCapacity.Add(i, d.TotalSize);
                    dFreeSpace.Add(i, d.TotalFreeSpace);
                    lockedSpace = d.TotalSize - d.TotalFreeSpace;
                    dLockedSpace.Add(i, lockedSpace);
                    dCurrentStatus.Add(i, "Защита отключена");
                    i++;
                }
            }

            //Передача словарей в кнопки
        }
        


        private void sysytem_status_Click(object sender, EventArgs e)
        {
            //Button of System-Status
            //Show-hide list start
            panel9.Show();
            panel8.Hide();
            panel7.Hide();
            adm_panel_p.Hide();
            info_panel.Hide();
            //Show-hide list end
            navigation_label_active.Text = "Статусе системы";
        }

        private void comboChanged(object sender, EventArgs e) // Смена диска
        {
            chart1.Series["Series1"].Points.Clear();
            int k = 0;
            for(int i=0;i<dLabels.Count;i++)
            {
                if(diskSelectionComboBox.Text == dLabels[i])
                {
                    k = i;
                    currentStatusLabelActive.Text = dCurrentStatus[k];
                    capacityLabelActive.Text = (dCapacity[k] / (1024 * 1024)).ToString() + " МБ";
                    busyLabelActive.Text = (dLockedSpace[k] / (1024 * 1024)).ToString() + " МБ";
                    freeLabelActive.Text = (dFreeSpace[k] / (1024 * 1024)).ToString() + " МБ";
                    realFreeSpaceLabelActive.Text = (dFreeSpace[k] / (1024 * 1024)).ToString() + " МБ";
                    arrowSpaceBusyLabelActive.Text = "0 МБ";
                    chart1.Series["Series1"].Points.AddXY("З", dLockedSpace[k] / (1024 * 1024));
                    chart1.Series["Series1"].Points.AddXY("C", dFreeSpace[k] / (1024 * 1024));
                }
            }
        }



        bool msclicked = false;
        private void mode_selection_Click(object sender, EventArgs e)
        {
            //Button of Mode-Selection
            //Show-hide list start
            panel9.Hide();
            panel8.Show();
            panel7.Hide();
            adm_panel_p.Hide();
            info_panel.Hide();
            int j = 0;
            //Show-hide list end
            navigation_label_active.Text = "Выборе режима";


            for (int i = 0; i < dLabels.Count; i++)
            {
                if(msclicked == false)
                {
                    dataGridView2.Rows.Add();
                }
                dataGridView2.Rows[i].Cells[j].Value = dLabels[i];
                dataGridView2.Rows[i].Cells[j + 1].Value = dCurrentStatus[i];
                dataGridView2.Rows[i].Cells[j + 2].Value = dFileSys[i];
                dataGridView2.Rows[i].Cells[j + 3].Value = dCapacity[i] / (1024 * 1024) + " МБ";
                dataGridView2.Rows[i].Cells[j + 4].Value = dFreeSpace[i] / (1024 * 1024) + " МБ";

            }
            msclicked = true;
            
        }

        private void exception_list_Click(object sender, EventArgs e)
        {
            //Button of Exception List
            //Show-hide list start
            panel9.Hide();
            panel8.Hide();
            panel7.Show();
            adm_panel_p.Hide();
            info_panel.Hide();
            //Show-hide list end
            navigation_label_active.Text = "Файлах Исключений";
        }

        private void admin_panel_Click(object sender, EventArgs e)
        {
            //Button of Admin Panel
            //Show-hide list start
            panel9.Hide();
            panel8.Hide();
            panel7.Hide();
            adm_panel_p.Show();
            info_panel.Hide();
            //Show-hide list end
            navigation_label_active.Text = "Администрировании";
        }

        private void about_Click(object sender, EventArgs e)
        {
            //Button of Info
            //Show-hide list start
            panel9.Hide();
            panel8.Hide();
            panel7.Hide();
            adm_panel_p.Hide();
            info_panel.Show();
            //Show-hide list end
            navigation_label_active.Text = "Информации";
        }
        int iterList = 0;
        bool two = false;
        private void add_files_list_Click(object sender, EventArgs e)
        {
            /*Файлы Исключений кнопка Добавить Файл*/
            openFileDialog1.Filter = "All Files|*.*";
            String fpath = "";
            using (openFileDialog1)
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fpath = openFileDialog1.FileName;
                }
            }
            bool twice = false;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (fpath == dataGridView1.Rows[i].Cells[0].Value.ToString())
                {
                    twice = true;
                    string message = "Этот файл уже был добавлен!";
                    string caption = "Ошибка!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;
                    result = MessageBox.Show(message, caption, buttons);
                }
            }
            if(twice==false) 
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[iterList].SetValues(fpath);
                iterList++;
            }
            else if(twice== true){}   
        }

        private void add_folder_list_Click(object sender, EventArgs e)
        {
            String fpath = "";
            using (folderBrowserDialog1)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    fpath = folderBrowserDialog1.SelectedPath;
                }
            }
            bool twice = false;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (fpath == dataGridView1.Rows[i].Cells[0].Value.ToString())
                {
                    twice = true;
                    string message = "Эта папка уже был добавлена!";
                    string caption = "Ошибка!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;
                    result = MessageBox.Show(message, caption, buttons);
                }
            }
            if (twice == false)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[iterList].SetValues(fpath);
                iterList++;
            }
            else if (twice == true) { }
        }

        private void delete_from_list_Click(object sender, EventArgs e)
        {
            int delet = dataGridView1.SelectedCells[0].RowIndex;
            dataGridView1.Rows.RemoveAt(delet);
        }

        private void save_list_Click(object sender, EventArgs e)
        {
            var someList = new List<string>();
            for (int i=0;i<dataGridView1.Rows.Count;i++) 
            {
                someList.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            File.WriteAllLines(filename, someList);
            MessageBox.Show("Файл сохранен");
        }

        private void load_list_Click(object sender, EventArgs e)
        {
            String fpath = "";
            using (openFileDialog1)
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Text files(*.txt)|*.txt";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fpath = openFileDialog1.FileName;
                }
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows.Clear();
            }
            var list = File.ReadLines(fpath).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                dataGridView1.Rows.Add();
                string test = list[i].ToString();
                dataGridView1.Rows[i].Cells[0].Value = test;
            }

        }

        private void apply_list_Click(object sender, EventArgs e)
        {
            string path = @"D:\Arrow";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            var someList = new List<string>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                someList.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            File.WriteAllLines(path, someList);
            MessageBox.Show("Успешно применено");
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.ShowInTaskbar = false;
                    notifyIcon1.Visible = true;
                }
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                    notifyIcon1.Visible = false;
                }
            }
        }

        private void TrayOff(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //Оповещение при малом объеме свободного пространства
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //Включение безопасной среды при любом открытом файле
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Включение автозагрузки
        }
    }
}
