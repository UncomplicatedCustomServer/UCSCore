using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UncomplicatedCustomServerCore.Integrations
{
#nullable enable
    internal static class UCR
    {
        public static readonly Assembly? assembly = Loader.Plugins.FirstOrDefault(p => p.Name is "UncomplicatedCustomRoles")?.Assembly;

        public static string GetRegisteredCustomRoles()
        {
            foreach (IPlugin<IConfig> pl in Loader.Plugins)
                Log.Info(pl.Name);

            try
            {
                Type? customRole = assembly?.GetType("UncomplicatedCustomRoles.API.Features.CustomRole");
                Log.Info($"UCR Status: ASSEMBLY - {assembly is not null} - custom roles: {customRole is not null} - list: {customRole?.GetProperty("List") is not null}");
                object? result = customRole?.GetProperty("List")?.GetValue(null, null);
                if (result is not null)
                    return Loader.Serializer.Serialize(result);
                return Loader.Serializer.Serialize(new List<object>());
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return string.Empty;
            }
        }
    }
}
