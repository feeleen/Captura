﻿using System;
using Screna.Audio;
using System.Diagnostics;
using System.IO;

namespace Captura
{
    class FFMpegAudioWriter : IAudioFileWriter
    {
        Process ffmpegProcess;
        Stream ffmpegIn;

        public FFMpegAudioWriter(string FileName)
        {
            ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = "ffmpeg.exe";
            ffmpegProcess.StartInfo.Arguments = $"-f s16le -acodec pcm_s16le -ar 44100 -ac 2 -i - -vn -acodec mp3 {FileName}";
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.CreateNoWindow = true;
            ffmpegProcess.StartInfo.RedirectStandardInput = true;

            ffmpegProcess.Start();

            ffmpegIn = ffmpegProcess.StandardInput.BaseStream;
        }

        public void Dispose()
        {
            Flush();

            ffmpegIn.Close();
            ffmpegProcess.WaitForExit();
        }

        public void Flush()
        {
            ffmpegIn.Flush();
        }

        public void Write(byte[] Data, int Offset, int Count)
        {
            ffmpegIn.Write(Data, Offset, Count);
        }
    }
}
