﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Kendo.Models;
using Kendo.Extensions;
using System.Web;

namespace Kendo.Controllers
{
    public class DemoController : BaseController
    {
        private List<string> examplesUrl = new List<string>();

        protected static readonly IDictionary<String, String> Docs =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                { "datasource", "framework" },
                { "templates", "framework" },
                { "mvvm", "framework" },
                { "drag", "framework" },
                { "validator", "framework" }
            };

        //
        // GET: /Web/
        public ActionResult Index(string section, string example)
        {
            var product = CurrentProduct();

            if (example == null)
            {
                return RedirectToRoutePermanent("Demo", new { section = section, example = "index" });
            }

            ViewBag.ShowCodeStrip = true;
            ViewBag.Product = product;
            ViewBag.NavProduct = CurrentNavProduct();
            ViewBag.Section = section;
            ViewBag.Example = example;

#if DEBUG
            ViewBag.Debug = true;
#else
            ViewBag.Debug = false;
#endif

            SetTheme();
            LoadNavigation();
            LoadCategories();

            FindCurrentExample(product);

            NavigationExample currentExample = ViewBag.CurrentExample;
            NavigationWidget currentWidget = ViewBag.CurrentWidget;
            if (currentWidget == null)
            {
                return HttpNotFound();
            }

            ViewBag.Description = Description(product, currentExample, currentWidget);

            var exampleFiles = new List<ExampleFile>();
            exampleFiles.AddRange(SourceCode(product, section, example));
            exampleFiles.AddRange(AdditionalSources(currentWidget.Sources, product));
            exampleFiles.AddRange(AdditionalSources(currentExample.Sources, product));
            ViewBag.ExampleFiles = exampleFiles.Where(file => file.Exists(Server));

            if (ViewBag.Mobile) {
                if (currentExample.Url.StartsWith("adaptive") && IsMobileDevice())
                {
                    return Redirect(Url.RouteUrl("MobileDeviceIndex"));
                }
            }

            var api = currentExample.Api ?? ViewBag.CurrentWidget.Api;
            if (!string.IsNullOrEmpty(api))
            {
                if (product == "kendo-ui")
                {
                    ViewBag.Api = "http://docs.telerik.com/kendo-ui/api/" + api;
                }
                else if (product == "php-ui")
                {
                    ViewBag.Api = "http://docs.telerik.com/kendo-ui/api/wrappers/php/kendo/ui" + Regex.Replace(api, "(web|dataviz|mobile)", "");
                }
                else if (product == "jsp-ui")
                {
                    ViewBag.Api = "http://docs.telerik.com/kendo-ui/api/wrappers/jsp" + Regex.Replace(api, "(web|dataviz|mobile)", "");
                }
                else if (product == "aspnet-mvc")
                {
                    if (api == "web/validator")
                    {
                        ViewBag.Api = "http://docs.telerik.com/kendo-ui/aspnet-mvc/validation";
                    }
                    else
                    {
                        ViewBag.Api = "http://docs.telerik.com/kendo-ui/api/wrappers/aspnet-mvc/kendo.mvc.ui.fluent" + Regex.Replace(api, "(web|dataviz)", "").Replace("mobile/", "/mobile") + "builder";
                    }
                }
            }

            if (currentWidget.Documentation != null && currentWidget.Documentation.ContainsKey(product))
            {
                ViewBag.Documentation = "http://docs.telerik.com/kendo-ui/" + currentWidget.Documentation[product];
            }

            if (currentWidget.Forum != null && currentWidget.Forum.ContainsKey(product))
            {
                ViewBag.Forum = "http://www.telerik.com/forums/" + currentWidget.Forum[product];
            }

            if (currentWidget.CodeLibrary != null && currentWidget.CodeLibrary.ContainsKey(product))
            {
                ViewBag.CodeLibrary = "http://www.telerik.com/support/code-library/" + currentWidget.CodeLibrary[product];
            }

            return View(
                string.Format("~/Views/demos/{0}/{1}.cshtml", section, example)
            );
        }

        private static readonly IDictionary<string, IFrameworkDescription> frameworkDescriptions = new Dictionary<string, IFrameworkDescription>
        {
            { "kendo-ui", new HtmlDescription() },
            { "aspnet-mvc", new AspNetMvcDescription() },
            { "jsp-ui", new JspDescription() },
            { "php-ui", new PhpDescription() },
        };

        private IEnumerable<ExampleFile> SourceCode(string product, string section, string example)
        {
            IFrameworkDescription framework = frameworkDescriptions[product];
            return framework.GetFiles(Server, example, section);
        }

        private IEnumerable<ExampleFile> AdditionalSources(IDictionary<string, IEnumerable<ExampleFile>> sources, string product)
        {
            var files = new List<ExampleFile>();

            if (sources != null && sources.ContainsKey(product))
            {
                files.AddRange(sources[product]);
            }

            return files;
        }

        protected string Description(string product, NavigationExample example, NavigationWidget widget)
        {
            if (example.Description != null && example.Description.ContainsKey(product))
            {
                return example.Description[product];
            }
            else if (widget.Description != null && widget.Description.ContainsKey(product))
            {
                return widget.Description[product];
            }

            return null;
        }

        protected void FindCurrentExample(string product)
        {
           var found = false;

           NavigationExample current = null;
           NavigationWidget currentWidget = null;

           foreach (NavigationWidget widget in ViewBag.Navigation)
           {
               foreach (NavigationExample example in widget.Items)
               {
                   if (example.ShouldInclude(product))
                   {
                       examplesUrl.Add("~/" + example.Url);
                   }

                   if (!found && IsCurrentExample(example.Url))
                   {
                       current = example;
                       currentWidget = widget;
                       found = true;
                   }
               }
           }

           ViewBag.CurrentWidget = currentWidget;

           if (currentWidget == null)
           {
               return;
           }

           ViewBag.Mobile = (currentWidget.Mobile && !current.DisableInMobile) || current.Mobile;
           ViewBag.MobileAngular = currentWidget.Mobile && current.Url.IndexOf("angular") > 0;
           ViewBag.CurrentExample = current;

           if (current.Title != null)
           {
               if (current.Title.ContainsKey(product))
               {
                   ViewBag.Title = current.Title[product];
               }
               else
               {
                   ViewBag.Title = current.Title["kendo-ui"];
               }
           }
           else
           {
               ViewBag.Title = current.Text;
           }

           if (current.Meta != null)
           {
               if (current.Meta.ContainsKey(product))
               {
                   ViewBag.Meta = current.Meta[product];
               }
               else
               {
                   ViewBag.Meta = current.Meta["kendo-ui"];
               }
           }
        }

        private bool IsCurrentExample(string url)
        {
            var section = ControllerContext.RouteData.GetRequiredString("section");
            var example = ControllerContext.RouteData.GetRequiredString("example");

            var components = url.Split('/');

            return (section == components[0] && example == components[1]) || (section == "upload" && example == "result" && components[0] == "upload" && components[1] == "index");
        }
    }
}