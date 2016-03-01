// Author: Anastasia Mukalled
// Дата: 29.02.2016
// This file contains the description of the application data model class Comment

using System;

namespace AdventurePRO.Model
{
    /// <summary>
    /// Describes a user comment to smth. described as an OnlineDescribed object 
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Author`s nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Author`s avatar
        /// </summary>
        public Uri Avatar { get; set; }

        /// <summary>
        /// Comment text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Comment publication date
        /// </summary>
        public DateTime Date { get; set; }
    }
}