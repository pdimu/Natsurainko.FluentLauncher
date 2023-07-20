﻿using Microsoft.UI.Xaml.Data;
using Nrk.FluentCore.Classes.Datas.Launch;
using System;
using System.Collections.Generic;

namespace Natsurainko.FluentLauncher.Utils.Xaml.Converters;

public class GameCoreTagConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is GameInfo game)
        {
            var strings = new List<string>
            {
                game.Type switch
                {
                    "release" => "Release",
                    "snapshot" => "Snapshot",
                    "old_beta" => "Old Beta",
                    "old_alpha" => "Old Alpha",
                    _ => "Unknown"
                }
            };

            if (game.IsInheritedFrom)
                strings.Add("Inherited From");

            strings.Add(game.AbsoluteVersion);

            return string.Join(" ", strings);
        }

        /*
        if (value is CoreManifestItem coreManifestItem)
            return coreManifestItem.Type switch
            {
                "release" => "Release",
                "snapshot" => "Snapshot",
                "old_beta" => "Old Beta",
                "old_alpha" => "Old Alpha",
                _ => "Unknown"
            };*/

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
