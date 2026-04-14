using System.Collections.Generic;

namespace myGame.Model.Menu
{
    public class MenuModel
    {
        public int SelectedIndex { get; set; }
        public List<string> MenuItems { get; set; } = new List<string> { "Start", "Help", "Exit" };
    }
}