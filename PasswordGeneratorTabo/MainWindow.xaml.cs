using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PasswordGeneratorTabo
{
    public sealed record Password(string Value, int Id);

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
            Helpers.WindowHelper.ResizeClinetWithDpiScale(this, 950, m_rootGrid.DesiredSize.Height + 350);
            var presenter = AppWindow.Presenter as OverlappedPresenter;
            if (presenter != null) {
                // リサイズや最大化を無効化.
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = true;
            }
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            if (m_passwords == null) {
                return;
            }

            for (int i = 0; i < m_passwordsCapacity; ++i) {
                m_passwords[i] = new Password(GeneratePassword((int)m_slider.Value), i);
            }
        }

        private void PasswordLengthSliderLoaded(object sender, RoutedEventArgs e)
        {
            var loader = new ResourceLoader();
            string label = loader.GetString("PasswordLengthLabel");
            m_slider.Header = String.Format("{0} {1}", label, m_slider.Value);
        }

        private void PasswordLengthChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (m_passwords == null) {
                return;
            }

            var loader = new ResourceLoader();
            string label = loader.GetString("PasswordLengthLabel");
            m_slider.Header = String.Format("{0} {1}", label, e.NewValue);

            for (int i = 0; i < m_passwordsCapacity; ++i) {
                m_passwords[i] = new Password(GeneratePassword((int)e.NewValue), i);
            }
        }

        private void PasswordListViewLoaded(object sender, RoutedEventArgs e)
        {
            m_passwords = new ObservableCollection<Password>(new List<Password>(m_passwordsCapacity));
            if (m_passwords == null) {
                return;
            }

            for (int i = 0; i < m_passwordsCapacity; ++i) {
                var pw = new Password(GeneratePassword((int)m_slider.Value), i);
                m_passwords.Add(pw);
            }
            m_passwordListView.ItemsSource = m_passwords;
        }

        private void PasswordSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            Password? selected = listView?.SelectedItem as Password;
            if ((selected != null) && (m_passwords != null)) {
                var package = new DataPackage();
                package.SetText(m_passwords[selected.Id].Value);
                Clipboard.SetContent(package);
            }
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

        const int m_passwordsCapacity = 50;
        ObservableCollection<Password>? m_passwords;
    }
}
