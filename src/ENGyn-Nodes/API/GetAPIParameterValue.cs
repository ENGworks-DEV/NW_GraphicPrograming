﻿using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System;
using System.Reflection;
using System.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace ENGyn.Nodes.API
{
    public class GetAPIPropertyValue : Node
    {
        public GetAPIPropertyValue(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Input", typeof(object));
            AddInputPortToNode("Name", typeof(string));
            
            AddOutputPortToNode("Output", typeof(object));

    }

        

        public override void Calculate()
        {
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            List<object> output = new List<object>();
            OutputPorts[0].Data =  Process(doc, output);

        }

        private IList<object> Process(Document doc, List<object> output)
        {
            if (InputPorts[0].Data != null)
            {
                var input = InputPorts[0].Data;
                var properties = InputPorts[1].Data;
                if (MainTools.IsList(input))
                {

                    foreach (var item in (System.Collections.IEnumerable)input)
                    {
                        var iterator = item;

                        if (item.GetType() == typeof(SavedItemReference))
                        {
                            iterator = doc.ResolveReference(item as SavedItemReference);
                        }

                        if (MainTools.IsList(properties))
                        {
                            foreach (var prop in (System.Collections.IEnumerable)properties)
                            {

                                dynamic d = prop;
                                string method = prop as string;
                                var types = iterator.GetType();
                                PropertyInfo props = types.GetProperty(method);

                                object value = props.GetValue(iterator);

                                output.Add(value);
                            }
                        }
                        else
                        {
                            try
                            {
                                dynamic d = properties;
                                string method = properties as string;
                                method = d;
                                var types = iterator.GetType();
                                PropertyInfo prop = types.GetProperty(method);

                                object value = prop.GetValue(iterator);

                                output.Add(value);
                            }

                            catch
                            {
                                output.Add(null);
                            }
                        }


                    }

                }
                else
                {
                    var iterator = input;

                    if (input.GetType() == typeof(SavedItemReference))
                    {
                        iterator = doc.ResolveReference(input as SavedItemReference);
                    }

                    if (MainTools.IsList(properties))
                    {


                        foreach (var prop in (System.Collections.IEnumerable)properties)
                        {
                            string method = prop as string;
                            var types = iterator.GetType();
                            PropertyInfo props = types.GetProperty(method);

                            object value = props.GetValue(iterator);

                            output.Add(value);
                        }
                    }
                    else
                    {
                        try
                        {
                            string method = properties as string;
                            var types = iterator.GetType();
                            PropertyInfo prop = types.GetProperty(method);

                           
                            if (MainTools.IsList(iterator))
                            {
                                Map(prop, (List<object>)iterator);
                            }

                            else
                            {
                                object value = prop.GetValue(iterator);

                                output.Add(value);
                            }

                        }

                        catch
                        {
                            output.Add(null);
                        }
                    }
                }


            }

           return output as IList<object>;
        }


        public override Node Clone()
        {
            return new GetAPIPropertyValue(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }


        /// <summary>
        ///     Produces a new list by applying a projection function to each item of the input list(s) and
        ///     storing the result.
        /// </summary>
        /// <param name="Property">
        ///     Function that consumes an item from each input list and produces a value that is stored
        ///     in the output list.
        /// </param>
        /// <param name="lists">Lists to be combined/mapped into a new list.</param>
        public static IList<object> Map(PropertyInfo Property, params IEnumerable<object>[] lists)
        {
            if (!lists.Any())
                throw new ArgumentException("Need at least one list to map.");
            IEnumerable<List<object>> argList = lists[0].Select(x => new List<object> { x });
            foreach (var pair in
                lists.Skip(1).SelectMany(list => list.Zip(argList, (o, objects) => new { o, objects })))
                pair.objects.Add(pair.o);
            return argList.Select(x => Property.GetValue(x.ToArray())).ToList();
        }
    }

}

