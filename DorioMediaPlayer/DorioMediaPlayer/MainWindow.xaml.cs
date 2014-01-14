using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DorioMediaPlayer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
	
    public class Media
    {
        private string filename;
        private Uri path;

        public override string ToString()
        {
            return this.filename;
        }

        public void setFilename(String filename)
        {
            this.filename = filename;
        }

        public string getFilename()
        {
            return this.filename;
        }

        public void setPath(Uri path)
        {
            this.path = path;
        }

        public Uri getPath()
        {
            return this.path;
        }
    }

    public partial class MainWindow : Window
    {
        private bool isPaused = false;
        private bool isPlaylist = false;
        private List<Media> listMedia = new List<Media>();
        private int currentItem;
		
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayMedia(object sender, System.Windows.RoutedEventArgs e)
        {
        	isPaused = false;
            isPlaylist = false;
			Player.Play();
        }

        private void StopMedia(object sender, System.Windows.RoutedEventArgs e)
        {
        	isPaused = false;
			Player.Stop();
        }

        private void PauseMedia(object sender, System.Windows.RoutedEventArgs e)
        {
        	if (isPaused)
			{
				Player.Play();
				isPaused = false;
			}
			else
			{
				Player.Pause();
				isPaused = true;
			}
        }

        private void OpenMedia(object sender, System.Windows.RoutedEventArgs e)
        {
			Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
			fileDialog.Filter = "Doit etre au format|*.wmv;*.avi;*.mpg;*.mkv;*.jpg;*.png;*.jpeg;*.mp4;*.mp3";
			if (fileDialog.ShowDialog() == true)
			{
				if (!String.IsNullOrEmpty(fileDialog.FileName))
				{
					Player.Source = new Uri(fileDialog.FileName, UriKind.Relative);
					Player.Stop();
					Player.Play();
					isPaused = false;
				}
			}
        }

        private void EndMedia(object sender, System.Windows.RoutedEventArgs e)
        {
            if (isPlaylist == true)
            {
                Player.Stop();
                if (Repeat.IsChecked == true)
                    Player.Play();
                else
                {
                    this.nextMedia();
                }
            }
            else
            {
                Player.Stop();
                if (Repeat.IsChecked == true)
                    Player.Play();
                isPaused = false;
            }
        }

        private void AddMediaToPlaylist(object sender, System.Windows.RoutedEventArgs e)
        {
			Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
			fileDialog.Filter = "Doit etre au format|*.wmv;*.avi;*.mpg;*.mkv;*.jpg;*.png;*.jpeg;*.mp4;*.mp3";
			if (fileDialog.ShowDialog() == true)
			{
				if (!String.IsNullOrEmpty(fileDialog.FileName))
				{
                    Media media = new Media();
                    media.setFilename(fileDialog.SafeFileName);
                    media.setPath(new Uri(fileDialog.FileName, UriKind.Relative));
					Playlist.Items.Add(media);
                    listMedia.Add(media);
				}
			}
        }

        private void LaunchPlaylistMedia(object sender, System.Windows.RoutedEventArgs e)
        {
            if (listMedia.Count > 0)
            {
                isPlaylist = true;
                currentItem = 0;
                Player.Source = listMedia[currentItem].getPath();
                Player.Stop();
                Player.Play();
                isPaused = false;
            }
        }

        private void nextMedia()
        {
            currentItem += 1;
            if (listMedia[currentItem] != null)
            {
                Player.Source = listMedia[currentItem].getPath();
                Player.Stop();
                Player.Play();
                isPaused = false;
            }
            else if(currentItem < listMedia.Count())
            {
                this.nextMedia();
            }
        }
        private void previousMedia()
        {
            currentItem -= 1;
            if (listMedia[currentItem] != null)
            {
                Player.Source = listMedia[currentItem].getPath();
                Player.Stop();
                Player.Play();
                isPaused = false;
            }
            else if (currentItem > 0)
            {
                this.previousMedia();
            }
        }

        private void ClearMediaPlaylist(object sender, System.Windows.RoutedEventArgs e)
        {
        	this.listMedia.Clear();
			Playlist.Items.Clear();
        }
    }
}
