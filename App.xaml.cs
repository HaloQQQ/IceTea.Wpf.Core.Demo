using IceTea.Atom.Contracts;
using IceTea.Atom.Extensions;
using IceTea.Wpf.Core.Demo.ViewModels.Controls;
using IceTea.Wpf.Core.Demo.Views.Controls;
using IceTea.Wpf.Core.Demo.Views.Controls.Buttons;
using IceTea.Wpf.Core.Demo.Views.Controls.TextBoxes;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using System.Windows;

namespace IceTea.Wpf.Core.Demo
{
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning disable CS8604 // 引用类型参数可能为 null。
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private IEnumerable<string> GetMessageList(Exception exception)
        {
            var list = new List<string>
            {
                exception.Message
            };

            while ((exception = exception.InnerException) != null)
            {
                list.Add(exception.Message);

                if (!exception.StackTrace.IsNullOrBlank())
                {
                    list.Add(exception.StackTrace);
                }
            }

            return list;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var list = this.GetMessageList(e.ExceptionObject as Exception);

            var message = $"Domain出现异常:{AppStatics.NewLineChars}" + AppStatics.NewLineChars.Join(list);
            MessageBox.Show(message);
        }

        private void Current_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var list = this.GetMessageList(e.Exception);

            var message = $"App出现异常:{AppStatics.NewLineChars}" + AppStatics.NewLineChars.Join(list);
            MessageBox.Show(message);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ViewModelLocationProvider.Register<MainWindow, ControlsDemoViewModel>();

            this.RegisterNavigation(containerRegistry);
        }

        private void RegisterNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ControlsDemoView>();

            containerRegistry.RegisterForNavigation<ButtonsView>();
            containerRegistry.RegisterForNavigation<ToggleButtonsView>();
            containerRegistry.RegisterForNavigation<IconFontView>();

            containerRegistry.RegisterForNavigation<PasswordBox>();
            containerRegistry.RegisterForNavigation<RichTextBox>();
            containerRegistry.RegisterForNavigation<TextBox>();

            containerRegistry.RegisterForNavigation<Panels>();

            containerRegistry.RegisterForNavigation<Selectors>();
            containerRegistry.RegisterForNavigation<TabControls>();

            containerRegistry.RegisterForNavigation<ItemsControls>();

            containerRegistry.RegisterForNavigation<VirtualizingPanels>();

            containerRegistry.RegisterForNavigation<Pickers>();
        }
    }

}
