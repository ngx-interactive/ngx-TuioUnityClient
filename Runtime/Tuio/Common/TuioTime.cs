﻿using System;
using OSC.NET;

namespace Tuio.Common
{
    public class TuioTime
    {
        /// <summary>
        /// The start time of the session.
        /// </summary>
        private static TuioTime _startTime;

        /// <summary>
        /// The time since session started in seconds.
        /// </summary>
        private readonly long _seconds;
        
        /// <summary>
        /// Time fraction in microseconds.
        /// </summary>
        private readonly long _microseconds;

        public TuioTime(long seconds, long microseconds)
        {
            _seconds = seconds;
            _microseconds = microseconds;
        }

        public static TuioTime FromOscTime(OscTimeTag oscTimeTag)
        {
            return new TuioTime(oscTimeTag.SecondsSinceEpoch, oscTimeTag.FractionalSecond);
        }

        /// <summary>
        /// Sums the provided time value represented in total microseconds to the base TuioTime.
        /// </summary>
        /// <param name="time">The base TuioTime.</param>
        /// <param name="microseconds">The total time to add in microseconds.</param>
        /// <returns>The sum of this TuioTime with the provided time in microseconds.</returns>
        public static TuioTime operator +(TuioTime time, long microseconds)
        {
            long sec = time._seconds + microseconds / 1000000;
            long microsec = time._microseconds + microseconds % 1000000;
            return new TuioTime(sec, microsec);
        }

        /// <summary>
        /// Sums the provided TuioTime to the base TuioTime.
        /// </summary>
        /// <param name="timeA">The base TuioTime.</param>
        /// <param name="timeB">The TuioTime to add.</param>
        /// <returns>Sum of this TuioTime with the provided TuioTime.</returns>
        public static TuioTime operator +(TuioTime timeA, TuioTime timeB)
        {
            long sec = timeA._seconds + timeB._seconds;
            long microsec = timeA._microseconds + timeB._microseconds;
            sec += microsec / 1000000;
            microsec %= 1000000;
            return new TuioTime(sec, microsec);
        }
        
        /// <summary>
        /// Subtracts the provided time represented in microseconds from the base TuioTime.
        /// </summary>
        /// <param name="time">The base TuioTime.</param>
        /// <param name="microseconds">The total time to subtract in microseconds.</param>
        /// <returns>The subtraction result of this TuioTime minus the provided time in microseconds.</returns>
        public static TuioTime operator -(TuioTime time, long microseconds)
        {
            long sec = time._seconds - microseconds / 1000000;
            long microsec = time._microseconds - microseconds % 1000000;
            if (microsec < 0)
            {
                microsec += 1000000;
                sec--;
            }

            return new TuioTime(sec, microsec);
        }

        /// <summary>
        /// Subtracts the provided TuioTime from the base TuioTime.
        /// </summary>
        /// <param name="timeA">The base TuioTime.</param>
        /// <param name="timeB">The TuioTime to subtract.</param>
        /// <returns>The subtraction result of this TuioTime minus the provided TuioTime.</returns>
        public static TuioTime operator -(TuioTime timeA, TuioTime timeB)
        {
            long sec = timeA._seconds - timeB._seconds;
            long microsec = timeA._microseconds - timeB._microseconds;
            if (microsec < 0)
            {
                microsec += 1000000;
                sec--;
            }

            return new TuioTime(sec, microsec);
        }
        
        /// <summary>
        /// Check for equality of this TuioTime and a given one.
        /// </summary>
        /// <param name="time">The TuioTime to compare</param>
        /// <returns>True if the TuioTime are equal in seconds and microseconds.</returns>
        public bool Equals(TuioTime time)
        {
            return (_seconds == time._seconds) && (_microseconds == time._microseconds);
        }
        
        /// <summary>
        /// Compare two TuioTimes for equality.
        /// </summary>
        /// <param name="timeA">The first TuioTime.</param>
        /// <param name="timeB">The second TuioTime.</param>
        /// <returns>True if both TuioTimes are equal in seconds and microseconds.</returns>
        public static bool operator ==(TuioTime timeA, TuioTime timeB)
        {
            return timeA.Equals(timeB);
        }
    
        /// <summary>
        /// Compare two TuioTimes for inequality.
        /// </summary>
        /// <param name="timeA">The first TuioTime.</param>
        /// <param name="timeB">The second TuioTime.</param>
        /// <returns>True if both TuioTimes are unequal in seconds or microseconds.</returns>
        public static bool operator !=(TuioTime timeA, TuioTime timeB)
        {
            return !(timeA == timeB);
        }

        public TuioTime Subtract(TuioTime other)
        {
            var seconds = _seconds - other._seconds;
            var microseconds = _microseconds - other._microseconds;

            if (microseconds < 0)
            {
                microseconds += 1000000;
                seconds -= 1;
            }

            return new TuioTime(seconds, microseconds);
        }

        public long GetTotalMilliseconds()
        {
            return 1000 * (long) _seconds + _microseconds / 1000;
        }

        public static void Init()
        {
            _startTime = GetSystemTime();
        }

        public static TuioTime GetSystemTime()
        {
            OscTimeTag oscTimeTag = new OscTimeTag(DateTime.Now);
            return FromOscTime(oscTimeTag);
        }

        public static TuioTime GetCurrentTime()
        {
            return GetSystemTime().Subtract(_startTime);
        }
    }
}