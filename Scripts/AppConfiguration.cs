public static class AppConfiguration
{
    public static bool FIRST_INIT                       = false;
    public static bool VIEW_ALL_COMMENTS                = false;
    public static string SERVER_PROTOCOL                = "https://";
    public static string SERVER_URL                     = "ndlapps.ce.gatech.edu/gtlb/";

    public static string URL { get { return SERVER_PROTOCOL + SERVER_URL; } }

    public readonly static string COMMENT_DOWNLOAD_KEY  = "0fasf932mrhc0987sa9fd8ym3u4hcm9875c98nq5c89c5iuhfgiuhr8tny454970m";
    public readonly static string COMMENT_UPLOAD_KEY    = "f70384as9fdasnf83y2mp84c3hymcuhasdfy0a7wy974m2395y9av760a98nrea9a";
    public readonly static string ADMIN_KEY             = "uf9um4r8v3h2ekhfowaeihvma793587v3ay4mv8a34vn8ahr0m8vyahmvf8a23bm8";
    public readonly static string SESSION_KEY           = "asdf7yva0m39rmv093878v9a0nfy309bhm0ubhmisahnf9agmvuhfe938yb74mi34";
}