using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSINTBrowser
{
    public static class Case
    {
        public static int CaseID;
        public static int UserID;
        public static string CaseName;
        public static string CaseCreationDate;
        public static string CaseFilePath;

        public static int userID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public static int caseID
        {
            get { return CaseID; }
            set { CaseID = value; }
        }
        public static string caseName
        {
            get { return CaseName; }
            set { CaseName = value; }
        }
        public static string caseCreationDate
        {
            get { return CaseCreationDate; }
            set { CaseCreationDate = value; }
        }
        public static string caseFilePath
        {
            get { return CaseFilePath; }
            set { CaseFilePath = value; }
        }
    }
}

