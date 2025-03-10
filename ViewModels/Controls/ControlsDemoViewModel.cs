﻿using IceTea.Wpf.Core.Demo.Models;
using IceTea.Wpf.Core.Demo.Views.Controls;
using IceTea.Wpf.Core.Demo.Views.Controls.Buttons;
using IceTea.Wpf.Core.Demo.Views.Controls.TextBoxes;
using IceTea.Atom.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using IceTea.Wpf.Core.Utils;

namespace IceTea.Wpf.Core.Demo.ViewModels.Controls
{
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    internal class ControlsDemoViewModel : BindableBase
    {
        public ControlsDemoViewModel(IRegionManager regionManager)
        {
            InitTreeViewData();

            InitMenuData();

            InitDataGrid();

            InitComamnds(regionManager);
        }

        private void InitComamnds(IRegionManager regionManager)
        {
            NavigateToCommand = new DelegateCommand<object>(param =>
            {
                if (param is ControlNode node && node.Items.Count == 0)
                {
                    var uri = node.Name;

                    if ("选择器控件".EqualsIgnoreCase(node.ParentName))
                    {
                        uri = nameof(Selectors);
                    }
                    else if ("集合控件".EqualsIgnoreCase(node.ParentName))
                    {
                        uri = nameof(ItemsControls);
                    }
                    else if ("选中控件".EqualsIgnoreCase(node.ParentName))
                    {
                        uri = nameof(Pickers);
                    }
                    else if ("虚拟化面板".EqualsIgnoreCase(node.ParentName))
                    {
                        uri = nameof(VirtualizingPanels);
                    }
                    else if (Controls.First(n => n.Name == "面板").Items.SelectMany(n =>
                    {
                        var list = new List<string>();

                        GetNames(list, n);

                        return list;

                        void GetNames(IList<string> names, ControlNode _node)
                        {
                            if (_node.Items.Count == 0)
                            {
                                names.Add(_node.Name);
                                return;
                            }

                            foreach (var item in _node.Items)
                            {
                                GetNames(names, item);
                            }
                        }

                    }).Contains(node.Name))
                    {
                        uri = nameof(Panels);
                    }

                    switch (node.Name)
                    {
                        case "单击按钮":
                            uri = nameof(ButtonsView);
                            break;
                        case "双击按钮":
                            uri = nameof(ToggleButtonsView);
                            break;
                        case "IconFton图标":
                            uri = nameof(IconFontView);
                            break;

                        case "单行文本":
                            uri = nameof(TextBox);
                            break;
                        case "多行文本":
                            uri = nameof(RichTextBox);
                            break;
                        case "密码框":
                            uri = nameof(PasswordBox);
                            break;
                        case "选项卡":
                            uri = nameof(TabControls);
                            break;
                        default:
                            break;
                    }

                    regionManager.RequestNavigate("ContentRegion", uri, nr => { }, new NavigationParameters()
                    {
                        { "Key", "Value" }
                    });
                }
            });

            this.SwitchThemeCommand = new DelegateCommand(() => CommonCoreUtils.RefreshTheme());
        }

        private Collection<ResourceDictionary> _resourceDictionaries => Application.Current.Resources.MergedDictionaries;

        public ICommand SwitchThemeCommand { get; private set; }

        public IEnumerable<DataItem> Datas { get; private set; }

        public IEnumerable<ControlNode> Controls { get; private set; }

        public IEnumerable<MenuNode> Menus { get; private set; }

        private void InitDataGrid()
        {
            this.Datas = new List<DataItem>()
            {
                new DataItem()
                {
                    IsSelected = true,
                    Name ="A",
                    Address ="北京"
                },
                new DataItem()
                {
                    IsSelected = false,
                    Name ="B",
                    Address ="上海"
                },
                new DataItem()
                {
                    IsSelected = true,
                    Name ="C",
                    Address ="广州"
                }
            };
        }

        private void InitMenuData()
        {
            var list = new List<MenuNode>()
            {
                new MenuNode("文件")
                    .Add(new MenuNode("新建").Add(new MenuNode("项目"))
                    .Add(new MenuNode("仓库")))
                    .Add(new MenuNode("打开")),

                new MenuNode("编辑").Add(new MenuNode("转到")).Add(new MenuNode("查找和替换")),

                new MenuNode("视图").Add(new MenuNode("代码")).Add(new MenuNode("设计器"))
            };

            Menus = list;
        }

        private void InitTreeViewData()
        {
            var list = new List<ControlNode>()
            {
                new ControlNode("按钮").Add(new ControlNode("单击按钮")).Add(new ControlNode("双击按钮")).Add(new ControlNode("IconFton图标")),

                new ControlNode("文本框").Add(new ControlNode("单行文本")).Add(new ControlNode("多行文本")).Add(new ControlNode("密码框")),

                new ControlNode("面板")
                        .Add(
                                new ControlNode("网格面板")
                                    .Add(new ControlNode("不平均网格")).Add(new ControlNode("平均网格"))
                            )
                        .Add(
                                new ControlNode("平铺面板")
                                    .Add(new ControlNode("堆叠面板")).Add(new ControlNode("折叠面板")).Add(new ControlNode("停靠面板"))
                            )
                        .Add(new ControlNode("画布"))
                        .Add(
                                new ControlNode("虚拟化面板")
                                    .Add(new ControlNode("垂直堆叠虚拟化面板"))
                                    .Add(new ControlNode("垂直折叠虚拟化面板"))
                                    .Add(new ControlNode("垂直平均网格虚拟化面板"))
                                    .Add(new ControlNode("水平堆叠虚拟化面板"))
                                    .Add(new ControlNode("水平折叠虚拟化面板"))
                                    .Add(new ControlNode("水平平均网格虚拟化面板"))
                            )
                        .Add(new ControlNode("边框"))
                        .Add(new ControlNode("滚动视图")),

                new ControlNode("选择器控件").Add(new ControlNode("选项卡")).Add(new ControlNode("下拉框")).Add(new ControlNode("表格")).Add(new ControlNode("列表")),

                new ControlNode("集合控件").Add(new ControlNode("菜单")).Add(new ControlNode("上下文菜单")).Add(new ControlNode("树形列表")),

                new ControlNode("选中控件").Add(new ControlNode("颜色选择器")).Add(new ControlNode("日期选择器")),

            };

            Controls = list;
        }

        public ICommand NavigateToCommand { get; private set; }
    }
}
