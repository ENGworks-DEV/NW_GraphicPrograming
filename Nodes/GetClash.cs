﻿using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System.Windows;

namespace NW_GraphicPrograming.Nodes
{
    public class NW_ClashResult : Node
    {
        public NW_ClashResult(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Clash Test", typeof(object));
            AddOutputPortToNode("Navis Clash", typeof(ClashResult));
            AddOutputPortToNode("Navis Clash Groups", typeof(ClashResult));


            AddControlToNode(new Label() { Content = "Clash Result", FontSize = 13, FontWeight = FontWeights.Bold });

            
        }


        public override void Calculate()
        {



            List<ClashResult> clashResultList = new List<ClashResult>();
            List<ClashResultGroup> clashResultGroupList = new List<ClashResultGroup>();
            List<object> lct = new List<object>();

            

            //Check for null in input
            if (InputPorts[0].Data != null && InputPorts[0].Data is List<ClashTest>)
            {
                foreach (var item in InputPorts[0].Data as List<ClashTest>)
                {
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
            return new NW_ClashResult(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}