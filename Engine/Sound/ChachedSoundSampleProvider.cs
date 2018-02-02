﻿using System;
using NAudio.Wave;

namespace Engine.Sound {
	internal class CachedSoundSampleProvider : ISampleProvider {
		private readonly CachedSound cachedSound;
		private long position;

		internal CachedSoundSampleProvider(CachedSound cachedSound) => this.cachedSound = cachedSound;

		public int Read(float[] buffer, int offset, int count) {
			var availableSamples = cachedSound.AudioData.Length - position;
			var samplesToCopy = Math.Min(availableSamples, count);
			Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
			position += samplesToCopy;
			return (int) samplesToCopy;
		}

		public WaveFormat WaveFormat => cachedSound.WaveFormat;
	}
}