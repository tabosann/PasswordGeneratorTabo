using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PasswordGeneratorTabo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

        #if DEBUG || DEBUG_UNPACKAGED
            // 開発版であることがタイトルバーからも分かるように.
            m_titleBar.Subtitle = "Dev";
        #endif
        }

        private void RootGridLoaded(object sender, RoutedEventArgs e)
        {
            Helpers.WindowHelper.FitClientToActualSize(this, m_rootGrid);

            var presenter = AppWindow.Presenter as OverlappedPresenter;
            if (presenter != null) {
                // リサイズや最大化を無効化.
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = true;
            }
        }

        private void CopyTextClick(object sender, RoutedEventArgs e)
        {
            var package = new DataPackage();
            package.SetText(m_passwordTextBox.Text);
            Clipboard.SetContent(package);
        }

        private void PasswordLengthChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var loader = new ResourceLoader();
            string label = loader.GetString("PasswordLengthLabel");
            string msg = String.Format("{0} {1}", label, e.NewValue);
            m_slider.Header = msg;
            m_passwordTextBox.Text = GeneratePassword((int)e.NewValue);
        }

        private string GeneratePassword(int length)
        {
            bool useNumbers = m_numbersCheckBox?.IsChecked ?? false;
            bool useSymbols = m_symbolsCheckBox?.IsChecked ?? false;

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            if (useNumbers) {
                chars += "0123456789";
            }
            if (useSymbols) {
                chars += "!@#$%^&*()-_=+[]{};:'\",.<>/?";
            }

            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
