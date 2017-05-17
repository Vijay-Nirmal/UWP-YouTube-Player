using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPYouTubePlayer.Themes
{
    public sealed class CustomMediaTransportControls : MediaTransportControls
    {
        //Constructor for CustomMediaTransportControls class
        public CustomMediaTransportControls()
        {
            this.DefaultStyleKey = typeof(CustomMediaTransportControls);
        }

        private AppBarButton compactOverlayButton;        //Button for "CompactOverlayButton"
        private CommandBar commandBar;
        private Slider progressSlider;

        //overriding OnApplyTemplate
        protected override void OnApplyTemplate()
        {
            //Setting Controls variables for Buttons and MenuFlyoutItem
            commandBar = GetTemplateChild("MediaControlsCommandBar") as CommandBar;
            compactOverlayButton = GetTemplateChild("CompactOverlayButton") as AppBarButton;
            progressSlider = GetTemplateChild("ProgressSlider") as Slider;
            var quality144p = GetTemplateChild("Quality144p") as MenuFlyoutItem;
            var quality240p = GetTemplateChild("Quality240p") as MenuFlyoutItem;
            var quality360p = GetTemplateChild("Quality360p") as MenuFlyoutItem;
            var quality480p = GetTemplateChild("Quality480p") as MenuFlyoutItem;
            var quality720p = GetTemplateChild("Quality720p") as MenuFlyoutItem;
            var quality1080p = GetTemplateChild("Quality1080p") as MenuFlyoutItem;

            //Creating Clicked Event for Buttons and MenuFlyoutItem
            compactOverlayButton.Click += CompactOverlayButton_Click;
            quality144p.Click += Quality144p_Click;
            quality240p.Click += Quality240p_Click;
            quality360p.Click += Quality360p_Click;
            quality480p.Click += Quality480p_Click;
            quality720p.Click += Quality720p_Click;
            quality1080p.Click += Quality1080p_Click;

            if (CompactOverlayButtonVisibility != Visibility.Visible)
                commandBar.PrimaryCommands.Remove(compactOverlayButton);
            else if (!commandBar.PrimaryCommands.Contains(compactOverlayButton))
                commandBar.PrimaryCommands.Insert(4, compactOverlayButton);
            compactOverlayButton.Visibility = Visibility.Visible;

            base.OnApplyTemplate();
        }

        public event EventHandler<EventArgs> CompactOverlaid;

        //Button click event for CompactOverlayButton to Create a Frame in CompactOverlay mode
        public void CompactOverlayButton_Click(object sender, RoutedEventArgs e)
        {
            CompactOverlaid?.Invoke(this, EventArgs.Empty);
            compactOverlayButton.Visibility = Visibility.Collapsed;
        }

        public double Time
        {
            get
            {
                return progressSlider.Value;
            }
            set
            {
                progressSlider.Value = value;
            }
        }

        //Initializing YouTubeQuality
        private YouTubeQuality oldQuality = YouTubeQuality.Quality144P;
        private YouTubeQuality quality = YouTubeQuality.Quality144P;

        //Settting the New Quality and setting the last Quality to oldQuality
        public YouTubeQuality Quality
        {
            get
            {
                return quality;     //Returning New Quality
            }
            set
            {
                oldQuality = quality;       //Updating Old Quality
                quality = value;
                OnQualityChanged();     //Calling Method to raising QualityChanged event
            }
        }

        //Setting Quality when the corresponding MenuFlyoutItem is clicked
        #region SetQuality
        private void Quality1080p_Click(object sender, RoutedEventArgs e)
        {
            Quality = YouTubeQuality.Quality1080P;
        }

        private void Quality720p_Click(object sender, RoutedEventArgs e)
        {
            Quality = YouTubeQuality.Quality720P;
        }

        private void Quality480p_Click(object sender, RoutedEventArgs e)
        {
            Quality = YouTubeQuality.Quality480P;
        }

        private void Quality360p_Click(object sender, RoutedEventArgs e)
        {
            Quality = YouTubeQuality.Quality360P;
        }

        private void Quality240p_Click(object sender, RoutedEventArgs e)
        {
            Quality = YouTubeQuality.Quality240P;
        }

        private void Quality144p_Click(object sender, RoutedEventArgs e)
        {
            Quality = YouTubeQuality.Quality144P;
        }
        #endregion

        private EventRegistrationTokenTable<EventHandler<QualityChangedEventArgs>> numberChangedTokenTable = null;        //numberChangedTokenTable Stores mappings between delegates and event tokens

        //Method to raising QualityChanged event
        internal void OnQualityChanged()
        {
            EventRegistrationTokenTable<EventHandler<QualityChangedEventArgs>>
            .GetOrCreateEventRegistrationTokenTable(ref numberChangedTokenTable)
            .InvocationList?.Invoke(this, new QualityChangedEventArgs(oldQuality, Quality));
        }

        //Method to AddEventHandler and RemoveEventHandler
        public event EventHandler<QualityChangedEventArgs> QualityChanged
        {
            add
            {
                EventRegistrationTokenTable<EventHandler<QualityChangedEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref numberChangedTokenTable)
                    .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<QualityChangedEventArgs>>
                    .GetOrCreateEventRegistrationTokenTable(ref numberChangedTokenTable)
                    .RemoveEventHandler(value);
            }
        }

        //To change Visibility for CompactOverlayButton
        public bool IsCompactOverlayButtonVisible
        {
            get
            {
                return compactOverlayButton != null && compactOverlayButton.Visibility == Visibility.Visible;
            }
            set
            {
                if (compactOverlayButton != null)       //To neglect the Visibility check before the Template has been applied
                {
                    compactOverlayButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public static readonly DependencyProperty CompactOverlayButtonVisibilityProperty = DependencyProperty.Register(nameof(CompactOverlayButtonVisibility), typeof(Visibility), typeof(CustomMediaTransportControls), new PropertyMetadata(Visibility.Visible, OnVisibisityChanged));

        public Visibility CompactOverlayButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(CompactOverlayButtonVisibilityProperty);
            }
            set
            {
                SetValue(CompactOverlayButtonVisibilityProperty, value);
            }
        }

        internal static void OnVisibisityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((CustomMediaTransportControls)d).compactOverlayButton != null)
            {
                if ((Visibility)e.NewValue != Visibility.Visible)
                    ((CustomMediaTransportControls)d).commandBar.PrimaryCommands.Remove(((CustomMediaTransportControls)d).compactOverlayButton);
                else if (!((CustomMediaTransportControls)d).commandBar.PrimaryCommands.Contains(((CustomMediaTransportControls)d).compactOverlayButton))
                    ((CustomMediaTransportControls)d).commandBar.PrimaryCommands.Insert(4, ((CustomMediaTransportControls)d).compactOverlayButton);
                ((CustomMediaTransportControls)d).compactOverlayButton.Visibility = Visibility.Visible;
            }
        }
    }

    //Class for QualityChangedEventArgs
    public class QualityChangedEventArgs : EventArgs
    {
        //Initializing OldQuality and NewQuality property
        public YouTubeQuality OldQuality { get; set; }
        public YouTubeQuality NewQuality { get; set; }

        //Method to assign OldQuality and NewQuality
        public QualityChangedEventArgs(YouTubeQuality oldValue, YouTubeQuality newValue)
        {
            OldQuality = oldValue;
            NewQuality = newValue;
        }
    }
}
