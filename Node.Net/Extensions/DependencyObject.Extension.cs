﻿// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net
{
    public static class DependencyObjectExtension
    {
        public static void RemoveChild(this DependencyObject parent, UIElement child)
        {
            var panel = parent as Panel;
            if (panel != null)
            {
                panel.Children.Remove(child);
                return;
            }

            var decorator = parent as Decorator;
            if (decorator != null)
            {
                if (decorator.Child == child)
                {
                    decorator.Child = null;
                }
                return;
            }

            var contentPresenter = parent as ContentPresenter;
            if (contentPresenter != null)
            {
                if (contentPresenter.Content == child)
                {
                    contentPresenter.Content = null;
                }
                return;
            }

            var contentControl = parent as ContentControl;
            if (contentControl != null)
            {
                if (contentControl.Content == child)
                {
                    contentControl.Content = null;
                }
                return;
            }

            // maybe more
        }

        public static void RemoveFromParent(this DependencyObject child)
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null)
            {
                var uielement = child as UIElement;
                if (uielement != null) DependencyObjectExtension.RemoveChild(parent,uielement);
            }
        }
    }
}
