using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace BoyCSwagger
{
    
    public static class BoySCExntension
    {
        public static IApplicationBuilder UseBoyCSwagger(this IApplicationBuilder builder,string MapPathSwaggerJson, string title, string ocelotConfigPath, string swaggerversion = "2.0", string inforversion = "v1")
        {
            builder.Map(MapPathSwaggerJson, configuration =>
            {
                configuration.Use(async (context, next) =>
                {
                    var jsonconfig = File.ReadAllText(ocelotConfigPath);

                    
                    var MainSwagger = JsonConvert.DeserializeObject<dynamic>("{\"swagger\":\"" + swaggerversion +
                        "\", \"info\": {" +
                        "\"version\":\"" + inforversion + "\",\"title\":\"" + title + "\"},\"paths\":{},\"definitions\":{" +
                        "}}");
                    var definitionsMain = new List<string>();
                    var objconfig = JsonConvert.DeserializeObject<dynamic>(jsonconfig);
                    foreach (var site in objconfig.ReRoutes)
                    {
                        try
                        {
                            var siteurl = "http://" + site.DownstreamHostAndPorts[0].Host + ":" + site.DownstreamHostAndPorts[0].Port;

                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Clear();
                            client.BaseAddress = new Uri(siteurl);
                            var res = client.GetAsync("/swagger/v1/swagger.json").Result;
                            var UpstreamPathTemplate = site.UpstreamPathTemplate;

                            var resultJson = res.Content.ReadAsStringAsync().Result;
                            var swag = JsonConvert.DeserializeObject<dynamic>(resultJson);
                            var paths = swag.paths;
                            var d = site.DownstreamPathTemplate;
                            var downpath = (string)site.DownstreamPathTemplate;
                            var downApi = paths.GetValue(downpath);
                            var path = JsonConvert.DeserializeObject<dynamic>("{\"" + (string)site.UpstreamPathTemplate + "\":{}}");
                            if (downApi != null)
                            {
                                foreach (var method in site.UpstreamHttpMethod)
                                {
                                    string m = ((string)method);
                                    switch (m.ToUpper())
                                    {
                                        case "GET":
                                            if (downApi.get != null)
                                            {

                                                if (downApi.get.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.get.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                path.Add("get", downApi.get);



                                            }
                                            break;
                                        case "POST":
                                            if (downApi.post != null)
                                            {

                                                foreach (var p in downApi.post.parameters)
                                                {
                                                    if (((string)JsonConvert.SerializeObject(p)).Contains("$ref"))
                                                    {
                                                        var defname = p.schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }
                                                    }
                                                }
                                                if (downApi.post.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.get.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                path.Add("post", downApi.post);

                                            }

                                            break;
                                        case "PUT":
                                            if (downApi.put != null)
                                            {

                                                foreach (var p in downApi.put.parameters)
                                                {
                                                    if (((string)JsonConvert.SerializeObject(p)).Contains("$ref"))
                                                    {
                                                        var defname = p.schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }
                                                    }
                                                }
                                                if (downApi.put.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.get.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                path.Add("put", downApi.put);

                                            }
                                            break;
                                        case "DELETE":
                                            if (downApi.delete != null)
                                            {

                                                if (downApi.delete.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.delete.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                if (downApi.delete.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.get.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                path.Add("delete", downApi.delete);


                                            }
                                            break;
                                    }
                                }
                                MainSwagger.paths.Add((string)site.UpstreamPathTemplate, path);

                            }



                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    await context.Response.WriteAsync((string)JsonConvert.SerializeObject(MainSwagger));
                });
            });
            return builder;
        }

        public static IApplicationBuilder UseBoyCSwagger(this IApplicationBuilder builder,string title,string ocelotConfigPath, string swaggerversion="2.0",string inforversion="v1")
        {
            builder.Map("/swagger2/v1/swagger.json", configuration =>
            {
                configuration.Use(async (context, next) =>
                {
                var jsonconfig = File.ReadAllText(ocelotConfigPath);
                
                
                var MainSwagger = JsonConvert.DeserializeObject<dynamic>("{\"swagger\":\""+swaggerversion+
                    "\", \"info\": {" +
                    "\"version\":\""+inforversion+"\",\"title\":\""+title+"\"},\"paths\":{},\"definitions\":{" +
                    "}}");
                    var definitionsMain = new List<string>();                    
                    var objconfig = JsonConvert.DeserializeObject<dynamic>(jsonconfig);
                    foreach (var site in objconfig.ReRoutes)
                    {
                        try
                        {
                            var siteurl = "http://" + site.DownstreamHostAndPorts[0].Host + ":" + site.DownstreamHostAndPorts[0].Port;

                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Clear();
                            client.BaseAddress = new Uri(siteurl);
                            var res = client.GetAsync("/swagger/v1/swagger.json").Result;
                            var UpstreamPathTemplate = site.UpstreamPathTemplate;

                            var resultJson = res.Content.ReadAsStringAsync().Result;
                            var swag = JsonConvert.DeserializeObject<dynamic>(resultJson);
                            var paths = swag.paths;
                            var d = site.DownstreamPathTemplate;
                            var downpath = (string)site.DownstreamPathTemplate;
                            var downApi = paths.GetValue(downpath);
                            var path = JsonConvert.DeserializeObject<dynamic>("{\""+(string)site.UpstreamPathTemplate+"\":{}}");
                            if (downApi != null)
                            {
                                foreach (var method in site.UpstreamHttpMethod)
                                {
                                    string m = ((string)method);
                                    switch (m.ToUpper())
                                    {
                                        case "GET":
                                            if (downApi.get != null)
                                            {
                                                
                                                if (downApi.get.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.get.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            if (MainSwagger.definitions.GetValue(defn) == null)
                                                                MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                path.Add("get", downApi.get);
                                                


                                            }
                                            break;
                                        case "POST":
                                            if (downApi.post != null)
                                            {
                                                
                                                foreach (var p in downApi.post.parameters)
                                                {
                                                     if (((string)JsonConvert.SerializeObject(p)).Contains("$ref"))
                                                    {
                                                        var defname = p.schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            if (MainSwagger.definitions.GetValue(defn) == null)
                                                                MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }
                                                    }
                                                }
                                                if (downApi.post.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.post.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            if (MainSwagger.definitions.GetValue(defn) == null)
                                                                MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                path.Add("post", downApi.post);
                                                
                                            }
                                          
                                            break;
                                        case "PUT":
                                            if (downApi.put != null)
                                            {

                                                foreach (var p in downApi.put.parameters)
                                                {
                                                   if (((string)JsonConvert.SerializeObject(p)).Contains("$ref"))
                                                    {
                                                        var defname = p.schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            if (MainSwagger.definitions.GetValue(defn)==null)
                                                            MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }
                                                    }
                                                }
                                                if (downApi.put.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.get.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            if (MainSwagger.definitions.GetValue(defn) == null)
                                                                MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                path.Add("put", downApi.put);

                                            }
                                            break;
                                        case "DELETE":
                                            if (downApi.delete != null)
                                            {

                                                if (downApi.delete.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.delete.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            if (MainSwagger.definitions.GetValue(defn) == null)
                                                                MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }
                                                if (downApi.delete.responses.GetValue("200") != null)
                                                {
                                                    var schema = downApi.get.responses.GetValue("200").GetValue("schema");
                                                    if (schema != null)
                                                    {
                                                        var defname = schema.GetValue("$ref");
                                                        if (defname != null)
                                                        {
                                                            string defn = ((string)defname).Replace("#/definitions/", "");
                                                            if (MainSwagger.definitions.GetValue(defn) == null)
                                                                MainSwagger.definitions.Add(defn, swag.definitions.GetValue(defn));
                                                        }

                                                    }
                                                }                                                
                                                path.Add("delete", downApi.delete);


                                            }
                                            break;
                                    }
                                }
                                MainSwagger.paths.Add((string)site.UpstreamPathTemplate, path);

                            }

                           

                        }
                        catch(Exception ex) {
                            Console.WriteLine(ex.Message);
                        }

                    }
                   
                    await context.Response.WriteAsync((string)JsonConvert.SerializeObject(MainSwagger));
                });
            });
            return builder;
        }
        private static string GetDefin(dynamic obj, dynamic defin, ref string proName)
        {
            var schema = (string)JsonConvert.SerializeObject(obj.schema);
            if (schema.Contains("$ref"))
            {
                var def = schema.Split(':')[1].Replace("#/definitions/", "");
                def = def.Substring(0, def.Length - 1).Replace("\"", "");
                proName = def;
                var defstr = (string)JsonConvert.SerializeObject(defin.GetValue(def));
                return defstr;

            }
            return "";


        }
    }
}
