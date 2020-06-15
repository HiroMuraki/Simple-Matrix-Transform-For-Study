using System;
using System.Collections.Generic;

namespace MatrixRowSwitch.MainCode {
    using element = String;
    abstract class BaseMatrix {
        public element[,] Matrix { get; protected set; }
        public int RowLen { get { return Matrix.GetLength(0); } }
        public int ColumnLen { get { return Matrix.GetLength(1); } }
        public BaseMatrix(element[,] matrix) {
            this.Matrix = new element[matrix.GetLength(0), matrix.GetLength(1)];
            for (int row = 0; row < matrix.GetLength(0); row++) {
                for (int column = 0; column < matrix.GetLength(1); column++) {
                    this.Matrix[row, column] = matrix[row, column];
                }
            }
        }
        public override string ToString() {
            string temp = "";
            for (int row = 0; row < RowLen; row++) {
                for (int column = 0; column < ColumnLen; column++) {
                    temp += $"{this.Matrix[row, column],5}\n";
                }
            }
            return temp;
        }
    }
    class SimpleMatrix : BaseMatrix {
        public SimpleMatrix(element[,] matrix) : base(matrix) {

        }
        //矩阵变化
        public void Trans() {
            element[,] tempMatrix = new element[ColumnLen, RowLen];
            for (int row = 0; row < ColumnLen; row++) {
                for (int col = 0; col < RowLen; col++) {
                    tempMatrix[row, col] = this.Matrix[col, row];
                }
            }
            this.Matrix = tempMatrix;
        }
        public void CrSwitch(int rowA, int rowB) {
            rowA -= 1;
            rowB -= 1;
            for (int column = 0; column < ColumnLen; column++) {
                element temp = this.Matrix[rowA, column];
                this.Matrix[rowA, column] = this.Matrix[rowB, column];
                this.Matrix[rowB, column] = temp;
            }
        }
        public void MrSwtich(int row, string n) {
            row -= 1;
            for (int column = 0; column < ColumnLen; column++) {
                this.Matrix[row, column] = RationNumber.Mulpitly(this.Matrix[row, column], n);
            }
        }
        public void DrSwtich(int rowA, int rowB, string n) {
            rowA -= 1;
            rowB -= 1;
            for (int column = 0; column < ColumnLen; column++) {
                string temp = RationNumber.Mulpitly(this.Matrix[rowB, column], n);
                this.Matrix[rowA, column] = RationNumber.Add(this.Matrix[rowA, column], temp);
            }
        }
        public void AsSwitch() {
            string detA = this.GetDeterminantValue();
            if (detA != "0") {
                this.BsSwtich();
                detA = RationNumber.Reciprocal(detA);
                this.Matrix = Nmatrix(this, detA).Matrix;
            }
        }
        public void BsSwtich() {
            this.Matrix = Determinant.GetAdjointMatrix(this.Matrix);
            this.Trans();
        }
        private int GetSt(element[,] matrix, int row) {
            for (int i = 0; i < ColumnLen; i++) {
                if (matrix[row, i] == "0") {
                    continue;
                } else if (matrix[row, i] == "1") {
                    return i;
                } else {
                    return -1;
                }
            }
            return -2;
        }
        //属性判断
        public static bool IsSameType(SimpleMatrix matrixA, SimpleMatrix matrixB) {
            if (matrixA.RowLen == matrixB.RowLen && matrixA.ColumnLen == matrixB.ColumnLen) {
                return true;
            } else {
                return false;
            }
        }
        private bool IsZeroRow(element[,] matrix, int row) {
            for (int i = 0; i < matrix.GetLength(1); i++) {
                if (matrix[row, i] != "0") {
                    return false;
                }
            }
            return true;
        }
        private bool IsZeroColumnRest(element[,] matrix, int currentRow, int col) {
            for (int row = 0; row < matrix.GetLength(0); row++) {
                if (row != currentRow && matrix[row, col] != "0") {
                    return false;
                }
            }
            return true;
        }
        public bool IsLineMinimalistMatrix() {
            int lasStIndex = -1;
            int row = 0;
            for (; row < RowLen; row++) {
                int currentStIndex = GetSt(this.Matrix, row);
                if (currentStIndex == -2) {
                    break;
                }
                if (currentStIndex <= lasStIndex
                    || IsZeroColumnRest(this.Matrix, row, currentStIndex) == false) {
                    return false;
                }
                lasStIndex = currentStIndex;
            }
            for (; row < RowLen; row++) {
                if (IsZeroRow(this.Matrix, row) == false) {
                    return false;
                }
            }
            return true;
        }
        //矩阵计算
        public element GetDeterminantValue() {
            return Determinant.GetValue(this.Matrix);
        }
        public string[] GetBaseSolutionList() {
            return BaseSoution.GetBaseSolutionList(this.Matrix);
        }
        //矩阵运算
        static private SimpleMatrix Nmatrix(SimpleMatrix sMatrix, string n) {
            SimpleMatrix NsMatrix = new SimpleMatrix(sMatrix.Matrix);
            for (int row = 0; row < sMatrix.RowLen; row++) {
                for (int col = 0; col < sMatrix.ColumnLen; col++) {
                    NsMatrix.Matrix[row, col] = RationNumber.Mulpitly(n, sMatrix.Matrix[row, col]);
                }
            }
            return NsMatrix;
        }
        static public SimpleMatrix Add(SimpleMatrix matrixA, SimpleMatrix matrixB) {
            element[,] matrix;
            if (IsSameType(matrixA, matrixB)) {
                int rowLen = matrixA.RowLen;
                int colLen = matrixA.ColumnLen;
                matrix = new element[rowLen, colLen];
                for (int row = 0; row < rowLen; row++) {
                    for (int col = 0; col < colLen; col++) {
                        matrix[row, col] = RationNumber.Add(matrixA.Matrix[row, col], matrixB.Matrix[row, col]);
                    }
                }
            } else {
                matrix = new element[0, 0];
            }
            return new SimpleMatrix(matrix);
        }
        static public SimpleMatrix Minus(SimpleMatrix matrixA, SimpleMatrix matrixB) {
            SimpleMatrix NsMatrix = Nmatrix(matrixB, "-1");
            return Add(matrixA, NsMatrix);
        }
        static public SimpleMatrix NormalProduct(SimpleMatrix matrixA, SimpleMatrix matrixB) {
            element[,] matrix;
            if (matrixA.ColumnLen == matrixB.RowLen) {
                int rowLen = matrixA.RowLen;
                int colLen = matrixB.ColumnLen;
                matrix = new element[rowLen, colLen];
                for (int row = 0; row < rowLen; row++) {
                    for (int col = 0; col < colLen; col++) {
                        string T = "0";
                        for (int i = 0; i < matrixA.RowLen; i++) {
                            string N = RationNumber.Mulpitly(matrixA.Matrix[row, i], matrixB.Matrix[i, col]);
                            T = RationNumber.Add(T, N);
                        }
                        matrix[row, col] = T;
                    }
                }
            } else {
                matrix = new element[0, 0];
            }
            return new SimpleMatrix(matrix);
        }
        static public SimpleMatrix HadamardProduct(SimpleMatrix matrixA, SimpleMatrix matrixB) {
            element[,] matrix;
            if (IsSameType(matrixA, matrixB)) {
                int rowLen = matrixA.RowLen;
                int colLen = matrixA.ColumnLen;
                matrix = new element[rowLen, colLen];
                for (int row = 0; row < rowLen; row++) {
                    for (int col = 0; col < colLen; col++) {
                        matrix[row, col] = RationNumber.Mulpitly(matrixA.Matrix[row, col], matrixB.Matrix[row, col]);
                    }
                }
            } else {
                matrix = new element[0, 0];
            }
            return new SimpleMatrix(matrix);
        }
        static public SimpleMatrix KroneckerProduct(SimpleMatrix matrixA, SimpleMatrix matrixB) {
            int BrowLen = matrixB.RowLen;
            int BcolLen = matrixB.ColumnLen;
            int Arow = 0;
            int Acol = 0;
            int rowLen = matrixA.RowLen * matrixB.RowLen;
            int colLen = matrixA.ColumnLen * matrixB.ColumnLen;
            element[,] matrix = new element[rowLen, colLen];
            for (int row = 0; row < rowLen; row++) {
                if (row != 0 && row % matrixA.RowLen == 0) {
                    Arow += 1;
                }
                for (int col = 0; col < colLen; col++) {
                    if (col != 0 && col % matrixA.ColumnLen == 0) {
                        Acol += 1;
                    }
                    matrix[row, col] = RationNumber.Mulpitly(matrixA.Matrix[Arow, Acol], matrixB.Matrix[row % BrowLen, col % BcolLen]);
                }
                Acol = 0;
            }
            return new SimpleMatrix(matrix);
        }
    }
}

namespace MatrixRowSwitch.MainCode {
    using element = String;
    static class Determinant {
        static private element[,] GetAijMatrix(element[,] matrix, int colR, int rowR = 0) {
            int rowLen = matrix.GetLength(0);
            int colLen = matrix.GetLength(1);
            int indexRow = 0;
            element[,] temp = new element[rowLen - 1, colLen - 1];
            for (int row = 0; row < rowLen; row++) {
                if (row != rowR) {
                    int indexCol = 0;
                    for (int col = 0; col < colLen; col++) {
                        if (col != colR) {
                            temp[indexRow, indexCol] = matrix[row, col];
                            indexCol += 1;
                        }
                    }
                    indexRow += 1;
                }
            }
            return temp;
        }
        static public element GetValue(element[,] matrix) {
            if (matrix.GetLength(0) == 1 && matrix.GetLength(1) == 1) {
                return matrix[0, 0];
            } else if (matrix.GetLength(0) == 2 && matrix.GetLength(1) == 2) {
                string num1 = RationNumber.Mulpitly(matrix[0, 0], matrix[1, 1]);
                string num2 = RationNumber.Mulpitly(matrix[0, 1], matrix[1, 0]);
                num2 = RationNumber.Mulpitly(num2, "-1");
                return RationNumber.Add(num1, num2);
            } else if (matrix.GetLength(0) == matrix.GetLength(1)) {
                element temp = "0";
                for (int i = 0; i < matrix.GetLength(0); i++) {
                    string num = "0";
                    num = RationNumber.Mulpitly(matrix[0, i], GetValue(GetAijMatrix(matrix, i)));
                    if (i % 2 == 1) {
                        num = RationNumber.Mulpitly(num, "-1");
                    }
                    temp = RationNumber.Add(temp, num);
                }
                return temp;
            } else {
                return "Null";
            }
        }
        static public element[,] GetAdjointMatrix(element[,] matrix) {
            int rowLen = matrix.GetLength(0);
            int colLen = matrix.GetLength(1);
            element[,] adjointMatrix = new element[rowLen, colLen];
            for (int row = 0; row < rowLen; row++) {
                for (int col = 0; col < colLen; col++) {
                    element N = GetValue(GetAijMatrix(matrix, col, row));
                    element n = Convert.ToString((int)Math.Pow(-1, (row + col + 2) % 2));
                    adjointMatrix[row, col] = RationNumber.Mulpitly(n, N);
                }
            }
            return adjointMatrix;
        }
    }
    static class BaseSoution {
        static private bool IsZeroRow(element[,] matrix, int row) {
            for (int i = 0; i < matrix.GetLength(1); i++) {
                if (matrix[row, i] != "0") {
                    return false;
                }
            }
            return true;
        }
        static private int GetSt(element[,] matrix, int row) {
            int columnLen = matrix.GetLength(1);
            for (int i = 0; i < columnLen; i++) {
                if (matrix[row, i] == "0") {
                    continue;
                } else if (matrix[row, i] == "1") {
                    return i;
                } else {
                    return -1;
                }
            }
            return -1;
        }
        static private List<int> GetStList(element[,] matrix) {
            int rowLen = matrix.GetLength(0);
            int columnLen = matrix.GetLength(1);
            List<int> temp = new List<int>();
            for (int row = 0; row < rowLen; row++) {
                int St = GetSt(matrix, row);
                if (St != -1) {
                    temp.Add(St);
                    for (int col = St + 1; col < columnLen; col++) {
                        matrix[row, col] = RationNumber.Mulpitly(matrix[row, col], "-1");
                    }
                }
            }
            return temp;
        }
        static private string GetRow(element[,] matrix, int row) {
            List<string> temp = new List<element>();
            for (int i = 0; i < matrix.GetLength(1); i++) {
                temp.Add(matrix[row, i]);
            }
            return string.Join(", ", temp);
        }
        static public void ToNonZeroRowMatrix(element[,] matrix) {
            int rowLen = matrix.GetLength(0);
            int columnLen = matrix.GetLength(1);
            element[,] temp = new element[rowLen, columnLen];
            for (int row = 0; row < rowLen; row++) {
                if (IsZeroRow(matrix, row)) {
                    rowLen -= 1;
                    continue;
                }
                for (int col = 0; col < columnLen; col++) {
                    temp[row, col] = matrix[row, col];
                }
            }
            matrix = new element[rowLen, columnLen];
            for (int row = 0; row < rowLen; row++) {
                for (int col = 0; col < columnLen; col++) {
                    matrix[row, col] = temp[row, col];
                }
            }
        }
        static private element[,] GetIdentityMatrix(int len) {
            element[,] temp = new element[len, len];
            for (int row = 0; row < len; row++) {
                for (int col = 0; col < len; col++) {
                    if (row == col) {
                        temp[row, col] = "1";
                    } else {
                        temp[row, col] = "0";
                    }
                }
            }
            return temp;
        }
        static private element[,] GetBaseSolution(element[,] matrix) {
            int rowLen = matrix.GetLength(0);
            int columnLen = matrix.GetLength(1);
            List<int> StList = GetStList(matrix);
            element[,] baseMatrix = new element[rowLen, columnLen - StList.Count];
            element[,] identityMatrix = GetIdentityMatrix(columnLen - StList.Count);
            for (int row = 0; row < rowLen; row++) {
                List<element> tempRow = new List<element>();
                int index = 0;
                for (int col = 0; col < columnLen; col++) {
                    if (StList.Contains(col) == false) {
                        baseMatrix[row, index] = matrix[row, col];
                        index += 1;
                    }
                }
            }
            int indexA = 0;
            int indexB = 0;
            element[,] totalMatrix = new element[columnLen, columnLen - StList.Count];
            for (int row = 0; row < columnLen; row++) {
                if (StList.Contains(row) == true) {
                    for (int col = 0; col < totalMatrix.GetLength(1); col++) {
                        totalMatrix[row, col] = baseMatrix[indexA, col];
                    }
                    indexA += 1;
                } else {
                    for (int col = 0; col < totalMatrix.GetLength(1); col++) {
                        totalMatrix[row, col] = identityMatrix[indexB, col];
                    }
                    indexB += 1;
                }
            }
            return totalMatrix;

        }
        static public string[] GetBaseSolutionList(element[,] matrix) {
            SimpleMatrix sMatrix = new SimpleMatrix(GetBaseSolution(matrix));
            sMatrix.Trans();
            string[] X = new string[sMatrix.RowLen];
            for (int row = 0; row < sMatrix.RowLen; row++) {
                X[row] = $"ξ{row} = ( {GetRow(sMatrix.Matrix, row)} )T";
            }
            return X;
        }
    }
}
