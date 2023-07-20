﻿using AppSettingsManagement.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Natsurainko.FluentLauncher.Components;
using Natsurainko.FluentLauncher.Services.Launch;
using Natsurainko.FluentLauncher.Services.Settings;
using Natsurainko.FluentLauncher.ViewModels.Common;
using Natsurainko.Toolkits.Values;
using Nrk.FluentCore.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace Natsurainko.FluentLauncher.ViewModels.Settings;

partial class LaunchViewModel : SettingsViewModelBase, ISettingsViewModel
{
    // TODO: Disable game window size settings when full screen mode is on

    [SettingsProvider]
    private readonly SettingsService _settingsService;
    private readonly GameService _gameService;

    #region Settings

    public ObservableCollection<string> MinecraftFolders => _settingsService.MinecraftFolders;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMinecraftFoldersEmpty))]
    [BindToSetting(Path = nameof(SettingsService.ActiveMinecraftFolder))]
    private string activeMinecraftFolder;

    // TODO: [BindToSetting(Path = nameof(SettingsService.Javas))]
    public ObservableCollection<string> Javas => _settingsService.Javas;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsJavasEmpty))]
    [BindToSetting(Path = nameof(SettingsService.ActiveJava))]
    private string activeJava;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.JavaMemory))]
    private int javaMemory;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.EnableAutoMemory))]
    private bool enableAutoMemory;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.EnableAutoJava))]
    private bool enableAutoJava;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.GameWindowTitle))]
    private string gameWindowTitle;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.GameWindowWidth))]
    private int gameWindowWidth;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.GameWindowHeight))]
    private int gameWindowHeight;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.GameServerAddress))]
    private string gameServerAddress;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.EnableFullScreen))]
    private bool enableFullScreen;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.EnableIndependencyCore))]
    private bool enableIndependencyCore;

    #endregion

    public bool IsMinecraftFoldersEmpty => MinecraftFolders.Count == 0;

    public bool IsJavasEmpty => Javas.Count == 0;

    public LaunchViewModel(SettingsService settingsService, GameService gameService)
    {
        _settingsService = settingsService;
        _gameService = gameService;

        (this as ISettingsViewModel).InitializeSettings();
    }

    [RelayCommand]
    public Task BrowserFolder() => Task.Run(async () =>
    {
        var folderPicker = new FolderPicker();

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        folderPicker.FileTypeFilter.Add("*");
        var folder = await folderPicker.PickSingleFolderAsync();

        if (folder != null)
            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                if (MinecraftFolders.Contains(folder.Path))
                {
                    MessageService.Show("This folder already exists");
                    return;
                }

                MinecraftFolders.Add(folder.Path);
                _gameService.ActivateMinecraftFolder(folder.Path);

                OnPropertyChanged(nameof(IsMinecraftFoldersEmpty));
            });

    });

    [RelayCommand]
    public void BrowserJava()
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Multiselect = false;
        openFileDialog.Filter = "Javaw Executable File|javaw.exe|Java Executable File|java.exe";

        if (openFileDialog.ShowDialog().GetValueOrDefault(false))
            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                Javas.Add(openFileDialog.FileName);
                ActiveJava = openFileDialog.FileName;

                OnPropertyChanged(nameof(IsJavasEmpty));
            });
    }

    [RelayCommand]
    public void SearchJava()
    {
        Javas.AddNotRepeating(JavaUtils.SearchJava());
        ActiveJava = Javas.Any() ? Javas[0] : null;

        OnPropertyChanged(nameof(Javas));

        MessageService.Show("Added the search Java to the runtime list");
    }

    [RelayCommand]
    public void RemoveFolder()
    {
        MinecraftFolders.Remove(ActiveMinecraftFolder);
        ActiveMinecraftFolder = MinecraftFolders.Any() ? MinecraftFolders[0] : null;

        OnPropertyChanged(nameof(IsMinecraftFoldersEmpty));
    }

    [RelayCommand]
    public void RemoveJava()
    {
        Javas.Remove(ActiveJava);
        ActiveJava = Javas.Any() ? Javas[0] : null;

        OnPropertyChanged(nameof(IsJavasEmpty));
    }

    [RelayCommand]
    public void ActivateCoresPage() => Views.ShellPage.ContentFrame.Navigate(typeof(Views.Cores.CoresPage));
}