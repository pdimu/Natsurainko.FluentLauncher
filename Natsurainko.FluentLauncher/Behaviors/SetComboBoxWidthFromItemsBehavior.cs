﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;
using Windows.Foundation.Collections;

namespace Natsurainko.FluentLauncher.Behaviors
{
    class SetComboBoxWidthFromItemsBehavior : Behavior<ComboBox>
    {
        public static readonly DependencyProperty SetComboBoxWidthFromItemsProperty =
        DependencyProperty.RegisterAttached
        (
            "SetComboBoxWidthFromItems",
            typeof(bool),
            typeof(SetComboBoxWidthFromItemsBehavior),
            new PropertyMetadata(false, OnSetComboBoxWidthFromItemsPropertyChanged)
        );

        public bool SetComboBoxWidthFromItems 
        { 
            get => (bool)GetValue(SetComboBoxWidthFromItemsProperty); 
            set => SetValue(SetComboBoxWidthFromItemsProperty, value);
        }
        
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += OnComboBoxLoaded;
            AssociatedObject.Items.VectorChanged += Items_VectorChanged;
        }

        public void Items_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs e)
        {
            SetComboBoxWidth(AssociatedObject);
        }

        private static void OnComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            SetComboBoxWidth((ComboBox)sender);
        }

        private static void OnSetComboBoxWidthFromItemsPropertyChanged(
            DependencyObject dpo, 
            DependencyPropertyChangedEventArgs e)
        {
            SetComboBoxWidthFromItemsBehavior behavior = (SetComboBoxWidthFromItemsBehavior)dpo;
            ComboBox comboBox = behavior.AssociatedObject;

            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;

            if (comboBox != null && newValue != oldValue)
            {
                if (newValue == true)
                {
                    comboBox.Loaded += OnComboBoxLoaded;
                    comboBox.Items.VectorChanged += behavior.Items_VectorChanged;

                }
                else
                {
                    comboBox.Loaded -= OnComboBoxLoaded;
                    comboBox.Items.VectorChanged -= behavior.Items_VectorChanged;
                }
            }
        }

        /// <summary>
        /// Set the width of a ComboBox to the longest item in its drop down menu
        /// </summary>
        /// <param name="comboBox">Target</param>
        private static void SetComboBoxWidth(ComboBox comboBox)
        {
            // Open ComboBox drop down and prepare ItemContainerGenerator
            comboBox.IsDropDownOpen = true;
            comboBox.ItemContainerGenerator.StartAt(new GeneratorPosition(0, 0), GeneratorDirection.Forward, true);

            double maxWidth = 0;
            ComboBoxItem? item;
            while ((item = comboBox.ItemContainerGenerator.GenerateNext(out _) as ComboBoxItem) != null)
            {
                item.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
                var size = item.DesiredSize;
                if (size.Width > maxWidth)
                {
                    maxWidth = size.Width;
                }
            }

            comboBox.Width = maxWidth + 20; // This constant adds more space to include the drop down button and paddings
            comboBox.ItemContainerGenerator.Stop();
            comboBox.IsDropDownOpen = false;
        }
    }
}
