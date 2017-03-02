﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Extensions
{
    public static class DependencyObjectExtension
    {
        public static void RemoveChild(DependencyObject parent, UIElement child)
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

        public static void RemoveFromParent(DependencyObject child)
        {
            if (child == null) return;
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null)
            {
                var uielement = child as UIElement;
                if (uielement != null)
                {
                    Extensions.DependencyObjectExtension.RemoveChild(parent, uielement);
                    //parent.RemoveChild(uielement);
                }
                //parent.RemoveChild(child);
            }
        }
    }
}