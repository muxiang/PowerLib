using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

using PowerLib.WPF.Controls;
using PowerLib.WPF.Tools;

namespace PowerLib.WPF.Samples
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public double Score { get; set; }
            public bool Active { get; set; }
            public Gender Gender { get; set; }
        }

        public enum Gender
        {
            Male,
            Female,
            Unknown
        }

        private readonly Random _rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 100; i++)
                MISubMenuOverflow.Items.Add(new MenuItem { Header = $"第{i + 1}项" });
            
            // DataGrid 模拟数据
            SampleDataGrid.ItemsSource = new List<Person>
            {
                new Person { Id=1, Name="宇智波赵四", Gender = Gender.Male, Department="营销部", Score=95.5, Active=true },
                new Person { Id=2, Name="旋涡刘能", Gender = Gender.Male, Department="研发部", Score=82.0, Active=true },
                new Person { Id=3, Name="春野大脚", Gender = Gender.Female, Department="研发部", Score=91.2, Active=false },
                new Person { Id=4, Name="日向玉田", Gender = Gender.Unknown, Department="营销部", Score=88.7, Active=true },
                new Person { Id=5, Name="旗木广坤", Gender = Gender.Male, Department="研发部", Score=76.3, Active=true },
                new Person { Id=6, Name="千手大拿", Gender = Gender.Unknown, Department="市场部", Score=93.1, Active=false },
                new Person { Id=7, Name="张三", Gender = Gender.Female, Department="市场部", Score=85.4, Active=true },
                new Person { Id=8, Name="李四", Gender = Gender.Female, Department="营销部", Score=79.8, Active=true },
            };
        }

        private void CmbSkin_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            ComboBoxItem cbi = (ComboBoxItem)CmbSkin.SelectedItem;
            string skinFileName = cbi.Tag.ToString();

            SkinHelper.ApplySkin(skinFileName);
        }

        private void BtnOpenLoadingLayer_Click(object sender, RoutedEventArgs e)
        {
            LoadingLayer.ShowAutoClose(this, owner =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(50);
                    int progress = i + 1;
                    LoadingLayer.UpdateProgress(owner, progress, "当前进度");
                }
            }, opacity: 0.5, showProgressBar: true);
        }

        private void BtnStartFileTransfer_Click(object sender, RoutedEventArgs e)
        {
            FileTransferProgressBar1.ClearData();

            // 模拟文件总大小2GB
            const long totalSizeInBytes = 2L * 1024 * 1024 * 1024;
            // 每次传输随机值下限：200KB
            const int bytesLow = 200 * 1024;
            // 每次传输随机值上限：800KB
            const int bytesHigh = 800 * 1024;

            FileTransferProgressBar1.TotalSizeInBytes = totalSizeInBytes;

            ThreadPool.QueueUserWorkItem(o =>
            {
                FileTransferProgressBar ftpb = (FileTransferProgressBar)o;

                while (Dispatcher.Invoke(() => ftpb.Percentage < 100))
                {
                    int bytes = _rnd.Next(bytesLow, bytesHigh);

                    Dispatcher.Invoke(() =>
                    {
                        ftpb.AddValue(bytes);
                    });

                    Thread.Sleep(10);
                }
            }, FileTransferProgressBar1);
        }
    }
}
