/*
 * Copyright (c) InWorldz Halcyon Developers
 * Copyright (c) Contributors, http://opensimulator.org/
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSim Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Drawing;
using System.Reflection;
using log4net;
using Nini.Config;
using OpenSim.Region.Framework.Scenes;

namespace OpenSim.Region.CoreModules.World.WorldMap
{
    public class ShadedMapTileRenderer : IMapTileTerrainRenderer
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Scene m_scene;

        public void Initialize(Scene scene, IConfigSource config)
        {
            m_scene = scene;
        }

        public void TerrainToBitmap(DirectBitmap mapbmp)
        {
            int tc = Environment.TickCount;
            m_log.Info("[Map Tile]: Generating Maptile Step 1: Terrain (Shaded)");

            double[,] hm = m_scene.Heightmap.GetDoubles();
            bool ShadowDebugContinue = true;

            bool terraincorruptedwarningsaid = false;

            float low = 255;
            float high = 0;

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    float hmval = (float)hm[x, y];

                    if (hmval < low)
                    {
                        low = hmval;
                    }

                    if (hmval > high)
                    {
                        high = hmval;
                    }
                }
            }

            float waterHeight = (float)m_scene.RegionInfo.RegionSettings.WaterHeight;

            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    // Y flip the cordinates for the bitmap: hf origin is lower left, bm origin is upper left
                    int yr = 255 - y;

                    float heightvalue = (float)hm[x, y];

                    if (heightvalue > waterHeight)
                    {
                        // scale height value
                        if (Single.IsInfinity(heightvalue) || Single.IsNaN(heightvalue))
                        {
                            heightvalue = 0;
                        }
                        else if (heightvalue > 255f)
                        {
                            heightvalue = 255f;
                        }
                        else if (heightvalue < 0f)
                        {
                            heightvalue = 0f;
                        }

                        Color color = Color.FromArgb((int)heightvalue, 100, (int)heightvalue);

                        mapbmp.Bitmap.SetPixel(x, yr, color);

                        try
                        {
                            /// <summary>
                            ///     X
                            ///     Shade the terrain for shadows
                            /// </summary>
                            if (x < 255 && yr < 255)
                            {
                                float hfvalue = (float)hm[x, y];
                                float hfvaluecompare = 0f;

                                if ((x + 1 < 256) && (y + 1 < 256))
                                {
                                    hfvaluecompare = (float)hm[x + 1, y + 1]; // light from north-east => look at land height there
                                }

                                if (Single.IsInfinity(hfvalue) || Single.IsNaN(hfvalue))
                                {
                                    hfvalue = 0f;
                                }

                                if (Single.IsInfinity(hfvaluecompare) || Single.IsNaN(hfvaluecompare))
                                {
                                    hfvaluecompare = 0f;
                                }

                                float hfdiff = hfvalue - hfvaluecompare;  // => positive if NE is lower, negative if here is lower

                                int hfdiffi = 0;
                                int hfdiffihighlight = 0;
                                float highlightfactor = 0.18f;

                                try
                                {
                                    hfdiffi = Math.Abs((int)(hfdiff * 4.5f)) + 1;

                                    if (hfdiff % 1f != 0)
                                    {
                                        hfdiffi = hfdiffi + Math.Abs((int)((hfdiff % 1f) * 5f) - 1);
                                    }

                                    hfdiffihighlight = Math.Abs((int)((hfdiff * highlightfactor) * 4.5f)) + 1;

                                    if (hfdiff % 1f != 0)
                                    {
                                        hfdiffihighlight = hfdiffihighlight + Math.Abs((int)(((hfdiff * highlightfactor) % 1f) * 5f) - 1);
                                    }
                                }
                                catch (OverflowException)
                                {
                                    m_log.Debug("[Map Tile]: Shadow failed at value: " + hfdiff.ToString());
                                    ShadowDebugContinue = false;
                                }

                                if (hfdiff > 0.3f)
                                {
                                    /// <summary>
                                    ///     NE is lower than here
                                    ///     We have to desaturate and lighten the land at the same
                                    ///     time we use floats, colors use bytes, so shrink our space
                                    ///     down to 0-255
                                    /// </summary>
                                    if (ShadowDebugContinue)
                                    {
                                        int r = color.R;
                                        int g = color.G;
                                        int b = color.B;
                                        color = Color.FromArgb((r + hfdiffihighlight < 255) ? r + hfdiffihighlight : 255,
                                                               (g + hfdiffihighlight < 255) ? g + hfdiffihighlight : 255,
                                                               (b + hfdiffihighlight < 255) ? b + hfdiffihighlight : 255);
                                    }
                                }
                                else if (hfdiff < -0.3f)
                                {
                                    /// <summary>
                                    ///     Here is lower then NE:
                                    ///     We have to desaturate and blacken the land at the same
                                    ///     time we use floats, colors use bytes, so shrink our space 
                                    ///     down to 0-255
                                    /// </summary>
                                    if (ShadowDebugContinue)
                                    {
                                        if ((x - 1 > 0) && (yr + 1 < 256))
                                        {
                                            color = mapbmp.Bitmap.GetPixel(x - 1, yr + 1);
                                            int r = color.R;
                                            int g = color.G;
                                            int b = color.B;
                                            color = Color.FromArgb((r - hfdiffi > 0) ? r - hfdiffi : 0,
                                                                   (g - hfdiffi > 0) ? g - hfdiffi : 0,
                                                                   (b - hfdiffi > 0) ? b - hfdiffi : 0);

                                            mapbmp.Bitmap.SetPixel(x-1, yr+1, color);
                                        }
                                    }
                                }
                            }
                        }
                        catch (ArgumentException)
                        {
                            if (!terraincorruptedwarningsaid)
                            {
                                m_log.WarnFormat("[Map Image]: Your terrain is corrupted in region {0}, it might take a few minutes to generate the map image depending on the corruption level", m_scene.RegionInfo.RegionName);
                                terraincorruptedwarningsaid = true;
                            }

                            color = Color.Black;
                            mapbmp.Bitmap.SetPixel(x, yr, color);
                        }
                    }
                    else
                    {
                        // We're under the water level with the terrain, so paint water instead of land

                        // Y flip the cordinates
                        heightvalue = waterHeight - heightvalue;

                        if (Single.IsInfinity(heightvalue) || Single.IsNaN(heightvalue))
                        {
                            heightvalue = 0f;
                        }
                        else if (heightvalue > 19f)
                        {
                            heightvalue = 19f;
                        }
                        else if (heightvalue < 0f)
                        {
                            heightvalue = 0f;
                        }

                        heightvalue = 100f - (heightvalue * 100f) / 19f;

                        try
                        {
                            Color water = Color.FromArgb((int)heightvalue, (int)heightvalue, 255);
                            mapbmp.Bitmap.SetPixel(x, yr, water);
                        }
                        catch (ArgumentException)
                        {
                            if (!terraincorruptedwarningsaid)
                            {
                                m_log.WarnFormat("[Map Image]: Your terrain is corrupted in region {0}, it might take a few minutes to generate the map image depending on the corruption level", m_scene.RegionInfo.RegionName);
                                terraincorruptedwarningsaid = true;
                            }

                            Color black = Color.Black;
                            mapbmp.Bitmap.SetPixel(x, (256 - y) - 1, black);
                        }
                    }
                }
            }

            m_log.Info("[Map Tile]: Generating Maptile Step 1: Done in " + (Environment.TickCount - tc) + " ms");
        }
    }
}