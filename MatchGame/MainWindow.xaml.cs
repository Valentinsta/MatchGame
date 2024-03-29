﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/*
 * # Improvement ides
 * - track time
 * - time limit
 * - add feedback (red on failed match / green before hidding)
 * - add shuffle button
 * - add effect on select
 * - add Pause / Resume button
 * - add WIN / GAME OVER
 */
using System.Windows.Threading;

namespace MatchGame
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private int tenthsOfSecondsElapsed;
        private int matchesFound;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text += " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐘", "🐘",
                "🐍", "🐍",
                "🐖", "🐖",
                "🐐", "🐐",
                "🐎", "🐎",
                "🐕", "🐕",
                "🐈", "🐈",
                "🐇", "🐇",
            };
            
            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                    if (textBlock.Visibility == Visibility.Hidden)
                    {
                        textBlock.Visibility = Visibility.Visible;
                    }
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        private TextBlock lastTextBlockClicked;
        private bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                lastTextBlockClicked = textBlock;
                lastTextBlockClicked.Visibility = Visibility.Hidden;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
