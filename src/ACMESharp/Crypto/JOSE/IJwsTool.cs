﻿using System;

namespace ACMESharp.Crypto.JOSE
{
    /// <summary>
    /// Defines the interface for a tool that provides the required
    /// JOSE Web Signature (JWS) functions as used by the ACME protocol.
    /// </summary>
    public interface IJwsTool : IDisposable
    {
        string JwsAlg
        { get; }

        void Init();

        string Export();

        void Import(string exported);

        object ExportJwk(bool canonical = false);

        ReadOnlySpan<byte> Sign(ReadOnlySpan<byte> raw);

        bool Verify(ReadOnlySpan<byte> raw, ReadOnlySpan<byte> sig);

        string ExportSubjectPublicKeyInfoPem();

    }
}
