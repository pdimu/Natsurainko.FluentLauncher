﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;

#nullable disable

namespace Natsurainko.FluentLauncher.ViewModels;

internal partial class WizardViewModelBase : ObservableObject
{
    public virtual bool CanNext => false;

    public virtual bool CanPrevious => true;

    public virtual bool CanCancel => true;

    public Type XamlPageType { get; init; }

    public virtual WizardViewModelBase GetNextViewModel()
    {
        throw new NotImplementedException();
    }
}
