using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MatrixRowSwitch {
    /// <summary>
    /// HelpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HelpWindow : Window {
        private string[] usableCommands = new string[6] {
            "MR [X] [n]      :  将第X行的元素乘以n",
            "CR [X] [Y]      :  交换第X行和第Y行的元素",
            "DR [X] [Y] [n]  :  将第Y行的元素乘以n后加到第X行",
            "TS              :  转置矩阵",
            "BS              :  伴随矩阵",
            "AS              :  逆矩阵"
        };
        private string[] tips = new string[2] {
            "命令不区分大小写，例如MR，mr，Mr，mR等效为MR",
            "支持有理数运算(即n可以为有理数)，但是有理数应\n使用分数表示，且分子分母必须为整数"
        };
        public HelpWindow() {
            InitializeComponent();
            LoadInformation();
        }
        private void LoadInformation() {
            foreach (string usableCommand in usableCommands) {
                this.lblUsableCommand.Content += $"{usableCommand}\n";
            }
            int index = 1;
            foreach (string tip in tips) {
                this.lblTips.Content += $"{index}: {tip}\n";
                index += 1;
            }
        }

        private void Window_Close(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Window_Minimize(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Move(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }
    }
}
