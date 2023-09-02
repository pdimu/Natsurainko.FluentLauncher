﻿using Natsurainko.FluentLauncher.Services.Settings;
using Natsurainko.FluentLauncher.Services.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Natsurainko.FluentLauncher.ViewModels;

class ShellViewModel : INavigationAware
{
    private readonly INavigationService _shellNavigationService;
    private readonly SettingsService _settings;

    public ShellViewModel(INavigationService shellNavigationService, SettingsService settings)
    {
        _shellNavigationService = shellNavigationService;
        _settings = settings;
    }

    #region Navigation

    void INavigationAware.OnNavigatedTo(object parameter)
    {
        _shellNavigationService.NavigateTo(_settings.UseNewHomePage ? "NewHomePage" : "HomePage");
    }

    public void NavigateTo(string pageKey, object? parameter = null)
        => _shellNavigationService.NavigateTo(pageKey, parameter);

    #endregion

}