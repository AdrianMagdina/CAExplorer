// programmed by Adrian Magdina in 2013
// in this file is the implementation of reading and writing of Main Window specific data.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Media;
using System.IO;
using System.Xml.XPath;
using System.Globalization;

namespace CAExplorerNamespace
{
    //in this class is the implementation of reading/writing of data connected with main window.
    public static class CAMainWindowRW
    {
        //reading of configuration file, here are read default colors.
        public static void ReadCAMainWindowData(string fileNameIn, CAMainWindowVM caMainWindowVMIn)
        {
            XmlDocument aXmlDocument = new XmlDocument();

            try
            {
                aXmlDocument.Load(fileNameIn);

                XmlNode aXmlNodeSelectedDefaultGridColor = aXmlDocument.SelectSingleNode("/CAMainWindowData/SelectedDefaultGridColor");
                if (aXmlNodeSelectedDefaultGridColor != null)
                {
                    caMainWindowVMIn.SelectedDefaultGridColor = ReadColor(aXmlNodeSelectedDefaultGridColor);
                }

                XmlNode aXmlNodeSelectedDefaultSelectionFrameColor = aXmlDocument.SelectSingleNode("/CAMainWindowData/SelectedDefaultSelectionFrameColor");
                if (aXmlNodeSelectedDefaultSelectionFrameColor != null)
                {
                    caMainWindowVMIn.SelectedDefaultSelectionFrameColor = ReadColor(aXmlNodeSelectedDefaultSelectionFrameColor);
                }

                XmlNode aXmlNodeSelectedDefaultMarkingColor = aXmlDocument.SelectSingleNode("/CAMainWindowData/SelectedDefaultMarkingColor");
                if (aXmlNodeSelectedDefaultMarkingColor != null)
                {
                    caMainWindowVMIn.SelectedDefaultMarkingColor = ReadColor(aXmlNodeSelectedDefaultMarkingColor);
                }

                XmlNode aXmlNodeSelectedDefaultMouseOverColor = aXmlDocument.SelectSingleNode("/CAMainWindowData/SelectedDefaultMouseOverColor");
                if (aXmlNodeSelectedDefaultMouseOverColor != null)
                {
                    caMainWindowVMIn.SelectedDefaultMouseOverColor = ReadColor(aXmlNodeSelectedDefaultMouseOverColor);
                }

                XmlNode aXmlNodeSelectedDefaultBackgroundColor = aXmlDocument.SelectSingleNode("/CAMainWindowData/SelectedDefaultBackgroundColor");
                if (aXmlNodeSelectedDefaultBackgroundColor != null)
                {
                    caMainWindowVMIn.SelectedDefaultBackgroundColor = ReadColor(aXmlNodeSelectedDefaultBackgroundColor);
                }

                XmlNode aXmlNodeSelectedDefaultStartInterpColor = aXmlDocument.SelectSingleNode("/CAMainWindowData/SelectedDefaultStartInterpColor");
                if (aXmlNodeSelectedDefaultStartInterpColor != null)
                {
                    caMainWindowVMIn.SelectedDefaultStartInterpColor = ReadColor(aXmlNodeSelectedDefaultStartInterpColor);
                }

                XmlNode aXmlNodeSelectedDefaultEndInterpColor = aXmlDocument.SelectSingleNode("/CAMainWindowData/SelectedDefaultEndInterpColor");
                if (aXmlNodeSelectedDefaultEndInterpColor != null)
                {
                    caMainWindowVMIn.SelectedDefaultEndInterpColor = ReadColor(aXmlNodeSelectedDefaultEndInterpColor);
                }

                XmlNodeList aDefaultStateColorsCollection = aXmlDocument.SelectNodes("/CAMainWindowData/DefaultStateColor");

                caMainWindowVMIn.DefaultStateColorsCollection.Clear();

                foreach (XmlNode aXmlNode in aDefaultStateColorsCollection)
                {
                    int aId = 0;
                    Color aColor;

                    if (aXmlNode.Attributes.Count == 1)
                    {
                        aId = Convert.ToInt32(aXmlNode.Attributes[0].Value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        throw new CAExplorerException("The attribute id must be present in Default State Color element!");
                    }

                    aColor = ReadColor(aXmlNode);

                    caMainWindowVMIn.DefaultStateColorsCollection.Add(new StateAndColor(aId, aColor));
                }
            }
            catch (Exception ex)
            {
                if (ex is XPathException || ex is ArgumentException || ex is FormatException || ex is OverflowException || ex is PathTooLongException
                    || ex is XmlException || ex is DirectoryNotFoundException || ex is IOException || ex is FileNotFoundException
                    || ex is UnauthorizedAccessException || ex is System.Security.SecurityException || ex is CAExplorerException)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during reading CA Explorer Configuration", "Problem with reading CA Explorer configuration : \n" + fileNameIn
                            + "\n" + "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    DialogMediator.ShowMessageBox(null, "Loading default backup configuration", "The default backup configuration will be loaded ! : \n",
                                                                                              System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);

                    caMainWindowVMIn.SelectedDefaultGridColor = DefaultBackupValues.DefaultBackupGridColor;
                    caMainWindowVMIn.SelectedDefaultSelectionFrameColor = DefaultBackupValues.DefaultBackupSelectionFrameColor;
                    caMainWindowVMIn.SelectedDefaultMarkingColor = DefaultBackupValues.DefaultBackupMarkingColor;
                    caMainWindowVMIn.SelectedDefaultMouseOverColor = DefaultBackupValues.DefaultBackupMouseOverColor;
                    caMainWindowVMIn.SelectedDefaultBackgroundColor = DefaultBackupValues.DefaultBackupBackgroundColor;
                    caMainWindowVMIn.SelectedDefaultStartInterpColor = DefaultBackupValues.DefaultBackupStartInterpColor;
                    caMainWindowVMIn.SelectedDefaultEndInterpColor = DefaultBackupValues.DefaultBackupEndInterpColor;
                    foreach (StateAndColor aStateAndColor in DefaultBackupValues.DefaultBackupStateColors)
                    {
                        caMainWindowVMIn.DefaultStateColorsCollection.Add(aStateAndColor);
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        private static Color ReadColor(XmlNode colorNodeIn)
        {
            Color aColor = new Color();

            XmlNode aXmlNodeR = colorNodeIn.SelectSingleNode("./R");

            if (aXmlNodeR != null)
            {
                aColor.R = Convert.ToByte(aXmlNodeR.InnerText, CultureInfo.InvariantCulture);
            }

            XmlNode aXmlNodeG = colorNodeIn.SelectSingleNode("./G");

            if (aXmlNodeG != null)
            {
                aColor.G = Convert.ToByte(aXmlNodeG.InnerText, CultureInfo.InvariantCulture);
            }

            XmlNode aXmlNodeB = colorNodeIn.SelectSingleNode("./B");

            if (aXmlNodeB != null)
            {
                aColor.B = Convert.ToByte(aXmlNodeB.InnerText, CultureInfo.InvariantCulture);
            }

            XmlNode aXmlNodeA = colorNodeIn.SelectSingleNode("./A");

            if (aXmlNodeA != null)
            {
                aColor.A = Convert.ToByte(aXmlNodeA.InnerText, CultureInfo.InvariantCulture);
            }

            return aColor;
        }

        //all available CAs are read here.
        public static void ReadAllAvailableCAs(CAMainWindowVM caMainWindowVMIn)
        {
            string[] aAvailableCAs = null;

            try
            {
                aAvailableCAs = Directory.GetFiles(@".\SavedCA\", "*.xml");
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is UnauthorizedAccessException || ex is PathTooLongException || ex is DirectoryNotFoundException)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during reading CAs", "Problem with reading files from directory \\SavedCA\\ \n" +
                        ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                else
                {
                    throw;
                }
            }

            if (aAvailableCAs != null)
            {
                string aCAsWithProblem = null;
                string aCAExceptions = null;

                foreach (string aAvailableCAItem in aAvailableCAs)
                {
                    try
                    {
                        CAGrid2DVM aCAGrid2DVM = caMainWindowVMIn.CreateNewEmptyCA();
                        bool aWasOK = CAGrid2DRW.ReadCA(aAvailableCAItem, caMainWindowVMIn, ref aCAGrid2DVM);

                        if (aWasOK == true)
                        {
                            caMainWindowVMIn.AddNewCA(aCAGrid2DVM);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is FileNotFoundException || ex is System.Security.SecurityException || ex is UriFormatException || ex is XmlException
                            || ex is FormatException || ex is OverflowException || ex is ArgumentOutOfRangeException || ex is NullReferenceException 
                            || ex is CAExplorerException || ex is InvalidOperationException)
                        {
                            aCAsWithProblem += aAvailableCAItem + "\n";
                            aCAExceptions += ex.Message + "\n";
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                if (aCAsWithProblem != null)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during reading CA", "Problem with reading following CA files : \n" + aCAsWithProblem
                        + "\n" + "Following exceptions occured : \n" + aCAExceptions, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
            }
        }

        //all available CAs are written here.
        public static void WriteAllAvailableCAs(CAMainWindowVM caMainWindowVMIn)
        {
            string aCAsWithProblem = null;
            string aCAExceptions = null;

            foreach (CAGrid2DVM aCAGrid2DVMItem in caMainWindowVMIn.Grid2DViewModelList)
            {
                if (aCAGrid2DVMItem != null)
                {
                    string aFilename = null;
                    try
                    {
                        aFilename = @".\SavedCA\" + aCAGrid2DVMItem.CAName + ".xml";
                        CAGrid2DRW.WriteCA(aFilename, aCAGrid2DVMItem);
                    }
                    catch (Exception ex)
                    {
                        if (ex is InvalidOperationException || ex is EncoderFallbackException || ex is ArgumentException || ex is UnauthorizedAccessException
                            || ex is DirectoryNotFoundException || ex is IOException || ex is System.Security.SecurityException || ex is PathTooLongException)
                        {
                            aCAsWithProblem += aFilename + "\n";
                            aCAExceptions += ex.Message + "\n";
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            if (aCAsWithProblem != null)
            {
                DialogMediator.ShowMessageBox(null, "Exception during writing CA", "Problem with writing following CA files : \n" + aCAsWithProblem
                    + "\n" + "Following exceptions occured : \n" + aCAExceptions, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        //one specific CA is deleted here.
        public static void DeleteSpecificCA(CAGrid2DVM caGrid2DVMIn)
        {
            string aFilename = null;

            try
            {
                if (caGrid2DVMIn != null)
                {
                    aFilename = @".\SavedCA\" + caGrid2DVMIn.CAName + ".xml";
                    File.Delete(aFilename);
                }
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException || ex is DirectoryNotFoundException || ex is IOException || ex is PathTooLongException)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during deleting CA", "Problem with deleting following CA : \n" + aFilename
                      + "\n" + "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                else
                {
                    throw;
                }
            }
        }

        //here is written the configuration file, default colors are written to this file.
        public static void WriteCAMainWindowData(string fileNameIn, CAMainWindowVM caMainWindowVMIn)
        {
            XmlWriter aXmlWriter = null;

            try
            {
                aXmlWriter = new XmlTextWriter(fileNameIn, Encoding.UTF8);

                aXmlWriter.WriteStartDocument();

                aXmlWriter.WriteStartElement("CAMainWindowData");

                WriteColor(aXmlWriter, "SelectedDefaultGridColor", caMainWindowVMIn.SelectedDefaultGridColor);
                WriteColor(aXmlWriter, "SelectedDefaultSelectionFrameColor", caMainWindowVMIn.SelectedDefaultSelectionFrameColor);
                WriteColor(aXmlWriter, "SelectedDefaultMarkingColor", caMainWindowVMIn.SelectedDefaultMarkingColor);
                WriteColor(aXmlWriter, "SelectedDefaultMouseOverColor", caMainWindowVMIn.SelectedDefaultMouseOverColor);
                WriteColor(aXmlWriter, "SelectedDefaultBackgroundColor", caMainWindowVMIn.SelectedDefaultBackgroundColor);
                WriteColor(aXmlWriter, "SelectedDefaultStartInterpColor", caMainWindowVMIn.SelectedDefaultStartInterpColor);
                WriteColor(aXmlWriter, "SelectedDefaultEndInterpColor", caMainWindowVMIn.SelectedDefaultEndInterpColor);

                foreach (StateAndColor aStateColor in caMainWindowVMIn.DefaultStateColorsCollection)
                {
                    WriteColor(aXmlWriter, "DefaultStateColor", aStateColor.StateColor, "id", aStateColor.State.ToString(CultureInfo.InvariantCulture));
                }

                aXmlWriter.WriteEndElement();

                aXmlWriter.WriteEndDocument();
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is EncoderFallbackException || ex is ArgumentException || ex is UnauthorizedAccessException
                        || ex is DirectoryNotFoundException || ex is IOException || ex is System.Security.SecurityException)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during writing CA Explorer configuration", "Problem with writing of CA Explorer configuration file!: \n" +
                        "Your changes will not be saved!" + "\n" + "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                aXmlWriter.Close();
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
