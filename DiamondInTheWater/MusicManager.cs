using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater
{
    public class MusicManager
    {
        public List<Song> songs;
        private Random rand;
        public MusicManager()
        {
            rand = new Random();
            songs = new List<Song>();
        }
        public void Load(ContentManager Content)
        {
            for (int i = 1; i <= 4; i++)
            {
                songs.Add(Content.Load<Song>("" + i));
            }
        }

        public void Update()
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                int i = rand.Next(0, 4);
                MediaPlayer.Play(songs[i]);
            }
        }

        public void ToggleMute()
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
        }
    }
}
