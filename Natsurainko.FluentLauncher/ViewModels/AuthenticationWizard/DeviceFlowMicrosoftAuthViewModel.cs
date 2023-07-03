﻿using Natsurainko.FluentLauncher.ViewModels.Common;
using Natsurainko.FluentLauncher.Views.AuthenticationWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natsurainko.FluentLauncher.ViewModels.AuthenticationWizard;

internal class DeviceFlowMicrosoftAuthViewModel : WizardViewModelBase
{
    public DeviceFlowMicrosoftAuthViewModel()
    {
        XamlPageType = typeof(DeviceFlowMicrosoftAuthPage);
    }
}
