using System.Collections.Generic;

namespace MatrixRowSwitch.MainCode {
    class SwitchRecord {
        private List<string[]> records;
        public List<string> Records {
            get {
                List<string> temp = new List<string>();
                int index = 1;
                foreach (string[] record in this.records) {
                    temp.Add(GetSwitchDetail(record));
                    index += 1;
                }
                return temp;
            }
        }
        public SwitchRecord() {
            this.records = new List<string[]>();
        }
        private string Trans() {
            return $"转置";
        }
        private string CrSwitch(string[] record) {
            return $"r{record[1]} ↔ r{record[2]}";
        }
        private string MrSwitch(string[] record) {
            return $"r{record[1]} x {record[2]}";
        }
        private string DrSwitch(string[] record) {
            return $"r{record[1]} + {record[3]}r{record[2]}";
        }
        private string AsSwitch(string[] record) {
            return $"逆矩阵";
        }
        private string BsSwitch(string[] record) {
            return $"伴随矩阵";
        }
        public void AddRecord(string[] record) {
            this.records.Add(record);
        }
        public string GetSwitchDetail(string[] record) {
            switch (record[0]) {
                case "TS":
                    return Trans();
                case "CR":
                    return CrSwitch(record);
                case "MR":
                    return MrSwitch(record);
                case "DR":
                    return DrSwitch(record);
                case "AS":
                    return AsSwitch(record);
                case "BS":
                    return BsSwitch(record);
                default:
                    return "未识别";
            }
        }
        public override string ToString() {
            string temp = "------------------";
            foreach (string[] record in this.records) {
                temp += $"{GetSwitchDetail(record)}\n";
            }
            return temp;
        }
    }
}
