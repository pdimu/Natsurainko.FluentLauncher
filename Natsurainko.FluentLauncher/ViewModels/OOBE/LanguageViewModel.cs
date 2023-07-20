﻿using AppSettingsManagement.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Natsurainko.FluentLauncher.Components;
using Natsurainko.FluentLauncher.Services.Settings;
using Natsurainko.FluentLauncher.Services.UI.Messaging;
using Natsurainko.FluentLauncher.ViewModels.Common;
using System.Collections.Generic;

namespace Natsurainko.FluentLauncher.ViewModels.OOBE;

partial class LanguageViewModel : SettingsViewModelBase, ISettingsViewModel
{
    #region Settings

    [SettingsProvider]
    private readonly SettingsService _settingsService;

    [ObservableProperty]
    [BindToSetting(Path = nameof(SettingsService.CurrentLanguage))]
    private string currentLanguage;

    #endregion

    public List<string> Languages => LanguageResources.SupportedLanguages;

    private bool _isLoading = true;

    public LanguageViewModel(SettingsService settingsService)
    {
        _settingsService = settingsService;
        (this as ISettingsViewModel).InitializeSettings();
        _isLoading = false;
    }

    partial void OnCurrentLanguageChanged(string oldValue, string newValue)
    {
        bool isValid = LanguageResources.SupportedLanguages.Contains(CurrentLanguage);
        WeakReferenceMessenger.Default.Send(new GuideNavigationMessage()
        {
            CanNext = isValid,
            NextPage = typeof(Views.OOBE.BasicPage)
        });
        if (isValid && !_isLoading)
        {
            LanguageResources.ApplyLanguage(CurrentLanguage);
        }
    }

}
