namespace OSINTBrowser
{
    //Static Case class, so all information releating to the case can be accessed when needed.
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

