using MyToolkit.Multimedia;
using System;
using System.Threading.Tasks;
using UWPYouTubePlayer.Themes;
using Windows.Media.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYouTubePlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoPlayerPage : Page
    {
        public VideoPlayerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is YoutubePlayerInfo YPI)
            {
                YoutubePlayer.Source = MediaSource.CreateFromUri(YPI.VideoUrl.Uri);
                YoutubePlayer.MediaPlayer.PlaybackSession.Position = YPI.StartingPosition;
            }
            YoutubePlayer.MediaPlayer.Play();
        }

        public async Task setVideoSourceAsync(YouTubeQuality videoQuality)
        {
            try
            {
                var youtubeUrl = await YouTube.GetVideoUriAsync("QTYVJhy04rs", YouTubeQuality.Quality144P, videoQuality);
                YoutubePlayer.Source = MediaSource.CreateFromUri(youtubeUrl.Uri);
                YoutubePlayer.AutoPlay = true;
            }
            catch (Exception)
            {
                //await setVideoSourceAsync();
            }
        }

        private async void CustomMediaControl_QualityChangedAsync(object sender, QualityChangedEventArgs e)
        {
            await setVideoSourceAsync(e.NewQuality);
        }
    }
}
