namespace FreeImageAPI
{
    using System;
    using System.Runtime.InteropServices;

    public class FreeImage
    {
        private const string dllName = "FreeImage.dll";

        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AcquireMemory")]
        public static extern long AcquireMemory(uint stream, ref IntPtr data, ref int size_in_bytes);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustBrightness")]
        public static extern bool AdjustBrightness(uint dib, double percentage);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustContrast")]
        public static extern bool AdjustContrast(uint dib, double percentage);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustCurve")]
        public static extern bool AdjustCurve(uint dib, byte[] lut, FREE_IMAGE_COLOR_CHANNEL channel);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustGamma")]
        public static extern bool AdjustGamma(uint dib, double gamma);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Allocate")]
        public static extern uint Allocate(int width, int height, int bpp, uint red_mask, uint green_mask, uint blue_mask);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AllocateT")]
        public static extern uint AllocateT(FREE_IMAGE_TYPE ftype, int width, int height, int bpp, uint red_mask, uint green_mask, uint blue_mask);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_AppendPage")]
        public static extern void AppendPage(uint bitmap, uint data);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Clone")]
        public static extern uint Clone(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_CloseMemory")]
        public static extern void CloseMemory(uint stream);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_CloseMultiBitmap")]
        public static extern long CloseMultiBitmap(uint bitmap, int flags);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ColorQuantize")]
        public static extern uint ColorQuantize(uint dib, FREE_IMAGE_QUANTIZE quantize);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Composite")]
        public static extern uint Composite(uint fg, bool useFileBkg, [In, MarshalAs(UnmanagedType.LPStruct)] RGBQUAD appBkColor, uint bg);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertFromRawBits")]
        public static extern uint ConvertFromRawBits(byte[] bits, int width, int height, int pitch, uint bpp, uint redMask, uint greenMask, uint blueMask, bool topDown);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo16Bits555")]
        public static extern uint ConvertTo16Bits555(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo16Bits565")]
        public static extern uint ConvertTo16Bits565(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo24Bits")]
        public static extern uint ConvertTo24Bits(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo32Bits")]
        public static extern uint ConvertTo32Bits(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo4Bits")]
        public static extern uint ConvertTo4Bits(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo8Bits")]
        public static extern uint ConvertTo8Bits(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertToRawBits")]
        public static extern void ConvertToRawBits(IntPtr bits, uint dib, int pitch, uint bpp, uint redMask, uint greenMask, uint blueMask, bool topDown);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertToRGBF")]
        public static extern uint ConvertToRGBF(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertToStandardType")]
        public static extern uint ConvertToStandardType(uint dib, FREE_IMAGE_TYPE dst_type, bool scale_linear);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertToType")]
        public static extern uint ConvertToType(uint dib, FREE_IMAGE_TYPE dst_type, bool scale_linear);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Copy")]
        public static extern uint Copy(uint dib, int left, int top, int right, int bottom);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_CreateICCProfile")]
        public static extern FIICCPROFILE CreateICCProfile(uint dib, IntPtr data, uint size);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_DeInitialise")]
        public static extern void DeInitialise();
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_DeInitialise")]
        public static extern void DeInitialize();
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_DeletePage")]
        public static extern void DeletePage(uint bitmap, int page);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_DestroyICCProfile")]
        public static extern void DestroyICCProfile(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Dither")]
        public static extern uint Dither(uint dib, FREE_IMAGE_DITHER algorithm);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsExportBPP")]
        public static extern bool FIFSupportsExportBPP(FREE_IMAGE_FORMAT format, int bpp);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsExportType")]
        public static extern bool FIFSupportsExportType(FREE_IMAGE_FORMAT format, FREE_IMAGE_TYPE ftype);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsICCProfiles")]
        public static extern bool FIFSupportsICCProfiles(FREE_IMAGE_FORMAT format, FREE_IMAGE_TYPE ftype);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsReading")]
        public static extern bool FIFSupportsReading(FREE_IMAGE_FORMAT format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsWriting")]
        public static extern bool FIFSupportsWriting(FREE_IMAGE_FORMAT format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_FlipHorizontal")]
        public static extern bool FlipHorizontal(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_FlipVertical")]
        public static extern bool FlipVertical(uint dib);
        [DllImport("FreeImage.dll")]
        public static extern IntPtr FreeImage_GetInfo(uint dib);
        [DllImport("FreeImage.dll")]
        public static extern IntPtr FreeImage_GetInfoHeader(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetBackgroundColor")]
        public static extern bool GetBackgroundColor(uint dib, [Out, MarshalAs(UnmanagedType.LPStruct)] RGBQUAD bkcolor);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetBits")]
        public static extern IntPtr GetBits(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetBlueMask")]
        public static extern uint GetBlueMask(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetBPP")]
        public static extern uint GetBPP(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetChannel")]
        public static extern uint GetChannel(uint dib, FREE_IMAGE_COLOR_CHANNEL channel);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetColorsUsed")]
        public static extern uint GetColorsUsed(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetColorType")]
        public static extern FREE_IMAGE_COLOR_TYPE GetColorType(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetCopyrightMessage")]
        public static extern string GetCopyrightMessage();
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetDIBSize")]
        public static extern uint GetDIBSize(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetDotsPerMeterX")]
        public static extern uint GetDotsPerMeterX(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetDotsPerMeterY")]
        public static extern uint GetDotsPerMeterY(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFCount")]
        public static extern int GetFIFCount();
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFDescription")]
        public static extern string GetFIFDescription(FREE_IMAGE_FORMAT format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFExtensionList")]
        public static extern string GetFIFExtensionList(FREE_IMAGE_FORMAT format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFFromFilename")]
        public static extern FREE_IMAGE_FORMAT GetFIFFromFilename(string filename);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFFromFormat")]
        public static extern FREE_IMAGE_FORMAT GetFIFFromFormat(string format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFFromMime")]
        public static extern FREE_IMAGE_FORMAT GetFIFFromMime(string mime);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFRegExpr")]
        public static extern string GetFIFRegExpr(FREE_IMAGE_FORMAT format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFileType")]
        public static extern FREE_IMAGE_FORMAT GetFileType(string filename, int size);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFileTypeFromMemory")]
        public static extern FREE_IMAGE_FORMAT GetFileTypeFromMemory(uint stream, int size);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFormatFromFIF")]
        public static extern string GetFormatFromFIF(FREE_IMAGE_FORMAT format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetGreenMask")]
        public static extern uint GetGreenMask(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetHeight")]
        public static extern uint GetHeight(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetHistogram")]
        public static extern bool GetHistogram(uint dib, IntPtr histo, FREE_IMAGE_COLOR_CHANNEL channel);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetICCProfile")]
        public static extern FIICCPROFILE GetICCProfile(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetImageType")]
        public static extern FREE_IMAGE_TYPE GetImageType(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetLine")]
        public static extern uint GetLine(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetLockedPageNumbers")]
        public static extern bool GetLockedPageNumbers(uint bitmap, IntPtr pages, IntPtr count);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPageCount")]
        public static extern int GetPageCount(uint bitmap);
        public static unsafe RGBQUAD[] GetPaletteCopy(uint dib)
        {
            RGBQUAD[] rgbquadArray = new RGBQUAD[0x100];
            if (GetBPP(dib) <= 8)
            {
                byte* rawPalette = (byte*) GetRawPalette(dib);
                for (int i = 0; i < 0x100; i++)
                {
                    rgbquadArray[i] = new RGBQUAD();
                    rgbquadArray[i].rgbBlue = rawPalette[0];
                    rawPalette++;
                    rgbquadArray[i].rgbGreen = rawPalette[0];
                    rawPalette++;
                    rgbquadArray[i].rgbRed = rawPalette[0];
                    rawPalette++;
                    rgbquadArray[i].rgbReserved = rawPalette[0];
                    rawPalette++;
                }
            }
            return rgbquadArray;
        }

        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPitch")]
        public static extern uint GetPitch(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPixelColor")]
        public static extern bool GetPixelColor(uint dib, uint x, uint y, [Out, MarshalAs(UnmanagedType.LPStruct)] RGBQUAD value);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPixelIndex")]
        public static extern bool GetPixelIndex(uint dib, uint x, uint y, ref byte value);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPalette")]
        private static extern UIntPtr GetRawPalette(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetRedMask")]
        public static extern uint GetRedMask(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetScanLine")]
        public static extern IntPtr GetScanLine(uint dib, int scanline);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetTransparencyCount")]
        public static extern uint GetTransparencyCount(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetTransparencyTable")]
        public static extern IntPtr GetTransparencyTable(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetVersion")]
        public static extern string GetVersion();
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_GetWidth")]
        public static extern uint GetWidth(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_HasBackgroundColor")]
        public static extern bool HasBackgroundColor(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Initialise")]
        public static extern void Initialise(bool loadLocalPluginsOnly);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Initialise")]
        public static extern void Initialize(bool loadLocalPluginsOnly);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_InsertPage")]
        public static extern void InsertPage(uint bitmap, int page, uint data);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Invert")]
        public static extern bool Invert(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_IsLittleEndian")]
        public static extern bool IsLittleEndian();
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_IsPluginEnabled")]
        public static extern int IsPluginEnabled(FREE_IMAGE_FORMAT format);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_IsTransparent")]
        public static extern bool IsTransparent(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_JPEGTransform")]
        public static extern bool JPEGTransform(string src_file, string dst_file, FREE_IMAGE_JPEG_OPERATION operation, bool perfect);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Load")]
        public static extern uint Load(FREE_IMAGE_FORMAT format, string filename, int flags);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_LoadFromMemory")]
        public static extern uint LoadFromMemory(FREE_IMAGE_FORMAT fif, uint stream, int flags);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_LockPage")]
        public static extern uint LockPage(uint bitmap, int page);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_LookupSVGColor")]
        public static extern bool LookupSVGColor(string szColor, ref int red, ref int green, ref int blue);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_LookupX11Color")]
        public static extern bool LookupX11Color(string szColor, ref int red, ref int green, ref int blue);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_MovePage")]
        public static extern bool MovePage(uint bitmap, int target, int source);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_OpenMemory")]
        public static extern uint OpenMemory(IntPtr bits, int size_in_bytes);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_OpenMultiBitmap")]
        public static extern uint OpenMultiBitmap(FREE_IMAGE_FORMAT format, string filename, bool createNew, bool readOnly, bool keepCacheInMemory, int flags);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Paste")]
        public static extern bool Paste(uint dst, uint src, int left, int top, int alpha);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Rescale")]
        public static extern uint Rescale(uint dib, int dst_width, int dst_height, FREE_IMAGE_FILTER filter);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_RotateClassic")]
        public static extern uint RotateClassic(uint dib, double angle);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_RotateEx")]
        public static extern uint RotateEx(uint dib, double angle, double xShift, double yShift, double xOrigin, double yOrigin, bool useMask);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Save")]
        public static extern bool Save(FREE_IMAGE_FORMAT format, uint dib, string filename, int flags);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SaveToMemory")]
        public static extern bool SaveToMemory(FREE_IMAGE_FORMAT fif, uint dib, uint stream, int flags);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SeekMemory")]
        public static extern bool SeekMemory(uint stream, long offset, int origin);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetBackgroundColor")]
        public static extern bool SetBackgroundColor(uint dib, [In, MarshalAs(UnmanagedType.LPStruct)] RGBQUAD bkcolor);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetChannel")]
        public static extern bool SetChannel(uint dib, uint dib8, FREE_IMAGE_COLOR_CHANNEL channel);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetOutputMessage")]
        public static extern void SetOutputMessage(FreeImage_OutputMessageFunction omf);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetPixelColor")]
        public static extern bool SetPixelColor(uint dib, uint x, uint y, [In, MarshalAs(UnmanagedType.LPStruct)] RGBQUAD value);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetPixelIndex")]
        public static extern bool SetPixelIndex(uint dib, uint x, uint y, ref byte value);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetPluginEnabled")]
        public static extern int SetPluginEnabled(FREE_IMAGE_FORMAT format, bool enabled);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetTransparencyTable")]
        public static extern void SetTransparencyTable(uint dib, IntPtr table, int count);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_SetTransparent")]
        public static extern void SetTransparent(uint dib, bool enabled);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_TellMemory")]
        public static extern long TellMemory(uint stream, int flags);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Threshold")]
        public static extern uint Threshold(uint dib, uint T);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_TmoDrago03")]
        public static extern uint TmoDrago03(uint dib, double gamma, double exposure);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_TmoReinhard05")]
        public static extern uint TmoReinhard05(uint dib, double intensity, double contrast);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ToneMapping")]
        public static extern uint ToneMapping(uint dib, FREE_IMAGE_TMO tmo, double first_param, double second_param);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_Unload")]
        public static extern void Unload(uint dib);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_UnlockPage")]
        public static extern void UnlockPage(uint bitmap, uint data, bool changed);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ZLibCompress")]
        public static extern int ZLibCompress(IntPtr target, int target_size, IntPtr source, int source_size);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ZLibCRC32")]
        public static extern int ZLibCRC32(int crc, IntPtr source, int source_size);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ZLibGUnzip")]
        public static extern int ZLibGUnzip(IntPtr target, int target_size, IntPtr source, int source_size);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ZLibGZip")]
        public static extern int ZLibGZip(IntPtr target, int target_size, IntPtr source, int source_size);
        [DllImport("FreeImage.dll", EntryPoint="FreeImage_ZLibUncompress")]
        public static extern int ZLibUncompress(IntPtr target, int target_size, IntPtr source, int source_size);
    }
}

