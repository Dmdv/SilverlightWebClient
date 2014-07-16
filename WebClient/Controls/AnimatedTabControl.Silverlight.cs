using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WebClient.ICS.Client.Controls
{
    /// <summary>
    /// Custom Tab control with animations.
    /// </summary>
    /// <remarks>
    /// This customization of the TabControl was required to create the animations for the transition 
    /// between the tab items.
    /// </remarks>
    public class AnimatedTabControl : TabControl
    {
        private TabItem _previousSelectedTabItem;
        private FrameworkElement _previousSelectedTabItemContent;

        public AnimatedTabControl()
        {
            DefaultStyleKey = typeof (AnimatedTabControl);
        }

        private FrameworkElement CurrentView { get; set; }

        private ContentControl BufferView { get; set; }

        private Storyboard StartingTransition { get; set; }

        private Storyboard EndingTransition { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CurrentView = (FrameworkElement) GetTemplateChild("ContentTop");
            BufferView = (ContentControl) GetTemplateChild("BufferView");

            var containerGrid = (FrameworkElement) GetTemplateChild("LayoutRoot");
            StartingTransition = (Storyboard) containerGrid.FindName("StartingTransition");
            EndingTransition = (Storyboard) containerGrid.FindName("EndingTransition");
            StartingTransition.Completed += StartingTransitionCompleted;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs args)
        {
            if (args.RemovedItems.Count > 0)
            {
                RestoreBufferedTabItemContent();

                // Put the "old" view in a buffer so we can still show it to perform the starting animation with it
                _previousSelectedTabItem = (TabItem) args.RemovedItems[0];
                _previousSelectedTabItemContent = (FrameworkElement) _previousSelectedTabItem.Content;
                _previousSelectedTabItem.Content = null;
                CurrentView.Visibility = Visibility.Collapsed;
                BufferView.Content = _previousSelectedTabItemContent;

                StartingTransition.Begin();
            }
        }

        private void RestoreBufferedTabItemContent()
        {
            if (_previousSelectedTabItemContent == null || _previousSelectedTabItem == null)
            {
                return;
            }

            BufferView.Content = null;
            _previousSelectedTabItem.Content = _previousSelectedTabItemContent;
            _previousSelectedTabItem = null;
            _previousSelectedTabItemContent = null;
        }

        private void StartingTransitionCompleted(object sender, EventArgs e)
        {
            RestoreBufferedTabItemContent();

            CurrentView.Visibility = Visibility.Visible;

            // fire transition
            EndingTransition.Begin();
        }
    }
}