﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using Aubio.NET.Vectors;
using JetBrains.Annotations;

namespace Aubio.NET.Detection
{
    /// <summary>
    ///     https://aubio.org/doc/latest/notes_8h.html
    /// </summary>
    public sealed class Notes : AubioObject
    {
        #region Fields

        [NotNull]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly unsafe Notes__* _notes;

        #endregion

        #region Constructors

        [PublicAPI]
        public unsafe Notes(int bufferSize = 1024, int hopSize = 256, int sampleRate = 44100)
        {
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            if (hopSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(hopSize));

            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException(nameof(sampleRate));

            var notes = new_aubio_notes("default", bufferSize.ToUInt32(), hopSize.ToUInt32(), sampleRate.ToUInt32());
            if (notes == null)
                throw new ArgumentNullException(nameof(notes));

            _notes = notes;
        }

        #endregion

        #region Public Members

        [PublicAPI]
        public float CentPrecision
        {
            get
            {
                return aubio_notes_get_cent_precision(this);
            }
            set
            {
                ThrowIfNot(aubio_notes_set_cent_precision(this, value));
            }
        }

        [PublicAPI]
        public Time MinimumInterOnsetInterval
        {
            get
            {
                var milliseconds = aubio_notes_get_minioi_ms(this);
                var time = Time.FromMilliseconds(SampleRate, milliseconds);
                return time;
            }
            set
            {
                ThrowIfNot(aubio_notes_set_minioi_ms(this, value.Milliseconds));
            }
        }

        [PublicAPI]
        public int SampleRate
        {
            get
            {
                return aubio_notes_get_samplerate(this).ToInt32();
            }
        }

        [PublicAPI]
        public float Silence
        {
            get
            {
                return aubio_notes_get_silence(this);
            }
            set
            {
                ThrowIfNot(aubio_notes_set_silence(this, value));
            }
        }

        [PublicAPI]
        public void Do([NotNull] FVec input, [NotNull] FVec output)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            aubio_notes_do(this, input, output);
        }

        #endregion

        #region AubioObject Members

        protected override void DisposeNative()
        {
            del_aubio_notes(this);
        }

        internal override unsafe IntPtr ToPointer()
        {
            return new IntPtr(_notes);
        }

        #endregion

        #region Native methods

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe Notes__* new_aubio_notes(
            [MarshalAs(UnmanagedType.LPStr)] string method,
            uint bufferSize,
            uint hopSize,
            uint sampleRate
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        private static extern void del_aubio_notes(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        private static extern void aubio_notes_do(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] FVec input,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] FVec output
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        private static extern float aubio_notes_get_minioi_ms(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        private static extern float aubio_notes_get_cent_precision(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        private static extern float aubio_notes_get_silence(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint aubio_notes_get_samplerate(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool aubio_notes_set_cent_precision(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance,
            float precision
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool aubio_notes_set_minioi_ms(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance,
            float minioi
        );

        [SuppressUnmanagedCodeSecurity]
        [DllImport("aubio", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool aubio_notes_set_silence(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(AubioObjectMarshaler))] Notes instance,
            float silence
        );

        #endregion
    }
}