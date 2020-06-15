using System;
using System.Windows;

namespace MatrixRowSwitch.MainCode {
    static class ResDict {
        static public ResourceDictionary customStyles = new ResourceDictionary() {
            Source = new Uri("Styles/customStyles.xaml", UriKind.Relative)
        };
    }
}
