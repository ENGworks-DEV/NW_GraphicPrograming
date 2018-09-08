﻿using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System.Windows;

namespace NW_GraphicPrograming.Nodes.Clash
{
    public class ClashResults : Node
    {
        public ClashResults(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Clash Test", typeof(object));
            AddOutputPortToNode("Navis Clash", typeof(ClashResult));
            AddOutputPortToNode("Navis Clash Groups", typeof(ClashResult));


            AddControlToNode(new Label() { Content = "Clash Result", FontSize = 13, FontWeight = FontWeights.Bold });
            Name = "Get clash results";

        }


        public override void Calculate()
        {



            List<ClashResult> clashResultList = new List<ClashResult>();
            List<ClashResultGroup> clashResultGroupList = new List<ClashResultGroup>();
            List<object> lct = new List<object>();


            var type = InputPorts[0].Data.GetType();
            //Check for null in input
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                foreach (var i in (System.Collections.IEnumerable)InputPorts[0].Data)
                {
                    if (i.GetType() == typeof(ClashTest))
                    {
                        ClashTest item = i as ClashTest;
                        foreach (var t in item.Children)
                        {

                            if (t.IsGroup)
                            {
                                clashResultGroupList.Add(t as ClashResultGroup);
                            }
                            else
                            {
                                clashResultList.Add(t as ClashResult);
                            }
                        }
                    }
                }



                OutputPorts[0].Data = clashResultList;
                OutputPorts[1].Data = clashResultGroupList;
            }

            if (type == typeof(ClashTest))
            {
                ClashTest item = InputPorts[0].Data as ClashTest;
                foreach (var t in item.Children)
                {

                    if (t.IsGroup)
                    {
                        clashResultGroupList.Add(t as ClashResultGroup);
                    }
                    else
                    {
                        clashResultList.Add(t as ClashResult);
                    }
                }
                OutputPorts[0].Data = clashResultList;
                OutputPorts[1].Data = clashResultGroupList;

            }


        }


        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);

            // add your xml serialization methods here
        }

        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);

            // add your xml deserialization methods here
        }

        public override Node Clone()
        {
            return new ClashResults(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}