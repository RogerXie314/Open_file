using Microsoft.VisualBasic.Devices;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Open_file
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Textbox_Enter();
            //Textbox_Leave();
            InitializeComponent();
        }
        //task状态值.
        bool task_status = true;


        //加载文件
        //检测指定文件是否存在
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        //检测指定目录是否存在
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        //获取指定目录的文件列表
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }
        //获取指定目录的子目录
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        //删除文件方法
        public static void DeleteFile(string file)
        {

            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        //删除指定目录方法
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        //创建文件方法
        public static void CreateNewFile(string file)
        {
            if (!IsExistFile(file))
            {
                try
                {
                    using (FileStream fs = File.Create(file))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes("this is some testing characters");
                        fs.Write(info, 0, info.Length);
                    }
                    //File.Create(file).Close();

                }
                catch { return; }
            }

        }
        //文件写入方法
        public static void WriteFile(string file)
        {
            if (File.Exists(file))
            {
                File.AppendAllText(file, "写入测试成功!");
            }
        }

        //文件运行方法
        public static void FileExecute(string file)
        {
            if (File.Exists(file))
            {
                Process.Start(file);
            }
        }
        //文件重命名方法
        public static void FileRename(string file)
        {
            if (IsExistFile(file))
            {
                String FileName = System.IO.Path.GetFileNameWithoutExtension(file);
                String FileExtension = Path.GetExtension(file);
                String newFileName = FileName + "renamed" + FileExtension;
                try
                {
                    Computer MyComputer = new Computer();
                    MyComputer.FileSystem.RenameFile(file, newFileName);
                }
                catch (Exception)
                { return; }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {

            OpenFileDialog filename = new OpenFileDialog(); //定义打开文件
            filename.InitialDirectory = Application.StartupPath; //初始路径,这里设置的是程序的起始位置，可自由设置
            //filename.Filter = "All files(*.*)|*.*|exe(*.exe)|*.exe";//设置打开类型,设置个*.*和*.txt就行了
            filename.Filter = "All files(*.*)|*.*";
            filename.FilterIndex = 2;                  //文件类型的显示顺序（上一行设为第二位）
            filename.RestoreDirectory = true; //对话框记忆之前打开的目录
            if (filename.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = filename.FileName.ToString();//获得完整路径在textBox1中显示

            }

        }

        //运行或打开文件
        private void button1_Click(object sender, EventArgs e)

        {
            String File_Execute = textBox1.Text;
            if (string.IsNullOrEmpty(File_Execute))
            {
                textBox3.Text = "不能为空！";
                return;

            }
            else
            {
                try
                { Process.Start(File_Execute); }
                catch (Exception)
                {
                    //WriteErrLog("error", file_error);
                    textBox3.Text = "文件运行错误，请重新选择！";
                    return;
                }
            }
        }
        //错误日志
        public static void WriteErrLog(string errTitle, Exception ex)
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\错误日志.txt";

            if (!File.Exists(path))
            {
                try { File.Create(path).Close(); }
                //File.Create(path).Close();
                catch (Exception)
                { return; }
            }

            StringBuilder strBuilderErrorMessage = new StringBuilder();

            strBuilderErrorMessage.Append("___________________________________________________________________\r\n");
            strBuilderErrorMessage.Append("日期:" + System.DateTime.Now.ToString() + "\r\n");
            strBuilderErrorMessage.Append("错误标题:" + errTitle + "\r\n");
            strBuilderErrorMessage.Append("错误信息:" + ex.Message + "\r\n");
            strBuilderErrorMessage.Append("错误内容:" + ex.StackTrace + "\r\n");
            strBuilderErrorMessage.Append("___________________________________________________________________\r\n");
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.Write(strBuilderErrorMessage);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            { return; }
        }



        //文件读取
        private void button2_Click_1(object sender, EventArgs e)
        {
            String pathSource = textBox1.Text;
            try
            {
                if (IsExistFile(pathSource))
                    using (var fileStream = new FileStream(pathSource, FileMode.Open, FileAccess.Read))
                    {
                        byte[] b = new byte[1024];
                        UTF8Encoding data = new UTF8Encoding(true);
                        if (fileStream.Read(b, 0, b.Length) > 0)
                        {
                            textBox3.Text = data.GetString(b);
                        }
                        else
                        {
                            textBox3.Text = "文件为空！";
                            return;
                        }
                    }
                else
                //输入为空提示
                {
                    textBox3.Text = "文件不存在！请输入文件路径！";
                    return;
                }
            }
            catch (Exception)
            {
                //WriteErrLog("error", file_error);
                //读取失败提示
                textBox3.Text = "文件读取错误，请重新选择！";
                return;
            }

        }

        //写文件
        private void button3_Click(object sender, EventArgs e)
        {
            String ImputCharacter = textBox2.Text; 
            String File_Write = textBox1.Text;    
            if (string.IsNullOrEmpty(ImputCharacter))
            {
                textBox3.Text = "路径不能为空！";
                return;
            }
            else
            {
                try
                {
                    File.AppendAllText(File_Write, ImputCharacter);
                    textBox3.Text = "写操作成功！";
                }
                catch (Exception)
                {
                    //WriteErrLog("error", file_error);
                    textBox3.Text = "写入失败，请重新选择！";
                    return;
                }
            }
        }

        //文件删除
        private void button4_Click(object sender, EventArgs e)
        {
            //删除指定文件
            String File_Del = textBox1.Text;
            if (string.IsNullOrEmpty(File_Del))
            {
                textBox3.Text = "路径不能为空！";
                return;
            }
            else
            {
                try
                {
                    File.Delete(File_Del);
                    textBox3.Text = "删除文件成功！";
                }
                catch (Exception)
                {
                    //WriteErrLog("error", file_error);
                    textBox3.Text = "删除失败，请重新选择！";
                    return;
                }
            }
        }
        //文件写入
        public static void AppendFile(string directoryPath, Int32 num)
        {
            if (IsExistDirectory(directoryPath))
            {
                //对目录中所有的文件写入
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                //foreach (string d in fileNames)
                {
                    try
                    {
                        Thread.Sleep(num);
                        WriteFile(fileNames[i]);
                    }
                    catch (Exception)
                    { continue; }
                }

            }
        }

        //文件删除压力设置        
        public static void ClearDirectory(string directoryPath, Int32 num)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件

                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                //foreach (string d in fileNames)
                {
                    try
                    {
                        Thread.Sleep(num);
                        DeleteFile(fileNames[i]);
                    }
                    catch (Exception)
                    { continue; }
                }
                //删除目录中所有的子目录
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    try
                    {
                        Thread.Sleep(num);
                        DeleteDirectory(directoryNames[i]);
                    }
                    catch (Exception)
                    { continue; }
                }

            }
        }
        //文件重命名压力设置
        public static void Stress_RenameFile(string directoryPath, Int32 num)
        {
            if (IsExistDirectory(directoryPath))
            {
                string[] filenames = GetFileNames(directoryPath);
                for (int i = 0; i < filenames.Length; i++)
                {
                    try
                    {
                        Thread.Sleep(num);
                        FileRename(filenames[i]);
                    }
                    catch
                    { continue; }
                }
            }
        }

        //文件运行压力设置
        public static void StartFile(string directoryPath, Int32 num)
        {
            if (IsExistDirectory(directoryPath))
            {
                //运行目录中所有的文件
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    try
                    {
                        Thread.Sleep(num);
                        FileExecute(fileNames[i]);
                    }
                    catch (Exception)
                    { continue; }
                }

            }
        }
        //判断输入阈值是否整型
        public static bool validateNum(string strNum)
        {
            return Regex.IsMatch(strNum, "^[0-9]*$");
        }

        //压力测试开始按钮
        private async void button1_Click_1(object sender, EventArgs e)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            task_status = true;
            //删除压力测试
            String File_del = textBox4.Text;
            //重命名压力测试
            String File_Rename = textBox5.Text;
            //修改压力测试
            String File_Edit = textBox6.Text;
            //运行压力测试
            String File_Start = textBox7.Text;
            //循环阈值

            String Num = textBox8.Text;
            

            if (string.IsNullOrEmpty(Num))
            {
                MessageBox.Show("循环阈值输入为空！");
                return;
            }
            else
            {
                if (validateNum(Num))
                {
                    int num_stress = Convert.ToInt32(Num);
                    while (true)
                    {
                        if (tokenSource.IsCancellationRequested)
                        {
                            MessageBox.Show("task cancelled!");
                            
                            button1.Enabled = true;
                            button2.Enabled = true; button3.Enabled = true; button4.Enabled = true; Button1_Execute.Enabled = true; button11.Enabled = true;
                            button1.Text = "开始"; button10.Enabled = true; CreateFile.Enabled = true;
                            break;
                        }
                        if (task_status == false)
                        {
                            tokenSource.Cancel();
                        }
                        try
                        {
                            Task t1 = new Task(() => { StartFile(File_Start, num_stress); }, cancellationToken);
                            t1.Start();
                            Task t2 = new Task(() => { AppendFile(File_Edit, num_stress); }, cancellationToken);
                            t2.Start();
                            Task t3 = new Task(() => { Stress_RenameFile(File_Rename, num_stress); }, cancellationToken);
                            t3.Start();
                            Task t4 = new Task(() => { ClearDirectory(File_del, num_stress); }, cancellationToken);
                            t4.Start();

                            button1.Enabled = false;
                            button2.Enabled = false; button3.Enabled = false; button4.Enabled = false; Button1_Execute.Enabled = false; button11.Enabled = false;
                            button1.Text = "执行中..."; button10.Enabled = false; CreateFile.Enabled = false;

                            await Task.Delay(num_stress);

                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                else
                { MessageBox.Show("请输入整数！"); }
            }


        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = dialog.SelectedPath;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox5.Text = dialog.SelectedPath;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = dialog.SelectedPath;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox7.Text = dialog.SelectedPath;
            }
        }

        //释放子程序并启动
        private void button9_Click(object sender, EventArgs e)
        {
            //获取C:\Windows\System32路径
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.System);

            //释放资源
            if (!File.Exists(path + @"\Filecopy.exe"))            
            {
                try
                {
                    byte[] Save = Properties.Resources.copyfile;                    
                    FileStream fsObj = new FileStream(path + @"\Filecopy.exe", FileMode.CreateNew);                    
                    fsObj.Write(Save, 0, Save.Length);
                    fsObj.Close();
                    //现在到系统目录中找一下释放的资源.exe吧
                }
                catch (Exception)
                { return; }
            }
            if (File.Exists(path + @"\Filecopy.exe"))
            //if (File.Exists(path + @"\wcry.exe"))
            {
                try
                {
                    Process.Start(path + @"\Filecopy.exe");
                    //Process.Start(path + @"\wcry.exe");
                    textBox3.Text = path + @"\Filecopy.exe";
                    //textBox3.Text = path + @"\wcry.exe";

                }
                catch (Exception)
                { return; }
            }


        }

        private void button10_Click(object sender, EventArgs e)
        {
            string pro_id = textBox1.Text;
            try
            {
                int id = Convert.ToInt32(pro_id);
                Process p = Process.GetProcessById(id);
                p.Kill();

            }
            catch (Exception)
            { return; }
        }

        //重命名文件
        private void button11_Click(object sender, EventArgs e)
        {

            String FileNamePath = textBox1.Text;
            if (string.IsNullOrEmpty(FileNamePath))
            {
                textBox3.Text = "路径不能为空！";
                return;
            }
            else
            {
                try
                {
                    FileRename(FileNamePath);
                    if (IsExistFile(FileNamePath))
                    { textBox3.Text = "重命名失败，请重试！"; }
                    else
                    { textBox3.Text = "重命名成功！"; }
                }
                catch (Exception)
                {
                    return;
                }
            }

        }


        //创建文件
        private void CreateFile_Click(object sender, EventArgs e)
        {
            String CreateFile_Num = textBox9.Text; //创建文件个数
            String Dir_file = textBox1.Text;  //文件路径
            bool isPath = Dir_file.EndsWith("\\"); //以斜杠结尾

            if (string.IsNullOrEmpty(CreateFile_Num) || string.IsNullOrEmpty(Dir_file))
            {
                textBox3.Text = "个数或目录输入为空！";
                return;
            }
            else
            {
                if (!isPath)
                {
                    textBox3.Text = "目录请以\\结尾！";
                    return;
                }
                else
                {
                    try
                    {
                        Task t6 = Task.Run(() =>
                        {
                            CreateFile.Enabled = false;
                            for (int i = 1; i <= Convert.ToInt32(CreateFile_Num); i++)
                            {
                                try
                                { CreateNewFile(Dir_file + "testfile" + i + ".txt"); }
                                catch { return; }
                            }
                        });

                        t6.ContinueWith(m => { textBox3.Text = "创建完成!"; CreateFile.Enabled = true; });
                        t6.Start();

                    }
                    catch (Exception) { return; }

                }
            }


        }
        private void button12_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath + "\\";
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //点击停止按钮task状态值置为false.
            task_status = false;
        }



    }

   

        
       
 }


