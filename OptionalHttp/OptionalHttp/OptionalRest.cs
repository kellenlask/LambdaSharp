using System;
using Optional;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OptionalHttp
{
    public static class OptionalRest
    {
        private const int DefaultTimeoutSeconds = 30;


        /// <summary>
        ///     Privately makes an HTTPClient with the given timeout and auth headers.
        /// </summary>
        private static HttpClient MakeHttpClient(
            TimeSpan? timeout = null,
            AuthenticationHeaderValue authHeader = null
        ) => new HttpClient {
            Timeout = timeout ?? TimeSpan.FromSeconds(DefaultTimeoutSeconds),
            DefaultRequestHeaders = {Authorization = authHeader}
        };


        /// <summary>
        ///     Nice little private exception handler so I don't have to duplicate a bunch of try-catch logic. Makes a
        ///     HTTP Client with the given auth headers and timeout. 
        /// </summary>
        /// <returns>The result as returned from the given function, or a None Option with exception details</returns>
        private static async Task<Option<T, Exception>> TryRequestAndDeserialize<T>(
            Func<HttpClient, Task<Option<T, Exception>>> requestFunc,
            TimeSpan? timeout = null,
            AuthenticationHeaderValue authHeader = null
        )
        {
            try
            {
                using (var client = MakeHttpClient(timeout, authHeader))
                {
                    return await requestFunc(client);
                }
            }
            catch (ArgumentNullException ex) // When URI is null
            {
                return Option.None<T, Exception>(ex);
            }
            catch (HttpRequestException ex) // HttpRequestException when request fails
            {
                return Option.None<T, Exception>(ex);
            }
            catch (JsonException ex) // When response deserialization fails
            {
                return Option.None<T, Exception>(ex);
            }
        }


        /// <summary>
        ///     Makes an HTTP GET against the given endpoint with the given authentication, timing out in the given
        ///     period. Does not provide Retry sugar -- you shouldn't be retrying anyway; consider a breaker box. 
        /// </summary>
        /// <param name="endpoint">Where to hit</param>
        /// <param name="timeout">How long to wait (default: 30sec)</param>
        /// <param name="authHeader">How to authenticate (default: no auth)</param>
        /// <typeparam name="T">What do you expect back</typeparam>
        /// <returns>Deserialized result if successful, else Option with Exception details</returns>
        public static async Task<Option<T, Exception>> GetAsync<T>(
            string endpoint,
            TimeSpan? timeout = null,
            AuthenticationHeaderValue authHeader = null
        ) => await TryRequestAndDeserialize(
            async client => {
                var response = await client.GetStringAsync(endpoint);
                return Option.Some<T, Exception>(JsonConvert.DeserializeObject<T>(response));
            },
            timeout,
            authHeader
        );


        /// <summary>
        ///     Makes an HTTP POST against the given endpoint with the given content & authentication, timing out in the
        ///     given period. Does not provide Retry sugar -- you shouldn't be retrying anyway; consider a breaker box. 
        /// </summary>
        /// <param name="endpoint">Where to hit</param>
        /// <param name="content">The content to POST</param>
        /// <param name="timeout">How long to wait (default: 30sec)</param>
        /// <param name="authHeader">How to authenticate (default: no auth)</param>
        /// <typeparam name="T">What do you expect back</typeparam>
        /// <returns>Deserialized result if successful, else Option with Exception details</returns>
        public static async Task<Option<T, Exception>> PostAsync<T>(
            string endpoint,
            HttpContent content,
            TimeSpan? timeout = null,
            AuthenticationHeaderValue authHeader = null
        ) => await TryRequestAndDeserialize(
            async client => {
                var response = await client.PostAsync(endpoint, content);
                var responseStr = await response.Content.ReadAsStringAsync();
                return Option.Some<T, Exception>(JsonConvert.DeserializeObject<T>(responseStr));
            },
            timeout,
            authHeader
        );


        /// <summary>
        ///     Makes an HTTP PUT against the given endpoint with the given content & authentication, timing out in the
        ///     given period. Does not provide Retry sugar -- you shouldn't be retrying anyway; consider a breaker box. 
        /// </summary>
        /// <param name="endpoint">Where to hit</param>
        /// <param name="content">The content to PUT</param>
        /// <param name="timeout">How long to wait (default: 30sec)</param>
        /// <param name="authHeader">How to authenticate (default: no auth)</param>
        /// <typeparam name="T">What do you expect back</typeparam>
        /// <returns>Deserialized result if successful, else Option with Exception details</returns>
        public static async Task<Option<T, Exception>> PutAsync<T>(
            string endpoint,
            HttpContent content,
            TimeSpan? timeout = null,
            AuthenticationHeaderValue authHeader = null
        ) => await TryRequestAndDeserialize(
            async client => {
                var response = await client.PutAsync(endpoint, content);
                var responseStr = await response.Content.ReadAsStringAsync();
                return Option.Some<T, Exception>(JsonConvert.DeserializeObject<T>(responseStr));
            },
            timeout,
            authHeader
        );
        
        
        /// <summary>
        ///     Makes an HTTP DELETE against the given endpoint with the given authentication, timing out in the given
        ///     period. Does not provide Retry sugar -- you shouldn't be retrying anyway; consider a breaker box. 
        /// </summary>
        /// <param name="endpoint">Where to hit</param>
        /// <param name="timeout">How long to wait (default: 30sec)</param>
        /// <param name="authHeader">How to authenticate (default: no auth)</param>
        /// <typeparam name="T">What do you expect back</typeparam>
        /// <returns>Deserialized result if successful, else Option with Exception details</returns>
        public static async Task<Option<T, Exception>> DeleteAsync<T>(
            string endpoint,
            TimeSpan? timeout = null,
            AuthenticationHeaderValue authHeader = null
        ) => await TryRequestAndDeserialize(
            async client => {
                var response = await client.DeleteAsync(endpoint);
                var responseStr = await response.Content.ReadAsStringAsync();
                return Option.Some<T, Exception>(JsonConvert.DeserializeObject<T>(responseStr));
            },
            timeout,
            authHeader
        );
    }
}
