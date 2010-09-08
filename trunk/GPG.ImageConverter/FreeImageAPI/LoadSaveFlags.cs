﻿namespace FreeImageAPI
{
    using System;

    public enum LoadSaveFlags
    {
        BMP_DEFAULT = 0,
        BMP_SAVE_RLE = 1,
        CUT_DEFAULT = 0,
        DDS_DEFAULT = 0,
        FAXG3_DEFAULT = 0,
        GIF_DEFAULT = 0,
        GIF_LOAD256 = 1,
        GIF_PLAYBACK = 2,
        HDR_DEFAULT = 0,
        ICO_DEFAULT = 0,
        ICO_MAKEALPHA = 1,
        IFF_DEFAULT = 0,
        JPEG_ACCURATE = 2,
        JPEG_CMYK = 0x1000,
        JPEG_DEFAULT = 0,
        JPEG_FAST = 1,
        JPEG_QUALITYAVERAGE = 0x400,
        JPEG_QUALITYBAD = 0x800,
        JPEG_QUALITYGOOD = 0x100,
        JPEG_QUALITYNORMAL = 0x200,
        JPEG_QUALITYSUPERB = 0x80,
        KOALA_DEFAULT = 0,
        LBM_DEFAULT = 0,
        MNG_DEFAULT = 0,
        PCD_BASE = 1,
        PCD_BASEDIV16 = 3,
        PCD_BASEDIV4 = 2,
        PCD_DEFAULT = 0,
        PCX_DEFAULT = 0,
        PNG_DEFAULT = 0,
        PNG_IGNOREGAMMA = 1,
        PNM_DEFAULT = 0,
        PNM_SAVE_ASCII = 1,
        PNM_SAVE_RAW = 0,
        PSD_DEFAULT = 0,
        RAS_DEFAULT = 0,
        TARGA_DEFAULT = 0,
        TARGA_LOAD_RGB888 = 1,
        TIFF_ADOBE_DEFLATE = 0x400,
        TIFF_CCITTFAX3 = 0x1000,
        TIFF_CCITTFAX4 = 0x2000,
        TIFF_CMYK = 1,
        TIFF_DEFAULT = 0,
        TIFF_DEFLATE = 0x200,
        TIFF_JPEG = 0x8000,
        TIFF_LZW = 0x4000,
        TIFF_NONE = 0x800,
        TIFF_PACKBITS = 0x100,
        WBMP_DEFAULT = 0,
        XBM_DEFAULT = 0,
        XPM_DEFAULT = 0
    }
}
