using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace libjfunx.imaging
{
    public class ImageResizer
    {
        /// <summary>
        /// Ändert die Größe eines Bildes und setzt eine Hintergrundfarbe.
        /// </summary>
        /// <param name="Source">Originalbild als Image</param>
        /// <param name="Width">Neue Breite in Pixel</param>
        /// <param name="Height">Neue Höhe in Pixel</param>
        /// <param name="Absolut">Wenn true wird ein Image mit exakt den angegebenen Maßen erstellt. Das Bild wird mittig plaziert und mit der angegebenen Farbe gefüllt. Seitenverhältnisse des Bildes selbst bleiben erhalten. Wenn False dann wird entsprechend dem kleinem Faktor das neue Bild berechnet (neues Bild hat dann aber ggfs. NICHT die angegebenen Maße!)</param>
        /// <param name="Fill">Füllfarbe für Hintergrund (vorallem wichtig bei Absolut = true oder Transparenten Bildern (PNG)</param>
        /// <param name="MaxFactorX">Maximaler Vergrößerungsfaktor X-Achse. Wenn aus Width = 10 z.B. 1000 werden soll, so kann der Wert 2 ein maximales verdoppeln ermöglichen. Das Bild hat dann Width = 20. -1 bedeutet kein Mindestfaktor</param>
        /// <param name="MaxFactorY">Maximaler Vergrößerungsfaktor Y-Achse. Wenn aus Height = 10 z.B. 1000 werden soll, so kann der Wert 2 ein maximales verdoppeln ermöglichen. Das Bild hat dann Height = 20. -1 bedeutet kein Mindestfaktor</param>
        /// <returns>Bitmap / Image</returns>
        private static Image ResizePic(Image Source, int Width, int Height, bool Absolut, Color Fill, double MaxFactorX, double MaxFactorY)
        {
            if (Source != null)
            {
                // Faktoren für X und Y Achse berechnen
                double dblFaktorX = (double)Width / (double)Source.Width;
                if (dblFaktorX > MaxFactorX & MaxFactorX > -1)
                {
                    Width = (int)((double)Source.Width * MaxFactorX);
                    dblFaktorX = MaxFactorX;
                }

                double dblFaxtorY = (double)Height / (double)Source.Height;
                if (dblFaxtorY > MaxFactorY & MaxFactorY > -1)
                {
                    Height = (int)((double)Source.Height * MaxFactorY);
                    dblFaxtorY = MaxFactorY;
                }

                // kleinern Faktor benutzen
                double dblFaktorUse;
                if (dblFaktorX < dblFaxtorY)
                    dblFaktorUse = dblFaktorX;
                else
                    dblFaktorUse = dblFaxtorY;

                int intNewX = (int)((double)Source.Width * dblFaktorUse);
                int intNewY = (int)((double)Source.Height * dblFaktorUse);

                Bitmap objNewImage;
                int intPosX, intPosY;

                if (Absolut)
                {
                    objNewImage = new Bitmap(Width, Height);
                    intPosX = (Width - intNewX) / 2;
                    intPosY = (Height - intNewY) / 2;
                }
                else
                {
                    objNewImage = new Bitmap(intNewX, intNewY);
                    intPosX = 0;
                    intPosY = 0;
                }

                using (Graphics objGfx = Graphics.FromImage(objNewImage))
                {
                    objGfx.FillRectangle(new SolidBrush(Fill), 0, 0, objNewImage.Width, objNewImage.Height);
                    objGfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    objGfx.DrawImage(Source, new Rectangle(intPosX, intPosY, intNewX, intNewY));
                }
                return objNewImage;
            }
            else
                return null;
        }


        /// <summary>
        /// Ändert die Größe eines Bildes und setzt eine Hintergrundfarbe.
        /// </summary>
        /// <param name="Source">Originalbild als Image</param>
        /// <param name="Width">Neue Breite in Pixel</param>
        /// <param name="Height">Neue Höhe in Pixel</param>
        /// <param name="Absolut">Wenn true wird ein Image mit exakt den angegebenen Maßen erstellt. Das Bild wird mittig plaziert und mit der angegebenen Farbe gefüllt. Seitenverhältnisse des Bildes selbst bleiben erhalten. Wenn False dann wird entsprechend dem kleinem Faktor das neue Bild berechnet (neues Bild hat dann aber ggfs. NICHT die angegebenen Maße!)</param>
        /// <param name="Fill">Füllfarbe für Hintergrund (vorallem wichtig bei Absolut = true oder Transparenten Bildern (PNG)</param>
        /// <param name="MaxFactorX">Maximaler Vergrößerungsfaktor X-Achse. Wenn aus Width = 10 z.B. 1000 werden soll, so kann der Wert 2 ein maximales verdoppeln ermöglichen. Das Bild hat dann Width = 20. -1 bedeutet kein Mindestfaktor</param>
        /// <param name="MaxFactorY">Maximaler Vergrößerungsfaktor Y-Achse. Wenn aus Height = 10 z.B. 1000 werden soll, so kann der Wert 2 ein maximales verdoppeln ermöglichen. Das Bild hat dann Height = 20. -1 bedeutet kein Mindestfaktor</param>
        /// <returns>Bitmap / Image</returns>        
        public static Image ResizePic(String Source, int Width, int Height, bool Absolut, Color Fill, double MaxFactorX, double MaxFactorY)
        {
            Image img = Image.FromFile(Source);
            return ResizePic(img, Width, Height, Absolut, Fill, MaxFactorX, MaxFactorY);
        }

        public static Image ResizePic(FileInfo Source, int Width, int Height, bool Absolut, Color Fill, double MaxFactorX, double MaxFactorY)
        {
            Image img = Image.FromFile(Source.FullName);
            return ResizePic(img, Width, Height, Absolut, Fill, MaxFactorX, MaxFactorY);
        }

        public static string ResizeAndSavePic(FileInfo Source, String Destination, int Width, int Height)
        {
            Image img = ResizePic(Source.FullName, Width, Height, false, Color.Transparent, -1, -1);
            string DestinationFileName = Destination + Source.Name;
            img.Save(DestinationFileName);
            return DestinationFileName;
        }

    }


}
