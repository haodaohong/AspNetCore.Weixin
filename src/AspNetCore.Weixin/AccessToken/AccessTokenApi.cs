﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 获取微信凭证数据服务接口
    /// </summary>
    internal static class AccessTokenApi
    {
        private const string GetAccessTokenEndpoint = "https://api.weixin.qq.com/cgi-bin/token";

        /// <summary>
        /// 获取微信凭证
        /// <para>access_token是公众号的全局唯一票据，公众号调用各接口时都需使用access_token。正常情况下access_token有效期为7200秒，重复获取将导致上次获取的access_token失效。由于获取access_token的api调用次数非常有限，建议开发者全局存储与更新access_token，频繁刷新access_token会导致api调用受限，影响自身业务。</para>
        /// <para>请开发者注意，由于技术升级，公众平台的开发接口的access_token长度将增长，其存储至少要保留512个字符空间。此修改将在1个月后生效，请开发者尽快修改兼容。</para>
        /// <para>公众号可以使用AppID和AppSecret调用本接口来获取access_token。AppID和AppSecret可在开发模式中获得（需要已经成为开发者，且帐号没有异常状态）。注意调用所有微信接口时均需使用https协议。</para>
        /// </summary>
        /// <param name="appid">开发者ID之AppId。
        /// <para>例如：<code>wxd8fb1eb9ecf48d15</code></para></param>
        /// <param name="secret">开发者ID之AppSecret。
        /// <para>例如：<code>b7ffe201d9f7db183b8827ebe789aa88</code></para></param>
        /// <param name="grantType">默认为：<code>client_credential</code></param>
        /// <returns>微信凭证数据
        /// <para>正常情况下，微信会返回下述JSON数据包给公众号。例如：</para>
        /// <code>{"access_token":"ACCESS_TOKEN","expires_in":7200}</code>
        /// </returns>
        /// <exception cref="WeixinException">
        /// <para>错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:</para>
        /// <code>{"errcode":40013,"errmsg":"invalid appid"}</code>
        /// </exception>
        public static async Task<AccessTokenJson> GetTokenAsync(string appid, string secret)
        {
            var query = new QueryBuilder();
            query.Add("grant_type", "client_credential");
            query.Add("appid", appid);
            query.Add("secret", secret);

            var requestUri = GetAccessTokenEndpoint + query.ToString();

            var sResult = await new HttpClient().GetStringAsync(requestUri);
            AccessTokenJson result = JsonConvert.DeserializeObject<AccessTokenJson>(sResult);
            if (result.Succeeded)
                return result;
            else
                throw new WeixinException(result);
        }
    }
}
