using System;

namespace MegaDesk_6_DaleVictor
{
    enum DesktopMaterial
    {
        Laminate = 100,
        Oak = 200,
        Rosewood = 300,
        Veneer = 125,
        Pine = 50
    }
    class Desk
    {
        // class members
        public int width { get; set; }
        public int depth { get; set; }
        public int drawers { get; set; }
        public DesktopMaterial surface { get; set; }

        // constructor method
        public Desk(int wide, int deep, int draw, DesktopMaterial surface)
        {
            width = wide;
            depth = deep;
            drawers = draw;
            this.surface = surface;
        }
    }
}
