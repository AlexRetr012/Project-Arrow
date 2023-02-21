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
            string path = @"C:\Program Files\Arrow";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            InitializeComponent();
        }

        DateTime dateTime = new DateTime();
        Dictionary<int, string> dLabels = new Dictionary<int, string>();
        Dictionary<int, string> dFileSys = new Dictionary<int, string>();
        Dictionary<int, long> dCapacity = new Dictionary<int, long>();
        Dictionary<int, long> dFreeSpace = new Dictionary<int, long>();
        Dictionary<int, long> dLockedSpace = new Dictionary<int, long>();
        Dictionary<int, string> dCurrentStatus = new Dictionary<int, string>();
        private static IEnumerable<string> SafeEnumerateFiles(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var dirs = new Stack<string>();
            dirs.Push(path);

            while (dirs.Count > 0)
            {
                string currentDirPath = dirs.Pop();
                if (searchOption == SearchOption.AllDirectories)
                {
                    try
                    {
                        string[] subDirs = Directory.GetDirectories(currentDirPath);
                        foreach (string subDirPath in subDirs)
                        {
                            dirs.Push(subDirPath);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        continue;
                    }
                }

                string[] files = null;
                try
                {
                    files = Directory.GetFiles(currentDirPath, searchPattern);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                foreach (string filePath in files)
                {
                    yield return filePath;
                }
            }
        }

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
            long dirSize = SafeEnumerateFiles(@"C:\Program Files\Arrow", "*.*", SearchOption.AllDirectories).Sum(n => new FileInfo(n).Length);
            chart1.Series["Series1"].Points.Clear();
            int k = 0;
            for(int i=0;i<dLabels.Count;i++)
            {
                if(diskSelectionComboBox.Text == dLabels[i])
                {
                    k = i;
                    currentStatusLabelActive.Text = dCurrentStatus[k];
                    capacityLabelActive.Text = (dCapacity[k] / (1024 * 1024 * 1024)).ToString() + " ГБ";
                    busyLabelActive.Text = (dLockedSpace[k] / (1024 * 1024 * 1024)).ToString() + " ГБ";
                    freeLabelActive.Text = (dFreeSpace[k] / (1024 * 1024 * 1024)).ToString() + " ГБ";
                    chart1.Series["Series1"].Points.AddXY("З", dLockedSpace[k] / (1024 * 1024));
                    chart1.Series["Series1"].Points.AddXY("C", dFreeSpace[k] / (1024 * 1024));
                    if (diskSelectionComboBox.Text == @"C:\")
                    {
                        chart1.Series["Series1"].Points.AddXY("O", dirSize / (1024 * 1024));
                        realFreeSpaceLabelActive.Text = ((dFreeSpace[k] / (1024 * 1024 * 1024)) - (dirSize / (1024 * 1024 * 1024))).ToString() + " ГБ";
                        arrowSpaceBusyLabelActive.Text = (dirSize / (1024 * 1024 * 1024)).ToString() + " ГБ";
                    }
                    else
                    {
                        chart1.Series["Series1"].Points.AddXY("O", 0);
                        realFreeSpaceLabelActive.Text = (dFreeSpace[k] / (1024 * 1024 * 1024)).ToString() + " ГБ";
                        arrowSpaceBusyLabelActive.Text = (0).ToString() + " ГБ";
                    }
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
        private void add_files_list_Click(object sender, EventArgs e)
        {
            /*Файлы Исключений кнопка Добавить Файл*/
            try
            {
                openFileDialog1.Filter = "All Files|*.*";
                string fpath = "";
                using (openFileDialog1)
                {
                    openFileDialog1.InitialDirectory = "c:\\";
                    openFileDialog1.Filter = "All files (*.*)|*.*";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.RestoreDirectory = true;
                    openFileDialog1.FileName = "";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        fpath = openFileDialog1.FileName;
                    }
                    else if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
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
                if (twice == false)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[iterList].SetValues(fpath);
                    iterList++;
                }
                else if (twice == true) { }
            }
            catch
            {
                MessageBox.Show("Ошибка!(#104) Не удалось добавить файл в список! ");
            }
               
        }

        private void add_folder_list_Click(object sender, EventArgs e)
        {
            try
            {
                string fpath = "";
                using (folderBrowserDialog1)
                {
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        fpath = folderBrowserDialog1.SelectedPath;
                    }
                    else if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
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
            catch
            {
                MessageBox.Show("Ошибка!(#104) Не удалось добавить папку в список! ");
            }
            
        }

        private void delete_from_list_Click(object sender, EventArgs e)
        {
            try
            {
                int delet = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows.RemoveAt(delet);
                iterList--;
            }
            catch
            {
                MessageBox.Show("Ошибка!(#103) Не удалось удалить путь/папку из списка! ");
            }
            
        }

        private void save_list_Click(object sender, EventArgs e)
        {
            try
            {
                var someList = new List<string>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
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
            catch
            {
                MessageBox.Show("Ошибка!(#101) Не удалось сохранить файл! ");
            }
            
        }

        private void load_list_Click(object sender, EventArgs e)
        {
            try
            {
                string fpath = "";
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
                iterList = dataGridView1.Rows.Count;
            }
            catch
            {
                MessageBox.Show("Ошибка!(#101) Не удалось загрузить файл! ");
            }

        }

        private void apply_list_Click(object sender, EventArgs e)
        {
            try
            {
                string path = @"C:\Program Files\Arrow\exclude_lists";
                string time;
                string timefix = "";
                time = DateTime.Now.ToString();
                for (int i = 0; i < time.Length; i++)
                {
                    if (time[i] == '.' || time[i] == ' ' || time[i] == ':')
                    {
                        timefix = timefix + '_';
                    }
                    else
                    {
                        timefix = timefix + time[i];
                    }
                }
                string fpath = @"C:\Program Files\Arrow\exclude_lists\" + timefix + "_applylist.txt";
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                bool fileExist = File.Exists(fpath);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                if (fileExist)
                {
                    File.Create(fpath);
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }
                var someList = new List<string>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    someList.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
                File.WriteAllLines(fpath, someList);
                MessageBox.Show("Успешно применено");
            }
            catch
            {
                MessageBox.Show("Ошибка!(#102) Не удалось применить файлы! ");
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void TrayOff(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //Оповещение при малом объеме свободного пространства
            if (checkBox1.Checked == true)
            {

            }
            else if (checkBox1.Checked == false)
            {

            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //Включение безопасной среды при любом открытом файле
            if (checkBox1.Checked == true)
            {

            }
            else if (checkBox1.Checked == false)
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Включение автозагрузки
            if (checkBox1.Checked == true)
            {
                
            }
            else if (checkBox1.Checked == false)
            {
                
            }
        }
    }
}
