using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordApi.LeagueApi.Services
{
    public class PlayMusicService : IDisposable
    {
        private DirectoryInfo musicDirectory; 
        private Process playProcess = null;
        //settigns
        private VoiceNextExtension _vnext;
        public VoiceNextConnection _connection;
        private VoiceTransmitSink _transmit;
        //tokens
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        public PlayMusicService(CommandContext context)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Configure(context);
        }

        public async Task Play(int countRepeat)
        {
            this._cancellationTokenSource = new CancellationTokenSource();
            
            for (int i = 0; i < countRepeat; i++) 
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
                await PlayOneRandomSong();
            }
        }

        private async Task PlayOneRandomSong()
        {
            //рандомим музыкальную webmку
            int rnd = new Random().Next(0, this.musicDirectory.EnumerateFiles().Count() - 1);
            //получаем путь к этой вебмке
            string path = this.musicDirectory.GetFiles().ElementAt(rnd).FullName;

            var pcm = ConvertAudioToPcm(path);
            await pcm.CopyToAsync(_transmit);
            await pcm.DisposeAsync();
        } 

        private Stream ConvertAudioToPcm(string filePath)
        {
            string arguments = $@"-i ""{filePath}"" -ac 2 -f s16le -ar 48000 pipe:1";
            playProcess = Process.Start(new ProcessStartInfo
            {
                FileName = @"C:\Users\User\Documents\ffmpeg-master-latest-win64-gpl-shared\bin\ffmpeg.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false
            });
            return playProcess.StandardOutput.BaseStream;
        }

        private void Configure(CommandContext context)
        {
            this.musicDirectory = new DirectoryInfo(@"C:\Users\User\Videos\туч\музыка");
            this._vnext = context.Client.GetVoiceNext();
            this._connection = this._vnext?.GetConnection(context.Guild);
            this._transmit = this._connection?.GetTransmitSink();
        }
        private void StopPlay()
        {
            this._cancellationTokenSource.Cancel();
            playProcess?.Kill();
        }
        public void Dispose()
        {
            StopPlay();
            _connection.Disconnect();
            this._vnext?.Dispose();
            this._connection?.Dispose();
            this._transmit?.Dispose();
        }
        public void Disconnect()
        {
            this._connection.Disconnect();
        }
        public void Connect(CommandContext context)
        {
            this._connection = this._vnext?.GetConnection(context.Guild);
        }
    }
}
