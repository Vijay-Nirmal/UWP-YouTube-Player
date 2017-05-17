using MyToolkit.Multimedia;
using System;
using System.Threading.Tasks;
using UWPYouTubePlayer.Themes;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPYouTubePlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        YoutubePlayerInfo YPI = new YoutubePlayerInfo();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    YoutubePlayer?.MediaPlayer.Dispose();
                }
                catch (Exception) { }
            });
        }

        public async Task setVideoSourceAsync(YouTubeQuality videoQuality)
        {

            try
            {
                var youtubeUrl = await YouTube.GetVideoUriAsync("QTYVJhy04rs", YouTubeQuality.Quality144P, videoQuality);
                YPI.VideoUrl = youtubeUrl;
                YoutubePlayer.Source = MediaSource.CreateFromUri(youtubeUrl.Uri);
                YoutubePlayer.AutoPlay = true;
            }
            catch (Exception)
            {
                //await setVideoSourceAsync();
            }
        }

        private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
                CustomMediaControl.IsCompactOverlayButtonVisible = true;
            else
                CustomMediaControl.IsCompactOverlayButtonVisible = false;

            await setVideoSourceAsync(YouTubeQuality.Quality360P);
        }

        private async void CustomMediaControl_QualityChangedAsync(object sender, QualityChangedEventArgs e)
        {
            await setVideoSourceAsync(e.NewQuality);
        }

        private async void CustomMediaControl_CompactOverlaidAsync(object sender, EventArgs e)
        {
            //Get current playback position
            YPI.StartingPosition = YoutubePlayer.MediaPlayer.PlaybackSession.Position;

            int compactViewId = 0;
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                compactViewId = ApplicationView.GetForCurrentView().Id;
                frame.Navigate(typeof(VideoPlayerPage), YPI);
                Window.Current.Content = frame;
                Window.Current.Activate();
                ApplicationView.GetForCurrentView().Title = "";
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsViewModeAsync(compactViewId, ApplicationViewMode.CompactOverlay);
            //YoutubePlayer.MediaPlayer.Dispose();
        }
    }

    public class YoutubePlayerInfo
    {
        public TimeSpan StartingPosition { get; set; }
        public YouTubeUri VideoUrl { get; set; }
    }
}
