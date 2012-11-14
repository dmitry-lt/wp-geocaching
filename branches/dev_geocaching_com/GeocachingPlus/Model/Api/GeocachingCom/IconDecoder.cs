using System;
using System.Windows.Media.Imaging;

namespace GeocachingPlus.Model.Api.GeocachingCom
{
    public class IconDecoder
    {
        public static void parseMapPNG(GeocachingComCache cache, WriteableBitmap bitmap, UTFGridPosition xy, int zoomlevel) {
            if (zoomlevel >= 14) {
                parseMapPNG14(cache, bitmap, xy);
            } else {
                parseMapPNG13(cache, bitmap, xy);
            }
        }

        private static readonly int[] OFFSET_X = new int[] { 0, -1, -1, 0, 1, 1, 1, 0, -1, -2, -2, -2, -2, -1, 0, 1, 2, 2, 2, 2, 2, 1, 0, -1, -2 };
        private static readonly int[] OFFSET_Y = new int[] { 0, 0, 1, 1, 1, 0, -1, -1, -1, -1, 0, 1, 2, 2, 2, 2, 2, 1, 0, -1, -2, -2, -2, -2, -2 };

        /**
         * The icon decoder walks a spiral around the center pixel position of the cache
         * and searches for characteristic colors.
         *
         * @param cache
         * @param bitmap
         * @param xy
         */
        private static void parseMapPNG13(GeocachingComCache cache, WriteableBitmap bitmap, UTFGridPosition xy) {
            int xCenter = xy.x * 4 + 2;
            int yCenter = xy.y * 4 + 2;
            int bitmapWidth = bitmap.PixelWidth;
            int bitmapHeight = bitmap.PixelHeight;

            int countMulti = 0;
            int countFound = 0;

            for (int i = 0; i < OFFSET_X.Length; i++) {

                // assert that we are still in the tile
                int x = xCenter + OFFSET_X[i];
                if (x < 0 || x >= bitmapWidth) {
                    continue;
                }

                int y = yCenter + OFFSET_Y[i];
                if (y < 0 || y >= bitmapHeight) {
                    continue;
                }

                int color = bitmap.GetPixel(x, y).ToArgb() & 0x00FFFFFF;

                // transparent pixels are not interesting
                if (color == 0) {
                    continue;
                }

                int red = (color & 0xFF0000) >> 16;
                int green = (color & 0xFF00) >> 8;
                int blue = color & 0xFF;

                // these are quite sure, so one pixel is enough for matching
                if (green > 0x80 && green > red && green > blue) {
                    cache.Type = GeocachingComCache.Types.TRADITIONAL;
                    return;
                }
                if (blue > 0x80 && blue > red && blue > green) {
                    cache.Type = GeocachingComCache.Types.MYSTERY;
                    return;
                }
                if (red > 0x90 && blue < 0x10 && green < 0x10) {
                    cache.Type = GeocachingComCache.Types.EVENT;
                    return;
                }

                // next two are hard to distinguish, therefore we sample all pixels of the spiral
                if (red > 0xFA && green > 0xD0) {
                    countMulti++;
                }
                if (red < 0xF3 && red > 0xa0 && green > 0x20 && blue < 0x80) {
                    countFound++;
                }
            }

            // now check whether we are sure about found/multi
            if (countFound > countMulti && countFound >= 2) {
//                cache.setFound(true);
            }
            if (countMulti > countFound && countMulti >= 5) {
                cache.Type = GeocachingComCache.Types.MULTI;
            }
        }

        // Pixel colors in tile
        private const int COLOR_BORDER_GRAY = 0x5F5F5F;
        private const int COLOR_TRADITIONAL = 0x316013;
        private const int COLOR_MYSTERY = 0x243C97;
        private const int COLOR_MULTI = 0xFFDE19;
        private const int COLOR_FOUND = 0xFBEA5D;

        // Offset inside cache icon
        private const int POSX_TRADI = 7;
        private const int POSY_TRADI = -12;
        private const int POSX_MULTI = 5; // for orange 8
        private const int POSY_MULTI = -9; // for orange 10
        private const int POSX_MYSTERY = 5;
        private const int POSY_MYSTERY = -13;
        private const int POSX_FOUND = 10;
        private const int POSY_FOUND = -8;

        /**
         * For level 14 find the borders of the icons and then use a single pixel and color to match.
         *
         * @param cache
         * @param bitmap
         * @param xy
         */
        private static void parseMapPNG14(GeocachingComCache cache, WriteableBitmap bitmap, UTFGridPosition xy) {
            int x = xy.x * 4 + 2;
            int y = xy.y * 4 + 2;

            // search for left border
            int countX = 0;
            while ((bitmap.GetPixel(x, y).ToArgb() & 0x00FFFFFF) != COLOR_BORDER_GRAY) {
                if (--x < 0 || ++countX > 20) {
                    return;
                }
            }
            // search for bottom border
            int countY = 0;
            while ((bitmap.GetPixel(x, y).ToArgb() & 0x00FFFFFF) != 0x000000) {
                if (++y >= Tile.TILE_SIZE || ++countY > 20) {
                    return;
                }
            }

            try {
                if ((bitmap.GetPixel(x + POSX_TRADI, y + POSY_TRADI).ToArgb() & 0x00FFFFFF) == COLOR_TRADITIONAL)
                {
                    cache.Type = GeocachingComCache.Types.TRADITIONAL;
                    return;
                }
                if ((bitmap.GetPixel(x + POSX_MYSTERY, y + POSY_MYSTERY).ToArgb() & 0x00FFFFFF) == COLOR_MYSTERY)
                {
                    cache.Type = GeocachingComCache.Types.MYSTERY;
                    return;
                }
                if ((bitmap.GetPixel(x + POSX_MULTI, y + POSY_MULTI).ToArgb() & 0x00FFFFFF) == COLOR_MULTI)
                {
                    cache.Type = GeocachingComCache.Types.MULTI;
                    return;
                }
                if ((bitmap.GetPixel(x + POSX_FOUND, y + POSY_FOUND).ToArgb() & 0x00FFFFFF) == COLOR_FOUND)
                {
//                    cache.setFound(true);
                }
            } catch (Exception e) {
                // intentionally left blank
            }

        }

    }
}
