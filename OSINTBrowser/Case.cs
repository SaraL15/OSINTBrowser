using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSINTBrowser
{
    public static class Case
    {

        public static string CaseName;
        public static string CaseUser;
        public static string CaseCreationDate;
        public static string CaseOrganisationName;
        public static string CaseFilePath;
        public static string CaseOperationName;

        public static string caseName
        {
            get { return CaseName; }
            set { CaseName = value; }
        }
        public static string caseUser
        {
            get { return CaseUser; }
            set { CaseUser = value; }
        }
        public static string caseCreationDate
        {
            get { return CaseCreationDate; }
            set { CaseCreationDate = value; }
        }
        public static string caseOrganisationName
        {
            get { return CaseOrganisationName; }
            set { CaseOrganisationName = value; }
        }

        public static string caseFilePath
        {
            get { return CaseFilePath; }
            set { CaseFilePath = value; }
        }

        public static string caseOperationName
        {
            get { return CaseOperationName; }
            set { CaseOperationName = value; }
        }

        //public static Case(string caseName, string caseUser, string caseCreationDate, string caseOrganisationName, string caseFilePath)
        //{
        //    CaseName = caseName;
        //    CaseUser = caseUser;
        //    CaseCreationDate = caseCreationDate;
        //    CaseOrganisationName = caseOrganisationName;
        //    CaseFilePath = caseFilePath;
        //}

        //public Case(string caseName, string caseUser, string caseCreationDate, string caseOrganisationName, string caseFilePath, string caseOperationName)
        //{
        //    CaseName = caseName;
        //    CaseUser = caseUser;
        //    CaseCreationDate = caseCreationDate;
        //    CaseOrganisationName = caseOrganisationName;
        //    CaseFilePath = caseFilePath;
        //    CaseOrganisationName = caseOperationName;
        //}


        //public Case(string caseFilePath)
        //{
        //    CaseFilePath = caseFilePath;
        //}
    }
}

