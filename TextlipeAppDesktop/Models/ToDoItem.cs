using System;
using System.Collections.Generic;
using System.Text;

namespace TextlipeAppDesktop.Models
{
    /// <summary>
    /// This is Model for a simple ToDoItem
    /// </summary>
    public class ToDoItem
    {
        /// <summary>
        /// Gets or sets the checked status of each item
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets the content of the to-do item
        /// </summary>
        public string? Content { get; set; }
    }
}
