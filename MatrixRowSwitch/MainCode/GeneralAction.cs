using System.Text.RegularExpressions;

namespace MatrixRowSwitch.MainCode {
    static class GeneralAction {
        static readonly private Regex seekElement = new Regex("[-/0-9]{0,}[^ \n\r\t]");
        static readonly private Regex seekMR = new Regex("MR[ ]{1,}[0-9]{1,}[ ]{1,}[-/0-9]{1,}");
        static readonly private Regex seekCR = new Regex("CR[ ]{1,}[0-9]{1,}[ ]{1,}[0-9]{1,}");
        static readonly private Regex seekDR = new Regex("DR[ ]{1,}[0-9]{1,}[ ]{1,}[0-9]{1,}[ ]{1,}[-/0-9]{1,}");
        static readonly private Regex seekBTSNM = new Regex("[BTA]S|NM");
        static readonly private Regex seekArgs = new Regex("[A-Z/0-9-]{0,}[^ \r\n\t]");
        static public string[] GetCommand(string sourceInput) {
            sourceInput = sourceInput.ToUpper();
            string[] args;
            MatchCollection argsPack = seekArgs.Matches(sourceInput);
            if (seekMR.IsMatch(sourceInput) || seekCR.IsMatch(sourceInput)) {
                args = new string[3] {
                    argsPack[0].ToString(), argsPack[1].ToString(), argsPack[2].ToString()
                };
            } else if (seekDR.IsMatch(sourceInput)) {
                args = new string[4] {
                    argsPack[0].ToString(), argsPack[1].ToString(), argsPack[2].ToString(), argsPack[3].ToString()
                };
            } else if (seekBTSNM.IsMatch(sourceInput)) {
                args = new string[1] {
                    argsPack[0].ToString()
                };
            } else {
                args = new string[1] {
                    "Unknow"
                };
            }
            return args;
        }
        static public string[,] GetMatrix(string sourceInput) {
            string[] rows = sourceInput.Split('\n');
            int rowLen = rows.Length;
            int columnLen = seekElement.Matches(rows[0]).Count;
            string[,] tempMatrix = new string[rowLen, columnLen];
            for (int row = 0; row < rowLen; row++) {
                rows[row] = rows[row].Trim();
                while (seekElement.Matches(rows[row]).Count < columnLen) {
                    rows[row] += " 0";
                }
                MatchCollection elements = seekElement.Matches(rows[row]);
                for (int column = 0; column < columnLen; column++) {
                    tempMatrix[row, column] = elements[column].ToString().Trim();
                }
            }
            return tempMatrix;
        }
    }
}
