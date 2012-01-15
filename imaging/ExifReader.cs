using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace libjfunx.imaging
{
    /// <summary>
    /// Klasse, um die Exif Daten eines Bildes auszulesen
    /// </summary>
    public class ExifReader
    {
        /// <summary>
        /// Gibt das Aufnahmedatum eines Bildes aus den EXIF-Daten zurück
        /// </summary>
        /// <param name="image"></param>
        /// <returns>string</returns>
        public string ReadExifDate(Image image)
        {
            PropertyItem[] items = image.PropertyItems;
            foreach (PropertyItem pi in items)
            {
                if (pi.Id == 306)
                {
                    string val =
                        System.Text.Encoding.Default.GetString
                            (pi.Value);
                    return val;
                }
            }
            return "n/a";
        }

        /// <summary>
        /// Gibt das Aufnahmedatum eines Bildes aus den EXIF-Daten zurück
        /// </summary>
        /// <param name="image"></param>
        /// <returns>string</returns>
        public string ReadExifDate(string ImagePath)
        {
            Image img = Image.FromFile(ImagePath);
            string date = ReadExifDate(img);
            img.Dispose();
            return date;
        }
    }
}
