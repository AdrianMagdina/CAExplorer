// programmed by Adrian Magdina in 2013
// in this file is the implementation of reading of all rules that are configured in the xml.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Globalization;

namespace CAExplorerNamespace
{
    //implementation of reading of all CA Rules
    public static class CARulesRW
    {
        public static void ReadCARules(string fileNameIn, ref IList<ICARuleData> caRuleListIn)
        {
            string aCARulesWithProblem = null;
            string aCARulesExceptions = null;

            XmlDocument aXmlDocument = new XmlDocument();

            try
            {
                aXmlDocument.Load(fileNameIn);

                XmlNodeList aXmlNodeList = aXmlDocument.SelectNodes("/CARules/CARule");

                foreach (XmlNode aXmlNode in aXmlNodeList)
                {
                    string aCAName = null;
                    string aCARuleFamilyString = null;

                    try
                    {
                        //All data that specifies a rule is loaded here.
                        IList<int> aBirthList = new List<int>();
                        IList<int> aSurvivalList = new List<int>();

                        XmlNode aXmlNodeName = aXmlNode.SelectSingleNode("./Name");
                        aCAName = aXmlNodeName.InnerText;

                        XmlNode aXmlNodeRuleFamily = aXmlNode.SelectSingleNode("./CARuleFamily");

                        if (aXmlNodeRuleFamily == null)
                        {
                            throw new CAExplorerException("There was no rule family found in CA Rules configuration file!");
                        }
                        aCARuleFamilyString = aXmlNodeRuleFamily.InnerText;

                        XmlNodeList aXmlNodeListSurvival = aXmlNode.SelectNodes("./Survival");

                        foreach (XmlNode aXmlNodeSurvival in aXmlNodeListSurvival)
                        {
                            int aSurvival = Convert.ToInt32(aXmlNodeSurvival.InnerText, CultureInfo.InvariantCulture);
                            aSurvivalList.Add(aSurvival);
                        }

                        XmlNodeList aXmlNodeListBirth = aXmlNode.SelectNodes("./Birth");

                        foreach (XmlNode aXmlNodeBirth in aXmlNodeListBirth)
                        {
                            int aBirth = Convert.ToInt32(aXmlNodeBirth.InnerText, CultureInfo.InvariantCulture);
                            aBirthList.Add(aBirth);
                        }

                        XmlNode aXmlNodeThreshold = aXmlNode.SelectSingleNode("./Threshold");

                        int? aThreshold = null;
                        if (aXmlNodeThreshold != null)
                        {
                            aThreshold = Convert.ToInt32(aXmlNodeThreshold.InnerText, CultureInfo.InvariantCulture);
                        }

                        if (aThreshold != null && aThreshold < 0)
                        {
                            throw new CAExplorerException("The value of Threshold cannot be lower than zero!");
                        }

                        CARuleFamilies? aCARuleFamilyEnum = null;

                        foreach (ComboBoxItem aCARuleFamilyItem in ConstantLists.CARuleFamilyItems)
                        {
                            if (aCARuleFamilyItem.ComboBoxString == aCARuleFamilyString)
                            {
                                aCARuleFamilyEnum = (CARuleFamilies)aCARuleFamilyItem.ComboBoxId;
                                break;
                            }
                        }

                        XmlNode aXmlNodeCountOfStates = aXmlNode.SelectSingleNode("./CountOfStates");

                        int? aCountOfStates = null;

                        if (aXmlNodeCountOfStates != null)
                        {
                            aCountOfStates = Convert.ToInt32(aXmlNodeCountOfStates.InnerText, CultureInfo.InvariantCulture);
                        }

                        if (aCountOfStates != null && aCountOfStates < 0)
                        {
                            throw new CAExplorerException("The value of count of states cannot be lower than zero!");
                        }

                        XmlNode aXmlNodeCANeighborhood = aXmlNode.SelectSingleNode("./CANeighborhood");

                        string aCANeighborhood = null;

                        if (aXmlNodeCANeighborhood != null)
                        {
                            aCANeighborhood = aXmlNodeCANeighborhood.InnerText;
                        }

                        CANeighborhoodTypes? aCANeighborhoodEnum = null;

                        foreach (ComboBoxItem aCANeighborhoodItem in ConstantLists.CANeighborhoodItems)
                        {
                            if (aCANeighborhoodItem.ComboBoxString == aCANeighborhood)
                            {
                                aCANeighborhoodEnum = (CANeighborhoodTypes)aCANeighborhoodItem.ComboBoxId;
                                break;
                            }
                        }

                        XmlNode aXmlNodeCANeighborhoodRange = aXmlNode.SelectSingleNode("./CANeighborhoodRange");

                        int? aCANeighborhoodRange = null;

                        if (aXmlNodeCANeighborhoodRange != null)
                        {
                            aCANeighborhoodRange = Convert.ToInt32(aXmlNodeCANeighborhoodRange.InnerText, CultureInfo.InvariantCulture);
                        }

                        if (aCANeighborhoodRange == null)
                        {
                            throw new CAExplorerException("The value of CA neighborhood range must be specified!");
                        }
                        else if (aCANeighborhoodRange <= 0)
                        {
                            throw new CAExplorerException("The value of CA neighborhood range must be higher than zero!");
                        }

                        //creating new CARule data class that contains all data needed for specific rule.
                        CARuleData aCARuleData = new CARuleData();
                        aCARuleData.CARuleName = aCAName;
                        aCARuleData.CARuleFamily = (CARuleFamilies)aCARuleFamilyEnum;
                        aCARuleData.Birth = aBirthList;
                        aCARuleData.Survival = aSurvivalList;
                        aCARuleData.CountOfStates = aCountOfStates;
                        aCARuleData.Threshold = aThreshold;
                        aCARuleData.CANeighborhoodType = (CANeighborhoodTypes)aCANeighborhoodEnum;
                        aCARuleData.CANeighborhoodRange = (int)aCANeighborhoodRange;

                        //adding new rule to rule list.
                        caRuleListIn.Add(aCARuleData);
                    }
                    catch (Exception ex)
                    {
                        if (ex is XPathException || ex is FormatException || ex is OverflowException || ex is CAExplorerException)
                        {
                            aCARulesWithProblem += "Rule family : " + aCARuleFamilyString + " Rule : " + aCAName + "\n";
                            aCARulesExceptions += ex.Message + "\n";
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is XPathException || ex is ArgumentException || ex is FormatException || ex is OverflowException || ex is PathTooLongException
                     || ex is XmlException || ex is DirectoryNotFoundException || ex is IOException || ex is FileNotFoundException
                     || ex is UnauthorizedAccessException || ex is System.Security.SecurityException || ex is CAExplorerException)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during reading of available CA Rules", "Problem with reading of CA Explorer available rules : \n" + fileNameIn
                            + "\n" + "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                else
                {
                    throw;
                }
            }

            if (aCARulesWithProblem != null )
            {
                DialogMediator.ShowMessageBox(null, "Exception during reading of available CA Rules", "Problem with reading of CA Explorer available rules : \n" + aCARulesWithProblem
                        + "\n" + "Following exception occured : \n" + aCARulesExceptions, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }

            // if there was no rule found, asking user if backup rules should be created.
            if (caRuleListIn.Count == 0)
            {
                System.Windows.MessageBoxResult aMBResult = DialogMediator.ShowMessageBox(null, "Loading default 2 CA rules", "Should the default 2 backup CA rules be loaded ? (if no there will be no CA rules loaded!): \n",
                                                                                   System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning);

                if (aMBResult == System.Windows.MessageBoxResult.Yes)
                {
                    CARuleData aCARuleBackupData = new CARuleData();
                    aCARuleBackupData.CARuleName = "Conway's Life";
                    aCARuleBackupData.CARuleFamily = CARuleFamilies.Life;
                    aCARuleBackupData.Birth = new List<int>() { 3 };
                    aCARuleBackupData.Survival = new List<int>() { 2, 3 };
                    aCARuleBackupData.CountOfStates = 2;
                    aCARuleBackupData.CANeighborhoodType = CANeighborhoodTypes.Moore;
                    aCARuleBackupData.CANeighborhoodRange = 1;

                    caRuleListIn.Add(aCARuleBackupData);

                    aCARuleBackupData = new CARuleData();
                    aCARuleBackupData.CARuleName = "Brian's Brain";
                    aCARuleBackupData.CARuleFamily = CARuleFamilies.Generations;
                    aCARuleBackupData.Birth = new List<int>() { 2 };
                    aCARuleBackupData.Survival = new List<int>();
                    aCARuleBackupData.CountOfStates = 3;
                    aCARuleBackupData.CANeighborhoodType = CANeighborhoodTypes.Moore;
                    aCARuleBackupData.CANeighborhoodRange = 1;

                    caRuleListIn.Add(aCARuleBackupData);
                }
            }
        }
    }
}
