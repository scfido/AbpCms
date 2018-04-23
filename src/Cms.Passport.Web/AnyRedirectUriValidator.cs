using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cms.Passport.Web
{
    /// <summary>
    /// 支持“*”的回调地址验证器。不想限制客户端回调地址时设置RedirectUris = { "*" }。
    /// </summary>
    public class AnyRedirectUriValidator : IRedirectUriValidator
    {
        static string anyHost = "*";

        /// <summary>
        /// Checks if a given URI string is in a collection of strings (using ordinal ignore case comparison)
        /// </summary>
        /// <param name="uris">The uris.</param>
        /// <param name="requestedUri">The requested URI.</param>
        /// <returns></returns>
        private bool ContainsUrl(IEnumerable<string> uris, string requestedUri)
        {
            if (uris.IsNullOrEmpty())
                return false;

            if (uris.Contains(anyHost))
                return true;
            else
                return uris.Contains(requestedUri, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether a redirect URI is valid for a client.
        /// </summary>
        /// <param name="requestedUri">The requested URI.</param>
        /// <param name="client">The client.</param>
        /// <returns>
        ///   <c>true</c> is the URI is valid; <c>false</c> otherwise.
        /// </returns>
        public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(ContainsUrl(client.RedirectUris, requestedUri));
        }

        /// <summary>
        /// Determines whether a post logout URI is valid for a client.
        /// </summary>
        /// <param name="requestedUri">The requested URI.</param>
        /// <param name="client">The client.</param>
        /// <returns>
        ///   <c>true</c> is the URI is valid; <c>false</c> otherwise.
        /// </returns>
        public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(ContainsUrl(client.PostLogoutRedirectUris, requestedUri));
        }
    }
}
