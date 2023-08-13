using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordApi.MusicHandler
{
    public class PlayMusic
    {
        public Process ffmpeg = null;
        public readonly string filePath;

       
        private PlayMusic(string filePath)
        {
            this.filePath = filePath;
        }

        public Stream ConvertAudioToPcm()
        {
            string arguments = $@"-i ""{filePath}"" -ac 2 -f s16le -ar 48000 pipe:1";
            ffmpeg = Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Users\User\Documents\ffmpeg-master-latest-win64-gpl-shared\bin\ffmpeg.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false
            });

            return ffmpeg.StandardOutput.BaseStream;
        }

    }
}
