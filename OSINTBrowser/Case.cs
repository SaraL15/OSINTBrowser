using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSINTBrowser
{
    class Case
    {
        private string CaseName;
        private string CaseUser;
        private string CaseCreationDate;
        private string CaseOrganisationName;
        private string CaseFilePath;
        private string CaseOperationName;

        public string caseName
        {
            get { return CaseName; }
            set { CaseName = value; }
        }
        public string caseUser
        {
            get { return CaseUser; }
            set { CaseUser = value; }
        }
        public string caseCreationDate
        {
            get { return CaseCreationDate; }
            set { CaseCreationDate = value; }
        }
        public string caseOrganisationName
        {
            get { return CaseOrganisationName; }
            set { CaseOrganisationName = value; }
        }

        public string caseFilePath
        {
            get { return CaseFilePath; }
            set { CaseFilePath = value; }
        }

        public string caseOperationName
        {
            get { return CaseOperationName; }
            set { CaseOperationName = value; }
        }

        public Case(string caseName, string caseUser, string caseCreationDate, string caseOrganisationName, string caseFilePath)
        {
            CaseName = caseName;
            CaseUser = caseUser;
            CaseCreationDate = caseCreationDate;
            CaseOrganisationName = caseOrganisationName;
            CaseFilePath = caseFilePath;
        }

        public Case(string caseName, string caseUser, string caseCreationDate, string caseOrganisationName, string caseFilePath, string caseOperationName)
        {
            CaseName = caseName;
            CaseUser = caseUser;
            CaseCreationDate = caseCreationDate;
            CaseOrganisationName = caseOrganisationName;
            CaseFilePath = caseFilePath;
            CaseOrganisationName = caseOperationName;
        }


        public Case(string caseFilePath)
        {
            CaseFilePath = caseFilePath;
        }

    }
}

