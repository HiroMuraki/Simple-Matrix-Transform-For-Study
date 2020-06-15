using MatrixRowSwitch.MainCode;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static MatrixRowSwitch.MainCode.GeneralAction;
using static MatrixRowSwitch.MainCode.ResDict;
using static System.Convert;

namespace MatrixRowSwitch {
    /// <summary>
    /// 主界面的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        #region 属性定义
        private SimpleMatrix sMatrix;
        private string[,] inputMatrix;
        private Label[,] elementLabel;
        SwitchRecord switchRecord = new SwitchRecord();
        #endregion
        #region 生成矩阵
        public MainWindow() {
            InitializeComponent();
            inputMatrix = new string[3, 3] {
                        {"1","0","0" },
                        {"0","1","0" },
                        {"0","0","1" },
                    };
            sMatrix = new SimpleMatrix(inputMatrix);
            GenerateMatrix(sMatrix);
        }
        private void GenerateMatrixRuler(int rowLen, int columnLen) {
            this.panelRowCount.Children.Clear();
            this.panelColumnCount.Children.Clear();
            for (int i = 0; i < rowLen; i++) {
                Label index = new Label();
                index.Style = customStyles["lblMatrixRuler"] as Style;
                index.Content = i + 1;
                this.panelRowCount.Children.Add(index);
            }
            for (int i = 0; i < columnLen; i++) {
                Label index = new Label();
                index.Style = customStyles["lblMatrixRuler"] as Style;
                index.Content = i + 1;
                this.panelColumnCount.Children.Add(index);
            }
        }
        private void CopyNumber(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                string linkedNumber = (sender as Label).Content.ToString();
                Clipboard.SetText(linkedNumber);
            }
        }
        private void GenerateMatrix(SimpleMatrix sMatrix) {
            this.panelMatrix.Children.Clear();
            this.panelMatrix.Rows = sMatrix.RowLen;
            this.panelMatrix.Columns = sMatrix.ColumnLen;
            GenerateMatrixRuler(sMatrix.RowLen, sMatrix.ColumnLen);
            elementLabel = new Label[sMatrix.RowLen, sMatrix.ColumnLen];
            for (int row = 0; row < sMatrix.RowLen; row++) {
                for (int col = 0; col < sMatrix.ColumnLen; col++) {
                    elementLabel[row, col] = new Label();
                    elementLabel[row, col].MouseRightButtonDown += CopyNumber;
                    elementLabel[row, col].Style = customStyles["lblMatrixElement"] as Style;
                    this.panelMatrix.Children.Add(elementLabel[row, col]);
                }
            }
            SyncMatrixData(this.elementLabel, sMatrix);
            GetMatrixInforMation();
        }
        private void txtSourceMatrix_TextChanged(object sender, TextChangedEventArgs e) {
            inputMatrix = GetMatrix(this.txtSourceMatrix.Text);
            sMatrix = new SimpleMatrix(inputMatrix);
            GenerateMatrix(sMatrix);
        }
        #endregion
        #region 矩阵变换
        private void SyncMatrixData(Label[,] elementLabel, SimpleMatrix sMatrix) {
            for (int row = 0; row < sMatrix.RowLen; row++) {
                for (int column = 0; column < sMatrix.ColumnLen; column++) {
                    elementLabel[row, column].Content = sMatrix.Matrix[row, column];
                }
            }
        }
        private void TransMatrix() {
            switchRecord = new SwitchRecord();
            sMatrix = new SimpleMatrix(inputMatrix);
            try {
                foreach (string switcher in this.txtSwitchInput.Text.Split('\n')) {
                    string[] command = GetCommand(switcher);
                    switch (command[0]) {
                        case "TS":
                            sMatrix.Trans();
                            break;
                        case "BS":
                            sMatrix.BsSwtich();
                            break;
                        case "AS":
                            sMatrix.AsSwitch();
                            break;
                        case "CR":
                            sMatrix.CrSwitch(ToInt32(command[1]), ToInt32(command[2]));
                            break;
                        case "MR":
                            sMatrix.MrSwtich(ToInt32(command[1]), command[2]);
                            break;
                        case "DR":
                            sMatrix.DrSwtich(ToInt32(command[1]), ToInt32(command[2]), command[3]);
                            break;
                    }
                    switchRecord.AddRecord(command);
                }
                SyncMatrixData(this.elementLabel, this.sMatrix);
                this.txtSwitchRecord.Text = "";
                foreach (string command in switchRecord.Records) {
                    this.txtSwitchRecord.Text += ($"{command}\n");
                }
            }
            catch { }
        }
        private void GetMatrixInforMation() {
            this.txtMatrixInformation.Text = "";
            List<string> avaliableInformation = new List<string>();
            avaliableInformation.Add($"基础解系 :\n{GetMatrixBaseSolution()}");
            avaliableInformation.Add($"对应行列式值 : {GetDeterminantValue()}");
            foreach (string information in avaliableInformation) {
                this.txtMatrixInformation.Text += $"{information}\n\n";
            }
        }
        private void txtSwitchInput_TextChanged(object sender, TextChangedEventArgs e) {
            TransMatrix();
            GetMatrixInforMation();
        }
        #endregion
        #region 其他操作与计算
        private void ReverseMatrixAB(object sender, RoutedEventArgs e) {
            string temp = this.txtSourceMatrix.Text;
            this.txtSourceMatrix.Text = this.txtSourceMatrixB.Text;
            this.txtSourceMatrixB.Text = temp;
            this.lblCurrentMatrix.Content = $"变化命令(矩阵A)";
        }
        private string GetMatrixBaseSolution() {
            string[] baseSolutions = new string[1] { "未知" };
            if (this.sMatrix.IsLineMinimalistMatrix()) {
                try {
                    baseSolutions = sMatrix.GetBaseSolutionList();
                }
                catch { }
            }
            return string.Join("\n", baseSolutions);
        }
        private string GetDeterminantValue() {
            string temp = "";
            try {
                temp = sMatrix.GetDeterminantValue();
            }
            catch { }
            return temp;
        }
        private void btnCaculate_Click(object sender, RoutedEventArgs e) {
            string[,] matrixA = GetMatrix(this.txtSourceMatrix.Text);
            SimpleMatrix sMatrixA = new SimpleMatrix(matrixA);
            string[,] matrixB = GetMatrix(this.txtSourceMatrixB.Text);
            SimpleMatrix sMatrixB = new SimpleMatrix(matrixB);
            string currentMatrix = "矩阵A";
            switch ((sender as Button).Tag as string) {
                case "btnUseMatrixA":
                    currentMatrix = "矩阵A";
                    sMatrix = new SimpleMatrix(GetMatrix(this.txtSourceMatrix.Text));
                    break;
                case "btnUseMatrixB":
                    currentMatrix = "矩阵B";
                    sMatrix = new SimpleMatrix(GetMatrix(this.txtSourceMatrixB.Text));
                    break;
                case "btnGetSum":
                    currentMatrix = "A + B";
                    sMatrix = SimpleMatrix.Add(sMatrixA, sMatrixB);
                    break;
                case "btnGetMinus":
                    currentMatrix = "A - B";
                    sMatrix = SimpleMatrix.Minus(sMatrixA, sMatrixB);
                    break;
                case "btnGetMultiply":
                    currentMatrix = "A x B";
                    sMatrix = SimpleMatrix.NormalProduct(sMatrixA, sMatrixB);
                    break;
                case "btnGetHadamardProduct":
                    currentMatrix = "A * B";
                    sMatrix = SimpleMatrix.HadamardProduct(sMatrixA, sMatrixB);
                    break;
                case "btnGetKroneckerProduct":
                    currentMatrix = "A \u2A02 B";
                    sMatrix = SimpleMatrix.KroneckerProduct(sMatrixA, sMatrixB);
                    break;
            }
            this.lblCurrentMatrix.Content = $"变化命令({currentMatrix})";
            inputMatrix = sMatrix.Matrix;
            GenerateMatrix(sMatrix);
        }
        private void ShowHelp(object sender, RoutedEventArgs e) {
            HelpWindow hw = new HelpWindow();
            hw.Owner = this;
            hw.Show();
        }
        #endregion
        #region 窗口操作
        private void Window_Maximize(object sender, RoutedEventArgs e) {
            if (this.WindowState == WindowState.Normal) {
                this.WindowState = WindowState.Maximized;
            } else {
                this.WindowState = WindowState.Normal;
            }
        }
        private void Window_DoubleClick(object sender, MouseButtonEventArgs e) {
            this.DragMove();
            if (e.ClickCount == 2) {
                if (this.WindowState == WindowState.Normal) {
                    this.WindowState = WindowState.Maximized;
                } else {
                    this.WindowState = WindowState.Normal;
                }
            }
        }
        private void Window_Minimize(object sender, RoutedEventArgs e) {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_Close(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        #endregion
    }
}
