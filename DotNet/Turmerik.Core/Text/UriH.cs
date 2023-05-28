﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turmerik.Core.Text
{
    public static class UriH
    {
        public const string URI_SCHEME_REGEX_STR = @"^[a-zA-Z0-9\-_]*\:$";
        public const string URI_SCHEME_START_REGEX_STR = @"^[a-zA-Z0-9\-_]*\:\/{2}";

        public static readonly Regex UriSchemeRegex = new Regex(URI_SCHEME_REGEX_STR);
        public static readonly Regex UriSchemeStartRegex = new Regex(URI_SCHEME_START_REGEX_STR);

        public static string? GetUriScheme(string uri)
        {
            string? schemeStartStr = GetUriSchemeStartStr(uri);
            string? schemeStr = schemeStartStr?.TrimEnd('/');

            return schemeStr;
        }

        public static string? GetUriSchemeStartStr(string uri)
        {
            var match = UriSchemeStartRegex.Match(uri);
            string? schemeStartStr = null;

            if (match.Success)
            {
                schemeStartStr = match.Value;
            }

            return schemeStartStr;
        }

        public static string GetUriWithoutScheme(string uri)
        {
            var match = UriSchemeStartRegex.Match(uri);
            string relUri = uri;

            if (match.Success)
            {
                relUri = uri.Substring(match.Value.Length);
            }

            return relUri;
        }

        public static string GetRelUri(string uri, bool trimFwSlashes = false)
        {
            string relUri = GetUriWithoutScheme(uri);

            if (relUri != uri)
            {
                int idx = relUri.IndexOf('/');

                if (idx >= 0)
                {
                    int qsIdx = relUri.IndexOf('?');

                    if (qsIdx < 0 || qsIdx > idx)
                    {
                        relUri = relUri.Substring(idx);
                    }
                }
                else
                {
                    relUri = string.Empty;
                }

                if (trimFwSlashes)
                {
                    relUri = relUri.Trim('/');
                }
            }

            return relUri;
        }

        public static string GetUriWithoutQueryString(string uri, bool trimFwSlashes = false)
        {
            string uriWithoutQueryString = uri;
            int idx = uri.IndexOf('?');

            if (idx >= 0)
            {
                uriWithoutQueryString = uri.Substring(0, idx);
            }

            if (trimFwSlashes)
            {
                uriWithoutQueryString = uriWithoutQueryString.Trim('/');
            }

            return uriWithoutQueryString;
        }

        public static string GetRelUriWithoutQueryString(string uri, bool trimFwSlashes = false)
        {
            string relUriWithoutQueryString = GetUriWithoutQueryString(
                    GetRelUri(
                        uri,
                        trimFwSlashes),
                trimFwSlashes);

            return relUriWithoutQueryString;
        }
    }
}
