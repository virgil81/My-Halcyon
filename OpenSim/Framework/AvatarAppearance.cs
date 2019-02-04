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
 *     * Neither the name of the OpenSimulator Project nor the
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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using log4net;

namespace OpenSim.Framework
{
    /// <summary>
    /// Contains the Avatar's Appearance and methods to manipulate the appearance.
    /// </summary>
    public class AvatarAppearance
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public readonly static int VISUALPARAM_COUNT = 253;

        public readonly static byte VISUALPARAM_DEFAULT = 100;   // what to use for a default value? 0=alien, 150=fat scientist

        public readonly static int TEXTURE_COUNT = 21;
        public readonly static byte[] BAKE_INDICES = new byte[] { 8, 9, 10, 11, 19, 20 };

        // The viewer defines the initial/default/unknown appearance serial number (version)
        // in LLViewerInventoryCategory, as VERSION_INITIAL=-1 and VERSION_UNKNOWN=-1 (not zero).
        public readonly static int VERSION_INITIAL = -1;

        protected UUID m_owner;
        protected int m_serial = VERSION_INITIAL;
        protected byte[] m_visualparams;
        protected Primitive.TextureEntry m_texture;
        protected Dictionary<int, AvatarWearable> m_wearables;
        protected Dictionary<int, List<AvatarAttachment>> m_attachments;
        protected float m_avatarHeight = 0;
        protected float m_hipOffset = 0;

        public bool IsBotAppearance { get; set; }

        public virtual UUID Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public virtual int Serial
        {
            get { return m_serial; }
            set { m_serial = value; }
        }

        public virtual byte[] VisualParams
        {
            get { return m_visualparams; }
            set { m_visualparams = value; }
        }

        public virtual Primitive.TextureEntry Texture
        {
            get { return m_texture; }
            set { m_texture = value; }
        }

        public virtual float AvatarHeight
        {
            get { return m_avatarHeight; }
            set { m_avatarHeight = value; }
        }

        public virtual float HipOffset
        {
            get { return m_hipOffset; }
        }

        public static byte[] GetDefaultVisualParams()
        {
            byte[] visualParams = new byte[VISUALPARAM_COUNT];

            //            m_visualparams = new byte[] { 
            //                33,61,85,23,58,127,63,85,63,42,0,85,63,36,85,95,153,63,34,0,63,109,88,132,63,136,81,85,103,136,127,0,150,150,150,127,0,0,0,0,0,127,0,0,255,127,114,127,99,63,127,140,127,127,0,0,0,191,0,104,0,0,0,0,0,0,0,0,0,145,216,133,0,127,0,127,170,0,0,127,127,109,85,127,127,63,85,42,150,150,150,150,150,150,150,25,150,150,150,0,127,0,0,144,85,127,132,127,85,0,127,127,127,127,127,127,59,127,85,127,127,106,47,79,127,127,204,2,141,66,0,0,127,127,0,0,0,0,127,0,159,0,0,178,127,36,85,131,127,127,127,153,95,0,140,75,27,127,127,0,150,150,198,0,0,63,30,127,165,209,198,127,127,153,204,51,51,255,255,255,204,0,255,150,150,150,150,150,150,150,150,150,150,0,150,150,150,150,150,0,127,127,150,150,150,150,150,150,150,150,0,0,150,51,132,150,150,150 };

            // This sets Visual Params with *less* weirder values then default.
            for (int i = 0; i < visualParams.Length; i++)
                visualParams[i] = VISUALPARAM_DEFAULT;

            return visualParams;
        }

        public static Primitive.TextureEntry GetDefaultTexture()
        {
            Primitive.TextureEntry textu = new Primitive.TextureEntry(new UUID("C228D1CF-4B5D-4BA8-84F4-899A0796AA97"));
            textu.CreateFace(0).TextureID = new UUID("00000000-0000-1111-9999-000000000012");
            textu.CreateFace(1).TextureID = Util.BLANK_TEXTURE_UUID;
            textu.CreateFace(2).TextureID = Util.BLANK_TEXTURE_UUID;
            textu.CreateFace(3).TextureID = new UUID("6522E74D-1660-4E7F-B601-6F48C1659A77");
            textu.CreateFace(4).TextureID = new UUID("7CA39B4C-BD19-4699-AFF7-F93FD03D3E7B");
            textu.CreateFace(5).TextureID = new UUID("00000000-0000-1111-9999-000000000010");
            textu.CreateFace(6).TextureID = new UUID("00000000-0000-1111-9999-000000000011");
            return textu;
        }

        public AvatarAppearance() : this(UUID.Zero)
        {
        }

        public AvatarAppearance(UUID owner)
        {
            m_owner = owner;
            m_serial = VERSION_INITIAL;
            m_attachments = new Dictionary<int, List<AvatarAttachment>>();
            m_wearables = new Dictionary<int, AvatarWearable>();
            
            SetDefaultWearables();
            SetDefaultTexture();
            SetDefaultParams();
            SetHeight();
        }

        public AvatarAppearance(OSDMap map)
        {
            Unpack(map);
            SetHeight();
        }

        public AvatarAppearance(UUID owner, AvatarWearable[] wearables, Primitive.TextureEntry textureEntry, byte[] visualParams)
        {
//            m_log.WarnFormat("[AVATAR APPEARANCE] create initialized appearance");

            m_owner = owner;
            m_serial = VERSION_INITIAL;
            m_attachments = new Dictionary<int, List<AvatarAttachment>>();
            m_wearables = new Dictionary<int, AvatarWearable>();

            if (wearables != null)
            {
                ClearWearables();
                SetWearables(new List<AvatarWearable>(wearables));
            }
            else
                SetDefaultWearables();

            if (textureEntry != null)
                m_texture = textureEntry;
            else
                SetDefaultTexture();

            if (visualParams != null)
                m_visualparams = visualParams;
            else
                SetDefaultParams();

            SetHeight();
        }

        public AvatarAppearance(AvatarAppearance appearance) : this(appearance, true)
        {
        }

        public AvatarAppearance(AvatarAppearance appearance, bool copyWearables)
        {
//            m_log.WarnFormat("[AVATAR APPEARANCE] create from an existing appearance");

            m_attachments = new Dictionary<int, List<AvatarAttachment>>();
            m_wearables = new Dictionary<int, AvatarWearable>(); 

            if (appearance == null)
            {
                m_serial = VERSION_INITIAL;
                m_owner = UUID.Zero;
                SetDefaultWearables();
                SetDefaultTexture();
                SetDefaultParams();
                SetHeight();
                return;
            }

            m_owner = appearance.Owner;
            m_serial = appearance.Serial;

            if (copyWearables == true)
            {
                ClearWearables();
                SetWearables(appearance.GetWearables());
            }
            else
                SetDefaultWearables();

            m_texture = null;
            if (appearance.Texture != null)
            {
                byte[] tbytes = appearance.Texture.GetBytes();
                m_texture = new Primitive.TextureEntry(tbytes, 0, tbytes.Length);
            }
            else
            {
                SetDefaultTexture();
            }

            m_visualparams = null;
            if (appearance.VisualParams != null)
                m_visualparams = (byte[])appearance.VisualParams.Clone();
            else
                SetDefaultParams();

            IsBotAppearance = appearance.IsBotAppearance;

            SetHeight();

            SetAttachments(appearance.GetAttachments());
        }

        protected virtual void SetDefaultWearables()
        {
            SetWearables(AvatarWearable.GetDefaultWearables());
        }

        /// <summary>
        /// Invalidate all of the baked textures in the appearance, useful
        /// if you know that none are valid
        /// </summary>
        public virtual void ResetAppearance()
        {
//            m_log.WarnFormat("[AVATAR APPEARANCE]: Reset appearance");
            m_serial = VERSION_INITIAL;
            SetDefaultTexture();
        }
        
        protected virtual void SetDefaultParams()
        {
            m_visualparams = GetDefaultVisualParams();
            SetHeight();
        }

        protected virtual void SetDefaultTexture()
        {
            m_texture = GetDefaultTexture();
        }

        /// <summary>
        /// Set up appearance texture ids.
        /// </summary>
        /// <returns>
        /// True if any existing texture id was changed by the new data.
        /// False if there were no changes or no existing texture ids.
        /// </returns>
        public virtual bool SetTextureEntries(Primitive.TextureEntry textureEntry)
        {
            if (textureEntry == null)
                return false;

            // There are much simpler versions of this copy that could be
            // made. We determine if any of the textures actually
            // changed to know if the appearance should be saved later
            bool changed = false;
            for (uint i = 0; i < AvatarAppearance.TEXTURE_COUNT; i++)
            {
                Primitive.TextureEntryFace newface = textureEntry.FaceTextures[i];
                Primitive.TextureEntryFace oldface = m_texture.FaceTextures[i];

                if (newface == null)
                {
                    if (oldface == null)
                        continue;
                }
                else
                {
                    if (oldface != null && oldface.TextureID == newface.TextureID)
                        continue;
                }

                changed = true;
            }

            m_texture = textureEntry;
            return changed;
        }

        /// <summary>
        /// Set up visual parameters for the avatar and refresh the avatar height
        /// </summary>
        /// <returns>
        /// True if any existing visual parameter was changed by the new data.
        /// False if there were no changes or no existing visual parameters.
        /// </returns>
        public virtual bool SetVisualParams(byte[] visualParams)
        {
            bool changed = false;

            if (visualParams == null)
                return changed;

            // If the arrays are different sizes replace the whole thing.
            // its likely from different viewers
            if (visualParams.Length != m_visualparams.Length)
            {
                m_visualparams = (byte[])visualParams.Clone();
                changed = true;
            }
            else
            {
                for (int i = 0; i < visualParams.Length; i++)
                {
                    if (visualParams[i] != m_visualparams[i])
                    {
                        m_visualparams[i] = visualParams[i];
                        changed = true;
                    }
                }
            }

            // Reset the height if the visual parameters actually changed
            if (changed)
                SetHeight();

            return changed;
        }

        public virtual void SetAppearance(Primitive.TextureEntry textureEntry, byte[] visualParams)
        {
            SetTextureEntries(textureEntry);
            SetVisualParams(visualParams);
            SetHeight();
        }

        public virtual void SetHeight()
        {
            m_avatarHeight = 1.23077f  // Shortest possible avatar height
                           + 0.516945f * (float)m_visualparams[25] / 255.0f   // Body height
                           + 0.072514f * (float)m_visualparams[120] / 255.0f  // Head size
                           + 0.3836f * (float)m_visualparams[125] / 255.0f    // Leg length
                           + 0.08f * (float)m_visualparams[77] / 255.0f    // Shoe heel height
                           + 0.07f * (float)m_visualparams[78] / 255.0f    // Shoe platform height
                           + 0.076f * (float)m_visualparams[148] / 255.0f;    // Neck length
            m_hipOffset = (0.615385f // Half of avatar
                           + 0.08f * (float)m_visualparams[77] / 255.0f    // Shoe heel height
                           + 0.07f * (float)m_visualparams[78] / 255.0f    // Shoe platform height
                           + 0.3836f * (float)m_visualparams[125] / 255.0f    // Leg length
                           - m_avatarHeight / 2) * 0.3f - 0.04f;

            //System.Console.WriteLine(">>>>>>> [APPEARANCE]: Height {0} Hip offset {1}" + m_avatarHeight + " " + m_hipOffset);
            //m_log.Debug("------------- Set Appearance Texture ---------------");
            //Primitive.TextureEntryFace[] faces = Texture.FaceTextures;
            //foreach (Primitive.TextureEntryFace face in faces)
            //{
            //    if (face != null)
            //        m_log.Debug("  ++ " + face.TextureID);
            //    else
            //        m_log.Debug("  ++ NULL ");
            //}
            //m_log.Debug("----------------------------");
        }

        // this is used for OGS1
        // It should go away soon in favor of the pack/unpack sections below
        public virtual Hashtable ToHashTable()
        {
            Hashtable h = new Hashtable();
            AvatarWearable wearable;

            h["owner"] = Owner.ToString();
            h["serial"] = Serial.ToString();
            h["visual_params"] = VisualParams;
            h["texture"] = Texture.GetBytes();
            h["avatar_height"] = AvatarHeight.ToString();

            wearable = GetWearableOfType(AvatarWearable.BODY);
            h["body_item"] = wearable.ItemID.ToString();
            h["body_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.SKIN);
            h["skin_item"] = wearable.ItemID.ToString();
            h["skin_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.HAIR);
            h["hair_item"] = wearable.ItemID.ToString();
            h["hair_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.EYES);
            h["eyes_item"] = wearable.ItemID.ToString();
            h["eyes_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.SHIRT);
            h["shirt_item"] = wearable.ItemID.ToString();
            h["shirt_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.PANTS);
            h["pants_item"] = wearable.ItemID.ToString();
            h["pants_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.SHOES);
            h["shoes_item"] = wearable.ItemID.ToString();
            h["shoes_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.SOCKS);
            h["socks_item"] = wearable.ItemID.ToString();
            h["socks_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.JACKET);
            h["jacket_item"] = wearable.ItemID.ToString();
            h["jacket_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.GLOVES);
            h["gloves_item"] = wearable.ItemID.ToString();
            h["gloves_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.UNDERSHIRT);
            h["undershirt_item"] = wearable.ItemID.ToString();
            h["undershirt_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.UNDERPANTS);
            h["underpants_item"] = wearable.ItemID.ToString();
            h["underpants_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.SKIRT);
            h["skirt_item"] = wearable.ItemID.ToString();
            h["skirt_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.ALPHA);
            h["alpha_item"] = wearable.ItemID.ToString();
            h["alpha_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.TATTOO);
            h["tattoo_item"] = wearable.ItemID.ToString();
            h["tattoo_asset"] = wearable.AssetID.ToString();
            wearable = GetWearableOfType(AvatarWearable.PHYSICS);
            h["physics_item"] = wearable.ItemID.ToString();
            h["physics_asset"] = wearable.AssetID.ToString();

            string attachments = GetAttachmentsString();
            if (!String.IsNullOrEmpty(attachments))
                h["attachments"] = attachments;

            return h;
        }

        public AvatarAppearance(Hashtable h)
        {
            if (h == null)
                return;

            if (h.ContainsKey("owner"))
                Owner = new UUID((string)h["owner"]);
            else
                Owner = UUID.Zero;

            if (h.ContainsKey("serial"))
                Serial = Convert.ToInt32((string)h["serial"]);

            if (h.ContainsKey("visual_params"))
                VisualParams = (byte[])h["visual_params"];
            else
                VisualParams = GetDefaultVisualParams();

            if (h.ContainsKey("texture") && ((byte[])h["texture"] != null))
            {
                byte[] textureData = (byte[])h["texture"];
                Texture = new Primitive.TextureEntry(textureData, 0, textureData.Length);
            }
            else
            {
                Texture = GetDefaultTexture();
            }

            if (h.ContainsKey("avatar_height"))
                AvatarHeight = (float)Convert.ToDouble((string)h["avatar_height"]);

            m_attachments = new Dictionary<int, List<AvatarAttachment>>();
            m_wearables = new Dictionary<int, AvatarWearable>();

            ClearWearables();

            SetWearable(new AvatarWearable(AvatarWearable.BODY, new UUID((string)h["body_item"]), new UUID((string)h["body_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.SKIN, new UUID((string)h["skin_item"]), new UUID((string)h["skin_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.HAIR, new UUID((string)h["hair_item"]), new UUID((string)h["hair_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.EYES, new UUID((string)h["eyes_item"]), new UUID((string)h["eyes_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.SHIRT, new UUID((string)h["shirt_item"]), new UUID((string)h["shirt_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.PANTS, new UUID((string)h["pants_item"]), new UUID((string)h["pants_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.SHOES, new UUID((string)h["shoes_item"]), new UUID((string)h["shoes_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.SOCKS, new UUID((string)h["socks_item"]), new UUID((string)h["socks_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.JACKET, new UUID((string)h["jacket_item"]), new UUID((string)h["jacket_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.GLOVES, new UUID((string)h["gloves_item"]), new UUID((string)h["gloves_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.UNDERSHIRT, new UUID((string)h["undershirt_item"]), new UUID((string)h["undershirt_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.UNDERPANTS, new UUID((string)h["underpants_item"]), new UUID((string)h["underpants_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.SKIRT, new UUID((string)h["skirt_item"]), new UUID((string)h["skirt_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.ALPHA, new UUID((string)h["alpha_item"]), new UUID((string)h["alpha_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.TATTOO, new UUID((string)h["tattoo_item"]), new UUID((string)h["tattoo_asset"])));
            SetWearable(new AvatarWearable(AvatarWearable.PHYSICS, new UUID((string)h["physics_item"]), new UUID((string)h["physics_asset"])));

            if (h.ContainsKey("attachments"))
            {
                SetAttachmentsString(h["attachments"].ToString());
            }
        } 
        
        #region Wearables

        public void ClearWearables()
        {
            lock (m_wearables)
            {
                m_wearables.Clear();
                SetWearable(AvatarWearable.DEFAULT_BODY);
                SetWearable(AvatarWearable.DEFAULT_HAIR);
                SetWearable(AvatarWearable.DEFAULT_SKIN);
                SetWearable(AvatarWearable.DEFAULT_EYES);
            }
        }

        /// <summary>
        /// Get the wearable of type "i".
        /// </summary>
        /// <remarks>
        /// </remarks>
        public AvatarWearable GetWearableOfType(int i)
        {
            lock (m_wearables)
            {
                if (m_wearables.ContainsKey(i))
                    return (m_wearables[i]);
            }

            return (new AvatarWearable(i, UUID.Zero, UUID.Zero));
        }

        public void SetWearable(AvatarWearable wearable)
        {
            if ((wearable.WearableType < 0) || (wearable.WearableType >= AvatarWearable.MAX_WEARABLES))
            {
                m_log.WarnFormat("[AVATAR APPEARANCE]: AvatarWearable type {0} is out of range", wearable.WearableType);
                return;
            }

            if (AvatarWearable.IsRequiredWearable(wearable.WearableType) && (wearable.ItemID == UUID.Zero))
            {
                m_log.WarnFormat("[AVATAR APPEARANCE]: Refusing to set a ZERO wearable for a required item of type {0}", wearable.WearableType);
                return;
            }

            lock (m_wearables)
            {
                m_wearables[wearable.WearableType] = wearable;
            }
        }

        public List<AvatarWearable> GetWearables()
        {
            lock (m_wearables)
            {
                return (new List<AvatarWearable>(m_wearables.Values));
            }
        }

        public List<int> GetWearableTypes()
        {
            lock (m_wearables)
            {
                return new List<int>(m_wearables.Keys);
            }
        }

        /// <summary>
        /// Rebuilds the entire list with locks held.  Use this.
        /// </summary>
        /// <param name="attachments"></param>
        public void SetWearables(List<AvatarWearable> wearables)
        {
            lock (m_wearables)
            {
                // Will also make sure reasonable defaults are applied
                ClearWearables();

                foreach (AvatarWearable wearable in wearables)
                {
                    SetWearable(wearable);
                }
            }
        }

        public AvatarWearable GetWearableForItem(UUID itemID)
        {
            lock (m_wearables)
            {
                foreach (KeyValuePair<int, AvatarWearable> kvp in m_wearables)
                {
                    if (kvp.Value.ItemID == itemID)
                        return (kvp.Value);
                }
            }
            return null;
        }

        public int GetWearableType(UUID itemID)
        {
            AvatarWearable wearable = GetWearableForItem(itemID);
            if (wearable == null)
                return AvatarWearable.NONE;
            else
                return (wearable.WearableType);
        }

        #endregion

        #region Attachments

        /// <summary>
        /// Get a list of the attachments.
        /// </summary>
        /// <remarks>
        /// There may be duplicate attachpoints
        /// </remarks>
        public List<AvatarAttachment> GetAttachments()
        {
            List<AvatarAttachment> alist = new List<AvatarAttachment>();

            lock (m_attachments)
            {
                foreach (KeyValuePair<int, List<AvatarAttachment>> kvp in m_attachments)
                {
                    foreach (AvatarAttachment attach in kvp.Value)
                        alist.Add(attach);
                }
            }

            return alist;
        }

        public List<int> GetAttachedPoints()
        {
            lock (m_attachments)
            {
                return new List<int>(m_attachments.Keys);
            }
        }

        public List<AvatarAttachment> GetAttachmentsAtPoint(int attachPoint)
        {
            lock (m_attachments)
            {
                return (new List<AvatarAttachment>(m_attachments[attachPoint]));
            }
        }

        internal void AppendAttachment(AvatarAttachment attach)
        {
//            m_log.DebugFormat(
//                "[AVATAR APPEARNCE]: Appending itemID={0}, assetID={1} at {2}",
//                attach.ItemID, attach.AssetID, attach.AttachPoint);

            lock (m_attachments)
            {
                if (!m_attachments.ContainsKey(attach.AttachPoint))
                    m_attachments[attach.AttachPoint] = new List<AvatarAttachment>();
    
                m_attachments[attach.AttachPoint].Add(attach);
            }
        }

        internal void ReplaceAttachment(AvatarAttachment attach)
        {
//            m_log.DebugFormat(
//                "[AVATAR APPEARANCE]: Replacing itemID={0}, assetID={1} at {2}",
//                attach.ItemID, attach.AssetID, attach.AttachPoint);

            lock (m_attachments)
            {
                m_attachments[attach.AttachPoint] = new List<AvatarAttachment>();
                m_attachments[attach.AttachPoint].Add(attach);
            }
        }

        /// <summary>
        /// Set an attachment
        /// </summary>
        /// <remarks>
        /// Append or Replace based on the append flag
        /// If item is passed in as UUID.Zero, then an any attachment at the 
        /// attachpoint is removed.
        /// </remarks>
        /// <param name="attachpoint"></param>
        /// <param name="item"></param>
        /// <param name="asset"></param>
        /// <returns>
        /// return true if something actually changed
        /// </returns>
        public bool SetAttachment(int attachpoint, bool append, UUID item, UUID asset)
        {
            //            m_log.DebugFormat(
            //                "[AVATAR APPEARANCE]: Setting attachment at {0} with item ID {1}, asset ID {2}",
            //                 attachpoint, item, asset);

            if (attachpoint == 0)
                return false;

            if (item == UUID.Zero)
            {
                lock (m_attachments)
                {
                    if (m_attachments.ContainsKey(attachpoint))
                    {
                        m_attachments.Remove(attachpoint);
                        return true;
                    }
                }

                return false;
            }

            if (append)
                AppendAttachment(new AvatarAttachment(attachpoint, item, asset));
            else
                ReplaceAttachment(new AvatarAttachment(attachpoint, item, asset));

            return true;
        }   

        /// <summary>
        /// Rebuilds the entire list with locks held.  Use this.
        /// </summary>
        /// <param name="attachments"></param>
        public void SetAttachments(List<AvatarAttachment> attachments)
        {
            lock (m_attachments)
            {
                m_attachments.Clear();

                foreach (AvatarAttachment attachment in attachments)
                {
                    if (!m_attachments.ContainsKey(attachment.AttachPoint))
                        m_attachments.Add(attachment.AttachPoint, new List<AvatarAttachment>());

                    m_attachments[attachment.AttachPoint].Add(attachment);
                }
            }
        }

        /// <summary>
        /// Rebuilds the entire list with locks held.  Use this.
        /// </summary>
        /// <param name="attachments"></param>
        public void SetAttachmentsForPoint(int attachPoint, List<AvatarAttachment> attachments)
        {
            lock (m_attachments)
            {
                m_attachments.Remove(attachPoint);
                foreach (AvatarAttachment attachment in attachments)
                {
                    if (!m_attachments.ContainsKey(attachment.AttachPoint))
                        m_attachments.Add(attachment.AttachPoint, new List<AvatarAttachment>());
                    m_attachments[attachment.AttachPoint].Add(attachment);
                }
            }
        }

        /// <summary>
        /// If the item is already attached, return it.
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns>Returns null if this item is not attached.</returns>
        public AvatarAttachment GetAttachmentForItem(UUID itemID)
        {
            lock (m_attachments)
            {
                foreach (KeyValuePair<int, List<AvatarAttachment>> kvp in m_attachments)
                {
                    int index = kvp.Value.FindIndex(delegate(AvatarAttachment a) { return a.ItemID == itemID; });
                    if (index >= 0)
                        return kvp.Value[index];
                }
            }

            return null;
        }

        public int GetAttachpoint(UUID itemID)
        {
            lock (m_attachments)
            {
                foreach (KeyValuePair<int, List<AvatarAttachment>> kvp in m_attachments)
                {
                    int index = kvp.Value.FindIndex(delegate(AvatarAttachment a) { return a.ItemID == itemID; });
                    if (index >= 0)
                        return kvp.Key;
                }
            }

            return 0;
        }

        /// <summary>
        /// Remove an attachment if it exists
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns>The AssetID for detached asset or UUID.Zero</returns>
        public UUID DetachAttachment(UUID itemID)
        {
            UUID assetID;

            lock (m_attachments)
            {
                foreach (KeyValuePair<int, List<AvatarAttachment>> kvp in m_attachments)
                {
                    int index = kvp.Value.FindIndex(delegate(AvatarAttachment a) { return a.ItemID == itemID; });
                    if (index >= 0)
                    {
                        AvatarAttachment attachment = m_attachments[kvp.Key][index];
                        assetID = attachment.AssetID;

                        // Remove it from the list of attachments at that attach point
                        m_attachments[kvp.Key].RemoveAt(index);
    
                        // And remove the list if there are no more attachments here
                        if (m_attachments[kvp.Key].Count == 0)
                            m_attachments.Remove(kvp.Key);

                        return assetID;
                    }
                }
            }

            return UUID.Zero;
        }

        string GetAttachmentsString()
        {
            List<string> strings = new List<string>();

            lock (m_attachments)
            {
                foreach (KeyValuePair<int, List<AvatarAttachment>> kvp in m_attachments)
                {
                    foreach (AvatarAttachment attachment in kvp.Value)
                    {
                        strings.Add(attachment.AttachPoint.ToString());
                        strings.Add(attachment.ItemID.ToString());
                        strings.Add(attachment.AssetID.ToString());
                    }
                }
            }

            return String.Join(",", strings.ToArray());
        }

        void SetAttachmentsString(string data)
        {
            string[] strings = data.Split(new char[] { ',' });
            int i = 0;

            List<AvatarAttachment> attachments = new List<AvatarAttachment>();

            while (strings.Length - i > 2)
            {
                int attachpoint = Int32.Parse(strings[i]);
                UUID item = new UUID(strings[i + 1]);
                UUID sogId = new UUID(strings[i + 2]);
                i += 3;

                AvatarAttachment attachment = new AvatarAttachment(attachpoint, item, sogId);
                attachments.Add(attachment);
            }

            SetAttachments(attachments);
        }

        public void ClearAttachments()
        {
            lock (m_attachments)
                m_attachments.Clear();
        }

        #endregion

        #region Packing Functions

        /// <summary>
        /// Create an OSDMap from the appearance data
        /// </summary>
        public OSDMap Pack()
        {
            OSDMap data = new OSDMap();

            data["owner"] = OSD.FromUUID(Owner);
            data["serial"] = OSD.FromInteger(m_serial);
            data["height"] = OSD.FromReal(m_avatarHeight);

            // Wearables
            List<AvatarWearable> wearables = GetWearables();
            OSDArray wears = new OSDArray(wearables.Count);
            foreach (AvatarWearable wearable in wearables)
                wears.Add(wearable.Pack());
            data["wearables"] = wears;

            // Avatar Textures
            OSDArray textures = new OSDArray(AvatarAppearance.TEXTURE_COUNT);
            for (uint i = 0; i < AvatarAppearance.TEXTURE_COUNT; i++)
            {
                if (m_texture.FaceTextures[i] != null)
                    textures.Add(OSD.FromUUID(m_texture.FaceTextures[i].TextureID));
                else
                    textures.Add(OSD.FromUUID(AppearanceManager.DEFAULT_AVATAR_TEXTURE));
            }
            data["textures"] = textures;

            // Visual Parameters
            OSDBinary visualparams = new OSDBinary(m_visualparams);
            data["visualparams"] = visualparams;

            // Attachments
            List<AvatarAttachment> attachments = GetAttachments();
            OSDArray attachs = new OSDArray(attachments.Count);
            foreach (AvatarAttachment attach in attachments)
                attachs.Add(attach.Pack());
            data["attachments"] = attachs;

            return data;
        } 

        /// <summary>
        /// Unpack and OSDMap and initialize the appearance
        /// from it
        /// </summary>
        public void Unpack(OSDMap data)
        {
            if (data == null)
            {
                m_log.Warn("[AVATAR APPEARANCE]: failed to unpack avatar appearance");
                return;
            }

            if (data.ContainsKey("owner"))
                m_owner = data["owner"].AsUUID();
            else
                m_owner = UUID.Zero;

            if (data.ContainsKey("serial"))
                m_serial = data["serial"].AsInteger();

            if (data.ContainsKey("height"))
                m_avatarHeight = (float)data["height"].AsReal();

            try
            {
                // Wearablles
                m_wearables = new Dictionary<int, AvatarWearable>();
                ClearWearables();

                if (data.ContainsKey("wearables") && ((data["wearables"]).Type == OSDType.Array))
                {
                    OSDArray wears = (OSDArray)data["wearables"];
                    for (int i = 0; i < wears.Count; i++)
                    {
                        AvatarWearable wearable = new AvatarWearable((OSDMap)wears[i]);
                        SetWearable(wearable);
                    }
                }
                else
                {
                    m_log.Warn("[AVATAR APPEARANCE]: failed to unpack wearables");
                }

                // Avatar Textures
                SetDefaultTexture();
                if (data.ContainsKey("textures") && ((data["textures"]).Type == OSDType.Array))
                {
                    OSDArray textures = (OSDArray)(data["textures"]);
                    for (int i = 0; i < AvatarAppearance.TEXTURE_COUNT && i < textures.Count; i++)
                    {
                        UUID textureID = AppearanceManager.DEFAULT_AVATAR_TEXTURE;
                        if (textures[i] != null)
                            textureID = textures[i].AsUUID();
                        m_texture.CreateFace((uint)i).TextureID = new UUID(textureID);
                    }
                }
                else
                {
                    m_log.Warn("[AVATAR APPEARANCE]: failed to unpack textures");
                }

                // Visual Parameters
                SetDefaultParams();
                if (data.ContainsKey("visualparams"))
                {
                    if ((data["visualparams"].Type == OSDType.Binary) || (data["visualparams"].Type == OSDType.Array))
                        m_visualparams = data["visualparams"].AsBinary();
                }
                else
                {
                    m_log.Warn("[AVATAR APPEARANCE]: failed to unpack visual parameters");
                }

                // Attachments
                m_attachments = new Dictionary<int, List<AvatarAttachment>>();
                if (data.ContainsKey("attachments") && ((data["attachments"]).Type == OSDType.Array))
                {
                    OSDArray attachs = (OSDArray)(data["attachments"]);
                    for (int i = 0; i < attachs.Count; i++)
                    {
                        AvatarAttachment att = new AvatarAttachment((OSDMap)attachs[i]);
                        AppendAttachment(att);
                    }
                }
            }
            catch (Exception e)
            {
                m_log.ErrorFormat("[AVATAR APPEARANCE]: unpack failed badly: {0}{1}", e.Message, e.StackTrace);
            }
        }
        #endregion

        #region VPElement

        /// <summary>
        /// Viewer Params Array Element for AgentSetAppearance
        /// Generated from LibOMV's Visual Params list
        /// </summary>
        public enum VPElement : int
        {
            /// <summary>
            /// Brow Size - Small 0--+255 Large
            /// </summary>
            SHAPE_BIG_BROW = 0,
            /// <summary>
            /// Nose Size - Small 0--+255 Large
            /// </summary>
            SHAPE_NOSE_BIG_OUT = 1,
            /// <summary>
            /// Nostril Width - Narrow 0--+255 Broad
            /// </summary>
            SHAPE_BROAD_NOSTRILS = 2,
            /// <summary>
            /// Chin Cleft - Round 0--+255 Cleft
            /// </summary>
            SHAPE_CLEFT_CHIN = 3,
            /// <summary>
            /// Nose Tip Shape - Pointy 0--+255 Bulbous
            /// </summary>
            SHAPE_BULBOUS_NOSE_TIP = 4,
            /// <summary>
            /// Chin Angle - Chin Out 0--+255 Chin In
            /// </summary>
            SHAPE_WEAK_CHIN = 5,
            /// <summary>
            /// Chin-Neck - Tight Chin 0--+255 Double Chin
            /// </summary>
            SHAPE_DOUBLE_CHIN = 6,
            /// <summary>
            /// Lower Cheeks - Well-Fed 0--+255 Sunken
            /// </summary>
            SHAPE_SUNKEN_CHEEKS = 7,
            /// <summary>
            /// Upper Bridge - Low 0--+255 High
            /// </summary>
            SHAPE_NOBLE_NOSE_BRIDGE = 8,
            /// <summary>
            ///  - Less 0--+255 More
            /// </summary>
            SHAPE_JOWLS = 9,
            /// <summary>
            /// Upper Chin Cleft - Round 0--+255 Cleft
            /// </summary>
            SHAPE_CLEFT_CHIN_UPPER = 10,
            /// <summary>
            /// Cheek Bones - Low 0--+255 High
            /// </summary>
            SHAPE_HIGH_CHEEK_BONES = 11,
            /// <summary>
            /// Ear Angle - In 0--+255 Out
            /// </summary>
            SHAPE_EARS_OUT = 12,
            /// <summary>
            /// Eyebrow Points - Smooth 0--+255 Pointy
            /// </summary>
            HAIR_POINTY_EYEBROWS = 13,
            /// <summary>
            /// Jaw Shape - Pointy 0--+255 Square
            /// </summary>
            SHAPE_SQUARE_JAW = 14,
            /// <summary>
            /// Upper Cheeks - Thin 0--+255 Puffy
            /// </summary>
            SHAPE_PUFFY_UPPER_CHEEKS = 15,
            /// <summary>
            /// Nose Tip Angle - Downturned 0--+255 Upturned
            /// </summary>
            SHAPE_UPTURNED_NOSE_TIP = 16,
            /// <summary>
            /// Nose Thickness - Thin Nose 0--+255 Bulbous Nose
            /// </summary>
            SHAPE_BULBOUS_NOSE = 17,
            /// <summary>
            /// Upper Eyelid Fold - Uncreased 0--+255 Creased
            /// </summary>
            SHAPE_UPPER_EYELID_FOLD = 18,
            /// <summary>
            /// Attached Earlobes - Unattached 0--+255 Attached
            /// </summary>
            SHAPE_ATTACHED_EARLOBES = 19,
            /// <summary>
            /// Eye Bags - Smooth 0--+255 Baggy
            /// </summary>
            SHAPE_BAGGY_EYES = 20,
            /// <summary>
            /// Eye Opening - Narrow 0--+255 Wide
            /// </summary>
            SHAPE_WIDE_EYES = 21,
            /// <summary>
            /// Lip Cleft - Narrow 0--+255 Wide
            /// </summary>
            SHAPE_WIDE_LIP_CLEFT = 22,
            /// <summary>
            /// Bridge Width - Narrow 0--+255 Wide
            /// </summary>
            SHAPE_WIDE_NOSE_BRIDGE = 23,
            /// <summary>
            /// Eyebrow Arc - Flat 0--+255 Arced
            /// </summary>
            HAIR_ARCED_EYEBROWS = 24,
            /// <summary>
            /// Height - Short 0--+255 Tall
            /// </summary>
            SHAPE_HEIGHT = 25,
            /// <summary>
            /// Body Thickness - Body Thin 0--+255 Body Thick
            /// </summary>
            SHAPE_THICKNESS = 26,
            /// <summary>
            /// Ear Size - Small 0--+255 Large
            /// </summary>
            SHAPE_BIG_EARS = 27,
            /// <summary>
            /// Shoulders - Narrow 0--+255 Broad
            /// </summary>
            SHAPE_SHOULDERS = 28,
            /// <summary>
            /// Hip Width - Narrow 0--+255 Wide
            /// </summary>
            SHAPE_HIP_WIDTH = 29,
            /// <summary>
            ///  - Short Torso 0--+255 Long Torso
            /// </summary>
            SHAPE_TORSO_LENGTH = 30,
            SHAPE_MALE = 31,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            GLOVES_GLOVE_LENGTH = 32,
            /// <summary>
            ///  - Darker 0--+255 Lighter
            /// </summary>
            EYES_EYE_LIGHTNESS = 33,
            /// <summary>
            ///  - Natural 0--+255 Unnatural
            /// </summary>
            EYES_EYE_COLOR = 34,
            /// <summary>
            ///  - Small 0--+255 Large
            /// </summary>
            SHAPE_BREAST_SIZE = 35,
            /// <summary>
            ///  - None 0--+255 Wild
            /// </summary>
            SKIN_RAINBOW_COLOR = 36,
            /// <summary>
            /// Ruddiness - Pale 0--+255 Ruddy
            /// </summary>
            SKIN_RED_SKIN = 37,
            /// <summary>
            ///  - Light 0--+255 Dark
            /// </summary>
            SKIN_PIGMENT = 38,
            HAIR_RAINBOW_COLOR_39 = 39,
            /// <summary>
            ///  - No Red 0--+255 Very Red
            /// </summary>
            HAIR_RED_HAIR = 40,
            /// <summary>
            ///  - Black 0--+255 Blonde
            /// </summary>
            HAIR_BLONDE_HAIR = 41,
            /// <summary>
            ///  - No White 0--+255 All White
            /// </summary>
            HAIR_WHITE_HAIR = 42,
            /// <summary>
            ///  - Less Rosy 0--+255 More Rosy
            /// </summary>
            SKIN_ROSY_COMPLEXION = 43,
            /// <summary>
            ///  - Darker 0--+255 Pinker
            /// </summary>
            SKIN_LIP_PINKNESS = 44,
            /// <summary>
            ///  - Thin Eyebrows 0--+255 Bushy Eyebrows
            /// </summary>
            HAIR_EYEBROW_SIZE = 45,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            HAIR_FRONT_FRINGE = 46,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            HAIR_SIDE_FRINGE = 47,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            HAIR_BACK_FRINGE = 48,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            HAIR_HAIR_FRONT = 49,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            HAIR_HAIR_SIDES = 50,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            HAIR_HAIR_BACK = 51,
            /// <summary>
            ///  - Sweep Forward 0--+255 Sweep Back
            /// </summary>
            HAIR_HAIR_SWEEP = 52,
            /// <summary>
            ///  - Left 0--+255 Right
            /// </summary>
            HAIR_HAIR_TILT = 53,
            /// <summary>
            /// Middle Part - No Part 0--+255 Part
            /// </summary>
            HAIR_HAIR_PART_MIDDLE = 54,
            /// <summary>
            /// Right Part - No Part 0--+255 Part
            /// </summary>
            HAIR_HAIR_PART_RIGHT = 55,
            /// <summary>
            /// Left Part - No Part 0--+255 Part
            /// </summary>
            HAIR_HAIR_PART_LEFT = 56,
            /// <summary>
            /// Full Hair Sides - Mowhawk 0--+255 Full Sides
            /// </summary>
            HAIR_HAIR_SIDES_FULL = 57,
            /// <summary>
            ///  - Less 0--+255 More
            /// </summary>
            SKIN_BODY_DEFINITION = 58,
            /// <summary>
            /// Lip Width - Narrow Lips 0--+255 Wide Lips
            /// </summary>
            SHAPE_LIP_WIDTH = 59,
            /// <summary>
            ///  - Small 0--+255 Big
            /// </summary>
            SHAPE_BELLY_SIZE = 60,
            /// <summary>
            ///  - Less 0--+255 More
            /// </summary>
            SKIN_FACIAL_DEFINITION = 61,
            /// <summary>
            ///  - Less 0--+255 More
            /// </summary>
            SKIN_WRINKLES = 62,
            /// <summary>
            ///  - Less 0--+255 More
            /// </summary>
            SKIN_FRECKLES = 63,
            /// <summary>
            ///  - Short Sideburns 0--+255 Mutton Chops
            /// </summary>
            HAIR_SIDEBURNS = 64,
            /// <summary>
            ///  - Chaplin 0--+255 Handlebars
            /// </summary>
            HAIR_MOUSTACHE = 65,
            /// <summary>
            ///  - Less soul 0--+255 More soul
            /// </summary>
            HAIR_SOULPATCH = 66,
            /// <summary>
            ///  - Less Curtains 0--+255 More Curtains
            /// </summary>
            HAIR_CHIN_CURTAINS = 67,
            /// <summary>
            /// Rumpled Hair - Smooth Hair 0--+255 Rumpled Hair
            /// </summary>
            HAIR_HAIR_RUMPLED = 68,
            /// <summary>
            /// Big Hair Front - Less 0--+255 More
            /// </summary>
            HAIR_HAIR_BIG_FRONT = 69,
            /// <summary>
            /// Big Hair Top - Less 0--+255 More
            /// </summary>
            HAIR_HAIR_BIG_TOP = 70,
            /// <summary>
            /// Big Hair Back - Less 0--+255 More
            /// </summary>
            HAIR_HAIR_BIG_BACK = 71,
            /// <summary>
            /// Spiked Hair - No Spikes 0--+255 Big Spikes
            /// </summary>
            HAIR_HAIR_SPIKED = 72,
            /// <summary>
            /// Chin Depth - Shallow 0--+255 Deep
            /// </summary>
            SHAPE_DEEP_CHIN = 73,
            /// <summary>
            /// Part Bangs - No Part 0--+255 Part Bangs
            /// </summary>
            HAIR_BANGS_PART_MIDDLE = 74,
            /// <summary>
            /// Head Shape - More Square 0--+255 More Round
            /// </summary>
            SHAPE_HEAD_SHAPE = 75,
            /// <summary>
            /// Eye Spacing - Close Set Eyes 0--+255 Far Set Eyes
            /// </summary>
            SHAPE_EYE_SPACING = 76,
            /// <summary>
            ///  - Low Heels 0--+255 High Heels
            /// </summary>
            SHOES_HEEL_HEIGHT = 77,
            /// <summary>
            ///  - Low Platforms 0--+255 High Platforms
            /// </summary>
            SHOES_PLATFORM_HEIGHT = 78,
            /// <summary>
            ///  - Thin Lips 0--+255 Fat Lips
            /// </summary>
            SHAPE_LIP_THICKNESS = 79,
            /// <summary>
            /// Mouth Position - High 0--+255 Low
            /// </summary>
            SHAPE_MOUTH_HEIGHT = 80,
            /// <summary>
            /// Breast Buoyancy - Less Gravity 0--+255 More Gravity
            /// </summary>
            SHAPE_BREAST_GRAVITY = 81,
            /// <summary>
            /// Platform Width - Narrow 0--+255 Wide
            /// </summary>
            SHOES_SHOE_PLATFORM_WIDTH = 82,
            /// <summary>
            ///  - Pointy Heels 0--+255 Thick Heels
            /// </summary>
            SHOES_HEEL_SHAPE = 83,
            /// <summary>
            ///  - Pointy 0--+255 Square
            /// </summary>
            SHOES_TOE_SHAPE = 84,
            /// <summary>
            /// Foot Size - Small 0--+255 Big
            /// </summary>
            SHAPE_FOOT_SIZE = 85,
            /// <summary>
            /// Nose Width - Narrow 0--+255 Wide
            /// </summary>
            SHAPE_WIDE_NOSE = 86,
            /// <summary>
            /// Eyelash Length - Short 0--+255 Long
            /// </summary>
            SHAPE_EYELASHES_LONG = 87,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            UNDERSHIRT_SLEEVE_LENGTH = 88,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            UNDERSHIRT_BOTTOM = 89,
            /// <summary>
            ///  - Low 0--+255 High
            /// </summary>
            UNDERSHIRT_COLLAR_FRONT = 90,
            JACKET_SLEEVE_LENGTH_91 = 91,
            JACKET_COLLAR_FRONT_92 = 92,
            /// <summary>
            /// Jacket Length - Short 0--+255 Long
            /// </summary>
            JACKET_BOTTOM_LENGTH_LOWER = 93,
            /// <summary>
            /// Open Front - Open 0--+255 Closed
            /// </summary>
            JACKET_OPEN_JACKET = 94,
            /// <summary>
            ///  - Short 0--+255 Tall
            /// </summary>
            SHOES_SHOE_HEIGHT = 95,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            SOCKS_SOCKS_LENGTH = 96,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            UNDERPANTS_PANTS_LENGTH = 97,
            /// <summary>
            ///  - Low 0--+255 High
            /// </summary>
            UNDERPANTS_PANTS_WAIST = 98,
            /// <summary>
            /// Cuff Flare - Tight Cuffs 0--+255 Flared Cuffs
            /// </summary>
            PANTS_LEG_PANTFLAIR = 99,
            /// <summary>
            ///  - More Vertical 0--+255 More Sloped
            /// </summary>
            SHAPE_FOREHEAD_ANGLE = 100,
            /// <summary>
            ///  - Less Body Fat 0--+255 More Body Fat
            /// </summary>
            SHAPE_BODY_FAT = 101,
            /// <summary>
            /// Pants Crotch - High and Tight 0--+255 Low and Loose
            /// </summary>
            PANTS_LOW_CROTCH = 102,
            /// <summary>
            /// Egg Head - Chin Heavy 0--+255 Forehead Heavy
            /// </summary>
            SHAPE_EGG_HEAD = 103,
            /// <summary>
            /// Head Stretch - Squash Head 0--+255 Stretch Head
            /// </summary>
            SHAPE_SQUASH_STRETCH_HEAD = 104,
            /// <summary>
            /// Torso Muscles - Less Muscular 0--+255 More Muscular
            /// </summary>
            SHAPE_TORSO_MUSCLES = 105,
            /// <summary>
            /// Outer Eye Corner - Corner Down 0--+255 Corner Up
            /// </summary>
            SHAPE_EYELID_CORNER_UP = 106,
            /// <summary>
            ///  - Less Muscular 0--+255 More Muscular
            /// </summary>
            SHAPE_LEG_MUSCLES = 107,
            /// <summary>
            /// Lip Fullness - Less Full 0--+255 More Full
            /// </summary>
            SHAPE_TALL_LIPS = 108,
            /// <summary>
            /// Toe Thickness - Flat Toe 0--+255 Thick Toe
            /// </summary>
            SHOES_SHOE_TOE_THICK = 109,
            /// <summary>
            /// Crooked Nose - Nose Left 0--+255 Nose Right
            /// </summary>
            SHAPE_CROOKED_NOSE = 110,
            /// <summary>
            ///  - Corner Down 0--+255 Corner Up
            /// </summary>
            SHAPE_MOUTH_CORNER = 111,
            /// <summary>
            ///  - Shear Right Up 0--+255 Shear Left Up
            /// </summary>
            SHAPE_FACE_SHEAR = 112,
            /// <summary>
            /// Shift Mouth - Shift Left 0--+255 Shift Right
            /// </summary>
            SHAPE_SHIFT_MOUTH = 113,
            /// <summary>
            /// Eye Pop - Pop Right Eye 0--+255 Pop Left Eye
            /// </summary>
            SHAPE_POP_EYE = 114,
            /// <summary>
            /// Jaw Jut - Overbite 0--+255 Underbite
            /// </summary>
            SHAPE_JAW_JUT = 115,
            /// <summary>
            /// Shear Back - Full Back 0--+255 Sheared Back
            /// </summary>
            HAIR_HAIR_SHEAR_BACK = 116,
            /// <summary>
            ///  - Small Hands 0--+255 Large Hands
            /// </summary>
            SHAPE_HAND_SIZE = 117,
            /// <summary>
            /// Love Handles - Less Love 0--+255 More Love
            /// </summary>
            SHAPE_LOVE_HANDLES = 118,
            SHAPE_TORSO_MUSCLES_119 = 119,
            /// <summary>
            /// Head Size - Small Head 0--+255 Big Head
            /// </summary>
            SHAPE_HEAD_SIZE = 120,
            /// <summary>
            ///  - Skinny Neck 0--+255 Thick Neck
            /// </summary>
            SHAPE_NECK_THICKNESS = 121,
            /// <summary>
            /// Breast Cleavage - Separate 0--+255 Join
            /// </summary>
            SHAPE_BREAST_FEMALE_CLEAVAGE = 122,
            /// <summary>
            /// Pectorals - Big Pectorals 0--+255 Sunken Chest
            /// </summary>
            SHAPE_CHEST_MALE_NO_PECS = 123,
            /// <summary>
            /// Eye Size - Beady Eyes 0--+255 Anime Eyes
            /// </summary>
            SHAPE_EYE_SIZE = 124,
            /// <summary>
            ///  - Short Legs 0--+255 Long Legs
            /// </summary>
            SHAPE_LEG_LENGTH = 125,
            /// <summary>
            ///  - Short Arms 0--+255 Long arms
            /// </summary>
            SHAPE_ARM_LENGTH = 126,
            /// <summary>
            ///  - Pink 0--+255 Black
            /// </summary>
            SKIN_LIPSTICK_COLOR = 127,
            /// <summary>
            ///  - No Lipstick 0--+255 More Lipstick
            /// </summary>
            SKIN_LIPSTICK = 128,
            /// <summary>
            ///  - No Lipgloss 0--+255 Glossy
            /// </summary>
            SKIN_LIPGLOSS = 129,
            /// <summary>
            ///  - No Eyeliner 0--+255 Full Eyeliner
            /// </summary>
            SKIN_EYELINER = 130,
            /// <summary>
            ///  - No Blush 0--+255 More Blush
            /// </summary>
            SKIN_BLUSH = 131,
            /// <summary>
            ///  - Pink 0--+255 Orange
            /// </summary>
            SKIN_BLUSH_COLOR = 132,
            /// <summary>
            ///  - Clear 0--+255 Opaque
            /// </summary>
            SKIN_OUT_SHDW_OPACITY = 133,
            /// <summary>
            ///  - No Eyeshadow 0--+255 More Eyeshadow
            /// </summary>
            SKIN_OUTER_SHADOW = 134,
            /// <summary>
            ///  - Light 0--+255 Dark
            /// </summary>
            SKIN_OUT_SHDW_COLOR = 135,
            /// <summary>
            ///  - No Eyeshadow 0--+255 More Eyeshadow
            /// </summary>
            SKIN_INNER_SHADOW = 136,
            /// <summary>
            ///  - No Polish 0--+255 Painted Nails
            /// </summary>
            SKIN_NAIL_POLISH = 137,
            /// <summary>
            ///  - Clear 0--+255 Opaque
            /// </summary>
            SKIN_BLUSH_OPACITY = 138,
            /// <summary>
            ///  - Light 0--+255 Dark
            /// </summary>
            SKIN_IN_SHDW_COLOR = 139,
            /// <summary>
            ///  - Clear 0--+255 Opaque
            /// </summary>
            SKIN_IN_SHDW_OPACITY = 140,
            /// <summary>
            ///  - Dark Green 0--+255 Black
            /// </summary>
            SKIN_EYELINER_COLOR = 141,
            /// <summary>
            ///  - Pink 0--+255 Black
            /// </summary>
            SKIN_NAIL_POLISH_COLOR = 142,
            /// <summary>
            ///  - Sparse 0--+255 Dense
            /// </summary>
            HAIR_EYEBROW_DENSITY = 143,
            /// <summary>
            ///  - 5 O'Clock Shadow 0--+255 Bushy Hair
            /// </summary>
            HAIR_HAIR_THICKNESS = 144,
            /// <summary>
            /// Saddle Bags - Less Saddle 0--+255 More Saddle
            /// </summary>
            SHAPE_SADDLEBAGS = 145,
            /// <summary>
            /// Taper Back - Wide Back 0--+255 Narrow Back
            /// </summary>
            HAIR_HAIR_TAPER_BACK = 146,
            /// <summary>
            /// Taper Front - Wide Front 0--+255 Narrow Front
            /// </summary>
            HAIR_HAIR_TAPER_FRONT = 147,
            /// <summary>
            ///  - Short Neck 0--+255 Long Neck
            /// </summary>
            SHAPE_NECK_LENGTH = 148,
            /// <summary>
            /// Eyebrow Height - Higher 0--+255 Lower
            /// </summary>
            HAIR_LOWER_EYEBROWS = 149,
            /// <summary>
            /// Lower Bridge - Low 0--+255 High
            /// </summary>
            SHAPE_LOWER_BRIDGE_NOSE = 150,
            /// <summary>
            /// Nostril Division - High 0--+255 Low
            /// </summary>
            SHAPE_LOW_SEPTUM_NOSE = 151,
            /// <summary>
            /// Jaw Angle - Low Jaw 0--+255 High Jaw
            /// </summary>
            SHAPE_JAW_ANGLE = 152,
            /// <summary>
            /// Shear Front - Full Front 0--+255 Sheared Front
            /// </summary>
            HAIR_HAIR_SHEAR_FRONT = 153,
            /// <summary>
            ///  - Less Volume 0--+255 More Volume
            /// </summary>
            HAIR_HAIR_VOLUME = 154,
            /// <summary>
            /// Lip Cleft Depth - Shallow 0--+255 Deep
            /// </summary>
            SHAPE_LIP_CLEFT_DEEP = 155,
            /// <summary>
            /// Puffy Eyelids - Flat 0--+255 Puffy
            /// </summary>
            SHAPE_PUFFY_LOWER_LIDS = 156,
            /// <summary>
            ///  - Sunken Eyes 0--+255 Bugged Eyes
            /// </summary>
            SHAPE_EYE_DEPTH = 157,
            /// <summary>
            ///  - Flat Head 0--+255 Long Head
            /// </summary>
            SHAPE_HEAD_LENGTH = 158,
            /// <summary>
            ///  - Less Freckles 0--+255 More Freckles
            /// </summary>
            SKIN_BODY_FRECKLES = 159,
            /// <summary>
            ///  - Low 0--+255 High
            /// </summary>
            UNDERSHIRT_COLLAR_BACK = 160,
            JACKET_COLLAR_BACK_161 = 161,
            SHIRT_COLLAR_BACK_162 = 162,
            /// <summary>
            ///  - Short Pigtails 0--+255 Long Pigtails
            /// </summary>
            HAIR_PIGTAILS = 163,
            /// <summary>
            ///  - Short Ponytail 0--+255 Long Ponytail
            /// </summary>
            HAIR_PONYTAIL = 164,
            /// <summary>
            /// Butt Size - Flat Butt 0--+255 Big Butt
            /// </summary>
            SHAPE_BUTT_SIZE = 165,
            /// <summary>
            /// Ear Tips - Flat 0--+255 Pointy
            /// </summary>
            SHAPE_POINTY_EARS = 166,
            /// <summary>
            /// Lip Ratio - More Upper Lip 0--+255 More Lower Lip
            /// </summary>
            SHAPE_LIP_RATIO = 167,
            SHIRT_SLEEVE_LENGTH_168 = 168,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            SHIRT_SHIRT_BOTTOM = 169,
            SHIRT_COLLAR_FRONT_170 = 170,
            SHIRT_SHIRT_RED = 171,
            SHIRT_SHIRT_GREEN = 172,
            SHIRT_SHIRT_BLUE = 173,
            PANTS_PANTS_RED = 174,
            PANTS_PANTS_GREEN = 175,
            PANTS_PANTS_BLUE = 176,
            SHOES_SHOES_RED = 177,
            SHOES_SHOES_GREEN = 178,
            /// <summary>
            ///  - Low 0--+255 High
            /// </summary>
            PANTS_WAIST_HEIGHT = 179,
            PANTS_PANTS_LENGTH_180 = 180,
            /// <summary>
            /// Pants Fit - Tight Pants 0--+255 Loose Pants
            /// </summary>
            PANTS_LOOSE_LOWER_CLOTHING = 181,
            SHOES_SHOES_BLUE = 182,
            SOCKS_SOCKS_RED = 183,
            SOCKS_SOCKS_GREEN = 184,
            SOCKS_SOCKS_BLUE = 185,
            UNDERSHIRT_UNDERSHIRT_RED = 186,
            UNDERSHIRT_UNDERSHIRT_GREEN = 187,
            UNDERSHIRT_UNDERSHIRT_BLUE = 188,
            UNDERPANTS_UNDERPANTS_RED = 189,
            UNDERPANTS_UNDERPANTS_GREEN = 190,
            UNDERPANTS_UNDERPANTS_BLUE = 191,
            GLOVES_GLOVES_RED = 192,
            /// <summary>
            /// Shirt Fit - Tight Shirt 0--+255 Loose Shirt
            /// </summary>
            SHIRT_LOOSE_UPPER_CLOTHING = 193,
            GLOVES_GLOVES_GREEN = 194,
            GLOVES_GLOVES_BLUE = 195,
            JACKET_JACKET_RED = 196,
            JACKET_JACKET_GREEN = 197,
            JACKET_JACKET_BLUE = 198,
            /// <summary>
            /// Sleeve Looseness - Tight Sleeves 0--+255 Loose Sleeves
            /// </summary>
            SHIRT_SHIRTSLEEVE_FLAIR = 199,
            /// <summary>
            /// Knee Angle - Knock Kneed 0--+255 Bow Legged
            /// </summary>
            SHAPE_BOWED_LEGS = 200,
            /// <summary>
            ///  - Short hips 0--+255 Long Hips
            /// </summary>
            SHAPE_HIP_LENGTH = 201,
            /// <summary>
            ///  - Fingerless 0--+255 Fingers
            /// </summary>
            GLOVES_GLOVE_FINGERS = 202,
            /// <summary>
            /// bustle skirt - no bustle 0--+255 more bustle
            /// </summary>
            SKIRT_SKIRT_BUSTLE = 203,
            /// <summary>
            ///  - Short 0--+255 Long
            /// </summary>
            SKIRT_SKIRT_LENGTH = 204,
            /// <summary>
            ///  - Open Front 0--+255 Closed Front
            /// </summary>
            SKIRT_SLIT_FRONT = 205,
            /// <summary>
            ///  - Open Back 0--+255 Closed Back
            /// </summary>
            SKIRT_SLIT_BACK = 206,
            /// <summary>
            ///  - Open Left 0--+255 Closed Left
            /// </summary>
            SKIRT_SLIT_LEFT = 207,
            /// <summary>
            ///  - Open Right 0--+255 Closed Right
            /// </summary>
            SKIRT_SLIT_RIGHT = 208,
            /// <summary>
            /// Skirt Fit - Tight Skirt 0--+255 Poofy Skirt
            /// </summary>
            SKIRT_SKIRT_LOOSENESS = 209,
            SHIRT_SHIRT_WRINKLES = 210,
            PANTS_PANTS_WRINKLES = 211,
            /// <summary>
            /// Jacket Wrinkles - No Wrinkles 0--+255 Wrinkles
            /// </summary>
            JACKET_JACKET_WRINKLES = 212,
            /// <summary>
            /// Package - Coin Purse 0--+255 Duffle Bag
            /// </summary>
            SHAPE_MALE_PACKAGE = 213,
            /// <summary>
            /// Inner Eye Corner - Corner Down 0--+255 Corner Up
            /// </summary>
            SHAPE_EYELID_INNER_CORNER_UP = 214,
            SKIRT_SKIRT_RED = 215,
            SKIRT_SKIRT_GREEN = 216,
            SKIRT_SKIRT_BLUE = 217, 

            /// <summary>
            /// Avatar Physics section.  These are 0 type visual params which get transmitted.
            /// </summary>

            /// <summary>
            /// Breast Part 1 
            /// </summary>
            BREAST_PHYSICS_MASS = 218,
            BREAST_PHYSICS_GRAVITY = 219,
            BREAST_PHYSICS_DRAG = 220,
            BREAST_PHYSICS_UPDOWN_MAX_EFFECT = 221,
            BREAST_PHYSICS_UPDOWN_SPRING = 222,
            BREAST_PHYSICS_UPDOWN_GAIN = 223,
            BREAST_PHYSICS_UPDOWN_DAMPING = 224,
            BREAST_PHYSICS_INOUT_MAX_EFFECT = 225,
            BREAST_PHYSICS_INOUT_SPRING = 226,
            BREAST_PHYSICS_INOUT_GAIN = 227,
            BREAST_PHYSICS_INOUT_DAMPING = 228,
            /// <summary>
            /// Belly
            /// </summary>
            BELLY_PHYISCS_MASS = 229,
            BELLY_PHYSICS_GRAVITY = 230,
            BELLY_PHYSICS_DRAG = 231,
            BELLY_PHYISCS_UPDOWN_MAX_EFFECT = 232,
            BELLY_PHYSICS_UPDOWN_SPRING = 233,
            BELLY_PHYSICS_UPDOWN_GAIN = 234,
            BELLY_PHYSICS_UPDOWN_DAMPING = 235,

            /// <summary>
            /// Butt
            /// </summary>
            BUTT_PHYSICS_MASS = 236,
            BUTT_PHYSICS_GRAVITY = 237,
            BUTT_PHYSICS_DRAG = 238,
            BUTT_PHYSICS_UPDOWN_MAX_EFFECT = 239,
            BUTT_PHYSICS_UPDOWN_SPRING = 240,
            BUTT_PHYSICS_UPDOWN_GAIN = 241,
            BUTT_PHYSICS_UPDOWN_DAMPING = 242,
            BUTT_PHYSICS_LEFTRIGHT_MAX_EFFECT = 243,
            BUTT_PHYSICS_LEFTRIGHT_SPRING = 244,
            BUTT_PHYSICS_LEFTRIGHT_GAIN = 245,
            BUTT_PHYSICS_LEFTRIGHT_DAMPING = 246,
            /// <summary>
            /// Breast Part 2
            /// </summary>
            BREAST_PHYSICS_LEFTRIGHT_MAX_EFFECT = 247,
            BREAST_PHYSICS_LEFTRIGHT_SPRING= 248,
            BREAST_PHYSICS_LEFTRIGHT_GAIN = 249,
            BREAST_PHYSICS_LEFTRIGHT_DAMPING = 250,

            // Ubit: 07/96/2013 new parameters 
            _APPEARANCEMESSAGE_VERSION = 251,    //ID 11000

            SHAPE_HOVER = 252,    //ID 11001
        }
        #endregion
    }
}
