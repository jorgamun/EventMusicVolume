using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StardewValley;
using StardewModdingAPI;

namespace EventMusicVolume
{
    public class EventMusicVolume : Mod
    {
        public float musicOutsideEvent;
        public float musicInsideEvent;

        public bool eventTriggered = false;

        public ModConfig config;

        public override void Entry(IModHelper helper)
        {
            //initialize and validate volume levels
            config = helper.ReadConfig<ModConfig>();
            musicOutsideEvent = (float)config.MusicOutsideEvent / 100f;
            musicInsideEvent = (float)config.MusicInsideEvent / 100f;

            musicOutsideEvent = Math.Min(1, musicOutsideEvent);
            musicOutsideEvent = Math.Max(0, musicOutsideEvent);

            musicInsideEvent = Math.Min(1, musicInsideEvent);
            musicInsideEvent = Math.Max(0, musicInsideEvent);

            helper.Events.GameLoop.UpdateTicking += GameLoop_UpdateTicking;
            helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;

        }

        private void GameLoop_DayStarted(object? sender, StardewModdingAPI.Events.DayStartedEventArgs e)
        {
            Game1.options.musicVolumeLevel = musicOutsideEvent;
            Game1.musicCategory.SetVolume(musicOutsideEvent);
            Game1.musicPlayerVolume = musicOutsideEvent;

            eventTriggered = false;
        }

        private void GameLoop_UpdateTicking(object? sender, StardewModdingAPI.Events.UpdateTickingEventArgs e)
        {

           if(Game1.eventUp)
            {
                Game1.options.musicVolumeLevel = musicInsideEvent;
                Game1.musicCategory.SetVolume(musicInsideEvent);
                Game1.musicPlayerVolume = musicInsideEvent;

                eventTriggered = true;
            }
           else if(eventTriggered)
            {
                Game1.options.musicVolumeLevel = musicOutsideEvent;
                Game1.musicCategory.SetVolume(musicOutsideEvent);
                Game1.musicPlayerVolume = musicOutsideEvent;

                eventTriggered = false;
            }
        }
    }

    public class ModConfig
    {
        public int MusicOutsideEvent { get; set; } = 0;
        public int MusicInsideEvent { get; set; } = 75;

    }
}
