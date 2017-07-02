// programmed by Adrian Magdina in 2013
// in this file is the implementation of factory for creating of rulefamily instances.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //factory for creation of specific rule family instance, depending on the input parameters a specific instance will be created.
    public static class CARuleFactory
    {
        public static ICARuleFamily CreateCARuleFamily(CARuleFamilies caRuleFamilyIn, ICARuleData caRuleDataIn)
        {
            ICARuleFamily aNewCA = null;

            if (caRuleFamilyIn == CARuleFamilies.Life)
            {
                aNewCA = new CARuleFamilyLife(caRuleDataIn);
            }
            else if (caRuleFamilyIn == CARuleFamilies.Generations)
            {
                aNewCA = new CARuleFamilyGenerations(caRuleDataIn);
            }
            else if (caRuleFamilyIn == CARuleFamilies.Cyclic)
            {
                aNewCA = new CARuleFamilyCyclic(caRuleDataIn);
            }

            return aNewCA;
        }
    }
}
