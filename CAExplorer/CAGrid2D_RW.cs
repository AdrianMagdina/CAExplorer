// programmed by Adrian Magdina in 2013
// in this file is the implementation of reading and writing of CA Grid specific data

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Media;
using System.IO;
using System.Globalization;

namespace CAExplorerNamespace
{
    //in this class is implemented reading/writing of data conntected to CA.
    public static class CAGrid2DRW
    {
        //reading of one specific CA.
        public static bool ReadCA(string fileNameIn, CAMainWindowVM caMainWindowVMIn, ref CAGrid2DVM caGrid2DVMIn)
        {
            int aColumns = 0;
            int aRows = 0;

            var aIsMarkedList = new List<bool>();
            var aCurrentCellStateList = new List<int>();
            var aXList = new List<int>();
            var aYList = new List<int>();
            var aStateToColorCollection = new List<StateAndColor>();

            CAGridInitializationMethodTypes? aCAGridCellInitializationMethod = null;
            string aCARuleFamilyString = null;
            string aCARule = null;

            bool aIsCA = false;

            XmlReader aXmlReader = null;

            try
            {
                aXmlReader = XmlReader.Create(fileNameIn);

                //reading all data connected to CA.
                while (aXmlReader.Read())
                {
                    if (aXmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (aXmlReader.Name)
                        {
                            case "ContentType":
                                {
                                    string aContent = aXmlReader.ReadString();
                                    if (aContent == "CellularAutomata")
                                    {
                                        aIsCA = true;
                                    }
                                    else if (aContent != "CellularAutomata")
                                    {
                                        return false;
                                    }
                                }
                                break;
                            case "CAGridCellInitializationMethod":
                                {
                                    aCAGridCellInitializationMethod = (CAGridInitializationMethodTypes)Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                }
                                break;
                            case "CARuleFamily":
                                {
                                    aCARuleFamilyString = aXmlReader.ReadString();
                                }
                                break;
                            case "CARule":
                                {
                                    aCARule = aXmlReader.ReadString();
                                }
                                break;
                            case "Name":
                                {
                                    caGrid2DVMIn.CAName = aXmlReader.ReadString();
                                }
                                break;
                            case "Columns":
                                {
                                    aColumns = Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                    if (aColumns < Constants.MinCAColumns)
                                    {
                                        throw new CAExplorerException("The value of columns must be higher than " + Constants.MinCAColumns.ToString(CultureInfo.InvariantCulture) + "!");
                                    }
                                }
                                break;
                            case "Rows":
                                {
                                    aRows = Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                    if (aRows < Constants.MinCARows)
                                    {
                                        throw new CAExplorerException("The value of rows must be higher than " + Constants.MinCARows.ToString(CultureInfo.InvariantCulture) + "!");
                                    }
                                }
                                break;
                            case "GridThickness":
                                {
                                    caGrid2DVMIn.GridThickness = (LineThickness)Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                }
                                break;
                            case "SelFrameThickness":
                                {
                                    caGrid2DVMIn.SelFrameThickness = (LineThickness)Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                }
                                break;
                            case "CellObjectWidth":
                                {
                                    caGrid2DVMIn.CellObjectWidth = Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                    if (caGrid2DVMIn.CellObjectWidth < Constants.MinCellSizeX)
                                    {
                                        throw new CAExplorerException("The value of cell object width must be higher than " + Constants.MinCellSizeX.ToString(CultureInfo.InvariantCulture) + "!");
                                    }
                                }
                                break;
                            case "CellObjectHeight":
                                {
                                    caGrid2DVMIn.CellObjectHeight = Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                    if (caGrid2DVMIn.CellObjectHeight < Constants.MinCellSizeY)
                                    {
                                        throw new CAExplorerException("The value of cell object height must be higher " + Constants.MinCellSizeY.ToString(CultureInfo.InvariantCulture) + "!");
                                    }
                                }
                                break;
                            case "StateColorAssigning":
                                {
                                    caGrid2DVMIn.StateColorAssigning = (StateColorAssigningType)Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                }
                                break;
                            case "SelectedGridColor":
                                {
                                    caGrid2DVMIn.SelectedGridColor = ReadColor(aXmlReader, "SelectedGridColor");
                                }
                                break;
                            case "SelectedSelectionFrameColor":
                                {
                                    caGrid2DVMIn.SelectedSelectionFrameColor = ReadColor(aXmlReader, "SelectedSelectionFrameColor");
                                }
                                break;
                            case "SelectedMarkingColor":
                                {
                                    caGrid2DVMIn.SelectedMarkingColor = ReadColor(aXmlReader, "SelectedMarkingColor");
                                }
                                break;
                            case "SelectedMouseOverColor":
                                {
                                    caGrid2DVMIn.SelectedMouseOverColor = ReadColor(aXmlReader, "SelectedMouseOverColor");
                                }
                                break;
                            case "SelectedBackgroundColor":
                                {
                                    caGrid2DVMIn.SelectedBackgroundColor = ReadColor(aXmlReader, "SelectedBackgroundColor");
                                }
                                break;
                            case "SelectedStartInterpColor":
                                {
                                    caGrid2DVMIn.SelectedStartInterpColor = ReadColor(aXmlReader, "SelectedStartInterpColor");
                                }
                                break;
                            case "SelectedEndInterpColor":
                                {
                                    caGrid2DVMIn.SelectedEndInterpColor = ReadColor(aXmlReader, "SelectedEndInterpColor");
                                }
                                break;
                            case "Cell":
                                {
                                    string aXString = null;
                                    string aYString = null;
                                    string aIsMarkedString = null;

                                    int aAttributeCount = aXmlReader.AttributeCount;
                                    if (aAttributeCount != 3)
                                    {
                                        throw new CAExplorerException("The cell element must have exactly 3 attributes!");
                                    }

                                    aXString = aXmlReader.GetAttribute(0);
                                    aYString = aXmlReader.GetAttribute(1);
                                    aIsMarkedString = aXmlReader.GetAttribute(2);

                                    int aXConverted = Convert.ToInt32(aXString, CultureInfo.InvariantCulture);
                                    int aYConverted = Convert.ToInt32(aYString, CultureInfo.InvariantCulture);
                                    aXList.Add(aXConverted);
                                    aYList.Add(aYConverted);

                                    bool aIsMarked = Convert.ToBoolean(aIsMarkedString, CultureInfo.InvariantCulture);
                                    aIsMarkedList.Add(aIsMarked);

                                    int aCurrentCellState = Convert.ToInt32(aXmlReader.ReadString(), CultureInfo.InvariantCulture);
                                    aCurrentCellStateList.Add(aCurrentCellState);

                                }
                                break;
                            case "StateColor":
                                {
                                    int aNr = 0;
                                    if (aXmlReader.HasAttributes)
                                    {
                                        aNr = Convert.ToInt32(aXmlReader.GetAttribute(0), CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        throw new CAExplorerException("The attribute id must be present in the State Color element!");
                                    }
   
                                    Color aStateColor = ReadColor(aXmlReader, "StateColor");
                                    aStateToColorCollection.Add(new StateAndColor(aNr, aStateColor));
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (aXmlReader != null)
                {
                    aXmlReader.Close();
                }
            }

            if (aIsCA == false)
            {
                return false;
            }

            //storing of read data in the viewmodels and models.
            CARuleFamilies? aCARuleFamilyEnum = null;

            foreach (ComboBoxItem aCARuleFamilyEnumItem in ConstantLists.CARuleFamilyItems)
            {
                if (aCARuleFamilyEnumItem.ComboBoxString == aCARuleFamilyString)
                {
                    aCARuleFamilyEnum = (CARuleFamilies)aCARuleFamilyEnumItem.ComboBoxId;
                    break;
                }
            }

            ICARuleData aCARuleData = null;

            foreach (ICARuleData aCARuleDataItem in caMainWindowVMIn.CAMainWindowModel.ListOfCARules)
            {
                if (aCARuleDataItem.CARuleFamily == aCARuleFamilyEnum && aCARule == aCARuleDataItem.CARuleName)
                {
                    aCARuleData = aCARuleDataItem;
                    break;
                }
            }

            ICARuleFamily aCARuleFamily = CARuleFactory.CreateCARuleFamily((CARuleFamilies)aCARuleFamilyEnum, aCARuleData);

            aCARuleData.CARuleName = aCARule;

            ICAGridCellInitialization aCAGridCellInitialization = CAGridCellInitializationFactory.CreateCAGridCellInitialization((CAGridInitializationMethodTypes)aCAGridCellInitializationMethod);

            caGrid2DVMIn.CreateCells(aColumns, aRows, aCARuleFamily, aCAGridCellInitialization);

            caGrid2DVMIn.StateToColorCollection.Clear();
            foreach (StateAndColor aStateAndColorItem in aStateToColorCollection)
            {
                caGrid2DVMIn.StateToColorCollection.Add(aStateAndColorItem);
            }

            for (int aCellNr = 0; aCellNr < aCurrentCellStateList.Count; aCellNr++)
            {
                caGrid2DVMIn.Cells[aCellNr].IsMarked = aIsMarkedList[aCellNr];

                caGrid2DVMIn.Cells[aCellNr].CellModel.CurrentCellState = aCurrentCellStateList[aCellNr];
                caGrid2DVMIn.Cells[aCellNr].CellModel.X = aXList[aCellNr];
                caGrid2DVMIn.Cells[aCellNr].CellModel.Y = aYList[aCellNr];
            }

            return true;
        }

        public static Color ReadColor(XmlReader xmlReaderIn, string colorNameIn)
        {
            byte aR = 0;
            byte aG = 0;
            byte aB = 0;
            byte aA = 0;

            while (xmlReaderIn.Read())
            {
                if (xmlReaderIn.NodeType == XmlNodeType.Element)
                {
                    switch (xmlReaderIn.Name)
                    {
                        case "R":
                            {
                                aR = Convert.ToByte(xmlReaderIn.ReadString(), CultureInfo.InvariantCulture);
                                break;
                            }
                        case "G":
                            {
                                aG = Convert.ToByte(xmlReaderIn.ReadString(), CultureInfo.InvariantCulture);
                                break;
                            }
                        case "B":
                            {
                                aB = Convert.ToByte(xmlReaderIn.ReadString(), CultureInfo.InvariantCulture);
                                break;
                            }
                        case "A":
                            {
                                aA = Convert.ToByte(xmlReaderIn.ReadString(), CultureInfo.InvariantCulture);
                                break;
                            }
                    }
                }
                else if (xmlReaderIn.NodeType == XmlNodeType.EndElement && xmlReaderIn.Name == colorNameIn)
                {
                    break;
                }
            }

            Color aColor = new Color();
            aColor.A = aA;
            aColor.R = aR;
            aColor.G = aG;
            aColor.B = aB;

            return aColor;
        }

        //here is implemented writing of one specific CA data.
        public static void WriteCA(string fileNameIn, CAGrid2DVM caGrid2DVMIn)
        {
            XmlWriter aXmlWriter = null;

            try
            {
                if (!Directory.Exists(fileNameIn))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileNameIn));
                }

                aXmlWriter = new XmlTextWriter(fileNameIn, Encoding.UTF8);

                aXmlWriter.WriteStartDocument();

                aXmlWriter.WriteStartElement("CA");

                aXmlWriter.WriteElementString("ContentType", "CellularAutomata");

                aXmlWriter.WriteElementString("CARuleFamily", caGrid2DVMIn.CAGrid2DModel.CARule.CARuleFamilyString);

                aXmlWriter.WriteElementString("CARule", caGrid2DVMIn.CAGrid2DModel.CARule.CARuleData.CARuleName);

                aXmlWriter.WriteElementString("Name", caGrid2DVMIn.CAName);

                aXmlWriter.WriteStartElement("Columns");
                aXmlWriter.WriteValue(caGrid2DVMIn.Columns);
                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteStartElement("Rows");
                aXmlWriter.WriteValue(caGrid2DVMIn.Rows);
                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteStartElement("GridThickness");
                aXmlWriter.WriteValue((int)caGrid2DVMIn.GridThickness);
                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteStartElement("SelFrameThickness");
                aXmlWriter.WriteValue((int)caGrid2DVMIn.SelFrameThickness);
                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteStartElement("CAGridCellInitializationMethod");
                aXmlWriter.WriteValue((int)caGrid2DVMIn.CAGrid2DModel.CAGridInitializationMethodType);
                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteStartElement("CellObjectWidth");
                aXmlWriter.WriteValue(caGrid2DVMIn.CellObjectWidth);
                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteStartElement("CellObjectHeight");
                aXmlWriter.WriteValue(caGrid2DVMIn.CellObjectHeight);
                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteStartElement("StateColorAssigning");
                aXmlWriter.WriteValue((int)caGrid2DVMIn.StateColorAssigning);
                aXmlWriter.WriteEndElement();

                WriteColor(aXmlWriter, "SelectedGridColor", caGrid2DVMIn.SelectedGridColor);
                WriteColor(aXmlWriter, "SelectedSelectionFrameColor", caGrid2DVMIn.SelectedSelectionFrameColor);
                WriteColor(aXmlWriter, "SelectedMarkingColor", caGrid2DVMIn.SelectedMarkingColor);
                WriteColor(aXmlWriter, "SelectedMouseOverColor", caGrid2DVMIn.SelectedMouseOverColor);
                WriteColor(aXmlWriter, "SelectedBackgroundColor", caGrid2DVMIn.SelectedBackgroundColor);
                WriteColor(aXmlWriter, "SelectedStartInterpColor", caGrid2DVMIn.SelectedStartInterpColor);
                WriteColor(aXmlWriter, "SelectedEndInterpColor", caGrid2DVMIn.SelectedEndInterpColor);

                foreach (StateAndColor aStateAndColor in caGrid2DVMIn.StateToColorCollection)
                {
                    WriteColor(aXmlWriter, "StateColor", aStateAndColor.StateColor, "id", aStateAndColor.State.ToString(CultureInfo.InvariantCulture));
                }

                foreach (CellVM aCellVM in caGrid2DVMIn.Cells)
                {
                    aXmlWriter.WriteStartElement("Cell");
                    aXmlWriter.WriteAttributeString("X", aCellVM.CellModel.X.ToString(CultureInfo.InvariantCulture));
                    aXmlWriter.WriteAttributeString("Y", aCellVM.CellModel.Y.ToString(CultureInfo.InvariantCulture));
                    aXmlWriter.WriteAttributeString("IsMarked", aCellVM.IsMarked.ToString(CultureInfo.InvariantCulture));
                    aXmlWriter.WriteValue(aCellVM.CellModel.CurrentCellState);
                    aXmlWriter.WriteEndElement();
                }

                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteEndDocument();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (aXmlWriter != null)
                {
                    aXmlWriter.Close();
                }
            }
        }

        public static void WriteColor(XmlWriter xmlWriterIn, string colorNameIn, Color colorIn, string attributeIn = null, string attributeValueIn = null)
        {
            xmlWriterIn.WriteStartElement(colorNameIn);

            if (attributeIn != null)
            {
                xmlWriterIn.WriteAttributeString(attributeIn, attributeValueIn);
            }

            xmlWriterIn.WriteStartElement("R");
            xmlWriterIn.WriteValue(colorIn.R);
            xmlWriterIn.WriteEndElement();

            xmlWriterIn.WriteStartElement("G");
            xmlWriterIn.WriteValue(colorIn.G);
            xmlWriterIn.WriteEndElement();

            xmlWriterIn.WriteStartElement("B");
            xmlWriterIn.WriteValue(colorIn.B);
            xmlWriterIn.WriteEndElement();

            xmlWriterIn.WriteStartElement("A");
            xmlWriterIn.WriteValue(colorIn.A);
            xmlWriterIn.WriteEndElement();

            xmlWriterIn.WriteEndElement();
        }
    }
}
