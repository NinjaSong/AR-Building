[System.Serializable]
public class SessionInfo
{
    public string welcomeText;
    public string consentText;
    public DemographicQuestion[] demographicQuestions;
    public string[] petalQuestions;
}

[System.Serializable]
public class DemographicQuestion
{
    public string prompt;
    public string[] responses;
}