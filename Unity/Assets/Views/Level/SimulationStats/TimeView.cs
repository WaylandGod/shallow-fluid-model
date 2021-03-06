﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Views.Level.SimulationStats
{
    /// <summary>
    /// Displays how much time has passed in the simulation and what the timescale is.
    /// </summary>
    public class TimeView
    {
        private List<int> _stepCountHistory;
        private List<float> _updateTimeHistory;

        private readonly int _sizeOfAveragingWindow;
        private readonly float _timestep;

        public TimeView(int sizeOfAveragingWindow, double timestep)
        {
            _sizeOfAveragingWindow = sizeOfAveragingWindow;
            _timestep = (float) timestep;

            _stepCountHistory = new List<int>(sizeOfAveragingWindow+1);
            _updateTimeHistory = new List<float>(sizeOfAveragingWindow+1);
        }

        public void Update(int numberOfSteps)
        {
            var time = Time.timeSinceLevelLoad;

            _stepCountHistory.Add(numberOfSteps);
            _updateTimeHistory.Add(time);

            if (_stepCountHistory.Count > _sizeOfAveragingWindow)
            {
                _stepCountHistory = _stepCountHistory.Skip(1).ToList();
                _updateTimeHistory = _updateTimeHistory.Skip(1).ToList();
            }
        }

        public void OnGUI()
        {
            if (_stepCountHistory.Count >= 2)
            {
                var changeInNumberOfSteps = _stepCountHistory.Last() - _stepCountHistory.First();
                var changeInTime = _updateTimeHistory.Last() - _updateTimeHistory.First();

                var timeDilation = _timestep*changeInNumberOfSteps/changeInTime;

                var labelText = FormatTimeDilation(timeDilation) + "\n" + FormatCurrentTime(_timestep*_stepCountHistory.Last());

                var rightAlign = new GUIStyle { alignment = TextAnchor.UpperRight, normal = new GUIStyleState { textColor = Color.black }};
                GUI.Label(new Rect(Screen.width - 280, 10, 270, 50), labelText, rightAlign);
            }
        }

        private static string FormatTimeDilation(double timeDilation)
        {
            var timespan = new TimeSpan(0, 0, (int) timeDilation);

            var output = String.Format("{0} hours per real-time second", timespan);

            return output;
        }

        private static string FormatCurrentTime(double updateTime)
        {
            var timespan = new TimeSpan(0, 0, (int)updateTime);

            var output = String.Format("{0} days since start", timespan);

            return output;
        }
    }
}
